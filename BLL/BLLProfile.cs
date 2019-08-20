using System.Linq;
using AutoMapper;
using BLL.DTO;
using Entities;

namespace BLL
{
    public class BLLProfile : Profile
    {
        public BLLProfile()
        {
            CreateMap<SlotCreationDTO, Slot>()
                .ForMember(src => src.Id, opt => opt.Ignore())
                .ForMember(dest => dest.MinBet, 
                    opt => opt.MapFrom(x=>x.Step))
                .ForMember(x => x.StarterPrice, 
                    opt => opt.MapFrom(x => x.Price));
            CreateMap<Slot, SlotMinimumDTO>();
            CreateMap<Slot, SlotFullDTO>()
                .ForMember(dest => dest.Step, opt => 
                    opt.MapFrom(x => x.MinBet))
                .ForMember(dest => dest.SlotTags, opt =>
                    opt.MapFrom(x => x.SlotTags.Select(y => new Tag() {Id = y.Tag.Id, Name = y.Tag.Name})));
            CreateMap<IdentityCreationDTO, User>().ForMember(src => src.Id, 
                opt => opt.Ignore());
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Name, 
                    opt => opt.MapFrom(x => x.UserName));
            CreateMap<ProfileCreationDTO, UserInfo>();
            CreateMap<UserInfo, ProfileDTO>()
                .ForMember(dest => dest.FollowingSlots, opt =>
                    opt.Ignore())
                .ForMember(x => x.BetSlots, opt =>
                    opt.Ignore());
        }
    }
}
