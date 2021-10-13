//Base for robot and utils
//Data ---------------------------------------------
static Sound sTurnNotGreen = new Sound("F2", 80);
static Sound sTurnGreen = new Sound("C3", 60);
static Sound sFakeGreen = new Sound("G", 100);
static Sound sAlertOffline = new Sound("D#", 100);
static Sound sFindLine = new Sound("C1", 50);
static Sound sMultiplesCross = new Sound("E", 80);
static Sound sRescueFindArea = new Sound("C2", 120);
static Sound sLifting = new Sound("G", 120);

static Color cFollowLine = new Color(255, 255, 255);
static Color cTurnNotGreen = new Color(0, 0, 0);
static Color cTurnGreen = new Color(0, 255, 0);
static Color cFakeGreen = new Color(255, 255, 0);
static Color cAlertOffline = new Color(255, 0, 0);
static Color cRampFollowLine = new Color(255, 0, 255);

static Direction cLeftMovement = new Direction(300, -70);
static Direction cRightMovement = new Direction(-70, 300);
static float cDegreesMovementProp = 3.5f;

int SETUPTIME = Time.current.millis;

static byte UNIQUEID = 0;
public delegate void ActionHandler();

public static class Actions{
	public static void Empty(){}
}
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
public static class Formatter {
	public static string parse(string data, string[] offsets) {
		foreach (string tag in offsets) {
			data = Formatter.marker(data, tag);
		}
		return data;
	}

	private static string marker(string data_, string tag) {
		string tag_ = (tag.Contains("=")) ? (tag.Split('=')[0] == "align") ? $"" : $"</{tag.Split('=')[0]}>" : $"</{tag}>";
		return $"<{tag}>{data_}{tag_}";
	}
}
public static class Log {
	public static void proc(object local, object process) => bc.Print(0, Formatter.parse($"{Formatter.parse(local.ToString(), new string[] { "color=#FF6188", "b" })} {Formatter.parse(process.ToString(), new string[] { "color=#947BAF", "b" })}", new string[] { "align=center" }));
	public static void proc() {
		var methodInfo = (new StackTrace()).GetFrame(1).GetMethod();
		bc.Print(0, Formatter.parse($"{Formatter.parse(methodInfo.ReflectedType.Name, new string[] { "color=#FF6188", "b" })} {Formatter.parse(methodInfo.Name, new string[] { "color=#947BAF", "b" })}", new string[] { "align=center" }));
	}

	public static void info(object data) => bc.Print(1, Formatter.parse(data.ToString(), new string[] { "align=center" }));

	public static void debug(object data) => bc.Print(2, Formatter.parse(data.ToString(), new string[] { "b", "color=#404040", "align=center" }));

	public static void custom(byte line, object data) => bc.Print((int)line, Formatter.parse(data.ToString(), new string[] { "align=center" }));

	public static void clear() => bc.ClearConsole();
}
public static class Robot {
	//Private robot info. current robo-3:
	public const byte kLights = 5;//number of sensors
	public const byte kUltrassonics = 3;//number of sensors
	public const byte kRefreshRate = 63;//ms of refresh rate in color/light sensor
	public const byte kSize = 56;
	public const byte kDiffFrontalDistance = 20;
	public const float kErrorDelta = 0.60f;
	//

	public static void throwError(object message) => bc.RobotError(message.ToString());
	public static void throwError() => bc.RobotError();
	public static void endCode() => bc.CodeEnd();
}
public struct Clock {
	public Clock(int millis_) {
		this.millis = millis_;
	}

	public float sec {
		get => (float)(this.millis / 1000);
	}

	public int millis;

	public uint micros {
		get => (uint)(this.millis * 1000);
	}

	//Basic operators
	public static bool operator >(Clock a, Clock b) => a.millis > b.millis;
	public static bool operator <(Clock a, Clock b) => a.millis < b.millis;
	public static bool operator >=(Clock a, Clock b) => a.millis >= b.millis;
	public static bool operator <=(Clock a, Clock b) => a.millis <= b.millis;
	public static int operator -(Clock a, Clock b) => a.millis - b.millis;
	public static int operator -(Clock a, int b) => a.millis - b;
	public static int operator +(Clock a, Clock b) => a.millis + b.millis;
	public static int operator *(Clock a, Clock b) => a.millis * b.millis;
	public static int operator /(Clock a, Clock b) => a.millis / b.millis;
}

public static class Time {
	public static Clock current {
		get => new Clock(bc.Millis());
	}

	static public Clock timer {
		get => new Clock(bc.Timer());
	}
	public static long currentUnparsed {
		get => DateTimeOffset.Now.ToUnixTimeMilliseconds();
	}

	public static void resetTimer() => bc.ResetTimer();

	public static void sleep(int ms) => bc.Wait(ms);
	public static void sleep(int ms, ActionHandler callwhile) {
		int toWait = Time.current.millis + ms;
		while (Time.current.millis < toWait) { callwhile(); }
	}
	public static void sleep(Clock clock) => bc.Wait(clock.millis);

	public static void debug() => bc.Wait(123456789);

	public static void skipFrame() => bc.Wait(17);

	public static string date {
		get => string.Format("{0:HH:mm:ss.fff}", DateTime.Now);
	}
};
public struct Action {
	public Action(bool raw_) {
		this.raw = raw_;
	}

	public bool pressed {
		get => this.raw;
	}

	public bool raw;
}

public class Button {
	private byte SensorIndex = 1;

	public Button(byte SensorIndex_) {
		this.SensorIndex = SensorIndex_;
	}

	public Action state {
		get => new Action(bc.Touch((int)this.SensorIndex));
	}

	public void NOP() {
		Log.clear();
		Log.proc();
		bc.Touch((int)this.SensorIndex);
	}
}
public static class Led {
	public static void on(byte r, byte g, byte b) => bc.TurnLedOn(r, g, b);

	public static void on(Color color) => bc.TurnLedOn((int)color.r, (int)color.g, (int)color.b);

	public static void off() => bc.TurnLedOff();
}
public struct Sound {
	public Sound(string note_, int time_) {
		this.note = note_;
		this.time = time_;
	}

	public string note;
	public int time;
}

public static class Buzzer {
	public static void play(string note, int time = 100, int index = 0) => bc.PlayNote(index, note, time);
	public static void play(Sound sound, int index = 0) => bc.PlayNote(index, sound.note, sound.time);

	public static void stop() => bc.StopSound(0);
}
public static class Pencil {
	public static void start() => bc.Draw();

	public static void stop() => bc.StopDrawing();

	public static void color(int r, int g, int b) => bc.ChangePencilColor(r, g, b);
	public static void color(Color color) => bc.ChangePencilColor((int)color.r, (int)color.g, (int)color.b);
}
public struct Celsius {
	public Celsius(float raw_) {
		this.raw = raw_;
	}

	public float raw;

	//Basic operators
	public static bool operator >(Celsius a, Celsius b) => a.raw > b.raw;
	public static bool operator <(Celsius a, Celsius b) => a.raw < b.raw;
	public static bool operator >=(Celsius a, Celsius b) => a.raw >= b.raw;
	public static bool operator <=(Celsius a, Celsius b) => a.raw <= b.raw;
	public static float operator -(Celsius a, Celsius b) => a.raw - b.raw;
	public static float operator +(Celsius a, Celsius b) => a.raw + b.raw;
	public static float operator *(Celsius a, Celsius b) => a.raw * b.raw;
	public static float operator /(Celsius a, Celsius b) => a.raw / b.raw;
}

public static class Temperature {
	public static Celsius celsius {
		get => new Celsius((float)bc.Heat());
	}

	public static bool victimAlive {
		get => bc.Heat() > 35;
	}

	public static void NOP() {
		Log.clear();
		Log.proc();
		bc.Heat();
	}
}
public struct Degrees {
	public Degrees(float raw_) {
		this.raw = (raw_ + 360) % 360;
	}

	public float raw;

	//Basic operators
	public static bool operator >(Degrees a, Degrees b) => a.raw > b.raw;
	public static bool operator <(Degrees a, Degrees b) => a.raw < b.raw;
	public static bool operator >=(Degrees a, Degrees b) => a.raw >= b.raw;
	public static bool operator <=(Degrees a, Degrees b) => a.raw <= b.raw;
	public static float operator -(Degrees a, Degrees b) => ((a.raw - b.raw) + 360) % 360;
	public static float operator +(Degrees a, Degrees b) => ((a.raw + b.raw) + 360) % 360;
	public static float operator *(Degrees a, Degrees b) => ((a.raw * b.raw) + 360) % 360;
	public static float operator /(Degrees a, Degrees b) => ((a.raw / b.raw) + 360) % 360;
	public static bool operator %(Degrees a, Degrees b) => (a.raw + 1 > b.raw) && (a.raw - 1 < b.raw);
}

private struct DegreesRange {
	public DegreesRange(float min_, float max_) {
		this.min = new Degrees(min_);
		this.max = new Degrees(max_);
	}
	public Degrees min, max;

	public bool isOnRange(Degrees currentGyro) => (currentGyro > this.min) && (currentGyro < this.max);
}

public static class Gyroscope {

	public static Degrees[] points = new Degrees[] { new Degrees(359), new Degrees(0), new Degrees(90), new Degrees(180), new Degrees(270) };
	public static Degrees[] diagonals = new Degrees[] { new Degrees(45), new Degrees(135), new Degrees(225), new Degrees(315) };

	public static Degrees x {
		get => new Degrees((float)bc.Compass());
	}
	public static Degrees z {
		get => new Degrees((float)bc.Inclination());
	}

	public static bool inPoint(bool angExpand = true, float offset = 8) {
		if (angExpand) {
			foreach (Degrees point in Gyroscope.points) {
				if (((Gyroscope.x.raw + offset) >= point.raw) && (Gyroscope.x.raw - offset <= point.raw)) {
					return true;
				}
			}
			return false;
		} else {
			foreach (Degrees point in Gyroscope.points) {
				if (Gyroscope.x % point) {
					return true;
				}
			}
			return false;
		}
	}

	public static bool inDiagonal(bool angExpand = true, float offset = 8) {
		if (angExpand) {
			foreach (Degrees diagonal in Gyroscope.diagonals) {
				if (((Gyroscope.x.raw + offset) >= diagonal.raw) && (Gyroscope.x.raw - offset <= diagonal.raw)) {
					return true;
				}
			}
			return false;
		} else {
			foreach (Degrees diagonal in Gyroscope.diagonals) {
				if (Gyroscope.x % diagonal) {
					return true;
				}
			}
			return false;
		}
	}

	public static float? inRawPoint(bool angExpand = true, float offset = 8) {
		if (angExpand) {
			foreach (Degrees point in Gyroscope.points) {
				if (((Gyroscope.x.raw + offset) >= point.raw) && (Gyroscope.x.raw - offset <= point.raw)) {
					return point.raw;
				}
			}
		} else {
			foreach (Degrees point in Gyroscope.points) {
				if (Gyroscope.x % point) {
					return point.raw;
				}
			}
		}
		return null;
	}

	public static float? inRawDiagonal(bool angExpand = true, float offset = 8) {
		if (angExpand) {
			foreach (Degrees diagonal in Gyroscope.diagonals) {
				if (((Gyroscope.x.raw + offset) >= diagonal.raw) && (Gyroscope.x.raw - offset <= diagonal.raw)) {
					return diagonal.raw;
				}
			}
		} else {
			foreach (Degrees diagonal in Gyroscope.diagonals) {
				if (Gyroscope.x % diagonal) {
					return diagonal.raw;
				}
			}
		}
		return null;
	}

	public static bool isLifted() => Gyroscope.z.raw > 300 && Gyroscope.z.raw < 358;

	public static void NOP() {
		Log.clear();
		Log.proc();
		bc.Compass();
		bc.Inclination();
	}
}
public struct Color {
	public Color(float r_, float g_, float b_) {
		this.r = r_;
		this.g = g_;
		this.b = b_;

		this.pR = 0;
		this.pG = 0;
		this.pB = 0;
	}

	public float r;
	public float g;
	public float b;

	private byte pR;
	private byte pG;
	private byte pB;

	public float[] raw {
		get => new float[] { this.r, this.g, this.b };
	}

	private void updatePorcentRGB() {
		float rgb = this.r + this.g + this.b;
		this.pR = (byte)Calc.map(this.r, 0, rgb, 0, 100);
		this.pG = (byte)Calc.map(this.g, 0, rgb, 0, 100);
		this.pB = (byte)Calc.map(this.b, 0, rgb, 0, 100);
	}

	public string showPorcentRGB() {
		this.updatePorcentRGB();
		return $"R: {this.pR} | G: {this.pG} | B: {this.pB}";
	}

	public bool hasGreen() {
		this.updatePorcentRGB();
		return ((this.pG > this.pR) && (this.pG > this.pB) && (this.pG > 65));
	}

	public bool hasKit() {
		this.updatePorcentRGB();
		return ((this.pR < 20) && (this.pB > this.pR) && (this.pB > this.pG) && (this.pB > 40));
	}

	public string toHex() {
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

public struct Light {
	public Light(float raw_) {
		this.raw = raw_;
		this.decorator = 100;
	}

	public int decorator;
	public float raw;
	public float value {
		get => decorator - raw;
	}

	public string toHex() {
		string grayscaleHex = Calc.DecToHex((int)(this.raw));
		return '#' + grayscaleHex + grayscaleHex + grayscaleHex;
	}

	//Basic operators
	public static bool operator >(Light a, Light b) => a.value > b.value;
	public static bool operator <(Light a, Light b) => a.value < b.value;
	public static bool operator >=(Light a, Light b) => a.value >= b.value;
	public static bool operator <=(Light a, Light b) => a.value <= b.value;
	public static float operator -(Light a, Light b) => a.value - b.value;
	public static float operator +(Light a, Light b) => a.value + b.value;
	public static float operator *(Light a, Light b) => a.value * b.value;
	public static float operator /(Light a, Light b) => a.value / b.value;
}

public class Reflective {
	private byte SensorIndex = 0;

	public Reflective(byte SensorIndex_) {
		this.SensorIndex = SensorIndex_;
	}

	public Light light {
		get => new Light(bc.Lightness((int)this.SensorIndex));
	}

	public Color rgb {
		get => new Color(
				bc.ReturnRed((int)this.SensorIndex),
				bc.ReturnGreen((int)this.SensorIndex),
				bc.ReturnBlue((int)this.SensorIndex)
			);
	}
	public bool hasLine() => bc.ReturnRed((int)this.SensorIndex) < 50 && bc.ReturnGreen((int)this.SensorIndex) < 50 && bc.Lightness((int)this.SensorIndex) < 52;

	public bool isMat() => bc.ReturnRed((int)this.SensorIndex) > 45 && bc.ReturnBlue((int)this.SensorIndex) < 20 && bc.ReturnGreen((int)this.SensorIndex) < 20;

	public bool isEndLine() => bc.ReturnRed((int)this.SensorIndex) > 60 && bc.ReturnBlue((int)this.SensorIndex) < 24 && bc.ReturnGreen((int)this.SensorIndex) < 24;

	public bool isRescueEnter() => (bc.ReturnBlue((int)this.SensorIndex) > (bc.ReturnRed((int)this.SensorIndex) + 4)) && (bc.ReturnBlue((int)this.SensorIndex) > (bc.ReturnGreen((int)this.SensorIndex) + 4));

	public bool isRescueExit() => (bc.ReturnGreen((int)this.SensorIndex) > (bc.ReturnRed((int)this.SensorIndex) + 4)) && (bc.ReturnGreen((int)this.SensorIndex) > (bc.ReturnBlue((int)this.SensorIndex) + 4));

	public bool isTriangle() => bc.ReturnRed((int)this.SensorIndex) < 5 && bc.ReturnBlue((int)this.SensorIndex) < 5 && bc.ReturnGreen((int)this.SensorIndex) < 5;

	public bool isLiveVictim() => bc.ReturnRed((int)this.SensorIndex) > 55 && bc.ReturnBlue((int)this.SensorIndex) > 55 && bc.ReturnGreen((int)this.SensorIndex) > 16;

	public bool isDeadVictim() => bc.ReturnRed((int)this.SensorIndex) < 16 && bc.ReturnBlue((int)this.SensorIndex) < 16 && bc.ReturnGreen((int)this.SensorIndex) < 16;

	public bool isColored() {
		for (int i = 0; i < 4; i++) {
			float r = bc.ReturnRed((int)this.SensorIndex);
			float b = bc.ReturnBlue((int)this.SensorIndex);
			if (!((r + 3) > b && (r - 3) < b)) {
				return true;
			}
		}
		return false;
	}

	public void NOP() {
		Log.clear();
		Log.proc();
		bc.Lightness((int)this.SensorIndex);
		bc.ReturnRed((int)this.SensorIndex);
		bc.ReturnGreen((int)this.SensorIndex);
		bc.ReturnBlue((int)this.SensorIndex);
	}
}
public struct Distance {
	public Distance(float raw_) {
		this.raw = raw_;
	}

	public float raw;

	public int toRotations() => (int)(this.raw / 2);

	public float fromCenter() => this.raw - Robot.kDiffFrontalDistance;

	//Basic operators
	public static bool operator >(Distance a, Distance b) => a.raw > b.raw;
	public static bool operator <(Distance a, Distance b) => a.raw < b.raw;
	public static bool operator >=(Distance a, Distance b) => a.raw >= b.raw;
	public static bool operator <=(Distance a, Distance b) => a.raw <= b.raw;
	public static float operator -(Distance a, Distance b) => a.raw - b.raw;
	public static float operator +(Distance a, Distance b) => a.raw + b.raw;
	public static float operator *(Distance a, Distance b) => a.raw * b.raw;
	public static float operator /(Distance a, Distance b) => a.raw / b.raw;
}

public class Ultrassonic {
	private byte SensorIndex = 0;

	public Ultrassonic(byte SensorIndex_) {
		this.SensorIndex = SensorIndex_;
	}

	public Distance distance {
		get => new Distance(bc.Distance((int)this.SensorIndex));
	}

	public void NOP() {
		Log.clear();
		Log.proc();
		bc.Distance((int)this.SensorIndex);
	}
}
public static class Actuator {
	public static void position(float degrees, int velocity = 150) {
		Log.clear();
		bc.ActuatorSpeed(velocity);

		int timeout = Time.current.millis + (3000 - (velocity * 10));
		float local_angle = bc.AngleActuator();

		degrees = (degrees < 0 || degrees > 300) ? 0 : (degrees > 88) ? 88 : degrees;

		Log.proc();

		if (degrees > local_angle) {
			while (degrees > local_angle) {
				bc.ChangeAngleActuatorUp();
				if (Time.current.millis > timeout) { return; }
				local_angle = bc.AngleActuator();
				//Log.info($"local_angle: {local_angle}");
			}
		} else if (degrees < local_angle) {
			while (degrees < local_angle) {
				bc.ChangeAngleActuatorDown();
				if (Time.current.millis > timeout) { return; }
				local_angle = bc.AngleActuator();
				//Log.info($"local_angle: {local_angle}");
			}
		}
		bc.ActuatorUp(1);
	}

	public static void angle(float degrees, int velocity = 150) {
		Log.clear();
		bc.ActuatorSpeed(velocity);

		int timeout = Time.current.millis + (2000 - (velocity * 10));
		float local_angle = bc.AngleScoop();

		degrees = (degrees < 0 || degrees > 300) ? 0 : (degrees > 12) ? 12 : degrees;

		Log.proc();

		if (degrees > local_angle) {
			while (degrees > local_angle) {
				bc.TurnActuatorDown(31);
				if (Time.current.millis > timeout) { return; }
				local_angle = bc.AngleScoop();
				//Log.info($"local_angle: {local_angle}");
			}
		} else if (degrees < local_angle) {
			while (degrees < local_angle) {
				bc.TurnActuatorUp(31);
				if (Time.current.millis > timeout) { return; }
				local_angle = bc.AngleScoop();
				//Log.info($"local_angle: {local_angle}");
			}
		}
		bc.TurnActuatorUp(1);
	}

	public static bool victim {
		get => bc.HasVictim();
	}

	public static bool kit {
		get => bc.HasRescueKit();
	}

	public static void setVelocity(int vel) => bc.ActuatorSpeed(vel);

	public static void open() {
		Log.clear();
		Log.proc();
		bc.OpenActuator();
	}

	public static void close() {
		Log.clear();
		Log.proc();
		bc.CloseActuator();
	}

	public static void alignUp() {
		position(88);
		angle(0);
	}
	public static void alignDown() {
		position(0);
		angle(0);
	}

	public static void dropVictim() {
		position(0);
		angle(10);
	}

	public static void NOP() {
		Log.clear();
		Log.proc();
		bc.AngleActuator();
		bc.AngleScoop();
	}
}
public struct Direction {
	public Direction(int left_, int right_) {
		this.left = left_;
		this.right = right_;
	}

	public int left, right;
}

public static class Servo {
	public static void move(Direction direction) => bc.Move(direction.left, direction.right);
	public static void move(float left = 300, float right = 300) => bc.Move(left, right);

	public static void forward(float velocity = 300) => bc.Move(Math.Abs(velocity), Math.Abs(velocity));

	public static void backward(float velocity = 300) => bc.Move(-velocity, -velocity);

	public static void left(float velocity = 1000) => bc.Move(-velocity, +velocity);

	public static void right(float velocity = 1000) => bc.Move(+velocity, -velocity);


	public static void rotate(float angle, float velocity = 1000) {
		Degrees alignLocal = new Degrees(Gyroscope.x.raw + angle);
		if (angle > 0) {
			Servo.right(velocity);
		} else {
			Servo.left(velocity);
		}
		while (!(Gyroscope.x % alignLocal)) { }
		Servo.stop();
	}
	public static void rotate(Degrees angle, float velocity = 1000) {
		Degrees alignLocal = new Degrees(Gyroscope.x.raw + angle.raw);
		if (angle.raw > 0) {
			Servo.right(velocity);
		} else {
			Servo.left(velocity);
		}
		while (!(Gyroscope.x % alignLocal)) { }
		Servo.stop();
	}

	public static void encoder(float rotations, float velocity = 300) => bc.MoveFrontalRotations(rotations > 0 ? velocity : -velocity, Math.Abs(rotations));

	public static float speed() => bc.RobotSpeed();

	public static void stop() => bc.Move(0, 0);

	public static void antiLifting() {
		if ((Gyroscope.isLifted() || !Gyroscope.inPoint(true, 6)) && Time.timer.millis > 312) {
			Log.proc();
			Buzzer.play(sLifting);
			Servo.stop();
			int timeout = Time.current.millis + 378;
			while (Gyroscope.isLifted() && Time.current.millis < timeout) {
				Servo.backward(200);
			}
			Time.sleep(128);
			Servo.stop();
			Servo.alignNextAngle();
			Time.resetTimer();
		}
	}

	public static void antiLiftingRescue() {
		if ((Gyroscope.isLifted() || !Gyroscope.inDiagonal()) && Time.timer.millis >= 312) {
			Log.proc();
			Buzzer.play(sLifting);
			Servo.stop();
			int timeout = Time.current.millis + 312;
			while (Gyroscope.isLifted() && Time.current.millis < timeout) {
				Servo.backward(200);
			}
			Time.sleep(128);
			Servo.stop();
			Time.resetTimer();
		}
	}

	public static void nextAngleRightDiagonal(byte ignoreAngles = 0) {
		Log.proc();
		Servo.rotate(Math.Abs(ignoreAngles));
		Servo.right();
		while (!Gyroscope.inDiagonal(false)) { }
		Servo.stop();
	}

	public static void nextAngleLeftDiagonal(byte ignoreAngles = 0) {
		Log.proc();
		Servo.rotate(-ignoreAngles);
		Servo.left();
		while (!Gyroscope.inDiagonal(false)) { }
		Servo.stop();
	}

	public static void nextAngleRight(byte ignoreAngles = 0) {
		Log.proc();
		Servo.rotate(Math.Abs(ignoreAngles));
		Servo.right();
		while (!Gyroscope.inPoint(false)) { }
		Servo.stop();
	}

	public static void nextAngleLeft(byte ignoreAngles = 0) {
		Log.proc();
		Servo.rotate(-ignoreAngles);
		Servo.left();
		while (!Gyroscope.inPoint(false)) { }
		Servo.stop();
	}

	public static void alignNextAngle() {
		Log.proc();
		if (Gyroscope.inPoint(true, 2)) { return; }
		Degrees alignLocal = new Degrees(0);
		if ((Gyroscope.x.raw > 315) || (Gyroscope.x.raw <= 45)) {
			alignLocal = new Degrees(0);
		} else if ((Gyroscope.x.raw > 45) && (Gyroscope.x.raw <= 135)) {
			alignLocal = new Degrees(90);
		} else if ((Gyroscope.x.raw > 135) && (Gyroscope.x.raw <= 225)) {
			alignLocal = new Degrees(180);
		} else if ((Gyroscope.x.raw > 225) && (Gyroscope.x.raw <= 315)) {
			alignLocal = new Degrees(270);
		}

		Log.info(Formatter.parse($"Align to {alignLocal.raw}°", new string[] { "i", "color=#505050" }));

		if ((alignLocal.raw == 0) && (Gyroscope.x.raw > 180)) {
			Servo.right();
		} else if ((alignLocal.raw == 0) && (Gyroscope.x.raw < 180)) {
			Servo.left();
		} else if (Gyroscope.x < alignLocal) {
			Servo.right();
		} else if (Gyroscope.x > alignLocal) {
			Servo.left();
		}
		while (!(Gyroscope.x % alignLocal)) { }
		Servo.stop();
	}

	public static void alignToAngle(object angle) {
		Log.proc();

		Degrees alignLocal = (angle is Degrees) ? (Degrees)angle : new Degrees((float)angle);

		Log.info(Formatter.parse($"Align to {alignLocal.raw}°", new string[] { "i", "color=#505050" }));

		float baseFind = Calc.toBearing(Gyroscope.x - alignLocal);

		if (baseFind >= 180) {
			Servo.right();
		} else if (baseFind < 180) {
			Servo.left();
		}
		while (!(Gyroscope.x % alignLocal)) { }
		Servo.stop();
	}

	public static bool SmoothAlignNextAngle(FloorRoute.FollowLine Follower) {

		if (Gyroscope.inPoint(true, 1.5f) || Gyroscope.inDiagonal(true, 1.5f)) { return false; }

		Degrees alignLocal = new Degrees(0);

		float? temp = Gyroscope.inRawPoint(true, 22);

		if (temp is null) {
			temp = Gyroscope.inRawDiagonal(true, 10);
			if (temp is null) {
				return false;
			}
		}
		alignLocal = new Degrees((float)temp);

		float diffDegrees = Math.Abs(alignLocal.raw - Gyroscope.x.raw);

		Log.debug(Formatter.parse($"Smooth to {alignLocal.raw}° | Diff: {diffDegrees}°", new string[] { "i", "color=#505050" }));

		diffDegrees = diffDegrees * cDegreesMovementProp;

		void leftMove() {
			Direction filtred = cLeftMovement;
			if (diffDegrees > 16) {
				filtred.right = (int)(filtred.right - 60);
				filtred.left = (int)(filtred.left);
			} else {
				filtred.right = (int)(filtred.right - diffDegrees);
			}
			Follower.moveVelocity = filtred;
		}

		void rightMove() {
			Direction filtred = cRightMovement;
			if (diffDegrees > 16) {
				filtred.left = (int)(filtred.left - 60);
				filtred.right = (int)(filtred.right);
			} else {
				filtred.left = (int)(filtred.left - diffDegrees);
			}
			Follower.moveVelocity = filtred;
		}

		if ((alignLocal.raw == 0) && (Gyroscope.x.raw > 180)) {
			leftMove();
		} else if ((alignLocal.raw == 0) && (Gyroscope.x.raw < 180)) {
			rightMove();
		} else if (Gyroscope.x < alignLocal) {
			leftMove();
		} else if (Gyroscope.x > alignLocal) {
			rightMove();
		} else {
			Log.proc();
			Log.info(Formatter.parse("Oh fuck 2", new string[] { "i", "color=#505050" }));
		}
		return true;
	}

	private static bool ultraGoToRecursive(Ultrassonic ultra, ActionHandler callback) {
		Log.info(Formatter.parse($"ultra: {ultra.distance.raw}, speed: {Servo.speed()}", new string[] { "i", "color=#505050" }));
		Servo.antiLifting();
		if (Servo.speed() < 0.5f && Time.timer.millis > 500) {
			callback?.Invoke();
			return true;
		}
		return false;
	}

	public static void ultraGoTo(float position, ref Ultrassonic ultra, ActionHandler callback = null, int velocity = 200) {
		Log.proc();
		Time.resetTimer();
		if (position > ultra.distance.raw) {
			while (position > ultra.distance.raw) {
				if (ultraGoToRecursive(ultra, callback)) { break; }
				Servo.backward(velocity);
			}
		} else {
			while (position < ultra.distance.raw) {
				if (ultraGoToRecursive(ultra, callback)) { break; }
				Servo.forward(velocity);
			}
		}
		Servo.stop();
		Log.clear();
	}

	public static void ultraGoTo(Distance dist, ref Ultrassonic ultra, ActionHandler callback = null, int velocity = 200) {
		Log.proc();
		Time.resetTimer();
		if (dist > ultra.distance) {
			while (dist > ultra.distance) {
				if (ultraGoToRecursive(ultra, callback)) { break; }
				Servo.backward(velocity);
			}
		} else {
			while (dist < ultra.distance) {
				if (ultraGoToRecursive(ultra, callback)) { break; }
				Servo.forward(velocity);
			}
		}
		Servo.stop();
		Log.clear();
	}
}
public struct Vector2 {
	public float x;
	public float y;

	public Vector2(float x, float y) {
		this.x = x;
		this.y = y;
	}

	public static Vector2 operator +(Vector2 _v1, Vector2 _v2) => new Vector2(_v1.x + _v2.x, _v1.y + _v2.y);

	public static Vector2 operator -(Vector2 _v1, Vector2 _v2) => new Vector2(_v1.x - _v2.x, _v1.y - _v2.y);

	public static Vector2 operator *(Vector2 _v1, float m) => new Vector2(_v1.x * m, _v1.y * m);

	public static Vector2 operator /(Vector2 _v1, float d) => new Vector2(_v1.x / d, _v1.y / d);

	public static float distance(Vector2 _v1, Vector2 _v2) => (float)Math.Sqrt(Math.Pow(_v1.x - _v2.x, 2) + Math.Pow(_v1.y - _v2.y, 2));

	public float distanceFrom(Vector2 _v) => (float)Math.Sqrt(Math.Pow(this.x - _v.x, 2) + Math.Pow(this.y - _v.y, 2));

	public float length() => (float)Math.Sqrt(x * x + y * y);
}


//Modules for competition challenges
//import("Modules/AI/ai.cs");
public static class FloorRoute {
	public class FollowLine {
		public FollowLine(ref Reflective refs1_, ref Reflective refs2_, int defaultVelocity_) {
			this.s1 = refs1_;
			this.s2 = refs2_;
			this.defaultVelocity = defaultVelocity_;
			this.moveVelocity = new Direction(defaultVelocity_, defaultVelocity_);
		}
	
		public Reflective s1, s2;
		public int defaultVelocity = 0;
		public Direction moveVelocity = new Direction(0, 0);
		public Clock lastGreen = new Clock(0);
		public Clock lastCrossPath = new Clock(0);
	
		private void debugSensors() => Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw}", new string[] { "align=center", "color=#FFEA79", "b" }));
	
		public void resetMovement() => this.moveVelocity = new Direction(this.defaultVelocity, this.defaultVelocity);
	
		public void proc() {
			Log.proc();
			this.debugSensors();
			this.checkEndLine();
	
			if (mainObstacle.verify(this)) { return; }
	
			if (Green.verify(this)) { return; }
	
			if (checkSensor(ref this.s1, () => Servo.left(), () => CrossPath.findLineLeft(this))) {
				return;
			} else if (checkSensor(ref this.s2, () => Servo.right(), () => CrossPath.findLineRight(this))) {
				return;
			} else {
				Servo.move(this.moveVelocity);
				Security.verify(this);
				RescueKit.verify(this);
			}
		}
	
		public void checkEndLine() {
			if (this.s1.isEndLine() || this.s2.isEndLine()) {
				Servo.stop();
				Servo.encoder(2);
				if (this.s1.isEndLine() || this.s2.isEndLine()) {
					Servo.stop();
					Servo.encoder(7);
					Log.clear();
					Led.on(255, 0, 0);
					Log.custom(0, Formatter.parse($"----------------------------------------", new string[] { "align=center", "color=#FF6188", "b" }));
					Log.custom(1, Formatter.parse($"!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", new string[] { "align=center", "color=#FFEA79", "b" }));
					Log.custom(2, Formatter.parse($"----------------------------------------", new string[] { "align=center", "color=#FF6188", "b" }));
					Time.debug();
				}
				Servo.encoder(-2);
			}
		}
	
		private bool checkSensor(ref Reflective refsensor_, ActionHandler correctCallback, ActionHandler crossCallback) {
			if (refsensor_.light.raw < 52 && !refsensor_.isMat()) {
				Clock timeout = new Clock(Time.current.millis + 128 + 64);
				correctCallback();
				while (refsensor_.light.raw < 52) {
					mainRescue.verify();
					if (Green.verify(this)) { return true; }
					if (Time.current > timeout) {
						if (Green.verify(this)) { return true; }
						if (CrossPath.verify(refsensor_)) {
							crossCallback();
							return true;
						}
						break;
					}
				}
				Servo.forward(this.defaultVelocity);
				Time.sleep(32, () => { Green.verify(this); mainRescue.verify(); });
				mainRescue.verify();
				Servo.stop();
				if (Green.verify(this)) { return true; }
				if ((this.lastGreen.millis + 320) > Time.current.millis) {
					return true;
				} else if (CrossPath.verify(refsensor_) && !refsensor_.isColored()) {
					crossCallback();
				}
				if (Green.verify(this)) { return true; }
				Time.resetTimer();
				return true;
			}
			return false;
		}
	
		public void alignSensors(bool right = true) {
			if (right) {
				Servo.right();
				while (!(this.s1.light.raw < 55) || this.s1.isColored() || this.s1.isMat()) { }
				Servo.rotate(-4.5f);
			} else {
				Servo.left();
				while (!(this.s2.light.raw < 55) || this.s2.isColored() || this.s2.isMat()) { }
				Servo.rotate(4.5f);
			}
		}
	}
	public static class CrossPath {
		private static void notify() {
			Buzzer.play(sTurnNotGreen);
			Led.on(cTurnNotGreen);
		}
	
		public static void findLineLeft(FloorRoute.FollowLine Follower) {
			Log.clear();
			Log.info(Formatter.parse($"Align to left", new string[] { "i", "color=#505050" }));
			CrossPath.findLineBase(Follower, new ActionHandler[] { () => Servo.left(), () => Servo.right(), () => Servo.nextAngleLeft(10) }, -90);
		}
	
		public static void findLineRight(FloorRoute.FollowLine Follower) {
			Log.clear();
			Log.info(Formatter.parse($"Align to right", new string[] { "i", "color=#505050" }));
			CrossPath.findLineBase(Follower, new ActionHandler[] { () => Servo.right(), () => Servo.left(), () => Servo.nextAngleRight(10) }, 90);
		}
	
		private static void findLineBase(FloorRoute.FollowLine Follower, ActionHandler[] turnsCallback, float maxDegrees) {
			//if ((Follower.lastCrossPath.millis + 256) > Time.current.millis) { Buzzer.play(sMultiplesCross); return; }
			CrossPath.notify();
			Log.proc();
			Degrees max = new Degrees(Gyroscope.x.raw + maxDegrees);
			Servo.encoder(7f);
			Servo.rotate(-(maxDegrees / 15)); // Check line before turn, inveted axis!
			turnsCallback[0]();
			while (true) {
				if (CrossPath.checkLine(Follower)) { Follower.lastCrossPath = Time.current; Time.resetTimer(); return; }
				if (Gyroscope.x % max) {
					max = new Degrees(Gyroscope.x.raw - (maxDegrees * 2));
					turnsCallback[1]();
					while (true) {
						if (CrossPath.checkLine(Follower)) { Follower.lastCrossPath = Time.current; Time.resetTimer(); return; }
						if (Gyroscope.x % max) {
							Servo.stop();
							Servo.encoder(-6f);
							max = new Degrees(Gyroscope.x.raw + (maxDegrees / 5));
							turnsCallback[0]();
							while (true) {
								if (Gyroscope.x % max) {
									Servo.encoder(6f);
									turnsCallback[2]();
									Servo.encoder(2.5f);
									Servo.rotate(maxDegrees);
									Follower.lastCrossPath = Time.current;
									Time.resetTimer();
									return;
								}
								if (Follower.s1.hasLine() || Follower.s2.hasLine()) {
									Servo.encoder(6f);
									turnsCallback[2]();
									Servo.encoder(2.5f);
									Servo.rotate(-maxDegrees);
									Follower.lastCrossPath = Time.current;
									Time.resetTimer();
									return;
								}
							}
						}
					}
				}
			}
		}
	
		public static bool checkLine(FloorRoute.FollowLine Follower) {
			if (Follower.s1.light.raw < 52 && !Follower.s1.isMat()) {
				Servo.stop();
				Follower.checkEndLine();
				Buzzer.play(sFindLine);
				Servo.left();
				while (Follower.s1.light.raw < 52) { }
				Servo.rotate(-3f);
				return true;
			}
			if (Follower.s2.light.raw < 52 && !Follower.s2.isMat()) {
				Servo.stop();
				Follower.checkEndLine();
				Buzzer.play(sFindLine);
				Servo.right();
				while (Follower.s2.light.raw < 52) { }
				Servo.rotate(3f);
				return true;
			}
			return false;
		}
	
		public static bool verify(Reflective tsensor) => tsensor.light.raw < 50 && !tsensor.isMat();
	}
	public class Green {
	
		private static void notify(int index = 0) {
			Buzzer.play(sTurnGreen, index);
			Led.on(cTurnGreen);
		}
	
		public static void findLineBack() {
			Green.notify();
			Log.clear();
			Log.proc();
			Servo.encoder(16f);
			Servo.rotate(180f);
		}
	
		public static void findLineLeft(FloorRoute.FollowLine Follower) {
			Green.findLineBase(Follower, () => Servo.left(), -32, -87);
		}
	
		public static void findLineRight(FloorRoute.FollowLine Follower) {
			Green.findLineBase(Follower, () => Servo.right(), 32, 87);
		}
	
		private static void findLineBase(FloorRoute.FollowLine Follower, ActionHandler turnCallback, float ignoreDegrees, float maxDegrees) {
			Green.notify();
			Log.clear();
			Log.proc();
			Servo.encoder(14f);
			Servo.rotate(ignoreDegrees);
			turnCallback();
			while (true) {
				if (CrossPath.checkLine(Follower)) { break; }
				if (Gyroscope.inPoint(true, 1)) { Servo.encoder(-5); break; }
			}
			Servo.stop();
		}
	
		public static bool verify(FloorRoute.FollowLine Follower) {
			if (Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen()) {
				Follower.alignSensors();
				Servo.encoder(1);
				if (Follower.s1.rgb.hasGreen() && Follower.s2.rgb.hasGreen()) {
					Green.findLineBack();
				} else if (Follower.s1.rgb.hasGreen()) {
					Green.findLineLeft(Follower);
				} else if (Follower.s2.rgb.hasGreen()) {
					Green.findLineRight(Follower);
				}
				Follower.lastGreen = Time.current;
				Time.resetTimer();
				return true;
			} else {
				return false;
			}
		}
	}
	public class Obstacle {
		public Obstacle(ref Ultrassonic refuObs_, byte distance_ = 15) {
			this.uObs = refuObs_;
			this.distance = distance_;
		}
	
		private Ultrassonic uObs;
		private byte distance;
	
		public bool verify(FloorRoute.FollowLine Follower) {
			if (uObs.distance.raw > 16 && uObs.distance.raw < this.distance && Time.current.millis > 1500) {
				this.dodge(Follower);
				this.verify(Follower);
				return true;
			}
			return false;
		}
	
		public void dodge(FloorRoute.FollowLine Follower) {
	
			void findLineAfterDodge(ActionHandler direction, ActionHandler fix, int maxDegrees) {
				Degrees defaultAxis = Gyroscope.x;
	
				Degrees max = new Degrees(defaultAxis.raw + maxDegrees);
	
				bool findLineBase(Degrees degrees) {
					while (!(Gyroscope.x % degrees)) {
						if (CrossPath.checkLine(Follower)) {
							Servo.stop();
							return true;
						}
					}
					Servo.stop();
					return false;
				}
	
	
				direction();
				if (!findLineBase(max)) {
					fix();
				}
			}
	
			// Find line left
			Servo.stop();
			Servo.alignNextAngle();
			Servo.encoder(-4);
			Servo.rotate(-36);
			Servo.encoder(10);
	
			Servo.forward(200);
			int timeout = Time.current.millis + 256 + 64;
			while (Time.current.millis < timeout) {
				if ((s1.hasLine() && !s1.isMat()) || (s2.hasLine() && !s2.isMat())) {
					Servo.encoder(9);
					Servo.rotate(-10);
					Servo.encoder(7);
					findLineAfterDodge(() => Servo.left(), () => Servo.nextAngleRight(15), -70);
					return;
				}
			}
	
			// Find line right
			Servo.encoder(-20);
			Servo.rotate(36);
			Servo.alignNextAngle();
	
			Servo.rotate(36);
			Servo.encoder(10);
	
			Servo.forward(200);
			timeout = Time.current.millis + 256 + 64;
			while (Time.current.millis < timeout) {
				if ((s1.hasLine() && !s1.isMat()) || (s2.hasLine() && !s2.isMat())) {
					Servo.encoder(9);
					Servo.rotate(10);
					Servo.encoder(7);
					findLineAfterDodge(() => Servo.right(), () => Servo.nextAngleLeft(15), 70);
					return;
				}
			}
			Servo.stop();
	
			// Find line straight
			Servo.rotate(-20);
			Servo.encoder(14);
			Servo.rotate(-70);
	
			Servo.forward(200);
			timeout = Time.current.millis + 256 + 128;
			while (Time.current.millis < timeout) {
				if ((s1.hasLine() && !s1.isMat()) || (s2.hasLine() && !s2.isMat())) {
					Servo.encoder(9);
					Servo.rotate(10);
					Servo.encoder(7);
					Servo.nextAngleRight(10);
					Servo.backward(300);
					Time.sleep(128);
					Servo.stop();
					return;
				}
			}
			Servo.stop();
		}
	}
	static private class Security {
		public static void verify(FloorRoute.FollowLine Follower) {
			if (Time.timer.millis > 112) {
				if (!Servo.SmoothAlignNextAngle(Follower)) {
					Follower.resetMovement();
				}
				return;
			}
	
			Follower.resetMovement();
		}
	
		private static void backToLine(FloorRoute.FollowLine Follower) {
			while (!Security.findLine(Follower)) { Servo.encoder(-6); }
		}
	
		private static void checkInLine(FloorRoute.FollowLine Follower, ActionHandler callback) {
			Clock timeout = new Clock(Time.current.millis + 256);
			while (!(Follower.s1.light.raw < 55 && !Follower.s1.isMat()) && !(Follower.s2.light.raw < 55 && !Follower.s2.isMat())) {
				Servo.left();
				if (Time.current > timeout) {
					Servo.right();
					Time.sleep(256);
					Servo.stop();
					callback();
					return;
				}
			}
			Servo.right();
			Time.sleep(timeout - Time.current);
			Servo.stop();
		}
	
		private static bool findLine(FloorRoute.FollowLine Follower) {
			Degrees defaultAxis = Gyroscope.x;
			Degrees max = new Degrees(defaultAxis.raw - 10);
	
			Func<Degrees, bool> findLineBase = (degrees) => {
				while (!(Gyroscope.x % degrees)) {
					if (CrossPath.checkLine(Follower)) {
						Servo.stop();
						return true;
					}
				}
				return false;
			};
	
			Servo.left();
			if (findLineBase(max)) { return true; }
			Servo.stop();
	
			max = new Degrees(defaultAxis.raw + 20);
			Servo.right();
			if (findLineBase(max)) { return true; }
			Servo.stop();
	
			Servo.left();
			if (findLineBase(defaultAxis)) { return true; }
			Servo.stop();
			return false;
		}
	}
	static private class RescueKit {
		public static void verify(FloorRoute.FollowLine Follower) {
			if (s3.rgb.hasKit()) {
				RescueKit.capture();
			}
		}
	
		public static void backCapture(Degrees defaultDirection) {
			Log.proc();
			Servo.forward(120);
			Time.sleep(700);
			Servo.forward(80);
			Actuator.close();
			Actuator.position(20);
			Time.sleep(128);
			Actuator.position(45);
			Time.sleep(128);
			Servo.encoder(1);
			Actuator.alignUp();
	
			Servo.backward(300);
			Time.sleep(600);
			Servo.stop();
			Servo.alignToAngle(defaultDirection);
		}
	
		public static void capture() {
			Log.proc();
			Degrees saveDirection = Gyroscope.x;
			Servo.alignNextAngle();
			Servo.backward(300);
			Time.sleep(350);
			Servo.stop();
	
			Actuator.open();
			Actuator.alignDown();
			Actuator.angle(10);
	
			int timeout = Time.current.millis + 1000;
			Servo.forward(165);
			while (!Actuator.kit) {
				if (Time.current.millis >= timeout) {
					Log.info($"Actuator.kit: {Actuator.kit}");
					Log.debug("Timeout capture");
					RescueKit.backCapture(saveDirection);
					return;
				}
			}
			Log.info($"Actuator.kit: {Actuator.kit}");
			Log.debug("Rescue Kit founded");
			RescueKit.backCapture(saveDirection);
		}
	}
}
public class RescueRoute {
	public class Victim {
		private bool isRescued { get; set; } = false;
		public Vector2 position { get; set; }
		public sbyte priority { get; set; }
	
		private byte id = 0;
	
	
		public Victim(Vector2 position_, sbyte priority_) {
			this.position = position_;
			this.priority = priority_;
			this.id = UNIQUEID++;
		}
	
		public void rescue() {
			if (!isRescued) {
				isRescued = true;
				return;
			}
		}
	
		public string infos() => $"'position':[{this.position.x},{this.position.y}], 'priority':{this.priority}, 'isRescued':{this.isRescued}, 'id':{this.id}";
	}
	
	public class AliveVictim : Victim {
		public AliveVictim(Vector2 position, sbyte priority = 0) : base(position, priority) { }
	}
	public class DeadVictim : Victim {
		public DeadVictim(Vector2 position, sbyte priority = 1) : base(position, priority) { }
	}
	public static class RescueAnalyzer {
	
		public static void setup() {
			bc.SetFileConsolePath("/home/vinicioslugli/Documentos/scripts/python/sBotics-viewer/res/out.txt");
			bc.EraseConsoleFile();
		}
	
		public static void exportVictim(AliveVictim victim) => bc.WriteText($"[ALIVEVICTIM]({victim.infos()})");
		public static void exportVictim(DeadVictim victim) => bc.WriteText($"[DEADVICTIM]({victim.infos()})");
		public static void exportPoint(Vector2 vector, string color, string info = "") => bc.WriteText($"[POINT]('position':[{vector.x},{vector.y}],'color':{color},'info':{info})");
		public static void exportLine(Vector2 vector1, Vector2 vector2, string color, string info = "") => bc.WriteText($"[LINE]('position1':[{vector1.x},{vector1.y}],'position2':[{vector2.x},{vector2.y}],'color':{color},'info':{info})");
		//public static void exportRescue(RescueInfo rescue) => bc.WriteText($"[RESCUE]('triangle':{rescue.triangle}, 'exit':{rescue.exit})");
		public static void exportClearLines() => bc.WriteText($"[CLEARLINES]()");
	}
	
	public class RescueBrain {
	
		public VictimsAnnihilator victimsAnnihilator;
		public Degrees defaultEnterDegrees;
		public bool currentSide;
	
		public ActionHandler[] actionMove = new ActionHandler[6];
	
		public sbyte sideMultiplier = 0;
	
		public RescueBrain() {
			victimsAnnihilator = new VictimsAnnihilator(this);
		}
	
		private void findExit() {
	
		}
	
		void checkEnterSideAndAlign() {
			Log.proc();
			if (uFrontal.distance.raw > 375) {
				Servo.encoder(10);
				Distance saveRight = uRight.distance;
				Servo.encoder(-10);
				Servo.nextAngleLeft(50);
				if (uFrontal.distance.raw < 260 && saveRight.raw < 65) {
					this.currentSide = false;
				} else {
					this.currentSide = true;
				}
			} else {
				if (uFrontal.distance.raw < 260) {
					this.currentSide = true;
				} else {
					this.currentSide = false;
				}
				Servo.nextAngleLeft(50);
			}
	
			Log.info($"this.currentSide: {this.currentSide}");
		}
	
		private bool isWall() => uFrontal.distance.raw < 25 || (s1.isRescueExit() || s2.isRescueExit()) || (s1.isRescueEnter() || s2.isRescueEnter());
	
		private bool isWallWithoutExit() => uFrontal.distance.raw < 25 || (s1.isRescueEnter() || s2.isRescueEnter());
	
		private sbyte isWallBack() {
			if (bBack.state.pressed) {
				return 1;
			} else if ((s1.isRescueExit() || s2.isRescueExit()) || (s1.isRescueEnter() || s2.isRescueEnter())) {
				return -1;
			}
			return 0;
		}
	
		public void rescueRescueKit() {
			if (Actuator.kit) {
				Servo.rotate(45);
				Servo.encoder(22);
				Servo.rotate(-90);
				victimsAnnihilator.rescueDefault();
				Servo.rotate(90);
				if (!this.currentSide) {
					Servo.forward();
					while (!this.isWall()) { }
					Servo.encoder(-6);
					Servo.rotate(45);
				} else {
					Servo.encoder(-20);
					Servo.rotate(-45);
				}
			} else {
				if (!this.currentSide) {
					Servo.rotate(45);
					Servo.forward();
					while (!this.isWall()) { }
					Servo.encoder(-6);
					Servo.rotate(45);
				}
			}
		}
	
		private void setupMoveActions() {
			if (uFrontal.distance.raw > 120) {
				this.sideMultiplier = 1;
				this.actionMove[0] = () => Servo.forward();
				this.actionMove[1] = () => Servo.backward();
				this.actionMove[2] = () => { Servo.rotate(90); Servo.alignNextAngle(); };
				this.actionMove[3] = () => { Servo.rotate(-180); Servo.alignNextAngle(); };
				this.actionMove[4] = () => Servo.rotate(-45);
				this.actionMove[5] = () => Servo.rotate(-135);
			} else {
				this.sideMultiplier = -1;
				this.actionMove[0] = () => Servo.backward();
				this.actionMove[1] = () => Servo.forward();
				this.actionMove[2] = () => { Servo.rotate(-90); Servo.alignNextAngle(); };
				this.actionMove[3] = () => { };
				this.actionMove[4] = () => Servo.rotate(45);
				this.actionMove[5] = () => Servo.rotate(-45);
			}
		}
	
		public void findTriangleArea() {
			Log.proc();
			this.checkEnterSideAndAlign();
			Log.debug("Finding triangle");
			while (!s3.isTriangle()) {
				Servo.forward();
				if (this.isWall()) {
					Log.debug("Wall finded");
					Servo.encoder(-2);
					Servo.nextAngleRight(50);
					this.currentSide = !this.currentSide;
					Log.info($"this.currentSide: {this.currentSide}");
					Log.debug("Finding triangle");
				}
			}
			Log.debug("Tringle founded!");
	
			this.rescueRescueKit();
	
			this.setupMoveActions();
		}
	
		private void findLineAfterExit(ActionHandler direction, ActionHandler fix, int maxDegrees) {
			Degrees defaultAxis = Gyroscope.x;
	
			Degrees max = new Degrees(defaultAxis.raw + maxDegrees);
	
			bool findLineBase(Degrees degrees) {
				while (!(Gyroscope.x % degrees)) {
					if (FloorRoute.CrossPath.checkLine(mainFollow)) {
						Servo.stop();
						return true;
					}
				}
				Servo.stop();
				return false;
			}
	
	
			direction();
			if (!findLineBase(max)) {
				fix();
			}
		}
	
		public void findExitArea() {
			Log.proc();
			Log.debug("Finding exit");
	
			byte counter = 0;
			while (true) {
				Servo.forward();
				if (this.isWallWithoutExit()) {
					counter = 0;
					Log.debug("Wall finded");
					Servo.encoder(-2);
					Servo.nextAngleLeft(80);
					Log.debug("Finding exit");
				}
	
				if (s1.isRescueExit() || s2.isRescueExit()) {
					counter = 0;
					Servo.stop();
					Servo.rotate(-30);
					Servo.encoder(8);
					Servo.rotate(15);
					this.findLineAfterExit(() => Servo.right(), () => Servo.nextAngleLeft(20), 35);
					break;
				}
	
				if (uRight.distance.raw > 40) {
					Log.info($"Last uRight read: {uRight.distance.raw}");
					counter++;
				} else {
					counter = 0;
				}
	
				if (counter > 8) {
					if (Time.timer.millis < (1024 - 256)) {
						continue;
					}
					counter = 0;
					Servo.stop();
					Log.debug("Finded hole!, verifying...");
					Servo.rotate(60);
					Time.resetTimer();
					while (true) {
						Servo.forward();
						if (s1.isRescueEnter() || s2.isRescueEnter()) {
							Log.debug("Enter hole!");
							Servo.stop();
							Servo.backward(280);
							Time.sleep(Time.timer);
							Servo.stop();
							Servo.rotate(-60);
							Servo.forward();
							Time.sleep(256);
							Servo.stop();
							Time.resetTimer();
							break;
						}
						if (s1.isRescueExit() || s2.isRescueExit()) {
							Log.debug("Exit hole!");
							Servo.stop();
							Servo.encoder(8);
							Servo.rotate(15);
							this.findLineAfterExit(() => Servo.right(), () => Servo.nextAngleLeft(20), 50);
							return;
						}
					}
				}
				Time.sleep(16);
			}
		}
	
		public class VictimsAnnihilator {
	
			byte rescuedVictims = 0;
	
			bool rescuedDeadVictim = false;
	
			RescueRoute.RescueBrain brain;
	
			public VictimsAnnihilator(RescueRoute.RescueBrain _brain) {
				this.brain = _brain;
			}
	
			private void exitMain(bool forceSide = false, bool fixRotate = true) {
				Log.proc();
				void mainFollowAfterRescue() {
					for (; ; ) {
						mainFollow.proc();
						mainRescue.verify();
					}
				}
	
				Log.info($"Exiting with forceSide: {forceSide}, fixRotate: {fixRotate}");
	
				if (fixRotate) {
					Servo.nextAngleLeftDiagonal(80);
				}
	
				if (!forceSide) {
					Time.resetTimer();
					while (true) {
						Servo.forward();
						if (this.brain.isWallWithoutExit() || (Servo.speed() < 0.5f && Time.timer.millis > 256)) {
							Log.debug("Wall finded");
							Servo.stop();
							Servo.encoder(-2);
							Servo.rotate(-45);
							Servo.alignNextAngle();
							break;
						}
	
						if ((s1.isRescueExit() || s2.isRescueExit())) {
							Servo.stop();
							Servo.encoder(7);
							Servo.rotate(15);
							this.brain.findLineAfterExit(() => Servo.right(), () => Servo.nextAngleLeft(20), 35);
							mainFollowAfterRescue();
						}
					}
				}
	
				this.brain.findExitArea();
	
				mainFollowAfterRescue();
			}
	
			public void find() {
				Log.debug($"Finding victim... rescuedVictims: {this.rescuedVictims}");
				Servo.alignNextAngle();
				this.brain.actionMove[0]();
				float currentDistance = uRight.distance.raw;
				while (true) {
					currentDistance = uRight.distance.raw;
					if (currentDistance < 230) {
						Servo.stop();
						break;
					}
				}
				this.captureRight(currentDistance);
			}
	
			private bool captureRight(float distance) {
				if (distance < 170) {
					if (this.brain.currentSide) {
						Servo.encoder(-1.7f);
					} else {
						Servo.encoder(1.7f);
					}
				}
				Servo.rotate(90);
				Servo.alignNextAngle();
				if (distance < 45) {
					Servo.encoder(12, 150);
					Servo.encoder(-12);
				}
				Actuator.open();
				Actuator.alignDown();
				Servo.forward(200);
				Time.resetTimer();
				int beforeDelay = 1024;
				while (true) {
					Log.debug($"Actuator.victim: {bc.HasVictim()}, Servo.speed: {Servo.speed()}");
					if (bc.HasVictim()) {
						break;
					}
					if (Servo.speed() < 1f && Time.timer.millis > 256) {
						break;
					}
	
					if (uFrontal.distance.raw <= 45) {
						Time.sleep(32);
						break;
					}
	
					if (this.brain.isWall()) {
						beforeDelay = 2048;
						break;
					}
					Time.sleep(32);
				}
				Actuator.close();
				Actuator.angle(0);
				Actuator.position(45);
				Servo.stop();
				Actuator.position(88);
				Servo.encoder(1);
				Time.sleep(128);
				Servo.encoder(-1);
				sbyte tempWallRead;
				Servo.backward();
				Time.sleep(beforeDelay);
				Time.resetTimer();
				while (true) {
					tempWallRead = this.brain.isWallBack();
					if ((tempWallRead == 1) || (Servo.speed() < 1f && Time.timer.millis > 256)) {
						Servo.encoder(8);
						break;
					} else if (tempWallRead == -1) {
						Servo.encoder(25);
						break;
					}
				}
				if (Actuator.victim) {
					if (Temperature.victimAlive || this.rescuedVictims == 2) {
						Log.debug("Live victim or dead can be rescue");
						this.brain.actionMove[2]();
	
						rescueFast();
						this.rescuedVictims++;
						if (this.rescuedVictims == 3) {
							Servo.stop();
							Log.clear();
							Log.debug("Rescued 3 victims");
							if (this.brain.currentSide) {
								Servo.rotate(-180);
								Servo.alignNextAngle();
							}
							if (uRight.distance.raw < 40 && !this.brain.currentSide) {
								Servo.rotate(-45);
								this.exitMain(false, false);
							}
							this.exitMain(true, false);
	
						} else if (this.rescuedVictims == 2 && this.rescuedDeadVictim) {
							Servo.stop();
							Log.clear();
	
							this.brain.actionMove[4]();
							Servo.encoder(-3);
							Actuator.open();
							Actuator.alignDown();
							beforeDelay = 0;
							Servo.forward(180);
							Time.resetTimer();
							while (true) {
								Log.debug($"Actuator.victim: {bc.HasVictim()}, Servo.speed: {Servo.speed()}");
								if (bc.HasVictim()) {
									break;
								}
								if (Servo.speed() < 0.5f && Time.timer.millis > 412) {
									break;
								}
								if (this.brain.isWall()) {
									beforeDelay = 1024;
									break;
								}
								Time.sleep(32);
							}
							Actuator.close();
							Time.sleep(128);
							Actuator.angle(0);
							Actuator.position(45);
							Servo.stop();
							Actuator.position(88);
							Servo.forward();
							Time.resetTimer();
							while (true) {
								if (Servo.speed() < 0.5f && Time.timer.millis > 256) {
									break;
								}
	
								if (this.brain.isWall()) {
									break;
								}
							}
							Servo.encoder(-30);
							Servo.rotate(this.brain.sideMultiplier * 90);
							this.rescueDefault();
	
							this.exitMain();
						}
						this.brain.actionMove[3]();
	
						return true;
					} else if (this.rescuedVictims == 2 && !this.rescuedDeadVictim) {
						this.brain.actionMove[3]();
						return false;
					} else {
						Log.debug("Dead victim, go to queue");
						this.rescuedDeadVictim = true;
						this.brain.actionMove[2]();
						Servo.forward();
						while (!s3.isTriangle()) { }
						Servo.stop();
						this.brain.actionMove[4]();
						Servo.forward();
						Time.resetTimer();
						while (true) {
							if (Servo.speed() < 0.5f && Time.timer.millis > 256) {
								break;
							}
	
							if (this.brain.isWall()) {
								break;
							}
						}
						Servo.encoder(-27);
						Time.sleep(128);
						for (int i = 0; i < 10; i++) {
							Actuator.position(88 - (i * 8.8f));
							Time.sleep(128);
						}
						Servo.encoder(-18);
						Actuator.alignUp();
						this.brain.actionMove[5]();
						return true;
					}
				} else {
					this.brain.actionMove[2]();
					return false;
				}
	
			}
	
			public void rescueFast() {
				if (uFrontal.distance.raw < 140) {
					Servo.encoder(-8);
				}
				Servo.forward();
				Actuator.position(25);
				while ((Servo.speed() > 2f || Time.timer.millis < 256)) { }
				Time.sleep(312);
				Actuator.angle(10);
				Servo.backward();
				Time.sleep(128);
				Servo.forward();
				Time.sleep(128);
				Servo.stop();
				Time.sleep(128);
				if (Actuator.victim) {
					Servo.encoder(-10);
					Servo.forward();
					while ((Servo.speed() > 0.8f || Time.timer.millis < 256)) { }
					Time.sleep(312);
					Servo.backward();
					Time.sleep(128);
					Servo.forward();
					Time.sleep(128);
					Servo.stop();
				}
	
				Actuator.alignUp();
			}
	
			public void rescueDefault() {
				Log.proc();
				Servo.forward(200);
				Time.sleep(64);
				Actuator.open();
				Actuator.dropVictim();
				for (int i = 0; i < 2; i++) {
					Servo.backward();
					Time.sleep(64);
					Servo.forward();
					Time.sleep(64);
					Servo.stop();
					Time.sleep(64);
				}
				if (Actuator.victim) {
					for (int i = 0; i < 2; i++) {
						Servo.backward();
						Time.sleep(64);
						Servo.forward();
						Time.sleep(64);
						Servo.stop();
						Time.sleep(128);
					}
				}
				Servo.stop();
				Actuator.close();
				Actuator.alignUp();
			}
		}
	}

	private RescueBrain brain = new RescueBrain();

	private void InitRescue() {
		Servo.stop();
		Buzzer.play(sFindLine);
		Time.sleep(50);
		Buzzer.play(sFindLine);
		this.main();
	}

	public void verify() {
		if (s1.isRescueEnter() && s2.isRescueEnter()) {
			Servo.stop();
			this.InitRescue();
		} else if (s1.isRescueEnter()) {
			Servo.stop();
			Servo.left();
			while (!s2.isRescueEnter()) { }
			Servo.stop();
			this.InitRescue();

		} else if (s2.isRescueEnter()) {
			Servo.stop();
			Servo.right();
			while (!s1.isRescueEnter()) { }
			Servo.stop();
			this.InitRescue();
		}
	}

	private void main() {
		//RescueAnalyzer.setup();

		Servo.encoder(5);
		Servo.alignNextAngle();
		this.brain.defaultEnterDegrees = Gyroscope.x;
		Servo.encoder(25);

		this.brain.findTriangleArea();
		Led.on(0, 0, 255);
		for (; ; ) {
			this.brain.victimsAnnihilator.find();
		}
	}
}

/* --------------- General code --------------- */

//Instances ---------------------------------------------
static DegreesRange upRamp = new DegreesRange(330, 355);
static DegreesRange downRamp = new DegreesRange(5, 30);
static DegreesRange floor = new DegreesRange(355, 5);

static Reflective s1 = new Reflective(1), s2 = new Reflective(0), s3 = new Reflective(2);
static Ultrassonic uFrontal = new Ultrassonic(0), uRight = new Ultrassonic(1);

static Button bBack = new Button(0);

static FloorRoute.FollowLine mainFollow = new FloorRoute.FollowLine(ref s1, ref s2, 200);
static FloorRoute.Obstacle mainObstacle = new FloorRoute.Obstacle(ref uFrontal, 26);
static RescueRoute mainRescue = new RescueRoute();

//----------------------------- Main ---------------------------------------//
//List<double[]> INPUT = new List<double[]>();
//List<double> OUTPUT = new List<double>();

#if (false) //DEBUG MODE MAIN ------------------------------------

void testRefreshRate() {
	long currentTime = Time.currentUnparsed;
	float pval = s1.light.raw;
	float after = s1.light.raw;
	Servo.right();
	while (pval == after) {
		after = s1.light.raw;
	}
	Log.debug($"Time: {Time.currentUnparsed - currentTime}");
	Log.info($"values: {pval} -> {after}");
	Servo.stop();
	Time.sleep(16);
}


void testTurnSpeed() {
	long currentTime = Time.currentUnparsed;
	Servo.rotate(90);
	Log.debug($"Time: {Time.currentUnparsed - currentTime}");
	Time.sleep(16);
}

void showPorcentRGB() {
	Log.info($"s3: {s3.rgb.showPorcentRGB()}");
}

void Main() {
	testTurnSpeed();
	for (; ; ) {
		//showPorcentRGB();
	}
}

#else //DEFAULT MAIN ------------------------------------------------------------------------------------

//Setup program
void setup() {
	//Actuator.alignUp();
	bc.ActuatorSpeed(150);
	bc.ActuatorUp(600);
	Time.resetTimer();
}

//Main loop
void loop() {
	mainFollow.proc();
	mainRescue.verify();
}

void Main() {
	setup();
	for (; ; ) {
		loop();
	}
}

#endif
