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
    public class CompetitionsLinksResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction == null || resultFromAction.StatusCode < 200 || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            var resultValueFromAction = resultFromAction.Value as IEnumerable<CompetitionDto>;
            if (resultValueFromAction == null)
            {
                await next();
                return;
            }

            bool hasAcceptHeader = context.HttpContext.Request.Headers.TryGetValue("Accept", out StringValues acceptHeader);
            if (hasAcceptHeader && acceptHeader.Contains("application/vnd.marvin.hateoas+json"))
                resultFromAction.Value = CreateCompetitionsWrapper(resultValueFromAction, context);
            await next();
        }

        private void AddLinksToCompetition(CompetitionDto competitionDto, ResultExecutingContext context)
        {
            LinkGenerator linkGenerator = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
            competitionDto.Links = new List<LinkDto>
            {
                new LinkDto(linkGenerator.GetUriByName(context.HttpContext, "GetEventsByCompetition", new {competitionId = competitionDto.Id }),
                    "events",
                    "GET")
            };
        }

        private IEnumerable<LinkDto> CreateLinksForCompetitions(ResultExecutingContext context, string eventTypeId)
        {
            if (eventTypeId == null)
                return null;

            LinkGenerator linkGenerator = context.HttpContext.RequestServices.GetRequiredService<LinkGenerator>();
            return new LinkDto[]
            {
                new LinkDto(linkGenerator.GetUriByName(context.HttpContext, "GetCompetitionsByEventType", new {eventTypeId }),
                    "self",
                    "GET")
            };
        }

        private LinkCollectionWrapperDto<CompetitionDto> CreateCompetitionsWrapper(IEnumerable<CompetitionDto> competitionDtos,
            ResultExecutingContext context)
        {
            foreach (var dto in competitionDtos)
            {
                AddLinksToCompetition(dto, context);
            }
            var wrapper = new LinkCollectionWrapperDto<CompetitionDto>(competitionDtos.ToList());
            wrapper.Links = CreateLinksForCompetitions(context, competitionDtos.FirstOrDefault()?.EventTypeId).ToList();
            return wrapper;
        }
    }
}
