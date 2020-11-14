#include "WiFi.h"
#include "AsyncUDP.h"
#include <M5StickC.h>


#define BAUD_RATE   9600
#define MYSSID      "ssid"
#define MYPASSWD    "pass"
#define LISTEN_PORT 50000


const char * ssid = MYSSID;
const char * password = MYPASSWD;

uint8_t * readBuffer;

AsyncUDP udp;

void setup() {
  // put your setup code here, to run once:
  M5.begin();
  M5.Lcd.setRotation(3);
  M5.Lcd.fillScreen(BLACK);
  Serial.begin(BAUD_RATE);
  WiFi.mode(WIFI_STA);
  WiFi.begin(ssid, password);
  if (WiFi.waitForConnectResult() != WL_CONNECTED) {
      while(1) {
          delay(1000);
      }
  }
  if(udp.listen(LISTEN_PORT)) {
      udp.onPacket([](AsyncUDPPacket packet) {
        readBuffer  = packet.data();
        Serial.print(*readBuffer);
      });
  }
}

void loop() {
  // put your main code here, to run repeatedly:

}
