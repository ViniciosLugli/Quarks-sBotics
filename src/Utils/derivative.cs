class Derivative{
	float lastError;
	float derivative;

	public float sample(float error){
		derivative = error - lastError;
		lastError = derivative;
		return derivative;
	}
}
