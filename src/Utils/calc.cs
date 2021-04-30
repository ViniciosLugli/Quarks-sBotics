class Calc{
	public static float constrain(float amt,float low,float high){
		return ((amt)<(low)?(low):((amt)>(high)?(high):(amt)));
	}

	public static float convert_degress(float degreesToConvert){
		return (degreesToConvert + 360) % 360;
	}

	public static float map(float value, float min, float max, float minTo, float maxTo){
		return (int)( (((value - min) * (maxTo - minTo)) / (max - min)) + minTo);
	}
};
