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

            CreateMap<Domain.BetfairDtos.Event, Domain.External.Event>();

            CreateMap<CompetitionResult, Domain.External.Competition>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(o => o.Competition.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.Competition.Id))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(o => o.Competition.Region));

            CreateMap<EventResult, Domain.External.Event>()
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(o => o.Event.CountryCode))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.Event.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(o => o.Event.Name))
                .ForMember(dest => dest.OpenDate, opt => opt.MapFrom(o => o.Event.OpenDate))
                .ForMember(dest => dest.Timezone, opt => opt.MapFrom(o => o.Event.Timezone))
                .ForMember(dest => dest.Venue, opt => opt.MapFrom(o => o.Event.Venue));

            CreateMap<MarketTypeResult, MarketType>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(mtr => mtr.MarketType));

            CreateMap<MarketCatalogue, Market>()
                .ForMember(dest => dest.EventId, opt => opt.MapFrom(mc => mc.Event.Id))
                .IncludeMembers(mc => mc.Description);

            CreateMap<Domain.BetfairDtos.MarketBettingType, Domain.External.MarketBettingType>();
            CreateMap<MarketDescription, Market>();
            CreateMap<RunnerDescription, Selection>();
        }
    }
}
