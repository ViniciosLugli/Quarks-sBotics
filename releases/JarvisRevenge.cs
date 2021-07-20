//Base for robot and utils
//Data ---------------------------------------------
byte CurrentState = 0b_0000_0001;//Init in followline

enum States : byte {
    FOLLOWLINE = 1 << 0,
    OBSTACLE = 1 << 1,
    UPRAMP = 1 << 2,
    DOWNRAMP = 1 << 3,
    RESCUERAMP = 1 << 4,
    RESCUE = 1 << 5,
    RESCUEEXIT = 1 << 6,
    NOP = 1 << 7
}

static Sound sTurnNotGreen = new Sound("F2", 80);
static Sound sTurnGreen = new Sound("C3", 60);

static Color cFollowLine = new Color(255, 255, 255);
static Color cTurnNotGreen = new Color(0, 0, 0);
static Color cTurnGreen = new Color(0, 255, 0);
public delegate void MethodHandler();
class Calc{
	public static float constrain(float amt,float low,float high) => ((amt)<(low)?(low):((amt)>(high)?(high):(amt)));

	public static float map(float value, float min, float max, float minTo, float maxTo) => ((((value - min) * (maxTo - minTo)) / (max - min)) + minTo);

	public static string DecToHex(int dec){
		string hexStr = Convert.ToString(dec, 16);
		return (hexStr.Length < 2) ? ("0" + hexStr) : hexStr;
	}
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
	public const byte kLights = 3;//number of sensors
	public const byte kUltrassonics = 2;//number of sensors
	public const byte kRefreshRate = 31;//ms of refresh rate in color/light sensor
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
	public static bool operator ==(Clock a, Clock b) => a.millis == b.millis;
	public static bool operator !=(Clock a, Clock b) => a.millis != b.millis;
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

	//Basic operators
	public static bool operator ==(Action a, Action b) => a.raw == b.raw;
	public static bool operator !=(Action a, Action b) => a.raw != b.raw;
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

	//Basic operators
	public static bool operator ==(Sound a, Sound b) => a.note == b.note;
	public static bool operator !=(Sound a, Sound b) => a.note != b.note;
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
	public static bool operator ==(Celsius a, Celsius b) => a.raw == b.raw;
	public static bool operator !=(Celsius a, Celsius b) => a.raw != b.raw;
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
	public static bool operator ==(Degrees a, Degrees b) => a.raw == b.raw;
	public static bool operator !=(Degrees a, Degrees b) => a.raw != b.raw;
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

	public bool isOnRange(byte offset = 0) => (Gyroscope.z >= this.min) && (Gyroscope.z <= this.max);
}

public static class Gyroscope{

	private static Degrees[] points = new Degrees[] {new Degrees(0), new Degrees(90), new Degrees(180), new Degrees(270)};

	public static Degrees x {
		get => new Degrees((float)bc.Compass());
	}
	public static Degrees z {
		get => new Degrees((float)bc.Inclination());
	}

	public static bool inPoint(){
		foreach(Degrees point in Gyroscope.points){
			if((Gyroscope.x.raw + 8 > point.raw) && (Gyroscope.x.raw - 8 < point.raw)){
				return true;
			}
		}
		return false;
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
	public static bool operator ==(Light a, Light b) => a.value == b.value;
	public static bool operator !=(Light a, Light b) => a.value != b.value;
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

	public bool isMat() => bc.ReturnRed((int)this.SensorIndex) > 65;

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
	public static bool operator ==(Distance a, Distance b) => a.raw == b.raw;
	public static bool operator !=(Distance a, Distance b) => a.raw != b.raw;
	public static float operator -(Distance a, Distance b) => a.raw - b.raw;
	public static float operator +(Distance a, Distance b) => a.raw + b.raw;
	public static float operator *(Distance a, Distance b) => a.raw * b.raw;
	public static float operator /(Distance a, Distance b) => a.raw / b.raw;
}

class Ultrassonic{
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

	public static void foward(float velocity=300) => bc.Move(velocity, velocity);

	public static void left(float velocity=1000) => bc.Move(-velocity, +velocity);

	public static void right(float velocity=1000) => bc.Move(+velocity, -velocity);

	public static void rotate(float angle, float velocity=500) => bc.MoveFrontalAngles(velocity, angle);
	public static void rotate(Degrees angle, float velocity=500) => bc.MoveFrontalAngles(velocity, angle.raw);

	public static void encoder(float rotations, float velocity=300) => bc.MoveFrontalRotations(rotations > 0 ? velocity : -velocity, Math.Abs(rotations));

	public static float speed() => bc.RobotSpeed();

	public static void stop() => bc.Move(0, 0);

	// public static void nextAngle()
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
		private int velocity = 0;
	
		private void debugSensors(){
			Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw}", new string[]{"align=center", "color=#FFEA79", "b"}));
		}
	
		public void proc(){
			Log.proc();
			this.debugSensors();
			Green.verify(this);
			if((this.s1.light.raw < 50) && !this.s1.isMat()){
				Servo.left();
				Time.resetTimer();
				while(this.s1.rgb.r < 60){
					Green.verify(this);
					if(Time.timer.millis > 112){
						Green.verify(this);
						if(Gyroscope.inPoint() && CrossPath.verify(this.s1)){
							CrossPath.findLineLeft(ref this.s1);
						}
						break;
					}
				}
				Time.sleep(48, () => Green.verify(this));
				Servo.foward(this.velocity);
				Time.sleep(32, () => Green.verify(this));
				Servo.stop();
				Time.sleep(Robot.kRefreshRate - 16, () => Green.verify(this));
				Green.verify(this);
				if(CrossPath.verify(this.s1)){
					CrossPath.findLineLeft(ref this.s1);
				}
				Green.verify(this);
			}else if((this.s2.light.raw < 50) && !this.s2.isMat()){
				Servo.right();
				Time.resetTimer();
				while(this.s2.rgb.r < 60){
					Green.verify(this);
					if(Time.timer.millis > 112){
						Green.verify(this);
						if(Gyroscope.inPoint() && CrossPath.verify(this.s2)){
							CrossPath.findLineRight(ref this.s2);
						}
						break;
					}
				}
				Time.sleep(48, () => Green.verify(this));
				Servo.foward(this.velocity);
				Time.sleep(32, () => Green.verify(this));
				Servo.stop();
				Time.sleep(Robot.kRefreshRate - 16, () => Green.verify(this));
				Green.verify(this);
				if(CrossPath.verify(this.s2)){
					CrossPath.findLineRight(ref this.s2);
				}
				Green.verify(this);
			}else{
				Servo.foward(this.velocity);
			}
		}
	
		public void alignSensors(bool right = true){
			if(right){
				Servo.right();
				while(!this.s1.hasLine()){}
				Servo.rotate(-4.5f);
			}else{
				Servo.left();
				while(!this.s2.hasLine()){}
				Servo.rotate(4.5f);
			}
		}
	}
	public static class CrossPath{
		private static void notify(){
			Buzzer.play(sTurnNotGreen);
			Led.on(cTurnNotGreen);
		}
	
		public static void findLineLeft(ref Reflective refsensor_){
			CrossPath.notify();
			Log.clear();
			Log.proc();
			Degrees initialDefault = new Degrees(Gyroscope.x.raw - 75);
			Degrees max = new Degrees(Gyroscope.x.raw - 80);
			Servo.encoder(8f);
			Servo.left();
			while((!refsensor_.hasLine())){
				if(Gyroscope.x % max){
					max = new Degrees(Gyroscope.x.raw + 165);
					Servo.encoder(-6f);
					Servo.right();
					while(true){
						if(refsensor_.hasLine()){
							Servo.rotate(-17);
							Servo.encoder(5f);
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
			Servo.rotate(-5f);
		}
	
		public static void findLineRight(ref Reflective refsensor_){
			CrossPath.notify();
			Log.clear();
			Log.proc();
			Degrees initialDefault = new Degrees(Gyroscope.x.raw + 75);
			Degrees max = new Degrees(Gyroscope.x.raw + 80);
			Servo.encoder(8f);
			Servo.right();
			while(!refsensor_.hasLine()){
				if(Gyroscope.x % max){
					max = new Degrees(Gyroscope.x.raw - 165);
					Servo.encoder(-6f);
					Servo.left();
					while (true){
						if(refsensor_.hasLine()){
							Servo.rotate(17);
							Servo.encoder(5f);
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
			Servo.rotate(5f);
		}
	
		public static bool verify(Reflective tsensor) => tsensor.light.raw < 45 && !tsensor.isMat();
	}
	public class Green{
	
		private static void notify(){
			Buzzer.play(sTurnGreen);
			Led.on(cTurnGreen);
		}
	
		public static void findLineLeft(ref Reflective refsensor_){
			Green.notify();
			Log.clear();
			Log.proc();
			Servo.encoder(14f);
			Servo.rotate(-20f);
			Degrees maxLeft = new Degrees(Gyroscope.x.raw - 87);
			Servo.left();
			while((!refsensor_.hasLine()) && (!(Gyroscope.x % maxLeft))){}
			Servo.stop();
			Servo.rotate(-3f);
		}
	
		public static void findLineRight(ref Reflective refsensor_){
			Green.notify();
			Log.clear();
			Log.proc();
			Servo.encoder(14f);
			Servo.rotate(20f);
			Degrees maxRight = new Degrees(Gyroscope.x.raw + 87);
			Servo.right();
			while((!refsensor_.hasLine()) && (!(Gyroscope.x % maxRight))){}
			Servo.stop();
			Servo.rotate(3f);
		}
	
		public static void verify(FloorRoute.FollowLine Follower){
			if(Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen()){
				Follower.alignSensors();
				Time.sleep(32);
				if(Follower.s1.rgb.hasGreen()){
					findLineLeft(ref Follower.s1);
				}else if(Follower.s2.rgb.hasGreen()){
					findLineRight(ref Follower.s2);
				}
			}
		}
	}
}

/* --------------- General code --------------- */

//Instances ---------------------------------------------
static DegreesRange upRamp = new DegreesRange(330, 355);
static DegreesRange downRamp = new DegreesRange(5, 30);

static Reflective s1 = new Reflective(1);
static Reflective s2 = new Reflective(0);
static FloorRoute.FollowLine mainFollow = new FloorRoute.FollowLine(ref s1, ref s2, 135);
//Instance modules ---------------------------------------------


//Setup program
void setup(){
	Actuator.alignUp();
}

//Main loop
void loop(){
	if((this.CurrentState & (byte)States.FOLLOWLINE) != 0){
		mainFollow.proc();
	} else if ((this.CurrentState & (byte)States.OBSTACLE) != 0){

	}else if ((this.CurrentState & (byte)States.UPRAMP) != 0){

	}else if ((this.CurrentState & (byte)States.DOWNRAMP) != 0){

	}else if ((this.CurrentState & (byte)States.RESCUERAMP) != 0){

	}else if ((this.CurrentState & (byte)States.RESCUE) != 0){

	}else if ((this.CurrentState & (byte)States.RESCUEEXIT) != 0){

	}else if ((this.CurrentState & (byte)States.NOP) != 0){
	}
}

//----------------------------- Main ---------------------------------------//

#if(false) //DEBUG MODE MAIN
	void Main(){
		mainFollow.alignSensors();
	for(;;){
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
