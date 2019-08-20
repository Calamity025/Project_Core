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
            CreateMap<UserRegistrationModel, IdentityCreationDTO>()
                .ForMember(src=>src.UserName, 
                    opt => opt.MapFrom(x=>x.Login));
            CreateMap<LoginInfoModel, LoginInfoDTO>();
            CreateMap<ProfileCreationModel, ProfileCreationDTO>();
            CreateMap<SlotUpdateModel, SlotUpdateDTO>();
        }
    }
}
