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
    public class EventsLinksResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction == null || resultFromAction.StatusCode < 200 || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var resultValueFromAction = resultFromAction.Value as IEnumerable<EventDto>;
            if (resultValueFromAction == null)
            {
                await next();
                return;
            }

            bool hasAcceptHeader = context.HttpContext.Request.Headers.TryGetValue("Accept", out StringValues acceptHeader);
            if (hasAcceptHeader && acceptHeader.Contains("application/vnd.marvin.hateoas+json"))
                resultFromAction.Value = CreateEventsWrapper(resultValueFromAction, context);
            await next();
        }

        private void AddLinksToEvent(EventDto eventDto, ResultExecutingContext context)
        {
            LinkGenerator linkGenerator = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
            eventDto.Links = new List<LinkDto>
            {
                new LinkDto(linkGenerator.GetUriByName(context.HttpContext, "GetMarketsByEvent", new {eventId = eventDto.Id }),
                    "markets",
                    "GET")
            };
        }

        private IEnumerable<LinkDto> CreateLinksForEvents(ResultExecutingContext context, string competitionId)
        {
            if (competitionId == null)
                return null;

            LinkGenerator linkGenerator = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
            return new LinkDto[]
            {
                new LinkDto(linkGenerator.GetUriByName(context.HttpContext, "GetEventsByCompetition", new {competitionId }),
                    "self",
                    "GET")
            };
        }

        private LinkCollectionWrapperDto<EventDto> CreateEventsWrapper(IEnumerable<EventDto> eventDtos,
            ResultExecutingContext context)
        {
            foreach (var dto in eventDtos)
            {
                AddLinksToEvent(dto, context);
            }
            var wrapper = new LinkCollectionWrapperDto<EventDto>(eventDtos.ToList());
            wrapper.Links = CreateLinksForEvents(context, eventDtos.FirstOrDefault()?.CompetitionId).ToList();
            return wrapper;
        }
    }
}
