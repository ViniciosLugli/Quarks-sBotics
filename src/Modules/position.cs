class Position{
	public static void alignSensors(){
		if(s2.light > s3.light){
			while(s2.light > s3.light){
				Servo.left();
			}
			Servo.stop();
			Servo.rotate(1);
		}else if(s3.light > s2.light){
			while(s3.light > s2.light){
				Servo.right();
			}
			Servo.stop();
			Servo.rotate(-1);
		}
	}
}
