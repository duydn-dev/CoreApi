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
        public UserRepository(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response<User>> GetUserByUserName(string userName)
        {
            try
            {
                return Response<User>.CreateSuccessResponse(await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserName == userName).FirstOrDefaultAsync());
            }
            catch(Exception ex)
            {
                return Response<User>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<string>> Login(UserLoginDto request)
        {
            try
            {
                var responseUser = await GetUserByUserName(request.UserName);
                if(responseUser.Data == null)
                {
                    return new Response<string>(false, 404, "không tìm thấy tài khoản này !", null);
                }
                if(responseUser.Data.Status == UserStatus.Locked)
                {
                    return new Response<string>(false, 200, "tài khoản đang bị khóa !", null);
                }
                if(Md5Encrypt.MD5Hash(request.PassWord) != responseUser.Data.PassWord)
                {
                    return new Response<string>(false, 404, "sai mật khẩu, vui lòng xem lại", null);
                }
                var token = await GenerateToken(responseUser.Data);
                return Response<string>.CreateSuccessResponse(token);
            }
            catch(Exception ex)
            {
                return Response<string>.CreateErrorResponse(ex);
            }
        }

        private async Task<string> GenerateToken(User user)
        {
            var authClaims = new List<Claim>
                {
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
    }
}
