#include <TroykaIMU.h>
#include <TroykaMeteoSensor.h>

Barometer barometer;
TroykaMeteoSensor meteoSensor;

void setup() {
Serial.begin(115200);
while(!Serial) {
}
Serial.println("Инициализация последовательного порта");
meteoSensor.begin();
Serial.println("инициализируется...");
delay(1000);
Serial.begin(9600);
Serial.println("Начало инициализации...");
barometer.begin();
Serial.println("Инициализация завершена!");
}

void loop() {
int stateSensor = meteoSensor.read();
float pressureMillimetersHg = barometer.readPressureMillimetersHg();
float temperature = barometer.readTemperatureC();
switch (stateSensor) {
case SHT_OK:

Serial.print("Температура = ");
Serial.print(meteoSensor.getTemperatureC());
Serial.println(" C \t");
Serial.print("Влажность = ");
Serial.print(meteoSensor.getHumidity());
Serial.println(" %\r\n\t");
Serial.print(pressureMillimetersHg);
Serial.print(" mmHg\t");
Serial.print("Температура: ");
Serial.print(temperature);
Serial.println(" C");
break;
case SHT_ERROR_DATA:
Serial.println("Ошибка");
break;
case SHT_ERROR_CHECKSUM:
Serial.println("Ошибка контрольной суммы!");
break;
}
delay(1000);
}