using AutoMapper;
using BetfairMetadataService.API.Models.FetchRoots;
using BetfairMetadataService.Domain.FetchRoots;

namespace BetfairMetadataService.API.AutomapperProfiles
{
    public class FetchRootsProfile : Profile
    {
        public FetchRootsProfile()
        {
            CreateMap<EventTypeMarketTypeCreationDto, EventTypeMarketType>();
            CreateMap<EventTypeMarketType, EventTypeMarketTypeDto>();
        }
    }
}
