using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class APINGException: Exception
    {
        internal APINGException()
        { }

        [JsonConstructor]
        protected APINGException(SerializationInfo info, StreamingContext context)
        {
            this.ErrorDetails = info.GetString("errorDetails");
            this.ErrorCode = info.GetString("errorCode");
            this.RequestUUID = info.GetString("requestUUID");
        }

        [JsonProperty(PropertyName = "errorDetails")]
        public string ErrorDetails { get; set; }

        [JsonProperty(PropertyName = "errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty(PropertyName = "requestUUID")]
        public string RequestUUID { get; set; }
    }
}
