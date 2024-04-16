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
#include <LiquidCrystal_I2C.h>

#define BOARD_LED 13

#define PREV_PIN 2
#define PREV_LED 3
#define PLAY_PAUSE_PIN 7
#define PLAY_PAUSE_LED 8
#define NEXT_PIN 12
#define NEXT_LED 25 // Hotwired this to PIN 25 on the protoboard
#define CLOCK_PIN 28
#define DATA_PIN 29
#define ENCODER_PIN 30

//LCD I2C address is usually 0x27
//20,4 = 16 chars x 2 line display
LiquidCrystal_I2C lcd(0x27,20,2); 

//Data from PC:
const byte numChars = 32;
char receivedChars[numChars];
char tempChars[numChars];
char rawMessageFromPC[numChars] = {0};
int area = 0;
char area1Label[numChars] = {0};
char area1Value[numChars] = {0};
char area2Label[numChars] = {0};
char area2Value[numChars] = {0};
char area3Label[numChars] = {0};
char area3Value[numChars] = {0};
//float floatFromPC = 0.0;
boolean newData = false;

//10 ms debounce time is good for most pushbuttons.
//Increase this time if a button is too sensitive.
//At times, the arcade buttons seem too sensitive.
Bounce previousButton = Bounce(PREV_PIN, 10);
Bounce playPauseButton = Bounce(PLAY_PAUSE_PIN, 10);
Bounce nextButton = Bounce(NEXT_PIN, 10);
Bounce encoderButton = Bounce(ENCODER_PIN, 10);

int VolumeChange = 0;
bool Mute = false;

void setup() {

  pinMode(BOARD_LED, OUTPUT);
  pinMode(PREV_LED, OUTPUT);
  pinMode(PLAY_PAUSE_LED, OUTPUT);
  pinMode(NEXT_LED, OUTPUT);
  
  pinMode(PREV_PIN, INPUT_PULLUP);
  pinMode(PLAY_PAUSE_PIN, INPUT_PULLUP);
  pinMode(NEXT_PIN, INPUT_PULLUP);
  pinMode(ENCODER_PIN, INPUT);
  pinMode(CLOCK_PIN, INPUT);
  pinMode(DATA_PIN, INPUT);

  //We're using arcade buttons with LEDs, let's light 'em up:
  digitalWrite(PREV_LED, HIGH);
  digitalWrite(PLAY_PAUSE_LED, HIGH);
  digitalWrite(NEXT_LED, HIGH);
  
  lcd.init();
  lcd.backlight();
  Serial.begin(9600);
}

void loop() {
  previousButton.update();
  playPauseButton.update();
  nextButton.update();
  encoderButton.update();

  CheckForMessages();
  if (newData == true) {
      // this temporary copy is necessary to protect the original data
      // strtok() used in parseData() replaces the | with \0
      strcpy(tempChars, receivedChars);
      parseData();
      showParsedData();
      newData = false;
  }
  if (previousButton.fallingEdge()) {
    Keyboard.press(KEY_MEDIA_PREV_TRACK);
    Keyboard.release(KEY_MEDIA_PREV_TRACK);
    Blink(BOARD_LED, 100);
  }
  if (playPauseButton.fallingEdge()) {
    Keyboard.press(KEY_MEDIA_PLAY_PAUSE);
    Keyboard.release(KEY_MEDIA_PLAY_PAUSE);
    Blink(BOARD_LED, 100);
  }
  if (nextButton.fallingEdge()) {
    Keyboard.press(KEY_MEDIA_NEXT_TRACK);
    Keyboard.release(KEY_MEDIA_NEXT_TRACK);
    Blink(BOARD_LED, 100);
  }
  if (encoderButton.fallingEdge()) {
    Keyboard.press(KEY_MEDIA_MUTE);
    Keyboard.release(KEY_MEDIA_MUTE);
    Blink(BOARD_LED, 100);
  }
  //Rotary Encoder:
  VolumeChange = CheckVolume();
  if (VolumeChange < 0) {
    //Serial.println("Descrease");
    Keyboard.press(KEY_MEDIA_VOLUME_DEC);
    Keyboard.release(KEY_MEDIA_VOLUME_DEC);
  }
  else if (VolumeChange > 0) {
    //Serial.println("Increase");
    Keyboard.press(KEY_MEDIA_VOLUME_INC);
    Keyboard.release(KEY_MEDIA_VOLUME_INC);
  }
}

void CheckForMessages() {
  static boolean recvInProgress = false;
  static byte ndx = 0;
  char startMarker = '<';
  char endMarker = '>';
  char input;
  /*
    Incoming data is formatted as:
    <Volume|MuteStatus|DeviceName>
    < = start marker
    | = data delimiter
    > = end marker 
  */
  while (Serial.available() > 0 && newData == false) {
    input = Serial.read();
    if (recvInProgress == true) {
      if (input != endMarker) {
        receivedChars[ndx] = input;
        ndx++;
        if (ndx >= numChars) {
          ndx = numChars - 1;
        }
      }
      else {
        receivedChars[ndx] = '\0'; //terminate the string
        recvInProgress = false;
        ndx = 0;
        newData = true;
      }
    }
    else if (input == startMarker) {
      recvInProgress = true;
    }
  }
}

/* We are sending the Arduino a | delimited string */
void parseData() {
  //Used by strtok() as an index
  char * strtokIndx; 

  //Get the first part of the message:
  strtokIndx = strtok(tempChars, "|");
  strcpy(area1Value, strtokIndx);

  //Continue where we left it:
  strtokIndx = strtok(NULL, "|");
  strcpy(area2Value, strtokIndx);

  //Continue where we left it:
  strtokIndx = strtok(NULL, "|");
  strcpy(area3Value, strtokIndx);
}

void showParsedData() {
  UpdateLCD();
  //Moved the blinking for the volume to here.
  //This way the board blinks when volumen change
  //is initiated on the usb host side, too.
  Blink(BOARD_LED, 20);
}

void Blink(int PIN,int HowLong){
  digitalWrite(PIN, HIGH);
  delay(HowLong);
  digitalWrite(PIN, LOW);
  Serial.println("Blink");
}

void UpdateLCD() {
  /*
  16x2 I2C LCD
  Areas 1, 2, 3:
  ------------------
  |1111111122222222|
  |3333333333333333|
  ------------------
  */


// byte smiley[8] {
// B00000,
// B10001,
// B00000,
// B00000,
// B10001,
// B01110,
// B00000,
// };
// lcd.createChar(0, smiley);
// lcd.begin(16, 2);
// lcd.write(byte(0));
// lcd.createChar(40,30);

  //Area 1
  //Row 0, 0 to 7
  //Label is 0 to 3
  //Value is 4 to 7
  lcd.setCursor(0,0);
  lcd.print("Vol ");
  lcd.printf("%-4s", area1Value);

  //Area 2
  //Row 0, 8 to 15
  //Label is 8 to 11
  //Value is 12 to 15
  lcd.setCursor(8,0);
  if (strcmp(area2Value, "On") == 0) {
    lcd.print("Mute On ");
  }
  else {
    lcd.print("        ");
  }
  
  //Area 3 is Row 1, 0 to 15
  //Centering the device name:
  int temp = strlen(area3Value);
  lcd.setCursor((16 - temp) / 2,1);
  lcd.print(area3Value);
}

int CheckVolume() {
  static uint16_t state = 0;
  delayMicroseconds(100);
  state = (state<<1) | digitalRead(CLOCK_PIN) | 0xe000;
  if (state==0xf000) {
    state=0x0000;
    if (digitalRead(DATA_PIN)) {
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