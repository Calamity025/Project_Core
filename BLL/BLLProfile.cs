using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using BLL.DTO;
using Entities;

namespace BLL
{
    public class BLLProfile : Profile
    {
        public BLLProfile()
        {
            CreateMap<SlotCreationDTO, Slot>().ForMember(src => src.Id, opt => opt.Ignore());
            CreateMap<Slot, SlotMinimumDTO>();
            CreateMap<Slot, SlotFullDTO>();
            CreateMap<UserCreationDTO, User>().ForMember(src => src.Id, opt => opt.Ignore());
            CreateMap<User, UserLoginResponse.UserDTO>();
        }
    }
}
