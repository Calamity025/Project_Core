﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            CreateMap<LoginResponse, CurrentUserModel>()
                .ForMember(src => src.Id, opt => opt.MapFrom(x => x.User.Id))
                .ForMember(src => src.Name, opt => opt.MapFrom(x => x.User.UserName))
                .ForMember(src => src.AvatarLink, opt => opt.MapFrom(x => x.User.AvatarLink));
            CreateMap<UserRegistrationModel, IdentityCreationDTO>()
                .ForMember(src=>src.UserName, opt => opt.MapFrom(x=>x.Login));
        }
    }
}
