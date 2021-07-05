//Base for robot and utils
//Private robot info. current robo-4:
private const byte kLights = 3;//number of sensors
private const byte kUltrassonics = 2;//number of sensors
private const byte kRefreshRate = 31;//ms of refresh rate in color/light sensor
//

//Data ---------------------------------------------
static byte CurrentState = 0b_0000_0001;//Init in followline

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
class Calc{
	public static float constrain(float amt,float low,float high) => ((amt)<(low)?(low):((amt)>(high)?(high):(amt)));

	public static float map(float value, float min, float max, float minTo, float maxTo) => ((((value - min) * (maxTo - minTo)) / (max - min)) + minTo);
}
static string BOLD(string _str) => $"<b>{_str.ToString()}</b>";

static string UNDERLINE(string _str) => $"<u>{_str.ToString()}</u>";

static string ITALIC(string _str) => $"<i>{_str.ToString()}</i>";

static string RESIZE(string _str, float _size) => $"<size={_size}>{_str.ToString()}</size>";

static string COLOR(string _str, string _color) => $"<color={_color}>{_str.ToString()}</color>";
static string COLOR(string _str, Color _color) => $"<color={_color.toHex()}>{_str.ToString()}</color>";

static string ALIGN(string _str, string _alignment) => $"<align=\"{_alignment}\">{_str.ToString()}";
public static class Robot{
	public static void throwError(object message) => bc.RobotError(message.ToString());
	public static void throwError() => bc.RobotError();
	public static void endCode() => bc.CodeEnd();
}
public static class Log{
	public static void proc(object local, object process) => bc.Print(0, $"<align=\"center\">{COLOR(BOLD(local.ToString()), "#FF6188")} {COLOR(process.ToString(), "#947BAF")}");
	public static void proc(){
		var methodInfo = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod();
		bc.Print(0,$"<align=\"center\">{COLOR(BOLD(methodInfo.ReflectedType.Name),"#FF6188")} {COLOR(methodInfo.Name, "#947BAF")}");
	}

	public static void info(object data) => bc.Print(1, "<align=\"center\">"+data.ToString());

	public static void debug(object data) => bc.Print(2, "<align=\"center\">"+data.ToString());

	public static void custom(byte line, object data) => bc.Print((int)line, "<align=\"center\">"+data.ToString());

	public static void clear() => bc.ClearConsole();
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
	public static Sound play(string note, int time=100){bc.PlayNote(1, note, time);return new Sound(note, time);}
	public static Sound play(Sound sound){bc.PlayNote(1, sound.note, sound.time);return new Sound(sound.note, sound.time);}

	public static void stop() => bc.StopSound(1);
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

public static class Gyroscope{
	private static List<Degrees> UP_RAMP = new List<Degrees>(){new Degrees(330), new Degrees(355)};
	private static List<Degrees> DOWN_RAMP = new List<Degrees>(){new Degrees(5), new Degrees(30)};

	public static Degrees x {
		get => new Degrees((float)bc.Compass());
	}
	public static Degrees z {
		get => new Degrees((float)bc.Inclination());
	}

	public static bool isUpRamp() => (Gyroscope.z >= UP_RAMP[0]) && (Gyroscope.z <= UP_RAMP[1]);
	public static bool isDownRamp() => (Gyroscope.z >= DOWN_RAMP[0]) && (Gyroscope.z <= DOWN_RAMP[1]);

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

	public string toHex(){
		string rs = Color.DecimalToHexadecimal((int)this.r);
		string gs = Color.DecimalToHexadecimal((int)this.g);
		string bs = Color.DecimalToHexadecimal((int)this.b);

		return '#' + rs + gs + bs;
	}

	private static string DecimalToHexadecimal(int dec){
		if (dec <= 0)
			return "00";
		int hex = dec;
		string hexStr = string.Empty;
		while (dec > 0){
			hex = dec % 16;

			if (hex < 10)
				hexStr = hexStr.Insert(0, Convert.ToChar(hex + 48).ToString());
			else
				hexStr = hexStr.Insert(0, Convert.ToChar(hex + 55).ToString());

			dec /= 16;
		}
		return hexStr;
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
	public bool hasLine() => bc.ReturnRed((int)this.SensorIndex) < 26;

	public bool hasGreen(){
			float rgb = this.rgb.r + this.rgb.g + this.rgb.b;
			byte pR = (byte)Calc.map(this.rgb.r, 0, rgb, 0, 100);
			byte pG = (byte)Calc.map(this.rgb.g, 0, rgb, 0, 100);
			byte pB = (byte)Calc.map(this.rgb.b, 0, rgb, 0, 100);
			return ((pG > pR) && (pG > pB) && (pG > 65));
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

	public static void foward(float velocity=1000) => bc.Move(velocity, velocity);

	public static void left(float velocity=1000) => bc.Move(-velocity, +velocity);

	public static void right(float velocity=1000) => bc.Move(+velocity, -velocity);

	public static void rotate(float angle, float velocity=500) => bc.MoveFrontalAngles(velocity, angle);
	public static void rotate(Degrees angle, float velocity=500) => bc.MoveFrontalAngles(velocity, angle.raw);

	public static void encoder(float rotations, float velocity=300) => bc.MoveFrontalRotations(velocity, rotations);

	public static float speed() => bc.RobotSpeed();

	public static void stop() => bc.Move(0, 0);

	// public static void nextAngle()
}


//Modules for competition challenges
//iamport("Modules/position.cs");
//iamport("Modules/green.cs");
//iamport("Modules/crosspath.cs");
public static class LowerRoute{
	public static class FollowLine{
		public static void proc(){
			Log.proc();
			Log.debug(BOLD(COLOR($"{s1.light.raw} | {s2.light.raw}", "#FFEA79")));
		}
	}

public static class CrossPath{
		public static void findLineLeft(){

		}

		public static void findLineRight(){

		}

		public static void verify()
		{
		}
	}

	public class Green{
		public static void findLineLeft(){

		}

		public static void findLineRight(){

		}

		public static void verify(){

		}
	}
	public class Position{
		public static void alignSensors(){

		}
	}

}

/* --------------- General code --------------- */

//Instance sensors ---------------------------------------------
static Reflective s1 = new Reflective(1);
static Reflective s2 = new Reflective(0);

//Instance modules ---------------------------------------------


//Setup program
void setup(){
	Actuator.alignUp();
}

//Main loop
void loop(){

	if((CurrentState & (byte)States.FOLLOWLINE) != 0){
	} else if ((CurrentState & (byte)States.OBSTACLE) != 0){

	}else if ((CurrentState & (byte)States.UPRAMP) != 0){

	}else if ((CurrentState & (byte)States.DOWNRAMP) != 0){

	}else if ((CurrentState & (byte)States.RESCUERAMP) != 0){

	}else if ((CurrentState & (byte)States.RESCUE) != 0){

	}else if ((CurrentState & (byte)States.RESCUEEXIT) != 0){

	}else if ((CurrentState & (byte)States.NOP) != 0){
	}
}

//----------------------------- Main ---------------------------------------//


#if(false) //DEBUG MODE MAIN
	void Main(){
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
