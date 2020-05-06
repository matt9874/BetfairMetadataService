using AutoMapper;
using BetfairMetadataService.DataAccess.Interfaces;
using BetfairMetadataService.Domain;
using BetfairMetadataService.WebRequests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetfairMetadataService.WebRequests.BetfairApi.Readers
{
    public abstract class AbstractBetfairBatchReader<TEntity, TBetfairDto> : IBatchReader<TEntity>
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

        public async Task<IEnumerable<TEntity>> Read(Func<TEntity, bool> filter)
        {
            IList<TBetfairDto> results = await _requestInvoker.Invoke<IList<TBetfairDto>>(_method);
            var eventTypes = _mapper.Map<IEnumerable<TEntity>>(results);
            return eventTypes.Where(filter);
        }
    }
}
