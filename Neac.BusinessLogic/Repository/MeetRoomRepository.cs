﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Neac.BusinessLogic.Contracts;
using Neac.BusinessLogic.UnitOfWork;
using Neac.Common;
using Neac.Common.Const;
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
        private readonly IMapper _mapper;
        public MeetRoomRepository(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<MeetRoom>> CreateRoomStatusAsync(MeetRoom request)
        {
            try
            {
                var data = await _unitOfWork.GetRepository<MeetRoom>().GetByExpression(n => n.DomainUrl == request.DomainUrl && n.RoomName == request.RoomName).FirstOrDefaultAsync();
                if (data != null)
                {
                    return Response<MeetRoom>.CreateErrorResponse(new Exception("Đã tồn tại domain và tên phòng !"));
                }
                request.MeetRoomId = Guid.NewGuid();
                await _unitOfWork.GetRepository<MeetRoom>().Add(request);
                await _unitOfWork.SaveAsync();
                return Response<MeetRoom>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                return Response<MeetRoom>.CreateErrorResponse(ex);
            }
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

        public async Task<Response<List<MeetRoom>>> GetRoomsOnlineAsync(GetFilterMeetRoomDto request)
        {
            try
            {
                var data = await _unitOfWork.GetRepository<MeetRoom>().GetAll()
                    .WhereIf(!string.IsNullOrEmpty(request.Domain), n => n.DomainUrl.Contains(request.Domain))
                    .WhereIf(!string.IsNullOrEmpty(request.RoomName), n => n.RoomName.Contains(request.RoomName))
                    .WhereIf(request.NumberMember.HasValue, n => n.MemberNumberInRoom == request.NumberMember)
                    .Where(n => n.Status == MeetRoomStatus.Online)
                    .ToListAsync();
                return Response<List<MeetRoom>>.CreateSuccessResponse(data);
            }
            catch(Exception ex)
            {
                return Response<List<MeetRoom>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<MeetRoom>> UpdateRoomSatusAsync(MeetRoom request)
        {
            try
            {
                var room = await _unitOfWork.GetRepository<MeetRoom>().GetByExpression(n => n.MeetRoomId == request.MeetRoomId).FirstOrDefaultAsync();
                var mappedRoom = _mapper.Map<MeetRoom, MeetRoom>(request, room);
                await _unitOfWork.GetRepository<MeetRoom>().Update(mappedRoom);
                await _unitOfWork.SaveAsync();
                return Response<MeetRoom>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                return Response<MeetRoom>.CreateErrorResponse(ex);
            }
        }
    }
}
