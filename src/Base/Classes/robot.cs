public static class Robot{
	//Private robot info. current robo-3:
	public const byte kLights = 6;//number of sensors
	public const byte kUltrassonics = 3;//number of sensors
	public const byte kRefreshRate = 75;//ms of refresh rate in color/light sensor
	public const byte ksize = 55;
	//

	public static void throwError(object message) => bc.RobotError(message.ToString());
	public static void throwError() => bc.RobotError();
	public static void endCode() => bc.CodeEnd();
}
