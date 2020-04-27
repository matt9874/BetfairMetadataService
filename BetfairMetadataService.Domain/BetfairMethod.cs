using System.Collections.Generic;

namespace BetfairMetadataService.Domain
{
    public enum BetfairMethod
    {
        UnknownMethod = 0,
        ListEventTypes = 1
    }

    public static class BetfairMethodExtensions
    {
        private static readonly Dictionary<BetfairMethod, string> _methodMappings = new Dictionary<BetfairMethod, string>()
        {
            {BetfairMethod.ListEventTypes, "listEventTypes" }
        };

        public static string GetMethodName(this BetfairMethod betfairMethod)
        {
            if (_methodMappings.TryGetValue(betfairMethod, out string methodName))
                return methodName;
            return null;
        }
    }
}
