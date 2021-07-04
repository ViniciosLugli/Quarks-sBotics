class Calc{
	public static float constrain(float amt,float low,float high) => ((amt)<(low)?(low):((amt)>(high)?(high):(amt)));

	public static float map(float value, float min, float max, float minTo, float maxTo) => ((((value - min) * (maxTo - minTo)) / (max - min)) + minTo);
}