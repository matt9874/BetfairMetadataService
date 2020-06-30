using System.Collections.Generic;

namespace BetfairMetadataService.API.Models
{
    public class LinkCollectionWrapperDto<T> : LinkResourceBaseDto
    {
        public List<T> Value { get; set; }

        public LinkCollectionWrapperDto()
        {
            Value = new List<T>();
        }

        public LinkCollectionWrapperDto(List<T> value)
        {
            Value = value;
        }
    }
}
