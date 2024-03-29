    #include <WiFi.h>
    #include <WebSocketsServer.h>
    #include <Wire.h>
    #include <SHT31.h>
    #include <LPS.h>
    #include <string>

    WebSocketsServer webSocket = WebSocketsServer(81);

    SHT31 sht31;
    LPS lps25;

    std::string ssid = "ASOIU";
    std::string password = "kaf.asoiu.48";

    void setup() {
    Serial.begin(115200);
    delay(2000);

    WiFi.begin(ssid.c_str(), password.c_str());
    while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Подключение к WiFi...");
    }

    Serial.println("Подключено к WiFi");

    webSocket.begin();
    webSocket.onEvent(webSocketEvent);
    }

    void loop() {
    webSocket.loop();
    float temperature = sht31.getTemperature();
    float humidity = sht31.getHumidity();
    float pressure = lps25.readPressureRaw();
    String data = "{\"temperature\":" + String(temperature) + ",\"humidity\":" + String(humidity) + ",\"pressure\":" + String(pressure) + "}";
    webSocket.broadcastTXT(data);
    delay(1000);
    }

    void webSocketEvent(uint8_t num, WStype_t type, uint8_t *payload, size_t length) {
    uint32_t eventNum = num;
    // Your WebSocket event handling code here using eventNum as uint32_t
    }

