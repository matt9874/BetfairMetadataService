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
    public class SelectionsLinksResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction == null || resultFromAction.StatusCode < 200 || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var resultValueFromAction = resultFromAction.Value as IEnumerable<SelectionDto>;
            if (resultValueFromAction == null)
            {
                await next();
                return;
            }

            bool hasAcceptHeader = context.HttpContext.Request.Headers.TryGetValue("Accept", out StringValues acceptHeader);
            if (hasAcceptHeader && acceptHeader.Contains("application/vnd.marvin.hateoas+json"))
                resultFromAction.Value = CreateSelectionsWrapper(resultValueFromAction, context);
            await next();
        }

        private IEnumerable<LinkDto> CreateLinksForSelections(ResultExecutingContext context, string marketId)
        {
            if (marketId == null)
                return null;

            LinkGenerator linkGenerator = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
            return new LinkDto[]
            {
                new LinkDto(linkGenerator.GetUriByName(context.HttpContext, "GetSelectionsByMarket", new {marketId }),
                    "self",
                    "GET")
            };
        }

        private LinkCollectionWrapperDto<SelectionDto> CreateSelectionsWrapper(IEnumerable<SelectionDto> selectionDtos,
            ResultExecutingContext context)
        {
            var wrapper = new LinkCollectionWrapperDto<SelectionDto>(selectionDtos.ToList());
            wrapper.Links = CreateLinksForSelections(context, selectionDtos.FirstOrDefault()?.MarketId).ToList();
            return wrapper;
        }
    }
}
