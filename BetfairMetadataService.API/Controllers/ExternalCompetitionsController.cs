using AutoMapper;
using BetfairMetadataService.API.Models.External;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain.External;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ExternalCompetitionsController : ControllerBase
    {
        private readonly Func<int, IBatchReader<Competition>> _batchReaderFactory;
        private readonly IMapper _mapper;

        public ExternalCompetitionsController(Func<int, IBatchReader<Competition>> batchReaderFactory, IMapper mapper)
        {
            _batchReaderFactory = batchReaderFactory;
            _mapper = mapper;
        }

        [HttpGet("dataProviders/{dataProviderId}/competitions")]
        public async Task<IActionResult> GetCompetitions(int dataProviderId)
        {
            IBatchReader<Competition> reader = _batchReaderFactory?.Invoke(dataProviderId);
            IEnumerable<Competition> competitions = await reader.Read(c => true);
            if (competitions == null)
                throw new Exception("IBatchReader<Competition> returned null IEnumerable");

            IEnumerable<CompetitionDto> competitionDtos = _mapper.Map<IList<CompetitionDto>>(competitions);
            return Ok(competitionDtos);
        }
    }
}
