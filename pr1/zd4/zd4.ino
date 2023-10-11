int g = 0;

void setup() {
  pinMode(D0, OUTPUT);  // lamp
  pinMode(D1, OUTPUT);
  pinMode(D2, OUTPUT);
  pinMode(D3, INPUT);   // button
  attachInterrupt(digitalPinToInterrupt(D3), changeState, RISING);  // Привязываем прерывание к пину D3
}

void loop() {
  // Ничего не делаем в основном цикле, прерывание обрабатывает изменения состояния кнопки
}

void changeState() {
  g++;
  switch (g) {
    case 1:
      digitalWrite(D0, HIGH);
      digitalWrite(D1, LOW);
      digitalWrite(D2, LOW);
      delay(500);
      break;
    
    case 2:
      digitalWrite(D0, LOW);
      digitalWrite(D1, HIGH);
      digitalWrite(D2, LOW);
      delay(500);
      break;
    
    case 3:
      digitalWrite(D0, LOW);
      digitalWrite(D1, LOW);
      digitalWrite(D2, HIGH);
      delay(500);
      break;
      
    default:
      if (g > 3) {
        g = 0;
      }
  }
}