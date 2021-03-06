﻿using AutoMapper;
using BetfairMetadataService.API.Models.External;
using BetfairMetadataService.Domain.External;

namespace BetfairMetadataService.API.AutomapperProfiles
{
    public class ExternalDtosProfile:Profile
    {
        public ExternalDtosProfile()
        {
            CreateMap<DataProvider, DataProviderDto>();
            CreateMap<EventType, EventTypeDto>();
            CreateMap<Competition, CompetitionDto>();
            CreateMap<MarketType, MarketTypeDto>()
                .ForMember(dest => dest.MarketType, opt => opt.MapFrom(mtr => mtr.Name));
        }
    }
}
