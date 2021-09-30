﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common;
using Neac.Common.Const;
using Neac.Common.Dtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserRepository(
                IConfiguration configuration,
                IUnitOfWork unitOfWork,
                IMapper mapper,
                IHttpContextAccessor httpContextAccessor
            )
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        #region CRUD
        public async Task<Response<GetListResponseModel<List<UserCreateDto>>>> GetListUser(GetListUserRequestDto request)
        {
            try
            {
                var query = _unitOfWork.GetRepository<User>().GetAll();
                query = query.WhereIf(!string.IsNullOrEmpty(request.TextSearch),
                    n => n.UserName.Contains(request.TextSearch) ||
                         n.FullName.Contains(request.TextSearch) ||
                         n.Address.Contains(request.TextSearch) ||
                         n.Email.Contains(request.TextSearch) ||
                         n.NumberPhone.Contains(request.TextSearch)
                    );
                query = query.WhereIf(request.Status.HasValue, n => n.Status == request.Status);

                GetListResponseModel<List<UserCreateDto>> responseData = new GetListResponseModel<List<UserCreateDto>>(query.Count(), request.PageSize);
                var result = await query
                    .OrderByDescending(n => n.CreatedDate)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();

                responseData.Data = _mapper.Map<List<User>, List<UserCreateDto>>(result);
                return Response<GetListResponseModel<List<UserCreateDto>>>.CreateSuccessResponse(responseData);
            }
            catch(Exception ex)
            {
                return Response<GetListResponseModel<List<UserCreateDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<UserCreateDto>> Create(UserCreateDto request)
        {
            try
            {
                request.CreatedBy = (request.UserId == Guid.Empty) ? (await GetIdentityUser()).UserId : request.UserId;
                request.UserId = (request.UserId == Guid.Empty) ? Guid.NewGuid() : request.UserId;
                request.CreatedDate = DateTime.Now;
                request.PassWord = Md5Encrypt.MD5Hash(request.PassWord);

                var userMapped = _mapper.Map<UserCreateDto, User>(request);
                await _unitOfWork.GetRepository<User>().Add(userMapped);
                await _unitOfWork.SaveAsync();
                return Response<UserCreateDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                return Response<UserCreateDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<bool>> Delete(Guid userId)
        {
            try
            {
                await _unitOfWork.GetRepository<User>().DeleteByExpression(n => n.UserId == userId);
                await _unitOfWork.SaveAsync();
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch(Exception ex)
            {
                return Response<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<bool>> DeleteMany(List<Guid> userIds)
        {
            try
            {
                await _unitOfWork.GetRepository<User>().DeleteByExpression(n => userIds.Contains(n.UserId));
                await _unitOfWork.SaveAsync();
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                return Response<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<UserCreateDto>> Update(UserCreateDto request)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserId == request.UserId).FirstOrDefaultAsync();
                var userMapped = _mapper.Map<UserCreateDto, User>(request, user);
                return Response<UserCreateDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                return Response<UserCreateDto>.CreateErrorResponse(ex);
            }
        }
        #endregion
        public async Task<Response<User>> GetUserByUserName(string userName)
        {
            try
            {
                return Response<User>.CreateSuccessResponse(await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserName == userName).Include(n => n.UserRoles).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                return Response<User>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<string>> Login(UserLoginDto request)
        {
            try
            {
                var responseUser = await GetUserByUserName(request.UserName);
                if (responseUser.ResponseData == null)
                {
                    return new Response<string>(false, 404, "không tìm thấy tài khoản này !", null);
                }
                if (responseUser.ResponseData.Status == UserStatus.Locked)
                {
                    return new Response<string>(false, 200, "tài khoản đang bị khóa !", null);
                }
                if (Md5Encrypt.MD5Hash(request.PassWord) != responseUser.ResponseData.PassWord)
                {
                    return new Response<string>(false, 404, "sai mật khẩu, vui lòng xem lại", null);
                }
                var token = await GenerateToken(responseUser.ResponseData);
                return Response<string>.CreateSuccessResponse(token);
            }
            catch (Exception ex)
            {
                return Response<string>.CreateErrorResponse(ex);
            }
        }
        private async Task<string> GenerateToken(User user)
        {
            var authClaims = new List<Claim>
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var userRole in user.UserRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole.Role.RoleCode));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<UserCreateDto> GetIdentityUser()
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims;
            UserCreateDto userInfo = new UserCreateDto();
            userInfo.UserId = new Guid(claims.FirstOrDefault(n => n.Type == nameof(userInfo.UserId)).Value);
            userInfo.UserName = claims.FirstOrDefault(n => n.Type == ClaimTypes.Name).Value;
            return userInfo;
        }
    }
}