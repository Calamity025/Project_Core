using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Entities;

namespace BLL
{
    public class BLLProfile : Profile
    {
        public BLLProfile()
        {
            CreateMap<SlotCreationDTO, Slot>();
            CreateMap<Slot, SlotMinimumDTO>();
        }
    }
}
