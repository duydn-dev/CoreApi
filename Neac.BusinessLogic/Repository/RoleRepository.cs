using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common.Dtos;
using Neac.Common.Dtos.RoleDtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _memoryCache;
        public RoleRepository(IUnitOfWork unitOfWork, IUserRepository userRepository, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _memoryCache = memoryCache;
        }

        public async Task<Response<GetRolesByUserDtos>> GetUserRole(Guid userId)
        {
            try
            {
                var userRoleMem = _memoryCache.Get($"user/{userId}");
                if (userRoleMem != null)
                {
                    return Response<GetRolesByUserDtos>.CreateSuccessResponse((GetRolesByUserDtos)userRoleMem);
                }
                var roles = await (from ur in _unitOfWork.GetRepository<UserRole>().GetAll()
                                   join r in _unitOfWork.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                   where ur.UserId == userId
                                   select r).ToListAsync();

                var userRoles = new GetRolesByUserDtos { UserId = userId, Roles = roles };
                _memoryCache.Set($"user/{userId}", userRoles);

                return Response<GetRolesByUserDtos>.CreateSuccessResponse(userRoles);
            }
            catch(Exception ex)
            {
                return Response<GetRolesByUserDtos>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<bool>> UpdateListRole(List<Role> roles)
        {
            try
            {
                var listRole = _unitOfWork.GetRepository<Role>().GetAll();
                var currentUser = await _userRepository.GetIdentityUser();
                foreach (var role in roles)
                {
                    
                    var roleCode = await listRole.FirstOrDefaultAsync(n => n.RoleCode == role.RoleCode);
                    if (roleCode != null)
                    {
                        roleCode.RoleName = role.RoleName;
                        roleCode.ModifiedBy = currentUser.UserId;
                        roleCode.ModifiedDate = DateTime.Now;
                        await _unitOfWork.GetRepository<Role>().Update(roleCode);
                    }
                    else
                    {
                        
                        role.RoleId = Guid.NewGuid();
                        role.CreatedBy = currentUser.UserId;
                        role.CreatedDate = DateTime.Now;
                        await _unitOfWork.GetRepository<Role>().Add(role);
                    }
                }
                await _unitOfWork.SaveAsync();
                return Response<bool>.CreateSuccessResponse(true); ;
            }
            catch(Exception ex)
            {
                return Response<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<Guid>> UpdateUserRole(UpdateRoleUserDto request)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserId == request.UserId).Include(n => n.UserRoles).FirstOrDefaultAsync();
                if(user?.UserRoles?.Count > 0)
                {
                    await _unitOfWork.GetRepository<UserRole>().DeleteByExpression(n => user.UserRoles.Select(g => g.UserId).Any(g => g == n.UserId));
                    await _unitOfWork.SaveAsync();
                }
                foreach (var item in request.RoleIds)
                {
                    await _unitOfWork.GetRepository<UserRole>().Add(new UserRole() { RoleId = item, UserId = request.UserId, UserRoleId = Guid.NewGuid() });
                }
                await _unitOfWork.SaveAsync();

                var roles = await (from ur in _unitOfWork.GetRepository<UserRole>().GetAll()
                                   join r in _unitOfWork.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                   where ur.UserId == request.UserId
                                   select r).ToListAsync();
                var userRoles = new GetRolesByUserDtos { UserId = request.UserId, Roles = roles };

                _memoryCache.Remove($"user/{request.UserId}");
                _memoryCache.Set($"user/{request.UserId}", userRoles);
                return Response<Guid>.CreateSuccessResponse(request.UserId);
            }
            catch(Exception ex)
            {
                return Response<Guid>.CreateErrorResponse(ex);
            }
        }
    }
}
