//Requires the ESP8266 WIFI library
#include <ESP8266WiFi.h>
//Requires the WifiUDP library
#include <WiFiUdp.h>

//Enter the details of your wifi network below
const char* ssid     = "SSID HERE";
const char* password = "PASSWORD HERE";
//set ip
IPAddress IP(192, 168, 0, 160); 
//set Subnet Gateway
IPAddress subnetIP(192, 168, 0, 1); 
//set Subnet Mask
IPAddress subMask(255, 255, 255, 0);

WiFiUDP UDPTestServer;
unsigned int UDPPort = 7777;

const int packetSize = 11;
byte packetBuffer[packetSize];

void setup() {
  Serial.begin(115200);
  delay(10);

  Serial.println();
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);
  
  WiFi.begin(ssid, password);
  WiFi.config(IPAddress(IP), IPAddress(subnetIP), IPAddress(subMask));
  
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");  
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());

  UDPTestServer.begin(UDPPort);
  
}

int value = 0;

void loop() {
   handleUDPServer();
   delay(1);
}

void handleUDPServer() {
  int cb = UDPTestServer.parsePacket();
  if (cb) {
    UDPTestServer.read(packetBuffer, packetSize);
    String myData = ""; 
    for(int i = 0; i < packetSize; i++) {
      myData += (char)packetBuffer[i];
    }
    Serial.println(myData);
  }
}
