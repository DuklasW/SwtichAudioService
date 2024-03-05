const int switchACC1 = 9;
const int ledPin = 6;
int switchState = HIGH;  // Początkowy stan przycisku
int oldSwitchState = HIGH;
unsigned long lastDebounceTime = 0;
unsigned long debounceDelay = 50;
bool initialized = false;  // Flaga określająca, czy Arduino jest już zainicjowane

void setup() {
  pinMode(ledPin, OUTPUT);
  pinMode(switchACC1, INPUT_PULLUP);
  Serial.begin(9600);
}

void loop() {
  int reading = digitalRead(switchACC1);

  if (!initialized) {
    switchState = reading;
    initialized = true;
  }

  if (reading != oldSwitchState) {
    lastDebounceTime = millis();
  }

  if ((millis() - lastDebounceTime) > debounceDelay) {
    if (reading != switchState) {
      switchState = reading;
      Serial.println("SWITCH");
      if (switchState == LOW) {
        digitalWrite(ledPin, HIGH);
        //Serial.println("ON");
      } else {
        digitalWrite(ledPin, LOW);
        //Serial.println("OFF");
      }
    }
  }

  oldSwitchState = reading;
}
