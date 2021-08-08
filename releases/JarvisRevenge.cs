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
static Color cRampFollowLine= new Color(255, 0, 255);

static int SETUPTIME = Time.current.millis;

static byte UNIQUEID = 0;
public delegate void ActionHandler();

public static class Actions{
	public static void Empty(){}
}
class Calc{
	public static float constrain(float amt,float low,float high) => ((amt)<(low)?(low):((amt)>(high)?(high):(amt)));

	public static float map(float value, float min, float max, float minTo, float maxTo) => ((((value - min) * (maxTo - minTo)) / (max - min)) + minTo);

	public static string DecToHex(int dec){
		string hexStr = Convert.ToString(dec, 16);
		return (hexStr.Length < 2) ? ("0" + hexStr) : hexStr;
	}
	public static float toBearing(float degrees) => (degrees + 360) % 360;
}
public static class Formatter{
	public static string parse(string data, string[] offsets){
		foreach(string tag in offsets){
			data = Formatter.marker(data, tag);
		}
		return data;
	}

	private static string marker(string data_, string tag){
		string tag_ = (tag.Contains("=")) ? (tag.Split('=')[0] == "align")? $"" : $"</{tag.Split('=')[0]}>" : $"</{tag}>";
		return $"<{tag}>{data_}{tag_}";
	}
}
public static class Log{
	public static void proc(object local, object process) => bc.Print(0, Formatter.parse($"{Formatter.parse(local.ToString(),new string[]{"color=#FF6188", "b"})} {Formatter.parse(process.ToString(),new string[]{"color=#947BAF", "b"})}", new string[]{"align=center"}));
	public static void proc(){
		var methodInfo = (new StackTrace()).GetFrame(1).GetMethod();
		bc.Print(0, Formatter.parse($"{Formatter.parse(methodInfo.ReflectedType.Name,new string[]{"color=#FF6188", "b"})} {Formatter.parse(methodInfo.Name,new string[]{"color=#947BAF", "b"})}", new string[]{"align=center"}));
	}

	public static void info(object data) => bc.Print(1, Formatter.parse(data.ToString(), new string[]{"align=center"}));

	public static void debug(object data) => bc.Print(2, Formatter.parse(data.ToString(), new string[]{"b", "color=#404040", "align=center"}));

	public static void custom(byte line, object data) => bc.Print((int)line, Formatter.parse(data.ToString(), new string[]{"align=center"}));

	public static void clear() => bc.ClearConsole();
}
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
public struct Clock{
	public Clock(int millis_){
		this.millis = millis_;
	}

	public int sec {
		get => (int)(this.millis/1000);
	}

	public int millis;

	public uint micros {
		get => (uint)(this.millis*1000);
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

public static class Time{
	public static Clock current {
		get => new Clock(bc.Millis());
	}

	static public Clock timer {
		get => new Clock(bc.Timer());
	}
	public static Clock currentUnparsed {
		get => new Clock((int)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - SETUPTIME));
	}

	public static void resetTimer() => bc.ResetTimer();

	public static void sleep(int ms) => bc.Wait(ms);
	public static void sleep(int ms, ActionHandler callwhile) {
		int toWait = Time.current.millis + ms;
		while (Time.current.millis < toWait){callwhile();}
	}
	public static void sleep(Clock clock) => bc.Wait(clock.millis);

	public static void debug() => bc.Wait(123456789);

	public static void skipFrame() => bc.Wait(17);
};
public struct Action{
	public Action(bool raw_){
		this.raw = raw_;
	}

	public bool raw;
}

public class Button{
	private byte SensorIndex = 1;

	public Button(byte SensorIndex_){
		this.SensorIndex = SensorIndex_;
	}

	public Action state{
		get => new Action(bc.Touch((int)this.SensorIndex));
	}

	public void NOP(){
		Log.clear();
		Log.proc();
		bc.Touch((int)this.SensorIndex);
	}
}
public static class Led{
	public static void on(byte r , byte g, byte b) => bc.TurnLedOn(r, g, b);

	public static void on(Color color) => bc.TurnLedOn((int)color.r, (int)color.g, (int)color.b);

	public static void off() => bc.TurnLedOff();
}
public struct Sound{
	public Sound(string note_, int time_){
		this.note = note_;
		this.time = time_;
	}

	public string note;
	public int time;
}

public static class Buzzer{
	public static void play(string note, int time=100, int index = 0) => bc.PlayNote(index, note, time);
	public static void play(Sound sound, int index = 0) => bc.PlayNote(index, sound.note, sound.time);

	public static void stop() => bc.StopSound(0);
}
public static class Pencil{
	public static void start() => bc.Draw();

	public static void stop() => bc.StopDrawing();

	public static void color(int r, int g, int b) => bc.ChangePencilColor(r, g, b);
	public static void color(Color color) => bc.ChangePencilColor((int)color.r, (int)color.g, (int)color.b);
}
public struct Celsius{
	public Celsius(float raw_){
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

public static class Temperature{
	public static Celsius celsius {
		get => new Celsius((float)bc.Heat());
	}

	public static void NOP(){
		Log.clear();
		Log.proc();
		bc.Heat();
	}
}
public struct Degrees{
	public Degrees(float raw_){
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
	public static bool operator %(Degrees a, Degrees b) => (a.raw+1 > b.raw) && (a.raw-1 < b.raw);
}

private struct DegreesRange{
	public DegreesRange(float min_, float max_){
		this.min = new Degrees(min_);
		this.max = new Degrees(max_);
	}
	public Degrees min, max;

	public bool isOnRange(Degrees currentGyro) => (currentGyro > this.min) && (currentGyro < this.max);
}

public static class Gyroscope{

	public static Degrees[] points = new Degrees[] {new Degrees(359),new Degrees(0), new Degrees(90), new Degrees(180), new Degrees(270)};
	public static Degrees[] diagonals = new Degrees[] {new Degrees(45),new Degrees(135), new Degrees(225), new Degrees(315)};

	public static Degrees x {
		get => new Degrees((float)bc.Compass());
	}
	public static Degrees z {
		get => new Degrees((float)bc.Inclination());
	}

	public static bool inPoint(bool angExpand = true, byte offset = 8){
		if(angExpand){
			foreach (Degrees point in Gyroscope.points){
				if (((Gyroscope.x.raw + offset) >= point.raw) && (Gyroscope.x.raw - offset <= point.raw)){
					return true;
				}
			}
			return false;
		}else{
			foreach (Degrees point in Gyroscope.points){
				if (Gyroscope.x % point){
					return true;
				}
			}
			return false;
		}
	}

	public static bool inDiagonal(bool angExpand = true, byte offset = 8){
		if(angExpand){
			foreach (Degrees diagonal in Gyroscope.diagonals){
				if (((Gyroscope.x.raw + offset) >= diagonal.raw) && (Gyroscope.x.raw - offset <= diagonal.raw)){
					return true;
				}
			}
			return false;
		}else{
			foreach (Degrees diagonal in Gyroscope.diagonals){
				if (Gyroscope.x % diagonal){
					return true;
				}
			}
			return false;
		}
	}

	public static bool isLifted() => Gyroscope.z.raw > 300 && Gyroscope.z.raw < 358;

	public static void NOP(){
		Log.clear();
		Log.proc();
		bc.Compass();
		bc.Inclination();
	}
}
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
	public static float operator -(Light a, Light b) => a.value - b.value;
	public static float operator +(Light a, Light b) => a.value + b.value;
	public static float operator *(Light a, Light b) => a.value * b.value;
	public static float operator /(Light a, Light b) => a.value / b.value;
}

public class Reflective{
	private byte SensorIndex = 0;

	public Reflective(byte SensorIndex_){
		this.SensorIndex = SensorIndex_;
	}

	public Light light{
		get => new Light(bc.Lightness((int)this.SensorIndex));
	}

	public Color rgb{
		get => new Color(
				bc.ReturnRed((int)this.SensorIndex),
				bc.ReturnGreen((int)this.SensorIndex),
				bc.ReturnBlue((int)this.SensorIndex)
			);
	}
	public bool hasLine() => bc.ReturnRed((int)this.SensorIndex) < 55 && bc.ReturnGreen((int)this.SensorIndex) < 55;

	public bool isMat() => bc.ReturnRed((int)this.SensorIndex) > 50;

	public bool isColored(){
		for (int i = 0; i < 4; i++){
			float r = bc.ReturnRed((int)this.SensorIndex);
			float b = bc.ReturnBlue((int)this.SensorIndex);
			if(!((r + 2) > b && (r - 2) < b)){
				return true;
			}
		}
		return false;
	}

	public void NOP(){
		Log.clear();
		Log.proc();
		bc.Lightness((int)this.SensorIndex);
		bc.ReturnRed((int)this.SensorIndex);
		bc.ReturnGreen((int)this.SensorIndex);
		bc.ReturnBlue((int)this.SensorIndex);
	}
}
public struct Distance{
	public Distance(float raw_){
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

public class Ultrassonic{
	private byte SensorIndex = 0;

	public Ultrassonic(byte SensorIndex_){
		this.SensorIndex = SensorIndex_;
	}

	public Distance distance{
		get => new Distance(bc.Distance((int)this.SensorIndex));
	}

	public void NOP(){
		Log.clear();
		Log.proc();
		bc.Distance((int)this.SensorIndex);
	}
}
public static class Actuator{
	public static void position(float degrees, int velocity=150){
		Log.clear();
		bc.ActuatorSpeed(velocity);

		int timeout = Time.current.millis + (3000 - (velocity*10));
		float local_angle = bc.AngleActuator();

		degrees = (degrees < 0 || degrees > 300) ? 0 : (degrees > 88) ? 88 : degrees;

		Log.proc();

		if(degrees > local_angle){
			while(degrees > local_angle){
				bc.ActuatorUp(1);
				if(Time.current.millis > timeout){return;}
				local_angle = bc.AngleActuator();
				Log.info($"local_angle: {local_angle}");
			}
		}else if(degrees < local_angle){
			while(degrees < local_angle){
				bc.ActuatorDown(1);
				if(Time.current.millis > timeout){return;}
				local_angle = bc.AngleActuator();
				Log.info($"local_angle: {local_angle}");
			}
		}
	}

	public static void angle(float degrees, int velocity=150){
		Log.clear();
		bc.ActuatorSpeed(velocity);

		int timeout = Time.current.millis + (3000 - (velocity*10));
		float local_angle = bc.AngleScoop();

		degrees = (degrees < 0 || degrees > 300) ? 0 : (degrees > 12) ? 12 : degrees;

		Log.proc();

		if(degrees > local_angle){
			while(degrees > local_angle){
				bc.TurnActuatorDown(1);
				if(Time.current.millis > timeout){return;}
				local_angle = bc.AngleScoop();
				Log.info($"local_angle: {local_angle}");
			}
		}else if(degrees < local_angle){
			while(degrees < local_angle){
				bc.TurnActuatorUp(1);
				if(Time.current.millis > timeout){return;}
				local_angle = bc.AngleScoop();
				Log.info($"local_angle: {local_angle}");
			}
		}
	}

	public static bool victim {
		get => bc.HasVictim();
	}

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

	public static void alignUp(){
		position(88);
		angle(0);
	}
	public static void alignDown(){
		position(0);
		angle(0);
	}

	public static void dropVictim(){
		position(0);
		angle(12);
	}

	public static void NOP(){
		Log.clear();
		Log.proc();
		bc.AngleActuator();
		bc.AngleScoop();
	}
}
public static class Servo{
	public static void move(float left=300, float right=300) => bc.Move(left, right);

	public static void foward(float velocity=300) => bc.Move(Math.Abs(velocity), Math.Abs(velocity));

	public static void backward(float velocity=300) => bc.Move(-velocity, -velocity);

	public static void left(float velocity=1000) => bc.Move(-velocity, +velocity);

	public static void right(float velocity=1000) => bc.Move(+velocity, -velocity);

	public static void rotate(float angle, float velocity=500) => bc.MoveFrontalAngles(velocity, angle);
	public static void rotate(Degrees angle, float velocity=500) => bc.MoveFrontalAngles(velocity, angle.raw);

	public static void encoder(float rotations, float velocity=300) => bc.MoveFrontalRotations(rotations > 0 ? velocity : -velocity, Math.Abs(rotations));

	public static float speed() => bc.RobotSpeed();

	public static void stop() => bc.Move(0, 0);

	public static void antiLifting(){
		if((Gyroscope.isLifted() || !Gyroscope.inPoint(true, 6)) && Time.timer.millis > 312){
			Log.proc();
			Buzzer.play(sLifting);
			Servo.stop();
			int timeout = Time.current.millis + 312;
			while(Gyroscope.isLifted() && Time.current.millis < timeout){
				Servo.backward(200);
			}
			Time.sleep(128);
			Servo.stop();
			Servo.alignNextAngle();
			Time.resetTimer();
		}
	}

	public static void antiLiftingRescue(){
		if((Gyroscope.isLifted() || !Gyroscope.inDiagonal()) && Time.timer.millis >= 312){
			Log.proc();
			Buzzer.play(sLifting);
			Servo.stop();
			int timeout = Time.current.millis + 312;
			while(Gyroscope.isLifted() && Time.current.millis < timeout){
				Servo.backward(200);
			}
			Time.sleep(128);
			Servo.stop();
			Time.resetTimer();
		}
	}

	public static void nextAngleRight(byte ignoreAngles = 0){
		Log.proc();
		Servo.rotate(Math.Abs(ignoreAngles));
		Servo.right();
		while(!Gyroscope.inPoint(false)){}
		Servo.stop();
	}

	public static void nextAngleLeft(byte ignoreAngles = 0){
		Log.proc();
		Servo.rotate(-ignoreAngles);
		Servo.left();
		while(!Gyroscope.inPoint(false)){}
		Servo.stop();
	}

	public static void alignNextAngle(){
		Log.proc();
		if(Gyroscope.inPoint(true, 2)){return;}
		Degrees alignLocal = new Degrees(0);
		if((Gyroscope.x.raw > 315) || (Gyroscope.x.raw <= 45)){
			alignLocal = new Degrees(0);
		}else if((Gyroscope.x.raw > 45) && (Gyroscope.x.raw <= 135)){
			alignLocal = new Degrees(90);
		}else if((Gyroscope.x.raw > 135) && (Gyroscope.x.raw <= 225)){
			alignLocal = new Degrees(180);
		}else if((Gyroscope.x.raw > 225) && (Gyroscope.x.raw <= 315)){
			alignLocal = new Degrees(270);
		}

		Log.info(Formatter.parse($"Align to {alignLocal.raw}°", new string[]{"i","color=#505050"}));

		if((alignLocal.raw == 0) && (Gyroscope.x.raw > 180)){
			Servo.right();
		}else if((alignLocal.raw == 0) && (Gyroscope.x.raw < 180)){
			Servo.left();
		}else if(Gyroscope.x < alignLocal){
			Servo.right();
		}else if(Gyroscope.x > alignLocal){
			Servo.left();
		}
		while(!(Gyroscope.x % alignLocal)){}
		Servo.stop();
	}

	public static void alignToAngle(object angle){
		Log.proc();

		Degrees alignLocal = (angle is Degrees) ? (Degrees)angle : new Degrees((float)angle);

		Log.info(Formatter.parse($"Align to {alignLocal.raw}°", new string[]{"i","color=#505050"}));

		float baseFind = Calc.toBearing(Gyroscope.x - alignLocal);

		if(baseFind >= 180){
			Servo.right();
		}else if(baseFind < 180){
			Servo.left();
		}
		while(!(Gyroscope.x % alignLocal)){}
		Servo.stop();
	}

	private static bool ultraGoToRecursive(Ultrassonic ultra, ActionHandler callback){
		Log.info(Formatter.parse($"ultra: {ultra.distance.raw}, speed: {Servo.speed()}", new string[]{"i","color=#505050"}));
		Servo.antiLifting();
		if(Servo.speed() < 0.5f && Time.timer.millis > 500){
			callback?.Invoke();
			return true;
		}
		return false;
	}

	public static void ultraGoTo(float position, ref Ultrassonic ultra, ActionHandler callback = null, int velocity = 200){
		Log.proc();
		Time.resetTimer();
		if(position > ultra.distance.raw){
			while(position > ultra.distance.raw){
				if(ultraGoToRecursive(ultra, callback)){break;}
				Servo.backward(velocity);
			}
		}else{
			while(position < ultra.distance.raw){
				if(ultraGoToRecursive(ultra, callback)){break;}
				Servo.foward(velocity);
			}
		}
		Servo.stop();
		Log.clear();
	}

	public static void ultraGoTo(Distance dist, ref Ultrassonic ultra, ActionHandler callback = null, int velocity = 200){
		Log.proc();
		Time.resetTimer();
		if(dist > ultra.distance){
			while(dist > ultra.distance){
				if(ultraGoToRecursive(ultra, callback)){break;}
				Servo.backward(velocity);
			}
		}else{
			while(dist < ultra.distance){
				if(ultraGoToRecursive(ultra, callback)){break;}
				Servo.foward(velocity);
			}
		}
		Servo.stop();
		Log.clear();
	}
}
public struct Vector2{
	public float x;
	public float y;

	public Vector2(float x, float y){
		this.x = x;
		this.y = y;
	}

	public static Vector2 operator + (Vector2 _v1, Vector2 _v2) => new Vector2(_v1.x + _v2.x, _v1.y + _v2.y);

	public static Vector2 operator - (Vector2 _v1, Vector2 _v2) => new Vector2(_v1.x - _v2.x, _v1.y - _v2.y);

	public static Vector2 operator * (Vector2 _v1, float m) => new Vector2(_v1.x * m, _v1.y * m);

	public static Vector2 operator / (Vector2 _v1, float d) => new Vector2(_v1.x / d, _v1.y / d);

	public static float distance(Vector2 _v1, Vector2 _v2) => (float)Math.Sqrt(Math.Pow(_v1.x - _v2.x, 2) + Math.Pow(_v1.y - _v2.y, 2));

	public float distanceFrom(Vector2 _v) => (float)Math.Sqrt(Math.Pow(this.x - _v.x, 2) + Math.Pow(this.y - _v.y, 2));

	public float length() => (float)Math.Sqrt(x * x + y * y);
}


//Modules for competition challenges
public static class FloorRoute{
	public class FollowLine{
		public FollowLine(ref Reflective refs1_, ref Reflective refs2_, int velocity_){
			this.s1 = refs1_;
			this.s2 = refs2_;
			this.velocity = velocity_;
		}
	
		public Reflective s1, s2;
		public int velocity = 0;
		public Clock lastGreen = new Clock(0);
		public Clock lastCrossPath = new Clock(0);
	
		private void debugSensors(){
			Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw}", new string[]{"align=center", "color=#FFEA79", "b"}));
		}
	
		public void proc(){
			Log.proc();
			this.debugSensors();
	
			if(Green.verify(this)){return;}
	
			if(checkSensor(ref this.s1, () => Servo.left(), () => CrossPath.findLineLeft(this))){}
			else if(checkSensor(ref this.s2, () => Servo.right(), () => CrossPath.findLineRight(this))){}
			else{Servo.foward(this.velocity); Security.verify(this);}
		}
	
		private bool checkSensor(ref Reflective refsensor_, ActionHandler correctCallback, ActionHandler crossCallback){
			if(refsensor_.light.raw < 55 && !refsensor_.isColored() && !refsensor_.isMat()){
				correctCallback();
				Clock timeout = new Clock(Time.current.millis + 176);
				while(refsensor_.light.raw < 55){
					if(Green.verify(this)){return true;}
					if(Time.current > timeout){
						if(Green.verify(this)){return true;}
						if(CrossPath.verify(refsensor_)){
							crossCallback();
							return true;
						}
						break;
					}
				}
				Time.sleep(32, () => Green.verify(this));
				Servo.foward(this.velocity);
				Time.sleep(32, () => Green.verify(this));
				Servo.stop();
				if(Green.verify(this)){return true;}
				if((this.lastGreen.millis + 320) > Time.current.millis){
					return true;
				}else if(CrossPath.verify(refsensor_) && !refsensor_.isColored()){
					crossCallback();
				}
				if(Green.verify(this)){return true;}
				Time.resetTimer();
				return true;
			}
			return false;
		}
	
		public void alignSensors(bool right = true){
			if(right){
				Servo.right();
				while(!(this.s1.light.raw < 55) || this.s1.isColored() || this.s1.isMat()){}
				Servo.rotate(-4.5f);
			}else{
				Servo.left();
				while(!(this.s2.light.raw < 55) || this.s2.isColored() || this.s2.isMat()){}
				Servo.rotate(4.5f);
			}
		}
	}
	public static class CrossPath{
		private static void notify(){
			Buzzer.play(sTurnNotGreen);
			Led.on(cTurnNotGreen);
		}
	
		public static void findLineLeft(FloorRoute.FollowLine Follower){
			CrossPath.findLineBase(Follower, new ActionHandler[]{() => Servo.left(), () => Servo.right(), () => Servo.nextAngleLeft(10)}, -90);
		}
	
		public static void findLineRight(FloorRoute.FollowLine Follower){
			CrossPath.findLineBase(Follower, new ActionHandler[]{() => Servo.right(), () => Servo.left(), () => Servo.nextAngleRight(10)}, 90);
		}
	
		private static void findLineBase(FloorRoute.FollowLine Follower, ActionHandler[] turnsCallback, float maxDegrees){
			if((Follower.lastCrossPath.millis + 256) > Time.current.millis){Buzzer.play(sMultiplesCross);return;}
			CrossPath.notify();
			Log.clear();
			Log.proc();
			Degrees max = new Degrees(Gyroscope.x.raw + maxDegrees);
			Servo.encoder(7f);
			Servo.rotate(-(maxDegrees / 9));
			turnsCallback[0]();
			while(true){
				if(CrossPath.checkLine(Follower)){Follower.lastCrossPath = Time.current;Time.resetTimer();return;}
				if(Gyroscope.x % max){
					max = new Degrees(Gyroscope.x.raw - (maxDegrees * 2));
					turnsCallback[1]();
					while (true){
						if(CrossPath.checkLine(Follower)){Follower.lastCrossPath = Time.current;Time.resetTimer();return;}
						if (Gyroscope.x % max){
							Servo.stop();
							Servo.encoder(-6f);
							max = new Degrees(Gyroscope.x.raw + (maxDegrees / 5));
							turnsCallback[0]();
							while(true){
								if(Gyroscope.x % max){
									Servo.encoder(6f);
									turnsCallback[2]();
									Servo.encoder(2.5f);
									Servo.rotate(maxDegrees);
									Follower.lastCrossPath = Time.current;
									Time.resetTimer();
									return;
								}
								if(Follower.s1.hasLine() || Follower.s2.hasLine()){
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
	
		public static bool checkLine(FloorRoute.FollowLine Follower){
			if(Follower.s1.light.raw < 55 && !Follower.s1.isColored() && !Follower.s1.isMat()){
				Servo.stop();
				Buzzer.play(sFindLine);
				Servo.rotate(-2f);
				return true;
			}
			if(Follower.s2.light.raw < 55 && !Follower.s2.isColored() && !Follower.s2.isMat()){
				Servo.stop();
				Buzzer.play(sFindLine);
				Servo.rotate(2f);
				return true;
			}
			return false;
		}
	
		public static bool verify(Reflective tsensor) => tsensor.light.raw < 45 && !tsensor.isMat() && !tsensor.isColored();
	}
	public class Green{
	
		private static void notify(int index = 0){
			Buzzer.play(sTurnGreen, index);
			Led.on(cTurnGreen);
		}
	
		public static void findLineBack(){
			Green.notify();
			Log.clear();
			Log.proc();
			Servo.encoder(16f);
			Servo.rotate(180f);
		}
	
		public static void findLineLeft(FloorRoute.FollowLine Follower){
			Green.findLineBase(Follower, () => Servo.left(), -25, -87);
		}
	
		public static void findLineRight(FloorRoute.FollowLine Follower){
			Green.findLineBase(Follower, () => Servo.right(), 25, 87);
		}
	
		private static void findLineBase(FloorRoute.FollowLine Follower, ActionHandler turnCallback, float ignoreDegrees, float maxDegrees){
			Green.notify();
			Log.clear();
			Log.proc();
			Servo.encoder(14f);
			Servo.rotate(ignoreDegrees);
			turnCallback();
			while(true){
				if(CrossPath.checkLine(Follower)){ break; }
				if(Gyroscope.inPoint(true, 3)){ Servo.encoder(-5); break; }
			}
			Servo.stop();
		}
	
		public static bool verify(FloorRoute.FollowLine Follower){
			if(Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen()){
				Follower.alignSensors();
				Servo.foward();
				Time.sleep(32);
				Servo.stop();
				if(Follower.s1.rgb.hasGreen() && Follower.s2.rgb.hasGreen()){
					Green.findLineBack();
				}else if(Follower.s1.rgb.hasGreen()){
					Green.findLineLeft(Follower);
				}else if(Follower.s2.rgb.hasGreen()){
					Green.findLineRight(Follower);
				}
				Follower.lastGreen = Time.current;
				Time.resetTimer();
				return true;
			}else{
				return false;
			}
		}
	}
	public class Obstacle{
		public Obstacle(ref Ultrassonic refuObs_, byte distance_ = 15){
			this.uObs = refuObs_;
			this.distance = distance_;
		}
	
		private Ultrassonic uObs;
		private byte distance;
	
		public void verify(){
			if(uObs.distance.raw > 16 && uObs.distance.raw < this.distance && Time.current.millis > 3000){
				this.dodge();
				this.verify();
			}
		}
	
		public void dodge(){
			Servo.stop();
			Servo.alignNextAngle();
			Servo.rotate(58);
			Servo.encoder(7);
			Servo.rotate(-27);
			Servo.encoder(7);
			Servo.rotate(-15);
			Servo.encoder(9);
			Servo.rotate(-10);
			Servo.encoder(17);
			Servo.nextAngleLeft(10);
			Servo.encoder(12);
			Servo.nextAngleRight(10);
		}
	}
	static private class Security{
		public static void verify(FloorRoute.FollowLine Follower){
			if(Time.timer.millis > (2800 - (Follower.velocity * 13)) && mainRescue.rampTimer == 0 && !floor.isOnRange(Gyroscope.z)){
				Security.checkInLine(Follower, () => Security.backToLine(Follower));
				Time.resetTimer();
			}
		}
	
		private static void backToLine(FloorRoute.FollowLine Follower){
			while(!Security.findLine(Follower)){Servo.encoder(-6);}
		}
	
		private static void checkInLine(FloorRoute.FollowLine Follower, ActionHandler callback){
			Clock timeout = new Clock(Time.current.millis + 256);
			while(!(Follower.s1.light.raw < 55 && !Follower.s1.isMat()) && !(Follower.s2.light.raw < 55 && !Follower.s2.isMat())){
				Servo.left();
				if(Time.current > timeout){
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
	
		private static bool findLine(FloorRoute.FollowLine Follower){
			Degrees defaultAxis = Gyroscope.x;
			Degrees max = new Degrees(defaultAxis.raw - 10);
	
			Func<Degrees, bool> findLineBase = (degrees) =>{
				while (!(Gyroscope.x % degrees)){
					if(CrossPath.checkLine(Follower)){
						Servo.stop();
						return true;
					}
				}
				return false;
			};
	
			Servo.left();
			if(findLineBase(max)){ return true; }
			Servo.stop();
	
			max = new Degrees(defaultAxis.raw + 20);
			Servo.right();
			if(findLineBase(max)){ return true; }
			Servo.stop();
	
			Servo.left();
			if(findLineBase(defaultAxis)){ return true; }
			Servo.stop();
			return false;
		}
	}
}
public class RescueRoute{
	public struct RescueInfo{
		public sbyte triangle;
		public sbyte exit;
	
		public int triangleBaseDegrees(){
			if(this.triangle == 1){
				return 135;
			}else if(this.triangle == 2){
				return 45;
			}else if(this.triangle == 3){
				return -45;
			}
			return 0;
		}
	
		public bool setTriangle(sbyte triangle_){
			if(this.triangle != 0){return false;}
			Buzzer.play(sRescueFindArea);
			Log.info($"Rescue triangle on: {triangle_}");
			this.triangle = triangle_;
			RescueAnalyzer.exportRescue(this);
			return true;
		}
	
		public bool setExit(sbyte exit_){
			if(this.exit != 0){return false;}
			Buzzer.play(sRescueFindArea);
			Log.info($"Rescue exit on: {exit_}");
			this.exit = exit_;
			RescueAnalyzer.exportRescue(this);
			return true;
		}
	
		public bool hasInfos(){
			return this.triangle != 0 && this.exit != 0;
		}
	}
	public class Victim{
		private bool isRescued { get; set; } = false;
		public Vector2 position { get; set; }
		public sbyte priority { get; set; }
	
		private byte id = 0;
	
	
		public Victim(Vector2 position_, sbyte priority_){
			this.position = position_;
			this.priority = priority_;
			this.id = UNIQUEID++;
		}
	
		public void rescue(){
			if(!isRescued){
				isRescued = true;
				return;
			}
		}
	
		public string infos() => $"'position':[{this.position.x},{this.position.y}], 'priority':{this.priority}, 'isRescued':{this.isRescued}, 'id':{this.id}";
	}
	
	public class AliveVictim : Victim{
		public AliveVictim(Vector2 position, sbyte priority = 0) : base(position, priority){}
	}
	public class DeadVictim: Victim{
		public DeadVictim(Vector2 position, sbyte priority = 1) : base(position, priority){}
	}
	public static class RescueAnalyzer{
	
		public static void setup(){
			bc.EraseConsoleFile();
			bc.SetFileConsolePath("/home/vinicioslugli/Documentos/scripts/python/sBotics-viewer/res/out.txt");
		}
	
		public static void exportVictim(AliveVictim victim) => bc.WriteText($"[ALIVEVICTIM]({victim.infos()})");
		public static void exportVictim(DeadVictim victim) => bc.WriteText($"[DEADVICTIM]({victim.infos()})");
		public static void exportPoint(Vector2 vector, string color, string info = "") => bc.WriteText($"[POINT]('position':[{vector.x},{vector.y}],'color':{color},'info':{info})");
		public static void exportLine(Vector2 vector1, Vector2 vector2, string color, string info = "") => bc.WriteText($"[LINE]('position1':[{vector1.x},{vector1.y}],'position2':[{vector2.x},{vector2.y}],'color':{color},'info':{info})");
		public static void exportRescue(RescueInfo rescue) => bc.WriteText($"[RESCUE]('triangle':{rescue.triangle}, 'exit':{rescue.exit})");
		public static void exportClearLines() => bc.WriteText($"[CLEARLINES]()");
	}
	public class RampFollowLine{
		public RampFollowLine(ref Reflective refs1_, ref Reflective refs2_, int velocity_){
			this.s1 = refs1_;
			this.s2 = refs2_;
			this.velocity = velocity_;
		}
	
		public Reflective s1, s2;
		public int velocity = 0;
	
		private void debugSensors(){
			Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw}", new string[] { "align=center", "color=#FFEA79", "b" }));
			Led.on(cRampFollowLine);
		}
	
		public void proc(){
			Log.proc();
			this.debugSensors();
			if(this.s1.light.raw < 55 && !this.s1.isColored()){
				Servo.rotate(-1);
				Servo.encoder(1);
			}else if(this.s2.light.raw < 55 && !this.s2.isColored()){
				Servo.rotate(1);
				Servo.encoder(1);
			}else{
				Servo.foward(this.velocity);
			}
		}
	}
	public class RescueBrain{
		public RescueBrain(){
			victimsAnnihilator = new VictimsAnnihilator(this);
		}
	
		public VictimsAnnihilator victimsAnnihilator;
		public Degrees defaultEnterDegrees;
	
		private const short RESCUE_SIZE = 300;
		private RescueInfo rescue = new RescueInfo();
	
		private void findExit(sbyte exitIndex, int maxTime = 600, ActionHandler callback = null){
			Log.proc();
			Time.resetTimer();
			while(Time.timer.millis < maxTime){
				Servo.antiLifting();
				Servo.foward(200);
				if(uRight.distance.raw > RESCUE_SIZE){
					rescue.setExit(exitIndex);
					callback?.Invoke();
	
				}
				Log.info(Formatter.parse($"uRight: {uRight.distance.raw}, speed: {Servo.speed()}, time: {maxTime - Time.timer.millis}", new string[]{"i","color=#505050"}));
				Log.debug($"FINDING EXIT {exitIndex}");
			}
			Servo.stop();
			Log.clear();
		}
	
		public void findTriangleArea(){
			findExit(1);
			Log.debug("FINDING TRIANGLE 3");
			Servo.ultraGoTo(40, ref uFrontal, () => {
				if(this.rescue.setTriangle(3) && this.rescue.exit == 0){
					this.rescue.setExit(2);
				}
			});
	
			Log.clear();
			if(this.rescue.exit == 1){
				this.rescue.setTriangle(2);
			}
	
			if(!this.rescue.hasInfos()){
				Servo.alignNextAngle();
				Servo.rotate(-180);
				Servo.alignNextAngle();
				findExit(3, 500);
				Servo.backward(200);
				Time.sleep(500);
				Servo.stop();
				if(this.rescue.setExit(2)){
					this.rescue.setTriangle(1);
				}
				if(this.rescue.triangle == 0){
					Servo.rotate(-90);
					Servo.alignNextAngle();
					Log.debug("FINDING TRIANGLE 2");
					Servo.ultraGoTo(40, ref uFrontal, () => {
						this.rescue.setTriangle(2);
					});
					Log.clear();
				}
				if(this.rescue.triangle == 0){
					this.rescue.setTriangle(1);
				}
			}else{
				Log.info($"Triangle: {this.rescue.triangle}, Exit: {this.rescue.exit}");
				Log.debug("HAS INFOS!");
			}
			Servo.alignNextAngle();
		}
	
		public void goToCenter(){
			Servo.ultraGoTo((300 / 2) - (Robot.kDiffFrontalDistance * Robot.kErrorDelta), ref uFrontal, null, 300);
			Servo.nextAngleLeft(50);
			Servo.ultraGoTo((300 / 2) - (Robot.kDiffFrontalDistance * Robot.kErrorDelta), ref uFrontal, null, 300);
		}
	
		public class VictimsAnnihilator{
			public Distance distThreshold;
			public float counterThreshold;
	
			private RescueRoute.RescueBrain tBrain;
			private byte verifyCounter = 0;
			private List<float> verifyOccurrences = new List<float>();
			private Degrees lastPoint;
	
			public VictimsAnnihilator(RescueRoute.RescueBrain tBrainInstance, float defaultDist = 130f, float defaultCounter = 3.2f){
				this.distThreshold = new Distance(defaultDist);
				this.counterThreshold = defaultCounter;
				this.tBrain = tBrainInstance;
			}
	
			private bool checkDiagonal(){
				float localDegrees = Calc.toBearing(Gyroscope.x.raw + 90 - this.tBrain.defaultEnterDegrees.raw);
				if(this.tBrain.rescue.triangle == 1){
					return localDegrees > (135 - 24) && localDegrees < (135 + 24);
				}else if(this.tBrain.rescue.triangle == 2){
					return localDegrees > (45 - 24) && localDegrees < (45 + 24);
				}else if(this.tBrain.rescue.triangle == 3){
					return localDegrees > (315 - 24) && localDegrees < (315 + 24);
				}
				return false;
			}
	
			private bool checkVerifyP(Distance cDistance, byte serial){
				if(this.verifyCounter >= serial){
					if(this.checkDiagonal()){
						if(this.verifyOccurrences.Count == 0){
							return true;
						}
						this.verifyOccurrences.Sort();
						float[] occurrences = this.verifyOccurrences.ToArray();
						try{
							float first = this.verifyOccurrences[1];
							float last = this.verifyOccurrences[occurrences.Length - 1];
	
							if(Math.Abs(first - last) <= 8){
								Log.debug($"Math.Abs(first - last): {Math.Abs(first - last)}");
								return true;
							}
						}finally{
							Led.on(255, 255, 0);
						}
					}else{
						return true;
					}
				}
				return false;
			}
	
			private void resetInstances(){
				this.verifyCounter = 0;
				this.verifyOccurrences.Clear();
			}
	
			public Distance[] find(){
				Distance localDistance = uFrontal.distance;
				byte serial = 255;
				this.resetInstances();
				while(true){
					Servo.right();
	
					localDistance = uRight.distance;
					serial = (byte)(((this.distThreshold - localDistance) + (Robot.kDiffFrontalDistance / 2)) / this.counterThreshold);
	
					Log.info(Formatter.parse($"FINDING VICTIM", new string[]{"i","color=#78DCE8"}));
					Log.debug(Formatter.parse($"uRight: {localDistance.raw}, verifyCounter: {this.verifyCounter}, serial: {serial}", new string[]{"i","color=#505050"}));
	
					if(localDistance < this.distThreshold){
						this.verifyCounter++;
						this.verifyOccurrences.Add(localDistance.raw);
						if(this.checkVerifyP(localDistance, serial)){
							this.lastPoint = new Degrees(Gyroscope.x.raw + 5);
							Led.on(0, 255, 0);
							this.resetInstances();
							break;
						}
					}else{
						this.resetInstances();
						Led.on(255, 0, 0);
					}
	
					if(Gyroscope.inPoint(true, 2) && Time.timer.millis > 328){
						Servo.ultraGoTo((300 / 2) - ((Robot.kDiffFrontalDistance * Robot.kErrorDelta) / 3), ref uFrontal, null, 300);
						Time.resetTimer();
					}
	
					Time.sleep(16);
				}
				Servo.stop();
				Log.clear();
				Log.info(Formatter.parse($"FINDED VICTIM", new string[]{"i","color=#A9DC76"}));
				Buzzer.play(sTurnGreen);
				return new Distance[]{new Distance(this.distThreshold - localDistance), localDistance};
			}
	
			private void alignRescue(){
				Log.clear();
				Log.proc();
				Servo.alignToAngle(new Degrees(this.tBrain.defaultEnterDegrees.raw + this.tBrain.rescue.triangleBaseDegrees()));
			}
	
			private bool capture(Distance realDist){
				Log.proc();
				float diffBack = 0f;
				if(realDist.raw <= 50){
					diffBack = (50 - realDist.raw) / 3f;
					Servo.encoder(-diffBack, 200);
				}
				Actuator.alignDown();
				Actuator.open();
				int cRotations = (int)(realDist.toRotations() * 0.95f);
				int lastRotations = 0;
				Time.resetTimer();
				for (int rotation = 0; rotation < cRotations; rotation++){
					Log.info(Formatter.parse($"cRotations: {cRotations}, currentFor: {rotation}", new string[]{"i","color=#A9DC76"}));
					Servo.foward(150);
					Time.sleep(48);
					lastRotations = rotation;
					if(Actuator.victim){
						break;
					}else if(Servo.speed() < 1f && Time.timer.millis >= 192){
						lastRotations = lastRotations - 4;
					}
				}
				Time.sleep(128);
				Clock saveTimeCost = Time.current;
				Actuator.close();
				Actuator.alignUp();
				int timeToReturn = (int)(Time.current - saveTimeCost);
				Servo.stop();
				Log.proc();
				Log.debug(Formatter.parse($"Actuator.victim: {Actuator.victim}", new string[]{"i","color=#FFEA79"}));
				if(!Actuator.victim){
					Servo.nextAngleRight();
					this.tBrain.goToCenter();
					Servo.alignToAngle(this.lastPoint);
					return false;
				}
				Servo.backward(300);
				Time.sleep((int)(((lastRotations * 48) + timeToReturn + 192) * 0.60f));
				Servo.stop();
				return true;
			}
	
			public void rescue(Distance diffDist, Distance realDist){
				Log.proc();
				Servo.rotate((float)((88 + (diffDist.raw / 35))));
				if(!this.capture(realDist)){return;}
				this.alignRescue();
				Log.proc();
				Servo.foward(200);
				Time.sleep(192);
				Time.resetTimer();
				while(Servo.speed() > 0.5f || Time.timer.millis <= 312){
					Log.debug(Formatter.parse($"speed: {Servo.speed()}, timer: {Time.timer.millis}", new string[]{"i","color=#FFEA79"}));
					Servo.antiLiftingRescue();
				}
				Time.sleep(64);
				Servo.stop();
				Actuator.open();
				Actuator.dropVictim();
				for (int i = 0; i < 2; i++){
					Servo.backward();
					Time.sleep(128);
					Servo.foward();
					Time.sleep(128);
				}
					Servo.stop();
				Actuator.close();
				Actuator.alignUp();
				Servo.nextAngleRight();
				this.tBrain.goToCenter();
				Servo.alignToAngle(this.lastPoint);
			}
		}
	}

	public RescueRoute(ref Reflective refs1_, ref Reflective refs2_, int velocity_){
		mainRampFollowLine = new RampFollowLine(ref refs1_, ref refs2_, velocity_);
	}

	public int rampTimer = 0;
	private RampFollowLine mainRampFollowLine;

	private RescueBrain brain = new RescueBrain();

	public void verify(){
		if(upRamp.isOnRange(Gyroscope.z) && (uRight.distance.raw < 42)){
			if(this.rampTimer == 0){
				this.rampTimer = Time.current.millis + 2000;

			}else if(Time.current.millis > this.rampTimer){
				Servo.stop();
				this.main();
			}
		}else{
			this.rampTimer = 0;
		}
	}

	public void check(){
		if(upRamp.isOnRange(Gyroscope.z)){
			main();
		}
	}

	private void main(){
		RescueAnalyzer.setup();
		while(upRamp.isOnRange(Gyroscope.z) && uRight.distance.raw < 42){
			mainRampFollowLine.proc();
		}
		Servo.alignNextAngle();
		for(byte i = 0; i < 4; i++){
			Servo.encoder(3, 200);
			Servo.encoder(-1, 200);
		}
		Servo.alignNextAngle();
		this.brain.defaultEnterDegrees = Gyroscope.x;
		Servo.ultraGoTo(260, ref uFrontal);

		this.brain.findTriangleArea();
		Led.on(0, 0, 255);
		this.brain.goToCenter();
		Distance[] localVictimInfoRescue;
		for(;;){
			localVictimInfoRescue = this.brain.victimsAnnihilator.find();
			this.brain.victimsAnnihilator.rescue(localVictimInfoRescue[0], localVictimInfoRescue[1]);
			//this.brain.victimsAnnihilator.checkExit();
		}
	}
}

/* --------------- General code --------------- */

//Instances ---------------------------------------------
static DegreesRange upRamp = new DegreesRange(330, 355);
static DegreesRange downRamp = new DegreesRange(5, 30);
static DegreesRange floor = new DegreesRange(355, 5);

static Reflective s1 = new Reflective(1), s2 = new Reflective(0);
static Ultrassonic uFrontal = new Ultrassonic(0), uRight = new Ultrassonic(1);

static FloorRoute.FollowLine mainFollow = new FloorRoute.FollowLine(ref s1, ref s2, 140);
static FloorRoute.Obstacle mainObstacle = new FloorRoute.Obstacle(ref uFrontal, 26);
static RescueRoute mainRescue = new RescueRoute(ref s1, ref s2, 180);
//Instance modules ---------------------------------------------


//Setup program
void setup(){
	Actuator.alignUp();
	Time.resetTimer();
}

//Main loop
void loop(){
	mainFollow.proc();
	mainObstacle.verify();
	mainRescue.verify();
}

//----------------------------- Main ---------------------------------------//

#if (false) //DEBUG MODE MAIN
	void Main(){
	//long a = Time.current.millis;
	//float abuble = bc.Lightness(1);
	//while (abuble == bc.Lightness(1)) {
	//	Servo.left();
	//}
	//bc.Print(Time.current.millis - a);
	//Servo.stop();
	for (;;){
		}
	}
#else //DEFAULT MAIN
	void Main(){
		setup();
		mainRescue.check();
		for(;;){
			loop();
		}
	}
#endif
