void setup() {
  // put your setup code here, to run once:
pinMode(D0,OUTPUT); // lamp
pinMode(D3,INPUT); // button
}

void loop() {
if ((digitalRead(D3)) == LOW) {
  digitalWrite(D0,HIGH);
}
else {
  digitalWrite(D0,LOW);
  
  }
}
