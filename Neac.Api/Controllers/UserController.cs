using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.Api.Attributes;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neac.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [Route("login")]
        [HttpPost]
        public async Task<Response<string>> Login([FromBody] UserLoginDto request)
        {
            return await _userRepository.Login(request);
        }
    }
}
