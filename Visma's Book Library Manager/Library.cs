using System;
using System.Collections.Generic;
using System.Linq;

namespace Visma_s_Book_Library_Manager
{
    public class Library
    {
        #region Private Fields

        private IBookStorage _bookstorage;
        private IOutputHandler _output;
        private List<Book> _books;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="output"></param>
        public Library(IBookStorage storage, IOutputHandler output)
        {
            _bookstorage = storage;
            _books = _bookstorage.Read();
            _output = output;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a copy of the list of books in the library
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Book> GetBooks() => _books.AsEnumerable();

        /// <summary>
        /// Adds a book to the library
        /// </summary>
        /// <param name="name">The name of the book</param>
        /// <param name="author">The author(s) of the book</param>
        /// <param name="category">The category of the book</param>
        /// <param name="language">The language of the book</param>
        /// <param name="year">The year this book was published</param>
        /// <param name="isbn">The ISBN of the book</param>
        public void AddBook(string name, string author, string category, string language, ushort year, string isbn)
        {
            if (_books.Exists(x => x.ISBN == isbn))
            {
                _output.PrintError("This book already exists.");
                return;
            }

            var book = new Book()
            {
                Name = name,
                Author = author,
                Category = category,
                Language = language,
                PublicationYear = year,
                ISBN = isbn
            };

            _books.Add(book);

            _bookstorage.Write(_books);

            _output.PrintConfirmation("Book added successfully.");
        }

        /// <summary>
        /// Gives out a book to a reader
        /// </summary>
        /// <param name="isbn">The ISBN of the book to lend</param>
        /// <param name="reader">The ID of the reader</param>
        /// <param name="months">The amount of months to lend the book</param>
        /// <param name="days">The amount of days to lend the book</param>
        public void TakeBook(string isbn, string reader, int months, int days)
        {
            if (months > 2 || (months == 2 && days > 0))
            {
                _output.PrintError("Taking books for longer than 2 months is not allowed.");
                return;
            }
            if (days > 31)
            {
                _output.PrintError($"You have specified a period of {months} months and {days} days.\n" +
                           "Please convert excess days into months and try again.");
                return;
            }

            if (!_books.Exists(x => x.ISBN == isbn))
            {
                _output.PrintError("A book with the specified ISBN does not exist.");
                return;
            }
            if (_books.Find(x => x.ISBN == isbn).Reader != null)
            {
                _output.PrintError("This book is already taken.");
                return;
            }
            if (_books.Where(x => x.Reader == reader).Count() >= 3)
            {
                _output.PrintError("This person has already taken 3 books.");
                return;
            }

            var bookToTake = _books.Find(x => x.ISBN == isbn);
            bookToTake.Reader = reader;
            bookToTake.DateTaken = DateTime.Now.ToString("yyyy-MM-dd");
            bookToTake.ReturnDate = DateTime.Now.AddMonths(months).AddDays(days).ToString("yyyy-MM-dd");

            _bookstorage.Write(_books);

            _output.PrintConfirmation("Book lent successfully.");
        }

        /// <summary>
        /// Returns a book from a reader to the library
        /// </summary>
        /// <param name="isbn">The ISBN of the book to return</param>
        public void ReturnBook(string isbn)
        {
            if (!_books.Exists(x => x.ISBN == isbn))
            {
                _output.PrintError("This book was never in the library.");
                return;
            }

            var returnedBook = _books.Find(x => x.ISBN == isbn);
            if (returnedBook.Reader == null)
            {
                _output.PrintError("This book was not taken.");
                return;
            }

            bool returnedLate = DateTime.Now - DateTime.Parse(returnedBook.ReturnDate) > TimeSpan.Zero;

            returnedBook.Reader = returnedBook.DateTaken = returnedBook.ReturnDate = null;

            _bookstorage.Write(_books);

            if (returnedLate)
            {
                _output.PrintConfirmation("The book was returned late.");
                _output.PrintError("Employee fired.");

                return;
            }

            _output.PrintConfirmation("Book successfully returned to the library.");
        }

        /// <summary>
        /// Lists all books and their properties
        /// </summary>
        /// <param name="byAuthor">Author to filter by</param>
        /// <param name="byCategory">Category to filter by</param>
        /// <param name="byISBN">ISBN to filter by</param>
        /// <param name="byLanguage">Language to filter by</param>
        /// <param name="byName">Book name to filter by</param>
        /// <param name="byStatus">Status to filter by (Taken/Available)</param>
        public void ListBooks(string byAuthor, string byCategory, string byLanguage, string byISBN, string byName, string byStatus)
        {
            // Filter the list
            if (byAuthor != "")
                _books = _books.Where(x => x.Author.ToLower().Contains(byAuthor.ToLower())).ToList();
            if (byCategory != "")
                _books = _books.Where(x => x.Category.ToLower().Contains(byCategory.ToLower())).ToList();
            if (byLanguage != "")
                _books = _books.Where(x => x.Language.ToLower() == byLanguage.ToLower()).ToList();
            if (byISBN != "")
                _books = _books.Where(x => x.ISBN == byISBN).ToList();
            if (byName != "")
                _books = _books.Where(x => x.Name.ToLower().Contains(byName.ToLower())).ToList();
            if (byStatus != "")
            {
                // If invalid argument is passed, return.
                if (byStatus.ToLower() != "taken" && byStatus.ToLower() != "available")
                {
                    _output.PrintError($"Invalid --byStatus argument: {byStatus}.\n" +
                                "Available arguments: \"Taken\", \"Available\".");
                    return;
                }
                else
                {
                    if (byStatus.ToLower() == "taken")
                        _books = _books.Where(x => x.Reader != null).ToList();
                    if (byStatus.ToLower() == "available")
                        _books = _books.Where(x => x.Reader == null).ToList();
                }
            }

            if (_books.Count == 0)
            {
                _output.PrintError("No books found.");
                return;
            }

            _output.DisplayData(_books);
        }

        /// <summary>
        /// Removes a book from the library
        /// </summary>
        /// <param name="isbn">The ISBN of the book to remove</param>
        public void DeleteBook(string isbn)
        {
            if (!_books.Exists(x => x.ISBN == isbn))
            {
                _output.PrintError("A book with the specified ISBN does not exist.");
                return;
            }

            _books.Remove(_books.Find(x => x.ISBN == isbn));

            _bookstorage.Write(_books);

            _output.PrintConfirmation($"Book successfully deleted.");
        }

        #endregion
    }
}