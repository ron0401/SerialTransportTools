#include "WiFi.h"
#include "AsyncUDP.h"
#include <M5StickC.h>


#define BAUD_RATE     9600
#define MYSSID        "ssid"
#define MYPASSWD      "pass"
#define LISTEN_PORT   (int)50000
#define SEND_BUF_SIZE 1024

#define NODATA        -1

const char*         ssid = MYSSID;
const char*         password = MYPASSWD;
static const char*  RemoteIp = "***.***.***.**";
static const int    RemoteUdpPort = 50000;

uint8_t*            readBuffer;
uint8_t             sendBuffer[SEND_BUF_SIZE];
unsigned int        buf_counter = 0;               

AsyncUDP udp;

void setup() {
  // put your setup code here, to run once:
  buf_clear();
  M5.begin();
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
  int buf;
  buf = Serial.read();
  if (buf == NODATA)
  {
    if (buf_counter != 0)
    {
      udp.write(sendBuffer, buf_counter);
      buf_clear();
    }
  }
  else
  {
    sendBuffer[buf_counter] = (uint8_t)buf;
    buf_counter++;
    if (buf_counter >= (SEND_BUF_SIZE - 1))
    {
      udp.write(sendBuffer, buf_counter);
      buf_clear();
    }    
  }
}

void buf_clear() {
  for (int i = 0; i < SEND_BUF_SIZE; i++)
  {
    sendBuffer[i] = 0;
  }      
}
