public static class Robot{
	//Private robot info. current robo-3:
	public const byte kLights = 3;//number of sensors
	public const byte kUltrassonics = 2;//number of sensors
	public const byte kRefreshRate = 31;//ms of refresh rate in color/light sensor
	//

	public static void throwError(object message) => bc.RobotError(message.ToString());
	public static void throwError() => bc.RobotError();
	public static void endCode() => bc.CodeEnd();
}
