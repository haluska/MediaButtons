/* 
 * MediaButtons ProtoboardLCD
 * github.com/EricThirteen
 * 
 * Buttons to USB Keyboard Example - Special Media Player Keys
 * Set USB Type: Tools > USB Type > Serial + Keyboard + Mouse + Joystick
 * 
 * Modified existing example from Teensy: 
 * Buttons to USB Keyboard Example - Special Media Player Keys
 * 
 * Added code for: 
 * - Rotary encoder (Inland brand with integrated button, don't but it)
 * - LCD I2C to display data from serial port
 * - Other stuff
 */

#include <Bounce.h>
#include <Wire.h> 

// #include <LiquidCrystal_I2C.h>

#define BOARD_LED 13

// #define PREV_PIN 2
// #define PREV_LED 3
// #define PLAY_PAUSE_PIN 7
// #define PLAY_PAUSE_LED 8
#define NEXT_PIN 12
#define NEXT_LED 25 // Hotwired this to PIN 25 on the protoboard
#define CLOCK_PIN 28
#define DATA_PIN 29
#define ENCODER_PIN 30

//LCD I2C address is usually 0x27
//20,4 = 16 chars x 2 line display
// LiquidCrystal_I2C lcd(0x27,20,4);  


//Data from PC:
// const byte numChars = 32;
// char receivedChars[numChars];
// char tempChars[numChars];
// char messageFromPC[numChars] = {0};
// int integerFromPC = 0;
// float floatFromPC = 0.0;
// boolean newData = false;

//10 ms debounce time is good for most pushbuttons.
//Increase this time if a button is too sensitive.
//At times, the arcade buttons seem too sensitive.
//Bounce previousButton = Bounce(PREV_PIN, 10);
//Bounce playPauseButton = Bounce(PLAY_PAUSE_PIN, 10);
Bounce nextButton = Bounce(NEXT_PIN, 10);
Bounce encoderButton = Bounce(ENCODER_PIN, 10);

int VolumeChange = 0;
//bool Mute = false;

void setup() {
  
  pinMode(BOARD_LED, OUTPUT);
  //pinMode(PREV_LED, OUTPUT);
  //pinMode(PLAY_PAUSE_LED, OUTPUT);
  pinMode(NEXT_LED, OUTPUT);
  
  //pinMode(PREV_PIN, INPUT_PULLUP);
  //pinMode(PLAY_PAUSE_PIN, INPUT_PULLUP);
  pinMode(NEXT_PIN, INPUT_PULLUP);
  pinMode(ENCODER_PIN, INPUT);
  pinMode(CLOCK_PIN, INPUT);
  pinMode(DATA_PIN, INPUT);

  //We're using arcade buttons with LEDs, let's light 'em up:
  //digitalWrite(PREV_LED, HIGH);
  //digitalWrite(PLAY_PAUSE_LED, HIGH);
  digitalWrite(NEXT_LED, HIGH);
  
  //lcd.init();
  //lcd.backlight();
  //Serial.begin(9600);
}

void loop() {
  //previousButton.update();
  //playPauseButton.update();
  nextButton.update();
  encoderButton.update();
  // CheckForMessages();
  // if (newData == true) {
  //     // this temporary copy is necessary to protect the original data
  //     // strtok() used in parseData() replaces the | with \0
  //     strcpy(tempChars, receivedChars);
  //     parseData();
  //     showParsedData();
  //     newData = false;
  // }
  
  // if (previousButton.fallingEdge()) {
  //   Keyboard.press(KEY_MEDIA_PREV_TRACK);
  //   Keyboard.release(KEY_MEDIA_PREV_TRACK);
  //   Blink(BOARD_LED, 100);
  // }
  // if (playPauseButton.fallingEdge()) {
  //   Keyboard.press(KEY_MEDIA_PLAY_PAUSE);
  //   Keyboard.release(KEY_MEDIA_PLAY_PAUSE);
  //   Blink(BOARD_LED, 100);
  // }
  if (nextButton.fallingEdge()) {
    Keyboard.press(KEY_MEDIA_MUTE);
    Keyboard.release(KEY_MEDIA_MUTE);
    Blink(BOARD_LED, 100);
  }
  if (encoderButton.fallingEdge()) {
    Keyboard.press(KEY_MEDIA_MUTE);
    Keyboard.release(KEY_MEDIA_MUTE);
    Blink(BOARD_LED, 100);
  }
  //Rotary Encoder:
  VolumeChange = CheckVolume();
  if (VolumeChange < 0){
    Serial.println("Descrease");
    Keyboard.press(KEY_MEDIA_VOLUME_DEC);
    Keyboard.release(KEY_MEDIA_VOLUME_DEC);
  }
  else if (VolumeChange > 0){
    Serial.println("Increase");
    Keyboard.press(KEY_MEDIA_VOLUME_INC);
    Keyboard.release(KEY_MEDIA_VOLUME_INC);
  }
}

/* Incoming data is formatted. Example:
 * <Volume|72|1.45>
 * 
 * < = start marker
 * | = data delimiter
 * > = end marker */
// void CheckForMessages(){
//   static boolean recvInProgress = false;
//   static byte ndx = 0;
//   char startMarker = '<';
//   char endMarker = '>';
//   char rc;

//   while (Serial.available() > 0 && newData == false){
//     rc = Serial.read();
//     if (recvInProgress == true) {
//       if (rc != endMarker) {
//         receivedChars[ndx] = rc;
//         ndx++;
//         if (ndx >= numChars) {
//           ndx = numChars - 1;
//         }
//       }
//       else {
//         receivedChars[ndx] = '\0'; //terminate the string
//         recvInProgress = false;
//         ndx = 0;
//         newData = true;
//       }
//     }
//     else if (rc == startMarker) {
//       recvInProgress = true;
//     }
//   }
// }

/* We are sending the Arduino a | delimited string and
 * parsing that as a string, int, float */
// void parseData(){
//   //Used by strtok() as an index
//   char * strtokIndx; 

//   //Get the first part of the message:
//   strtokIndx = strtok(tempChars,"|");
//   strcpy(messageFromPC, strtokIndx);

//   //Continue where we left it:
//   strtokIndx = strtok(NULL, "|");
//   //Convert from int:
//   integerFromPC = atoi(strtokIndx);

//   //Continue where we left it:
//   strtokIndx = strtok(NULL, "|");
//   //Convert from float:
//   floatFromPC = atof(strtokIndx);       
// }

// void showParsedData(){
//   UpdateLCD(messageFromPC);
//   //Moved the blinking for the volume to here.
//   //This way the board blinks when volumen change
//   //is initiated on the usb host side, too.
//   Blink(BOARD_LED, 20);
// }

void Blink(int PIN,int HowLong){
  digitalWrite(PIN, HIGH);
  delay(HowLong);
  digitalWrite(PIN, LOW);
  Serial.println("Blink");
}

// void UpdateLCD(String Message){
//   //lcd.autoscroll();
//   lcd.clear();
//   lcd.setCursor(0,0);
//   lcd.print(Message + integerFromPC);
// }

int CheckVolume(){
  static uint16_t state = 0;
  delayMicroseconds(100);
  state = (state<<1) | digitalRead(CLOCK_PIN) | 0xe000;
  if (state==0xf000){
    state=0x0000;
    if(digitalRead(DATA_PIN)){
      return 1;
    }
    else {
      return -1;
    }
  }
  else {
    return 0;
  }
}
