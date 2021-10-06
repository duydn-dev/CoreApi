﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neac.BusinessLogic.Contracts;
using Neac.Common.Dtos;
using Neac.Common.Dtos.MeetRooms;
using Neac.DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Neac.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetRoomController : ControllerBase
    {
        private readonly IMeetRoomRepository _meetRoomRepository;
        public MeetRoomController(IMeetRoomRepository meetRoomRepository)
        {
            _meetRoomRepository = meetRoomRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("meet-rooms")]
        public async Task<Response<List<MeetRoom>>> GetFilter([FromQuery]string filter)
        {
            var request = JsonConvert.DeserializeObject<GetFilterMeetRoomDto>(filter);
            return await _meetRoomRepository.GetMeetRooms(request);
        }
    }
}
