using AutoMapper;
using BetfairMetadataService.Domain;
using BetfairMetadataService.Domain.BetfairDtos;
using BetfairMetadataService.DataAccess.Interfaces.WebRequests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public abstract class AbstractBetfairBatchReader<TEntity, TBetfairDto> : IBetfairBatchReader<TEntity>
    {
        private readonly IRequestInvokerAsync _requestInvoker;
        private readonly IMapper _mapper;
        private readonly BetfairMethod _method;

        public AbstractBetfairBatchReader(IRequestInvokerAsync requestInvoker, IMapper mapper, BetfairMethod method)
        {
            _requestInvoker = requestInvoker;
            _mapper = mapper;
            _method = method;
        }

        public async Task<IEnumerable<TEntity>> Read(MarketFilter filter)
        {
            IList<TBetfairDto> results = await _requestInvoker.Invoke<IList<TBetfairDto>>(_method, filter);
            return _mapper.Map<IEnumerable<TEntity>>(results);
        }
    }
}
