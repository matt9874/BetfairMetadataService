using AutoMapper;
using BetfairMetadataService.API.Models.Betfair;
using BetfairMetadataService.Domain.BetfairDtos;

namespace BetfairMetadataService.API.AutomapperProfiles
{
    public class BetfairDtosProfile : Profile
    {
        public BetfairDtosProfile()
        {
            CreateMap<EventTypeResult, EventTypeDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(o => o.EventType.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.EventType.Id));
        }
    }
}
