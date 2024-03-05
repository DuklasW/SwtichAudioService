# SwitchAudioByArduino

## Opis Projektu
SwitchAudioByArduino to usługa napisana w języku C# przy użyciu platformy .NET Framework, która umożliwia automatyczne przełączanie domyślnego urządzenia dźwiękowego na systemie Windows przy otrzymaniu sygnału od urządzenia Arduino. Usługa odbiera komunikaty od Arduino poprzez port COM i wykonuje przełączanie na kolejne aktywne urządzenie odtwarzające dźwięk.

## Instalacja
1. Skompiluj projekt w środowisku Visual Studio.
2. Uruchom projekt instalatora, aby zainstalować usługę na systemie Windows.

## Konfiguracja
W pliku `App.config` znajdują się ustawienia konfiguracyjne, które można dostosować według własnych potrzeb:
- `PortCOM`: Numer portu COM, na którym usługa będzie odbierać dane od Arduino.
- `BautRate`: Szybkość transmisji (baud rate) dla połączenia szeregowego.

## Uruchamianie
Po zainstalowaniu, usługa będzie uruchamiana automatycznie przy starcie systemu. Można ją również uruchomić, zatrzymać lub zrestartować za pomocą menedżera usług systemowych (`services.msc`).

## Użycie
Po uruchomieniu, usługa nasłuchuje na określonym porcie COM na sygnały od Arduino. Gdy otrzyma komunikat "SWITCH", automatycznie przełącza domyślne urządzenie dźwiękowe na kolejne aktywne urządzenie odtwarzające dźwięk.

## Integracja z Arduino
Projekt zawiera plik `Arduino1Swtich.ino`, który zawiera program, którym zaprogramowałeś swoje Arduino do współpracy z usługą. Twoje Arduino powinno być podłączone do komputera za pomocą portu COM, a na pinie 9 należy podłączyć przełącznik, który służy do wysyłania sygnałów do usługi. Ponadto, dioda podłączona do pinu 6 może służyć do wizualnego potwierdzenia stanu przełącznika.

## Zależności
- AudioSwitcher.AudioApi: biblioteka do zarządzania urządzeniami audio w systemie Windows.

## Autor
Autor: Wojciech Duklas
