//Digital Read with Button

int ledPin = 3; //select pin for led
int inByte = 0;
int buttonInPorts[] = {2,3,4,5,10,11,12}; //testing
//int buttonInPorts[] = {9,10,11,12};


void setup() {

  Serial.begin(9600); //initialize serial comm. at 9600 bits per second
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  } 

  //pinMode(ledPin, OUTPUT); //Make led as the output
  //digitalWrite(3,LOW); 
  //establishContact();  // send a byte to establish contact until receiver responfds

}

void loop() {

  if(Serial.available() > 0) //Check when serial command comes thru serial line
  {
    inByte = Serial.read();  // Puls off first byte in line
  }

/////////////////////////////////////////////////////////////READ INPUT Btn x 2

int combinedButtonState = 0;
for(int i = 0; i < sizeof(buttonInPorts)/sizeof(int); i++){
  int buttonState = digitalRead(buttonInPorts[i]);
  buttonState = (buttonState & 0x1) << i;

  combinedButtonState = combinedButtonState | buttonState;
}

   Serial.println(combinedButtonState); //print out the state of the button
  delay(20); //delay in between reads for stability


  ///////////////////////////////////////////////////////////// WRITE OUTPUT VALUES
  //Serial.write(combinedButtonState);

  ////////////////////////////////////////////////////////////
}

void establishContact() {
  while (Serial.available() <= 0) {
    digitalWrite(ledPin, LOW);
    //delay(300);
  }
}
