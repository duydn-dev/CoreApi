using Neac.Common.Dtos;
using Neac.Common.Dtos.RoleDtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface IRoleRepository
    {
        Task<Response<bool>> UpdateListRole(List<Role> roles);
        Task<Response<Guid>> UpdateUserRole(UpdateRoleUserDto request);
        Task<Response<GetRolesByUserDtos>> GetUserRole(Guid userId);
    }
}
