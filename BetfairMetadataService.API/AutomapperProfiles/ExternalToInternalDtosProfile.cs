using AutoMapper;
using BetfairMetadataService.Domain.External;

namespace BetfairMetadataService.API.AutomapperProfiles
{
    public class ExternalToInternalDtosProfile:Profile
    {
        public ExternalToInternalDtosProfile()
        {
            CreateMap<DataProvider, Domain.Internal.DataProvider>();
            CreateMap<EventType, Domain.Internal.EventType>();
            CreateMap<Competition, Domain.Internal.Competition>();
            CreateMap<Event, Domain.Internal.Event>();
            CreateMap<Market, Domain.Internal.Market>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(m => m.MarketId));
            CreateMap<MarketBettingType, Domain.Internal.MarketBettingType>();
            CreateMap<Selection, Domain.Internal.Selection>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(s=>s.SelectionId));
        }
    }
}
