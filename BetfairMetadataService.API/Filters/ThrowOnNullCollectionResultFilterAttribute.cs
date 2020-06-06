using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using System;

namespace BetfairMetadataService.API.Filters
{
    public class ThrowOnNullCollectionResultFilterAttribute : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            var enumerable = resultFromAction?.Value as System.Collections.IEnumerable;

            if (resultFromAction == null || resultFromAction.StatusCode < 200 || resultFromAction.StatusCode >= 300
                || enumerable != null)
            {
                await next();
                return;
            }

            throw new InvalidOperationException("Cannot return a null enumerable from this controller action");
        }
    }
}
