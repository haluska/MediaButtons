/*  
 * MediaButtons ProtoboardEncoder
 * github.com/EricThirteen
 * 
 * Send Media Button keystroke to USB host. Tested on Windows and MacOS.
 * 
 * Buttons to USB Keyboard Example - Special Media Player Keys
 * Set USB Type: Tools > USB Type > Serial + Keyboard + Mouse + Joystick
 * 
 * Modified existing example from Teensy.
 */

#include <Bounce.h>

#define BOARD_LED 13
#define PREV_PIN 27
#define PLAY_PAUSE_PIN 28
#define NEXT_PIN 29
#define VOL_DOWN_PIN 30
#define MUTE_PIN 31
#define VOL_UP_PIN 32

//10 ms debounce time is good for most pushbuttons.
//Increase this time if a button is too sensitive.
//At times, the arcade buttons seem to sensitive.
Bounce previousButton = Bounce(PREV_PIN, 10);
Bounce playPauseButton = Bounce(PLAY_PAUSE_PIN, 10);
Bounce nextButton = Bounce(NEXT_PIN, 10);
Bounce volDownButton = Bounce(VOL_DOWN_PIN, 10);
Bounce volMuteButton = Bounce(MUTE_PIN, 10);
Bounce volUpButton = Bounce(VOL_UP_PIN, 10);

void setup() {

  pinMode(BOARD_LED, OUTPUT);
  pinMode(PREV_PIN, INPUT_PULLUP);
  pinMode(PLAY_PAUSE_PIN, INPUT_PULLUP);
  pinMode(NEXT_PIN, INPUT_PULLUP);
  pinMode(VOL_DOWN_PIN, INPUT_PULLUP);
  pinMode(MUTE_PIN, INPUT_PULLUP);
  pinMode(VOL_UP_PIN, INPUT_PULLUP);
}

void loop() {
  previousButton.update();
  playPauseButton.update();
  nextButton.update();
  volDownButton.update();
  volMuteButton.update();
  volUpButton.update();

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
  if (volDownButton.fallingEdge()) {
    Keyboard.press(KEY_MEDIA_VOLUME_DEC);
    Keyboard.release(KEY_MEDIA_VOLUME_DEC);
    Blink(BOARD_LED, 100);
  }
  if (volMuteButton.fallingEdge()) {
    Keyboard.press(KEY_MEDIA_MUTE);
    Keyboard.release(KEY_MEDIA_MUTE);
    Blink(BOARD_LED, 100);
  }
  if (volUpButton.fallingEdge()) {
    Keyboard.press(KEY_MEDIA_VOLUME_INC);
    Keyboard.release(KEY_MEDIA_VOLUME_INC);
    Blink(BOARD_LED, 100);
  }
}

void Blink(int PIN,int HowLong) {
  digitalWrite(PIN, HIGH);
  delay(HowLong);
  digitalWrite(PIN, LOW);
  Serial.println("Blink");
}
