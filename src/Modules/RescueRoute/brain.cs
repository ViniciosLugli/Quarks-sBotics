public class RescueBrain{
	//public RescueBrain(){}

	private const short RESCUE_SIZE = 300;
	private RescueInfo rescue = new RescueInfo();

	private void findExit(sbyte exitIndex, int maxTime = 1200, ActionHandler callback = null){
		Log.proc();
		Time.resetTimer();
		while(Time.timer.millis < maxTime){
			Servo.antiLifting();
			Servo.foward(200);
			if(uRight.distance.raw > RESCUE_SIZE){
				rescue.setExit(exitIndex);
				if(!(callback is null)){
					callback();
				}
			}
			Log.info(Formatter.parse($"uRight: {uRight.distance.raw}", new string[]{"i","color=#505050", "align=center"}));
		}
		Servo.stop();
	}

	public void findTriangleArea(){
		findExit(1);
		Time.resetTimer();
		Servo.ultraGoTo(40, ref uFrontal, () => {
			if(this.rescue.setTriangle(3) && !this.rescue.hasInfos()){
				this.rescue.setExit(2);
			}
		});

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
				Time.resetTimer();
				Servo.ultraGoTo(40, ref uFrontal, () => {
					this.rescue.setTriangle(2);
				});
			}
			if(this.rescue.triangle == 0){
				this.rescue.setTriangle(1);
			}
		}
	}
}

//Idealização aaaaaaaaa
/*
Objetivos de caminho:
	1° encontrar resgate e saída;
	2° encontrar e mapear as bolinhas próximas e encontrar a melhor solução de tempo em relação a pontuaçao;
	3° refazer o segundo processo de forma mais precisa / lenta em buscad e pegar todas as áreas do resgate;
	4° ir até a saída e terminar o pércurso.
*/
