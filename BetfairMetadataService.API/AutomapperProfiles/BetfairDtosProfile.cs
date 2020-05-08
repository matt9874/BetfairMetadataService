using AutoMapper;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.Domain.External;

namespace BetfairMetadataService.API.AutomapperProfiles
{
    public class BetfairDtosProfile : Profile
    {
        public BetfairDtosProfile()
        {
            CreateMap<EventTypeResult, Domain.External.EventType>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(o => o.EventType.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.EventType.Id));
            CreateMap<CompetitionResult, Domain.External.Competition>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(o => o.Competition.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.Competition.Id))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(o => o.Competition.Region));

            CreateMap<MarketTypeResult, MarketType>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(mtr => mtr.MarketType));
        }
    }
}
