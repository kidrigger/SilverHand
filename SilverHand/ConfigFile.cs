using Newtonsoft.Json;

namespace SilverHand
{
	[JsonObject]
	public struct ConfigFile
	{
		[JsonProperty("user_registry")]
		public string UserRegistry { get; private set; }

		[JsonProperty("character_registry")]
		public string CharacterRegistry { get; private set; }

		[JsonProperty("character_sheet_directory")]
		public string CharacterSheetDirectory { get; private set; }
	}
}
