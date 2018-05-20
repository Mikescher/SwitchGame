using Newtonsoft.Json;

namespace SwitchGame.Content.Pipeline.TextureAtlases
{
	/// <summary>
	/// 
	/// This class comes originally from [MonoGame.Extended](https://github.com/craftworkgames/MonoGame.Extended/commit/f62467f)
	/// (MIT License)
	/// 
	/// @author MonoGame.Extended
	/// 
	/// </summary>
	public class TexturePackerMeta
	{
		[JsonProperty("app")]
		public string App { get; set; }

		[JsonProperty("version")]
		public string Version { get; set; }

		[JsonProperty("image")]
		public string Image { get; set; }

		[JsonProperty("format")]
		public string Format { get; set; }

		[JsonProperty("size")]
		public TexturePackerSize Size { get; set; }

		[JsonProperty("scale")]
		public float Scale { get; set; }

		[JsonProperty("smartupdate")]
		public string SmartUpdate { get; set; }

		public override string ToString()
		{
			return Image;
		}

	}
}