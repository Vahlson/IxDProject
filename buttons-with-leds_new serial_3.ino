//#include <BlueFairy.h>
//bluefairy::Scheduler scheduler;

#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
#include <avr/power.h>  // Required for 16 MHz Adafruit Trinket
#endif

#define NUM_LEDS 30  //total number of LEDs on strip
#define PIN8 8
#define PIN6 6
#define PIN4 4

//GLOBAL
double timePressed[] = {0, 0, 0};
const double lightUpTime = 200;

int inByte = 0;
int buttonInPorts[] = {3, 5, 7, 9, 10, 11, 12 };  //testing

Adafruit_NeoPixel strip8 = Adafruit_NeoPixel(NUM_LEDS, PIN8, NEO_GRB + NEO_KHZ800);  //# of pixels
Adafruit_NeoPixel strip6 = Adafruit_NeoPixel(NUM_LEDS, PIN6, NEO_GRB + NEO_KHZ800);  //# of pixels
Adafruit_NeoPixel strip4 = Adafruit_NeoPixel(NUM_LEDS, PIN4, NEO_GRB + NEO_KHZ800);  //# of pixels


// int switchState3 = 0;  // variable for reading the pushbutton status
// const int LIMIT_SWITCH_PIN3 = 3;


void setup() {
  Serial.begin(9600);  //initialize serial comm. at 9600 bits per second
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }

#if defined(__AVR_ATtiny85__) && (F_CPU == 16000000)
  clock_prescale_set(clock_div_1);
#endif
  //END of Trinket-specific code.

  strip4.begin();  // INITIALIZE NeoPixel strip object (REQUIRED)
  strip4.show();   // Turn OFF all pixels ASAP
  strip4.setBrightness(50);

  strip6.begin();  // INITIALIZE NeoPixel strip object (REQUIRED)
  strip6.show();   // Turn OFF all pixels ASAP
  strip6.setBrightness(50);

  strip8.begin();  // INITIALIZE NeoPixel strip object (REQUIRED)
  strip8.show();   // Turn OFF all pixels ASAP
  strip8.setBrightness(50);

  // pinMode(3, INPUT); //switches
  // pinMode(5, INPUT);
}

void loop() {
  //   if(Serial.available() > 0) //Check when serial command comes thru serial line
  //   {
  //     inByte = Serial.read();  // Puls off first byte in line
  //   }

  ///// switch states for initiating switches for LED strips 6 and 4
  //bool buttonLED3 = false; //reading switch state.   //////just need 0 or 1, delete later
  //bool buttonLED5 = false; //stored in variables
  // bool buttonLED7 = false;

  int combinedButtonState = 0;
  for (int i = 0; i < sizeof(buttonInPorts) / sizeof(int); i++) {
    int buttonState = digitalRead(buttonInPorts[i]);
    int buttonStateCopy = buttonState;


    if (i == 0) {
      if (buttonStateCopy != 0) {
        // buttonLED3 = true;
        timePressed[0] = millis();
        strip4.fill(strip4.Color(0, 255, 0), 0);  //what other colors are built into the library?
        strip4.show();
      } else {
        //buttonLED3 = false;
      }
    }
    if (i == 1) {
      if (buttonStateCopy != 0) {
        //buttonLED5 = true;
        timePressed[1] = millis();
        strip6.fill(strip6.Color(255, 0, 0), 0);  //what other colors are built into the library?
        strip6.show();
      } else {
        //buttonLED5 = false;
      }
    }
    if (i == 2) {
      if (buttonStateCopy != 0) {
        //buttonLED5 = true;
        timePressed[2] = millis();
        strip8.fill(strip8.Color(0, 0, 255), 0);  //what other colors are built into the library?
        strip8.show();
      } else {
        //buttonLED5 = false;
      }
    }

    // if (i==1 && buttonState & 0x1 !=0) {
    //     buttonLED5 = true;
    // }
    // if (i==2 && buttonState & 0x1 !=0) {
    //     buttonLED7 = true;
    // }

    buttonState = (buttonState & 0x1) << i;
    combinedButtonState = combinedButtonState | buttonState;
  }

  Serial.println(combinedButtonState);
  delay(20);
  //switchState2 = digitalRead(switch2); //reading switch state
  //delay(100);

  if (millis() - timePressed[0] >= lightUpTime && timePressed[0]!=-1) {
    strip4.fill(strip4.Color(0, 0, 0), 0);  //set led4 to black
    strip4.show();
    timePressed[0] = -1;
  }

  if (millis() - timePressed[1] >= lightUpTime && timePressed[1]!=-1) {
    strip6.fill(strip6.Color(0, 0, 0), 0);  //set led6 to black
    strip6.show();
    timePressed[1] = -1;
  }

  if( millis() - timePressed[2] >= lightUpTime  && timePressed[2]!=-1){
    strip8.fill(strip8.Color(0,0,0), 0); //set led8 to black
    strip8.show();
    timePressed[2] = -1;
  }


  // if (buttonLED3) {
  //   //turn LED on:
  //   strip4.fill(strip4.Color(0,255,0), 0); //what other colors are built into the library?
  //   strip4.show();
  //   //delay(50);
  //   //strip4.clear();
  // }
  // else {
  //   strip4.fill(strip4.Color(0,0,0), 0);
  //   strip4.show();

  //   // strip4.fill((0, 0, 0), 1);
  //   // strip4.show();
  // }
  // if (buttonLED5) {
  //   //Serial.println(switchState2);
  //   //turn LED on:
  //   strip6.fill(strip6.Color(255,0,0), 0); //what other colors are built into the library?
  //   strip6.show();
  //   //delay(50);
  //   strip6.clear();
  // }
  // else {
  //   strip4.fill(strip6.Color(0,0,0), 0);
  //   strip6.show();
  // }
}


void establishContact() {
  while (Serial.available() <= 0) {
    //digitalWrite(ledPin, LOW);
    //delay(300);
  }
}