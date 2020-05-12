using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BetfairMetadataService.API.Filters
{
    public abstract class MappingResultFilterAttribute : ResultFilterAttribute
    {
        protected abstract object MapActionResultValue(IMapper mapper, object value);

        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var resultFromAction = context.Result as ObjectResult;
            if (resultFromAction == null || resultFromAction.StatusCode < 200 || resultFromAction.StatusCode >= 300)
            {
                await next();
                return;
            }

            IMapper mapper = context.HttpContext.RequestServices.GetService<IMapper>();

            object mappedValue = MapActionResultValue(mapper, resultFromAction.Value);
            resultFromAction.Value = mappedValue;

            await next();
        }
    }
}
