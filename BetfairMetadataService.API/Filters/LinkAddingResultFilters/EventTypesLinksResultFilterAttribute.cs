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
    public class EventTypesLinksResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction == null || resultFromAction.StatusCode < 200 || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var resultValueFromAction = resultFromAction.Value as IEnumerable<EventTypeDto>;
            if (resultValueFromAction == null)
            {
                await next();
                return;
            }

            bool hasAcceptHeader = context.HttpContext.Request.Headers.TryGetValue("Accept", out StringValues acceptHeader);
            if (hasAcceptHeader && acceptHeader.Contains("application/vnd.marvin.hateoas+json"))
                resultFromAction.Value = CreateEventTypesWrapper(resultValueFromAction, context);
            await next();
        }

        private void AddLinksToEventType(EventTypeDto eventTypeDto, ResultExecutingContext context)
        {
            LinkGenerator linkGenerator = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
            eventTypeDto.Links = new List<LinkDto>
            {
                new LinkDto(linkGenerator.GetUriByName(context.HttpContext, "GetCompetitionsByEventType", new {eventTypeId = eventTypeDto.Id }),
                    "competitions",
                    "GET")
            };
        }

        private IEnumerable<LinkDto> CreateLinksForEventTypes(ResultExecutingContext context)
        {
            LinkGenerator linkGenerator = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
            return new LinkDto[]
            {
                new LinkDto(linkGenerator.GetUriByName(context.HttpContext, "GetEventTypes", new { }),
                    "self",
                    "GET")
            };
        }

        private LinkCollectionWrapperDto<EventTypeDto> CreateEventTypesWrapper(IEnumerable<EventTypeDto> eventTypeDtos,
            ResultExecutingContext context)
        {
            foreach (var dto in eventTypeDtos)
            {
                AddLinksToEventType(dto, context);
            }
            var wrapper = new LinkCollectionWrapperDto<EventTypeDto>(eventTypeDtos.ToList());
            wrapper.Links = CreateLinksForEventTypes(context).ToList();
            return wrapper;
        }
    }
}
