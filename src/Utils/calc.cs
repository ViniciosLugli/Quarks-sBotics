class Calc {
	public static float constrain(float amt, float low, float high) => ((amt) < (low) ? (low) : ((amt) > (high) ? (high) : (amt)));

	public static float map(float value, float min, float max, float minTo, float maxTo) => ((((value - min) * (maxTo - minTo)) / (max - min)) + minTo);

	public static string DecToHex(int dec) {
		string hexStr = Convert.ToString(dec, 16);
		return (hexStr.Length < 2) ? ("0" + hexStr) : hexStr;
	}
	public static float toBearing(float degrees) => (degrees + 360) % 360;

	public static string repeatString(string value, int count) => new System.Text.StringBuilder(value.Length * count).Insert(0, value, count).ToString();
}
