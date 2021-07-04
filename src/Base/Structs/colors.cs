public struct Color{
	public Color(float r_, float g_, float b_){
		this.r = r_;
		this.g = g_;
		this.b = b_;
	}

	public float r;
    public float g;
	public float b;

	public float[] raw{
		get => new float[]{this.r, this.g, this.b};
	}

	public string toHex(){
		string rs = Color.DecimalToHexadecimal((int)this.r);
		string gs = Color.DecimalToHexadecimal((int)this.g);
		string bs = Color.DecimalToHexadecimal((int)this.b);

		return '#' + rs + gs + bs;
	}

	private static string DecimalToHexadecimal(int dec){
		if (dec <= 0)
			return "00";
		int hex = dec;
		string hexStr = string.Empty;
		while (dec > 0){
			hex = dec % 16;

			if (hex < 10)
				hexStr = hexStr.Insert(0, Convert.ToChar(hex + 48).ToString());
			else
				hexStr = hexStr.Insert(0, Convert.ToChar(hex + 55).ToString());

			dec /= 16;
		}
		return hexStr;
	}

	//Basic operators
	public static Color operator -(Color a, Color b) => new Color(a.r - b.r, a.g - b.g, a.b - b.b);
	public static Color operator +(Color a, Color b) => new Color(a.r + b.r, a.g + b.g, a.b + b.b);
	public static Color operator *(Color a, Color b) => new Color(a.r * b.r, a.g * b.g, a.b * b.b);
	public static Color operator /(Color a, Color b) => new Color(a.r / b.r, a.g / b.g, a.b / b.b);
}

public struct Light{
	public Light(float raw_){
		this.raw = raw_;
		this.decorator = 100;
	}

	public int decorator;
	public float raw;
	public float value {
		get => decorator-raw;
	}

	//Basic operators
	public static bool operator >(Light a, Light b) => a.value > b.value;
	public static bool operator <(Light a, Light b) => a.value < b.value;
	public static bool operator >=(Light a, Light b) => a.value >= b.value;
	public static bool operator <=(Light a, Light b) => a.value <= b.value;
	public static bool operator ==(Light a, Light b) => a.value == b.value;
	public static bool operator !=(Light a, Light b) => a.value != b.value;
	public static float operator -(Light a, Light b) => a.value - b.value;
	public static float operator +(Light a, Light b) => a.value + b.value;
	public static float operator *(Light a, Light b) => a.value * b.value;
	public static float operator /(Light a, Light b) => a.value / b.value;
}
