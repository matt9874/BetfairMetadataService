using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.External;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.API
{
    public class ConfigurationBatchDataProviderReader : IBatchReader<DataProvider>
    {
        private readonly IOptionsMonitor<DataProviderMappings> _dataProviderMappings;

        public ConfigurationBatchDataProviderReader(IOptionsMonitor<DataProviderMappings> dataProviderMappings)
        {
            _dataProviderMappings = dataProviderMappings;
        }
        public Task<IEnumerable<DataProvider>> Read(Func<DataProvider, bool> filter)
        {
            List<DataProvider> dataProviders = new List<DataProvider>();
            foreach (var mapping in _dataProviderMappings.CurrentValue.Values)
            {
                if (Int32.TryParse(mapping.Key, out int result))
                    dataProviders.Add(new DataProvider()
                    {
                        Id = result,
                        Name = mapping.Value
                    });
            }
            return Task.FromResult(dataProviders.AsEnumerable());
        }
    }
}
