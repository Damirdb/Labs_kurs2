#include <SPI.h> 
#include <AmperkaFET.h> 
#include <sstream> 
#include <iostream> 


FET mosfet(D17, 2); 

void setup() {
mosfet.begin(); 
Serial.begin(9600); 

}

void loop() {
if (Serial.available()) { 
std::stringstream ss;
ss << Serial.readString().c_str(); 
uint32_t modul, key, i; 
int level; 
ss >> modul >> key >> i; 

if (i > 0)
level = HIGH; 
else
level = LOW; 
if (i < 0)
level = LOW; 
else
level = HIGH;

if (modul < 0)
return; 
else if (modul > 1)
modul = 255;
if (key < 0)
key = 0; 
else if (key > 7)
key = 255; 

mosfet.digitalWrite(modul, key, level); 
}
}