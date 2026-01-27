Opis projektu
Projekt stanowi cyfrową implementację klasycznej gry planszowej Super Farmer, stworzoną jako aplikacja webowa w technologii .NET. Gra pozwala na rywalizację gracza z inteligentnym botem w systemie turowym, odwzorowując matematyczne zasady przyrostu stada oraz mechanikę handlu zwierzętami.

Architektura techniczna
Aplikacja została zbudowana z wykorzystaniem nowoczesnych wzorców programowania w C#:

Blazor InteractiveServer: Zapewnia dynamiczne odświeżanie interfejsu użytkownika bez przeładowywania strony.

GameEngine: Scentralizowany silnik gry zarządzający logiką rzutów, atakami drapieżników oraz stanem magazynu głównego (MainHerd).

Delegatowe mapowanie zasobów: Wykorzystanie słowników z wyrażeniami lambda (Func/Action) do bezstanowego zarządzania inwentarzem graczy, co eliminuje rozbudowane instrukcje warunkowe.

Asynchroniczny system turowy: Implementacja logiki bota przy użyciu Task.Delay oraz programowania asynchronicznego, co pozwala na płynną wizualizację ruchów komputera.

Funkcjonalności
FarmerBot AI: Automatyczny przeciwnik realizujący wielopoziomową strategię handlową, dążący do optymalizacji składu stada i zakupu konia.

System drapieżników: Pełna obsługa zdarzeń losowych (Lis i Wilk) wraz z systemem ochrony stada przez małe i duże psy.

Dynamiczny rynek: Moduł wymiany zwierząt zgodny z oficjalną tabelą wartości (np. 6 królików za 1 owcę).

Zarządzanie zasobami: System limitowanego stada głównego – gracze mogą otrzymać tylko tyle zwierząt, ile aktualnie znajduje się w magazynie.

Stack technologiczny
Język: C#

Framework: .NET Core / Blazor

Interfejs: HTML / CSS (Bootstrap)
