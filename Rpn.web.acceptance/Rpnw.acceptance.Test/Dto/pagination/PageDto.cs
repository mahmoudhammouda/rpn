using Newtonsoft.Json;

namespace Rpnw.acceptance.Test
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
