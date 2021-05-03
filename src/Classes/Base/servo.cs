class Servo{
	public static void move(float left, float right) => bc.MoveFrontal(left, right);

	public static void foward(float velocity) => bc.MoveFrontal(velocity, velocity);

	public static void rotate(float angle, float velocity=500) => bc.MoveFrontalAngles(velocity, angle);
	public static void rotate(Degrees angle, float velocity=500) => bc.MoveFrontalAngles(velocity, angle.raw);

	public static void encoder(int rotations, float velocity=300) => bc.MoveFrontalRotations(velocity, rotations);

	public static float speed() => bc.RobotSpeed();
}
