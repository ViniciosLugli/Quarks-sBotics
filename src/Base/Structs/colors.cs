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

	public bool hasGreen(){
		float rgb = this.r + this.g + this.b;
		byte pR = (byte)Calc.map(this.r, 0, rgb, 0, 100);
		byte pG = (byte)Calc.map(this.g, 0, rgb, 0, 100);
		byte pB = (byte)Calc.map(this.b, 0, rgb, 0, 100);
		return ((pG > pR) && (pG > pB) && (pG > 65));
	}

	public string toHex(){
		float rgb = this.r + this.g + this.b;
		byte pR = (byte)Calc.map(this.r, 0, rgb, 0, 255);
		byte pG = (byte)Calc.map(this.g, 0, rgb, 0, 255);
		byte pB = (byte)Calc.map(this.b, 0, rgb, 0, 255);
		string rs = Calc.DecToHex((int)(pR));
		string gs = Calc.DecToHex((int)(pG));
		string bs = Calc.DecToHex((int)(pB));

		return '#' + rs + gs + bs;
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

	public string toHex(){
		string grayscaleHex = Calc.DecToHex((int)(this.raw));
		return '#' + grayscaleHex + grayscaleHex + grayscaleHex;
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
