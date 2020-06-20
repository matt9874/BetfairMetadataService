using BetfairMetadataService.Domain.Internal;
using System;

namespace BetfairMetadataService.API.ResourceParameters.Internal
{
    public class EventResourceParameters
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
