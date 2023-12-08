#include <Servo.h> 
Servo servo1;
int potPin = A0;  
int servoPin = D0; 
int valpot = 0; 
int angleServo = 0; 
void setup()
{
servo1.attach(servoPin,600,2600);
}
void loop()
{
valpot = analogRead(potPin); 
angleServo=map(valpot,0,4095,0,255);

servo1.write(angleServo);
delay(15); 
}