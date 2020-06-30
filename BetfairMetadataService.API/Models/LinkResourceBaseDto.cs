using System.Collections.Generic;

namespace BetfairMetadataService.API.Models
{
    public class LinkResourceBaseDto
    {
        public LinkResourceBaseDto()
        {

        }
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();

        public bool ShouldSerializeLinks()
        {
            return Links != null && Links.Count>0;
        }
    }
}
