using AutoMapper;
using BetfairMetadataService.API.Models.Internal;
using BetfairMetadataService.Domain.Internal;

namespace BetfairMetadataService.API.AutomapperProfiles
{
    public class InternalDtosProfile:Profile
    {
        public InternalDtosProfile()
        {
            CreateMap<EventType, EventTypeDto>();
            CreateMap<Competition, CompetitionDto>();
            CreateMap<Event, EventDto>();
            CreateMap<Market, MarketDto>();
            CreateMap<Selection, SelectionDto>();
        }
    }
}
