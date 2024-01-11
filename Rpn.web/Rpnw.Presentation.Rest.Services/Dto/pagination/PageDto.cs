using Newtonsoft.Json;

namespace Rpnw.Presentation.Rest.Services.Dto.pagination
{
	public class PageDto
	{
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public int ItemCount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalPages { get; set; }

    }
}
