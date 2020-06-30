using System.Collections.Generic;
using BetfairMetadataService.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BetfairMetadataService.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>();

            links.Add(
              new LinkDto(Url.Link("GetRoot", new { }),
              "self",
              "GET"));

            links.Add(
              new LinkDto(Url.Link("GetEventTypes", new { }),
              "eventTypes",
              "GET"));

            return Ok(links);

        }
    }
}
