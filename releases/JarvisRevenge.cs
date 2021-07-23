//Base for robot and utils
//Data ---------------------------------------------
static Sound sTurnNotGreen = new Sound("F2", 80);
static Sound sTurnGreen = new Sound("C3", 60);
static Sound sFakeGreen = new Sound("G", 100);
static Sound sAlertOffline = new Sound("D#", 100);

static Color cFollowLine = new Color(255, 255, 255);
static Color cTurnNotGreen = new Color(0, 0, 0);
static Color cTurnGreen = new Color(0, 255, 0);
static Color cFakeGreen = new Color(255, 255, 0);
static Color cAlertOffline = new Color(255, 0, 0);

static long SETUPTIME = DateTimeOffset.Now.ToUnixTimeMilliseconds();
public delegate void MethodHandler();
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

	public static void debug(object data) => bc.Print(2, Formatter.parse(data.ToString(), new string[]{"align=center"}));

	public static void custom(byte line, object data) => bc.Print((int)line, Formatter.parse(data.ToString(), new string[]{"align=center"}));

	public static void clear() => bc.ClearConsole();
}
public static class Robot{
	//Private robot info. current robo-3:
	public const byte kLights = 5;//number of sensors
	public const byte kUltrassonics = 3;//number of sensors
	public const byte kRefreshRate = 63;//ms of refresh rate in color/light sensor
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
	public static void sleep(int ms, MethodHandler callwhile) {
		int toWait = Time.current.millis + ms;
		while (Time.current.millis < toWait){callwhile();}
	}
	public static void sleep(Clock clock) => bc.Wait(clock.millis);
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
	public static void play(string note, int time=100) => bc.PlayNote(0, note, time);
	public static void play(Sound sound) => bc.PlayNote(0, sound.note, sound.time);

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
	public bool hasLine() => bc.ReturnRed((int)this.SensorIndex) < 35 && bc.ReturnGreen((int)this.SensorIndex) < 35;

	public bool isMat() => bc.ReturnRed((int)this.SensorIndex) > 50;

	public bool isColored() => bc.ReturnRed((int)this.SensorIndex) != bc.ReturnBlue((int)this.SensorIndex);

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

	public float toRotations() => this.raw / 2;

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

	public static void nextAngleRight(byte ignoreAngles = 0){
		Servo.rotate(Math.Abs(ignoreAngles));
		Servo.right();
		while(!Gyroscope.inPoint(false)){}
		Servo.stop();
	}

	public static void nextAngleLeft(byte ignoreAngles = 0){
		Servo.rotate(-ignoreAngles);
		Servo.left();
		while(!Gyroscope.inPoint(false)){}
		Servo.stop();
	}

	public static void alignNextAngle(){
		Log.proc();
		if(Gyroscope.inPoint(true, 2)){return;}
		Degrees alignLocal = new Degrees(0);;
		if((Gyroscope.x.raw > 315) || (Gyroscope.x.raw <= 45)){
			alignLocal = new Degrees(0);
		}else if((Gyroscope.x.raw > 45) && (Gyroscope.x.raw <= 135)){
			alignLocal = new Degrees(90);
		}else if((Gyroscope.x.raw > 135) && (Gyroscope.x.raw <= 225)){
			alignLocal = new Degrees(180);
		}else if((Gyroscope.x.raw > 225) && (Gyroscope.x.raw <= 315)){
			alignLocal = new Degrees(270);
		}

		Log.info(Formatter.parse($"Align to {alignLocal.raw}°", new string[]{"i","color=#505050", "align=center"}));

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
}
public struct Vector2{
    public float X;
    public float Y;

    public Vector2(float x, float y){
        this.X = x;
        this.Y = y;
    }

    public static Vector2 operator + (Vector2 _v1, Vector2 _v2){
        return new Vector2(_v1.X + _v2.X, _v1.Y + _v2.Y);
    }

    public static Vector2 operator - (Vector2 _v1, Vector2 _v2){
        return new Vector2(_v1.X - _v2.X, _v1.Y - _v2.Y);
    }

    public static Vector2 operator * (Vector2 _v1, float m){
        return new Vector2(_v1.X * m, _v1.Y * m);
    }

    public static Vector2 operator / (Vector2 _v1, float d){
        return new Vector2(_v1.X / d, _v1.Y / d);
    }

    public static float distance(Vector2 _v1, Vector2 _v2){
        return (float) Math.Sqrt(Math.Pow(_v1.X - _v2.X, 2) + Math.Pow(_v1.Y - _v2.Y, 2));
    }

    public float length(){
        return (float) Math.Sqrt(X * X + Y * Y);
    }
}


//Modules for competition challenges
public static class FloorRoute{
	public class FollowLine{
		public FollowLine(ref Reflective refs1_, ref Reflective refs2_, ref Reflective refs3_, ref Reflective refs4_, int velocity_){
			this.s1 = refs1_;
			this.s2 = refs2_;
			this.s3 = refs3_;
			this.s4 = refs4_;
			this.velocity = velocity_;
		}
	
		public Reflective s1, s2, s3, s4;
		public int velocity = 0;
	
		private void debugSensors(){
			Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw} | {this.s3.light.raw} | {this.s4.light.raw}", new string[]{"align=center", "color=#FFEA79", "b"}));
			Led.on(cFollowLine);
		}
	
		public void proc(){
			Log.proc();
			this.debugSensors();
			Green.verify(this);
			CrossPath.verify(this);
			if((50 - this.s2.light.raw) > 16 && !this.s2.isColored()){
				Servo.left();
				Time.sleep(32, () => {Green.verify(this);CrossPath.verify(this);});
				Servo.stop();
				Servo.foward(this.velocity);
				Time.sleep(16, () => {Green.verify(this);CrossPath.verify(this);});
				Time.resetTimer();
			}else if((50 - this.s3.light.raw) > 16 && !this.s3.isColored()){
				Servo.right();
				Time.sleep(32, () => {Green.verify(this);CrossPath.verify(this);});
				Servo.stop();
				Servo.foward(this.velocity);
				Time.sleep(16, () => {Green.verify(this);CrossPath.verify(this);});
				Time.resetTimer();
			}else{
				Servo.foward(this.velocity);
				Security.verify(this);
			}
		}
	
		public void alignSensors(){
			if(this.s2.light > this.s3.light){
				while(this.s2.light.raw < 45){Servo.left();}
				Servo.right();
				while(this.s2.light.raw > 45){}
				Servo.left();
				Time.sleep(32);
				Servo.stop();
			}else{
				while(this.s3.light.raw < 45){Servo.right();}
				Servo.left();
				while(this.s3.light.raw > 45){}
				Servo.right();
				Time.sleep(32);
				Servo.stop();
			}
		}
	}
	private static class CrossPath{
		private static void notify(){
			Buzzer.play(sTurnNotGreen);
			Led.on(cTurnNotGreen);
		}
	
		public static void findLineLeft(ref Reflective refsensor_){
			CrossPath.notify();
			Log.clear();
			Log.proc();
			Degrees initialDefault = new Degrees(Gyroscope.x.raw - 80);
			Degrees max = new Degrees(Gyroscope.x.raw - 100);
			Servo.encoder(9f);
			Servo.left();
			while((!refsensor_.hasLine())){
				if(Gyroscope.x % max){
					max = new Degrees(Gyroscope.x.raw + 165);
					Servo.encoder(-6f);
					Servo.right();
					while(true){
						if(refsensor_.hasLine()){
							Servo.rotate(2f);
							return;
						}
						if (Gyroscope.x % max){
							Servo.left();
							while(!(Gyroscope.x % initialDefault)){}
							Servo.stop();
							return;
						}
					}
				}
			}
			Servo.stop();
			Servo.rotate(1f);
		}
	
		public static void findLineRight(ref Reflective refsensor_){
			CrossPath.notify();
			Log.clear();
			Log.proc();
			Degrees initialDefault = new Degrees(Gyroscope.x.raw + 80);
			Degrees max = new Degrees(Gyroscope.x.raw + 100);
			Servo.encoder(9f);
			Servo.right();
			while(!refsensor_.hasLine()){
				if(Gyroscope.x % max){
					max = new Degrees(Gyroscope.x.raw - 165);
					Servo.encoder(-6f);
					Servo.left();
					while (true){
						if(refsensor_.hasLine()){
							Servo.rotate(-2f);
							return;
						}
						if (Gyroscope.x % max){
							Servo.right();
							while(!(Gyroscope.x % initialDefault)){}
							Servo.stop();
							return;
						}
					}
				}
			}
			Servo.stop();
			Servo.rotate(-1f);
		}
	
		public static void verify(FloorRoute.FollowLine Follower){
			if(Follower.s1.light.raw < 52 && !Follower.s1.isMat()){
				if (Green.verify(Follower)) { Time.resetTimer(); return; }
				findLineLeft(ref Follower.s3);
				Time.resetTimer();
			}else if(Follower.s4.light.raw < 52 && !Follower.s4.isMat()){
				if (Green.verify(Follower)) { Time.resetTimer(); return; }
				findLineRight(ref Follower.s2);
				Time.resetTimer();
			}
		}
	}
	private class Green{
	
		private static void notifyGreen(){
			Buzzer.play(sTurnGreen);
			Led.on(cTurnGreen);
		}
	
		private static void notifyFakeGreen(){
			Buzzer.play(sFakeGreen);
			Led.on(cFakeGreen);
		}
	
		private static void findLineBack(FloorRoute.FollowLine Follower){
			Log.clear();
			Log.proc();
			Servo.encoder(13f);
			Servo.rotate(180);
		}
	
		private static void findLineLeft(FloorRoute.FollowLine Follower){
			Log.clear();
			Log.proc();
			Servo.encoder(11f);
			Servo.rotate(-15f);
			Degrees maxLeft = new Degrees(Gyroscope.x.raw - 88);
			Servo.left();
			while((!Follower.s2.hasLine()) && (!(Gyroscope.x % maxLeft))){}
			Servo.stop();
			Servo.rotate(-2f);
		}
	
		private static void findLineRight(FloorRoute.FollowLine Follower){
			Log.clear();
			Log.proc();
			Servo.encoder(11f);
			Servo.rotate(15f);
			Degrees maxRight = new Degrees(Gyroscope.x.raw + 88);
			Servo.right();
			while((!Follower.s3.hasLine()) && (!(Gyroscope.x % maxRight))){}
			Servo.stop();
			Servo.rotate(2f);
		}
	
		public static void confirm(FloorRoute.FollowLine Follower, MethodHandler callback){
			Clock bTimer = new Clock(Time.current.millis + 256);
			Servo.foward(Follower.velocity);
			while(bTimer > Time.current){
				if((Follower.s1.light.raw < 52 && !Follower.s1.isColored()) || (Follower.s2.light.raw < 52 && !Follower.s2.isColored()) || (Follower.s1.light.raw < 52 && !Follower.s3.isColored()) || (Follower.s4.light.raw < 52 && !Follower.s2.isColored())){
					Servo.stop();
					Green.notifyGreen();
					callback();
					return;
				}
			}
			Servo.stop();
			Green.notifyFakeGreen();
		}
	
		public static bool verify(FloorRoute.FollowLine Follower){
			if(Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen() || Follower.s3.rgb.hasGreen() || Follower.s4.rgb.hasGreen()){
				Follower.alignSensors();
				Time.sleep(32);
	
				if((Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen()) && (Follower.s3.rgb.hasGreen() || Follower.s4.rgb.hasGreen())){
					Green.confirm(Follower, () => Green.findLineBack(Follower));
	
				}else if(Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen()){
					Green.confirm(Follower, () => Green.findLineLeft(Follower));
	
				}else if(Follower.s3.rgb.hasGreen() || Follower.s4.rgb.hasGreen()){
					Green.confirm(Follower, () => Green.findLineRight(Follower));
				}
				Time.resetTimer();
				return true;
			}
			return false;
		}
	}
	static private class Security{
		public static void verify(FloorRoute.FollowLine Follower){
			if(Time.timer.millis > (2800 - (Follower.velocity * 10))){
				if(Gyroscope.inPoint()){
					Security.checkInLine(Follower, () => Security.backToLine(Follower));
				}else{
					Security.backToLine(Follower);
				}
				Time.resetTimer();
			}
		}
	
		private static void backToLine(FloorRoute.FollowLine Follower){
			Servo.backward(Follower.velocity);
			while(!(Follower.s1.light.raw < 55) && !(Follower.s2.light.raw < 55) && !(Follower.s3.light.raw < 55) && !(Follower.s4.light.raw < 55)){}
			Servo.stop();
			Servo.encoder(-3);
		}
	
		private static void checkInLine(FloorRoute.FollowLine Follower, MethodHandler callback){
			Servo.rotate(1.5f);
			if(!(Follower.s1.light.raw < 55) && !(Follower.s2.light.raw < 55) && !(Follower.s3.light.raw < 55) && !(Follower.s4.light.raw < 55)){
				Servo.rotate(-1.5f);
				callback();
			}else{
				Servo.rotate(-1.5f);
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
			if(uObs.distance.raw < this.distance){
				this.dodge();
				this.verify();
			}
		}
	
		public void dodge(){//TODO: Double obstacle...
			Servo.stop();
			Servo.alignNextAngle();
			Servo.rotate(58);
			Servo.encoder(7);
			Servo.rotate(-25);
			Servo.encoder(7);
			Servo.rotate(-15);
			Servo.encoder(9);
			Servo.rotate(-10);
			Servo.encoder(12);
			Servo.rotate(-20);
			Servo.encoder(5);
			Servo.rotate(-30);
			Servo.encoder(3);
			Servo.rotate(-30);
			Servo.encoder(3);
			Servo.rotate(10);
			Servo.encoder(6);
			Servo.nextAngleRight();
		}
	}
}
public static class RescueRoute{

}

/* --------------- General code --------------- */

//Instances ---------------------------------------------
static DegreesRange upRamp = new DegreesRange(330, 355);
static DegreesRange downRamp = new DegreesRange(5, 30);

static Reflective s1 = new Reflective(3), s2 = new Reflective(2), s3 = new Reflective(1), s4 = new Reflective(0);
static Ultrassonic uFrontal = new Ultrassonic(0), uRight = new Ultrassonic(1), uLeft = new Ultrassonic(2);

static FloorRoute.FollowLine mainFollow = new FloorRoute.FollowLine(ref s1, ref s2, ref s3, ref s4, 170);
static FloorRoute.Obstacle mainObstacle = new FloorRoute.Obstacle(ref uFrontal, 15);
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
}

//----------------------------- Main ---------------------------------------//

#if (false) //DEBUG MODE MAIN
	void Main(){
		long a = Time.current.millis;
		float abuble = bc.Lightness(1);
		while (abuble == bc.Lightness(1)) {
			Servo.left();
		}
		bc.Print(Time.current.millis - a);
		Servo.stop();
		for (;;){
		}
	}
#else //DEFAULT MAIN
	void Main(){
		setup();
		for(;;){
			loop();
		}
	}
#endif
