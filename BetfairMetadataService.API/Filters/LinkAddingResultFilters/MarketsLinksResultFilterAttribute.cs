using BetfairMetadataService.API.Models;
using BetfairMetadataService.API.Models.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;

namespace BetfairMetadataService.API.Filters.LinkAddingResultFilters
{
    public class MarketsLinksResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction == null || resultFromAction.StatusCode < 200 || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var resultValueFromAction = resultFromAction.Value as IEnumerable<MarketDto>;
            if (resultValueFromAction == null)
            {
                await next();
                return;
            }

            bool hasAcceptHeader = context.HttpContext.Request.Headers.TryGetValue("Accept", out StringValues acceptHeader);
            if (hasAcceptHeader && acceptHeader.Contains("application/vnd.marvin.hateoas+json"))
                resultFromAction.Value = CreateMarketsWrapper(resultValueFromAction, context);
            await next();
        }

        private void AddLinksToMarket(MarketDto marketDto, ResultExecutingContext context)
        {
            LinkGenerator linkGenerator = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
            marketDto.Links = new List<LinkDto>
            {
                new LinkDto(linkGenerator.GetUriByName(context.HttpContext, "GetSelectionsByMarket", new {marketId = marketDto.Id }),
                    "selections",
                    "GET")
            };
        }

        private IEnumerable<LinkDto> CreateLinksForMarkets(ResultExecutingContext context, string eventId)
        {
            if (eventId == null)
                return null;

            LinkGenerator linkGenerator = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
            return new LinkDto[]
            {
                new LinkDto(linkGenerator.GetUriByName(context.HttpContext, "GetMarketsByEvent", new {eventId }),
                    "self",
                    "GET")
            };
        }

        private LinkCollectionWrapperDto<MarketDto> CreateMarketsWrapper(IEnumerable<MarketDto> marketDtos,
            ResultExecutingContext context)
        {
            foreach (var dto in marketDtos)
            {
                AddLinksToMarket(dto, context);
            }
            var wrapper = new LinkCollectionWrapperDto<MarketDto>(marketDtos.ToList());
            wrapper.Links = CreateLinksForMarkets(context, marketDtos.FirstOrDefault()?.EventId).ToList();
            return wrapper;
        }
    }
}
