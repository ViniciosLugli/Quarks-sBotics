public static class Formatter{
	public static string parse(string data, string[] offsets){
		foreach(string tag in offsets){
			data = Formatter.marker(data, tag);
		}
		return data;
	}

	private static string marker(string data_, string tag){
		string tag_ = (tag.Contains("=")) ? (tag.Split('=')[0] == "align")? $"" : $"</{tag.Split('=')[0]}>" : $"</{tag}>";
		return $"<{tag}>{data_}{tag_}";
	}
}
