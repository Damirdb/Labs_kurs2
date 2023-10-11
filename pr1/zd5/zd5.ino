int g = 0;
unsigned long previousMillis = 0;  // переменная для хранения времени последнего изменения состояния светодиода
const int interval = 500;  // интервал времени между сменой состояний светодиода (в миллисекундах)

void setup() {
  pinMode(D0, OUTPUT);  // lamp
  pinMode(D1, OUTPUT);
  pinMode(D2, OUTPUT);
}

void loop() {
  unsigned long currentMillis = millis();  // получаем текущее время
  
  if (currentMillis - previousMillis >= interval) {
    // сохраняем текущее время
    previousMillis = currentMillis;

    // выключаем все светодиоды
    digitalWrite(D0, LOW);
    digitalWrite(D1, LOW);
    digitalWrite(D2, LOW);

    // увеличиваем счетчик
    g++;

    // проверяем значение счетчика и устанавливаем состояние светодиода соответственно
    if (g == 1) {
      digitalWrite(D0, HIGH);
    } else if (g == 2) {
      digitalWrite(D1, HIGH);
    } else if (g == 3) {
      digitalWrite(D2, HIGH);
    } else {
      // если счетчик больше 3, сбрасываем его
      g = 0;
    }
  }
}