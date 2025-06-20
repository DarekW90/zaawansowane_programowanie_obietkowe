📚 Biblioteka Konsolowa w C#:

Prosta aplikacja konsolowa do zarządzania biblioteką książek. Umożliwia dodawanie, edytowanie, usuwanie i przeglądanie książek, z wykorzystaniem wzorców projektowych takich jak singleton, interfejsy, dziedziczenie oraz enumeracje.

---

🛠️ Funkcjonalności:

1. Dodawanie książki z wyborem gatunku (w tym opcja własnego gatunku).
2. Edytowanie książki (tytuł, autor, gatunek).
3. Usuwanie książki po ID.
4. Przegląd wszystkich książek w bazie.
5. Wzorzec Singleton jako baza danych.
6. Interfejsy do operacji CRUD oraz logowania użytkownika.
7. Dziedziczenie klasy AdminUser z klasy User.

---

📦 Struktura Kodu:

Plik/Klasa	Opis
- Book - Klasa reprezentująca książkę (ID, tytuł, autor, gatunek, opcjonalny własny gatunek).
- BookGenre	- Enum zawierający gatunki książek.
- User / AdminUser	- Klasy użytkowników, dziedziczenie + interfejs logowania.
- ICrudOperations - Interfejs definiujący operacje CRUD.
- LibraryDatabase - Singleton przechowujący listę książek.
- LibraryManager - Interfejs użytkownika w trybie tekstowym (menu, logika interakcji).
- Program - Punkt wejścia aplikacji (tworzy użytkownika i uruchamia menedżera).

---

▶️ Uruchomienie:
Skorzystaj z Visual Studio, Visual Studio Code (z .NET SDK), lub uruchom w terminalu:

    dotnet run

---

💻 === Library Menu ===
1. Dodaj książkę
2. Usuń książkę
3. Edytuj książkę
4. Wyświetl wszystkie książki
5. Wyjście

---

🔧 Technologie
.NET 6+ / C#
Aplikacja konsolowa
Programowanie obiektowe
Wzorce projektowe (singleton, interfejsy, dziedziczenie)

---

✍️ Autorzy

- Julia Klaine
- Dariusz Walda
- Oskar Walda
- Jakub Daszkiewicz
- Sebastian Sikora