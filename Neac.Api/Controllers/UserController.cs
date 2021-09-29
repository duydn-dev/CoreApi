using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Neac.Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [RoleDescription("Cập nhật danh sách quyền")]
        [Route("update-role")]
        [HttpPost]
        public async Task<Response<bool>> UpdateRole()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Where(m => !m.CustomAttributes.Any(n => n.AttributeType == typeof(AllowAnonymousAttribute)))
                    .Select(x => new Role {
                        RoleCode = x.DeclaringType.Name.Replace("Controller","") + "-" + x.Name,
                        RoleId = Guid.Empty,
                        RoleName = ((RoleDescriptionAttribute)x.GetCustomAttribute(typeof(RoleDescriptionAttribute)))?.Description
                    })
                    .OrderBy(x => x.RoleCode).ToList();
            return new Response<bool>(true, 200, "ok", true);
        }

        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<Response<string>> Login([FromBody] UserLoginDto request)
        {
            return await _userRepository.Login(request);
        }
    }
}
