using Microsoft.EntityFrameworkCore;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common;
using Neac.Common.Dtos;
using Neac.Common.Dtos.MeetRooms;
using Neac.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neac.BusinessLogic.Repository
{
    public class MeetRoomRepository : IMeetRoomRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public MeetRoomRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<List<MeetRoom>>> GetMeetRooms(GetFilterMeetRoomDto request)
        {
            try
            {
                var data = await _unitOfWork.GetRepository<MeetRoom>().GetAll()
                    .WhereIf(!string.IsNullOrEmpty(request.Domain), n => n.DomainUrl.Contains(request.Domain))
                    .WhereIf(!string.IsNullOrEmpty(request.RoomName), n => n.RoomName.Contains(request.RoomName))
                    .WhereIf(request.NumberMember.HasValue, n => n.MemberNumberInRoom == request.NumberMember)
                    .ToListAsync();

                return Response<List<MeetRoom>>.CreateSuccessResponse(data);
            }
            catch(Exception ex)
            {
                return Response<List<MeetRoom>>.CreateErrorResponse(ex);
            }
        }
    }
}
