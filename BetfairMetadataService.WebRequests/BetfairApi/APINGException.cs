using System;
using System.Runtime.Serialization;

namespace BetfairMetadataService.WebRequests.BetfairApi
{
    public class APINGException: Exception
    {
        protected APINGException(SerializationInfo info, StreamingContext context)
        {
            this.ErrorDetails = info.GetString("errorDetails");
            this.ErrorCode = info.GetString("errorCode");
            this.RequestUUID = info.GetString("requestUUID");
        }

        public string ErrorDetails { get; set; }

        public string ErrorCode { get; set; }

        public string RequestUUID { get; set; }
    }
}
