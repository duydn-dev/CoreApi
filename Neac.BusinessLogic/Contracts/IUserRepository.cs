using Neac.Common.Dtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface IUserRepository
    {
        Task<Response<string>> Login(UserLoginDto request);
        Task<Response<User>> GetUserByUserName(string userName);
    }
}
