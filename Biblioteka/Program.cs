using System;
using System.Collections.Generic;

// ENUM
public enum BookGenre
{
    Fantastyka,
    ScienceFiction,
    Biografia,
    Historia,
    Romans,
    Thriller,
    Kryminał,
    Horror,
    Poezja,
    Dzieci,
    Własny
}

// INTERFEJSY
public interface ICrudOperations<T>
{
    void Add(T item);
    void Remove(int id);
    void Edit(int id, T newItem);
    T Read(int id);
    List<T> ReadAll();
}

public interface IUserActions
{
    void LogIn();
    void LogOut();
}

// KLASA BOOK (1/5)
public class Book
{
    private static int _nextId = 1;
    public int Id { get; private set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public BookGenre Genre { get; set; }
    

    public Book(string title, string author, BookGenre genre)
    {
        Id = _nextId++;
        Title = title;
        Author = author;
        Genre = genre;
    }

    // public override string ToString()
    // {
    //     return $"{Id}: \"{Title}\" by {Author} [{Genre}]";
    // }

    public string? CustomGenreName { get; set; }

        public Book(string title, string author, BookGenre genre, string? customGenreName = null)
        {
            Id = _nextId++;
            Title = title;
            Author = author;
            Genre = genre;
            CustomGenreName = customGenreName;
        }

        public override string ToString()
        {
            string genreText = Genre == BookGenre.Własny ? (CustomGenreName ?? "Własny") : Genre.ToString();
            return $"{Id}: {Title} by {Author} ({genreText})";
        }
}
// KLASA USER (DZIEDZICZENIE) (2/5) (Dziedziczenie i implementacja)
public class User : IUserActions
{
    public string Username { get; set; }

    public User(string username)
    {
        Username = username;
    }

    public virtual void LogIn()
    {
        Console.WriteLine($"{Username} zalogował się.");
    }

    public virtual void LogOut()
    {
        Console.WriteLine($"{Username} wylogował się.");
    }
}

// KLASA ADMIN USER (DZIEDZICZENIE) (3/5)  (Dziedziczenie i implementacja)
public class AdminUser : User
{
    public AdminUser(string username) : base(username) { }

    public override void LogIn()
    {
        Console.WriteLine($"Admin {Username} logged in with full access.");
    }

    public void ManageLibrary()
    {
        Console.WriteLine("Admin is managing the library.");
    }
}

// SINGLETON - BAZA DANYCH (singleton jako DB)
public class LibraryDatabase : ICrudOperations<Book>
{
    private static LibraryDatabase _instance;
    private List<Book> _books;

    private LibraryDatabase()
    {
        _books = new List<Book>();
    }

    public static LibraryDatabase Instance
    {
        get
        {
            if (_instance == null)
                _instance = new LibraryDatabase();
            return _instance;
        }
    }

    public void Add(Book item)
    {
        _books.Add(item);
    }

    public void Remove(int id)
    {
        _books.RemoveAll(b => b.Id == id);
    }

    public void Edit(int id, Book newItem)
    {
        var book = _books.Find(b => b.Id == id);
        if (book != null)
        {
            book.Title = newItem.Title;
            book.Author = newItem.Author;
            book.Genre = newItem.Genre;
        }
    }


    public Book Read(int id)
    {
        return _books.Find(b => b.Id == id);
    }

    public List<Book> ReadAll()
    {
        return new List<Book>(_books);
    }
}

// KLASA ZARZĄDZAJĄCA BIBLIOTEKĄ I INTERFEJSEM (4/5)
public class LibraryManager
{
    private LibraryDatabase _db = LibraryDatabase.Instance;

    public void Run()
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("\n=== Library Menu ===");
            Console.WriteLine("1. Dodaj książkę");
            Console.WriteLine("2. Usuń książkę");
            Console.WriteLine("3. Edytuj książkę");
            Console.WriteLine("4. Wyświetl wszystkie książki");
            Console.WriteLine("5. Wyjście");
            Console.Write("Wybierz opcję: ");

            switch (Console.ReadLine())
            {
                case "1":
                    AddBook();
                    break;
                case "2":
                    RemoveBook();
                    break;
                case "3":
                    EditBook();
                    break;
                case "4":
                    ListBooks();
                    break;
                case "5":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Wybrano nie poprawnie, spróbuj ponownie.");
                    break;
            }
        }
    }

    private void AddBook()
    {
        Console.Write("Tytuł: ");
        string title = Console.ReadLine();

        Console.Write("Autor: ");
        string author = Console.ReadLine();

        Console.WriteLine("Typ:");
        var genres = Enum.GetValues(typeof(BookGenre));
        for (int i = 0; i < genres.Length; i++)
        {
            Console.WriteLine($"{i} - {genres.GetValue(i)}");
        }
        Console.Write("Wybierz typ książki po numerze: ");
        int genreNum = int.Parse(Console.ReadLine());
        BookGenre genreEnum = (BookGenre)genreNum;

        string? customGenreName = null;
        if (genreEnum == BookGenre.Własny)
        {
            Console.Write("Podaj własny typ książki: ");
            customGenreName = Console.ReadLine();
        }

        Book book = new Book(title, author, genreEnum, customGenreName);
        _db.Add(book);
        Console.WriteLine("Książka dodana!");
    }


    private void RemoveBook()
    {
        Console.Write("Wprowadź ID książki by usunąć: ");
        int id = int.Parse(Console.ReadLine());
        _db.Remove(id);
        Console.WriteLine($"Książka o ID {id} została usunięta.");
    }

    private void EditBook()
    {
        Console.Write("Wprowadź ID książki by ją edytować: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Nieprawidłowe ID.");
            return;
        }

        var book = _db.Read(id);
        if (book == null)
        {
            Console.WriteLine("Książka nie została znaleziona.");
            return;
        }

        Console.Write($"Nowy tytuł (pozostaw puste by nie zmieniać \"{book.Title}\"): ");
        string newTitle = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newTitle)) newTitle = book.Title;

        Console.Write($"Nowy autor (pozostaw puste by nie zmieniać \"{book.Author}\"): ");
        string newAuthor = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(newAuthor)) newAuthor = book.Author;

        Console.WriteLine("Typ:");
        foreach (var genre in Enum.GetValues(typeof(BookGenre)))
        {
            Console.WriteLine($"{(int)genre} - {genre}");
        }
        Console.Write($"Wybierz nowy typ książki (pozostaw puste by nie zmieniać {book.Genre}): ");
        string input = Console.ReadLine();

        BookGenre newGenre = book.Genre;
        string? customGenreName = book.CustomGenreName; // przechowujemy aktualny custom typ (jeśli jest)

        if (!string.IsNullOrWhiteSpace(input))
        {
            if (int.TryParse(input, out int genreNum))
            {
                newGenre = (BookGenre)genreNum;

                if (newGenre == BookGenre.Własny)
                {
                    Console.Write("Podaj własny typ książki: ");
                    customGenreName = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(customGenreName))
                    {
                        Console.WriteLine("Własny typ nie może być pusty, ustawiam domyślny \"Własny\".");
                        customGenreName = "Własny";
                    }
                }
                else
                {
                    // jeśli wybrano inny gatunek niż Własny, czyścimy customGenreName
                    customGenreName = null;
                }
            }
            else
            {
                Console.WriteLine("Nieprawidłowy numer gatunku, pozostawiam stary.");
            }
        }

        Book editedBook = new Book(newTitle, newAuthor, newGenre, customGenreName);
        _db.Edit(id, editedBook);
        Console.WriteLine($"Książka o ID {id} została zedytowana nowy tytuł: {newTitle}, nowy autor: {newAuthor}, nowy typ: {(newGenre == BookGenre.Własny ? customGenreName : newGenre.ToString())}.");
    }


    private void ListBooks()
    {
        var books = _db.ReadAll();
        if (books.Count == 0)
            Console.WriteLine("Brak książek w bibliotece.");
        else
            books.ForEach(b => Console.WriteLine(b));
    }
}

// PROGRAM (5/5)
class Program
{
    static void Main()
    {
        User user = new User("Użytkownik");
        user.LogIn();

        LibraryManager manager = new LibraryManager();
        manager.Run();

        user.LogOut();
    }
}
