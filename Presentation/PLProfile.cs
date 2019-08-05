using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using Presentation.Models;

namespace Presentation
{
    public class PLProfile : Profile
    {
        public PLProfile()
        {
            CreateMap<SlotCreationModel, SlotCreationDTO>();
        }
    }
}
