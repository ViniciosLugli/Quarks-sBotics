public static class Robot{
	//Private robot info. current robo-3:
	public const byte kLights = 5;//number of sensors
	public const byte kUltrassonics = 3;//number of sensors
	public const byte kRefreshRate = 63;//ms of refresh rate in color/light sensor
	public const byte kSize = 56;
	public const byte kDiffFrontalDistance = 20;
	public const float kErrorDelta = 0.70f;
	//

	public static void throwError(object message) => bc.RobotError(message.ToString());
	public static void throwError() => bc.RobotError();
	public static void endCode() => bc.CodeEnd();
}
