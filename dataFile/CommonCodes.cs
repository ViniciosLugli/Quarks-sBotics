
//Controle atuador escavadeira:
Action<int> garra = (degrees) =>{
	uint timeout = millis() + 2000;
	float local_angle = angulo_atuador();
	if((degrees <= 360) && (degrees >= 289) && (local_angle >= 0) && (local_angle <= 15)){
		while(local_angle >= 0 && local_angle <= 15){
			bc.actuatorUp(1);
			if(millis() > timeout){return;}
			local_angle = angulo_atuador();
		}
	}else if((degrees >= 0) && (degrees <= 11) && (local_angle >= 289) && (local_angle <= 360)){
		while((local_angle >= 289)&& (local_angle <= 360)){
			bc.actuatorDown(1);
			if(millis() > timeout){return;}
			local_angle = angulo_atuador();
		}
	}
	if(degrees > local_angle){
		while(degrees > local_angle){
			bc.actuatorDown(1);
			if(millis() > timeout){return;}
			local_angle = angulo_atuador();
		}
	}else if(degrees < local_angle){
		while(degrees < local_angle){
			bc.actuatorUp(1);
			if(millis() > timeout){return;}
			local_angle = angulo_atuador();
		}
	}
};

Action<int> espatula = (degrees) =>{
	uint timeout = millis() + 2000;
	float local_angle = angulo_espatula();
	if((local_angle >= 0) && (local_angle <= 51) && (degrees <= 360) && (degrees >= 310)){
		while((local_angle >= 0) && (local_angle <= 5)){
			bc.turnActuatorDown(1);
			if(millis() > timeout){return;}
			local_angle = angulo_espatula();
		}
	}else if((local_angle >= 310) && (local_angle <= 360) && (degrees >= 0) && (degrees <= 51)){
		while((local_angle >= 310) && (local_angle <= 360)){
			bc.turnActuatorUp(1);
			if(millis() > timeout){return;}
			local_angle = angulo_espatula();
		}
	}if(degrees > local_angle){
		while(degrees > local_angle){
			bc.turnActuatorDown(1);
			if(millis() > timeout){return;}
			local_angle = angulo_espatula();
		}
	}else if(degrees < local_angle){
		while(degrees < local_angle){
			bc.turnActuatorUp(1);
			if(millis() > timeout){return;}
			local_angle = angulo_espatula();
		}
	}
};
//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *//

//Controle atuador empilhadeira:
Action<int> garra = (degrees) =>{
	uint timeout = millis() + 1200;
	float local_angle = angulo_atuador();
	if(degrees > local_angle){
		while(degrees > local_angle){
			bc.actuatorUp(1);
			if(millis() > timeout){return;}
			local_angle = angulo_atuador();
		}
	}else if(degrees < local_angle){
		while(degrees < local_angle){
			bc.actuatorDown(1);
			if(millis() > timeout){return;}
			local_angle = angulo_atuador();
		}
	}
};

Action<int> espatula = (degrees) =>{
	uint timeout = millis() + 1200;
	float local_angle = angulo_espatula();
	if(degrees > local_angle){
		while(degrees > local_angle){
			bc.turnActuatorDown(1);
			if(millis() > timeout){return;}
			local_angle = angulo_espatula();
		}
	}else if(degrees < local_angle){
		while(degrees < local_angle){
			bc.turnActuatorUp(1);
			if(millis() > timeout){return;}
			local_angle = angulo_espatula();
		}
	}
};
//* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *//