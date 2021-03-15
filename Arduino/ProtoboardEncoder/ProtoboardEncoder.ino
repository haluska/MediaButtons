/* 
 * MediaButtons ProtoboardEncoder
 * github.com/EricThirteen
 * 
 * Buttons to USB Keyboard Example - Special Media Player Keys
 * Set USB Type: Tools > USB Type > Serial + Keyboard + Mouse + Joystick
 * 
 * Modified existing example from Teensy.
 * Added code for: 
 * - Rotary encoder (Inland brand with integrated button, don't but it)
 * - Other stuff
 */

#include <Bounce.h>

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
}

void loop() {
  previousButton.update();
  playPauseButton.update();
  nextButton.update();
  encoderButton.update();

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
  if (VolumeChange < 0){
    Serial.println("Descrease");
    Keyboard.press(KEY_MEDIA_VOLUME_DEC);
    Keyboard.release(KEY_MEDIA_VOLUME_DEC);
    //Keep this short so it doesn't interfere with a quick twist of the encoder:
    Blink(BOARD_LED, 25);
  }
  else if (VolumeChange > 0){
    Serial.println("Increase");
    Keyboard.press(KEY_MEDIA_VOLUME_INC);
    Keyboard.release(KEY_MEDIA_VOLUME_INC);
    //Keep this short so it doesn't interfere with a quick twist of the encoder:
    Blink(BOARD_LED, 25);
  }
}

void Blink(int PIN,int HowLong){
  digitalWrite(PIN, HIGH);
  delay(HowLong);
  digitalWrite(PIN, LOW);
  Serial.println("Blink");
}

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
