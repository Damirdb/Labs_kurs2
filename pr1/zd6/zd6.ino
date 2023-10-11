void setup() {

}

void loop() {
  int potValue = analogRead(A0); 
  
  int brightness = map(potValue, 0, 4095, 0, 255);
  analogWrite(D0, brightness);
  
}

