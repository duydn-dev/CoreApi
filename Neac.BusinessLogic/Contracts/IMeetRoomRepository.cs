using Neac.Common.Dtos;
using Neac.Common.Dtos.MeetRooms;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Contracts
{
    public interface IMeetRoomRepository
    {
        Task<Response<List<MeetRoom>>> GetMeetRooms(GetFilterMeetRoomDto request);
    }
}
