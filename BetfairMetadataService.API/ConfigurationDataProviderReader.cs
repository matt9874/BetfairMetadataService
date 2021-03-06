﻿using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.External;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BetfairMetadataService.API
{
    public class ConfigurationDataProviderReader : IReader<DataProvider, int>
    {
        private readonly IOptionsMonitor<DataProviderMappings> _dataProviderMappings;

        public ConfigurationDataProviderReader(IOptionsMonitor<DataProviderMappings> dataProviderMappings)
        {
            _dataProviderMappings = dataProviderMappings;
        }

        public Task<DataProvider> Read(int id)
        {
            if (_dataProviderMappings.CurrentValue.Values.TryGetValue(id.ToString(), out Domain.Internal.DataProvider dataProvider))
            {
                return Task.FromResult(new DataProvider()
                {
                    Id = id,
                    Name = dataProvider.Name,
                    Code = dataProvider.Code
                });
            }
            return Task.FromResult((DataProvider)null);
        }
    }
}
