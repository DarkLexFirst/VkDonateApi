using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VkDonateApi
{
	public abstract class BaseResponse
	{
		[JsonProperty]
		internal bool Success { get; set; }
		[JsonProperty]
		internal string Text { get; set; }

		public static T FromJson<T>(string json) where T : BaseResponse
		{
			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			return JsonConvert.DeserializeObject<T>(json, settings);
		}
	}
}
