using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Visma_s_Book_Library_Manager.Tests
{
    public class LibraryTests
    {
        #region AddBook Tests

        [Fact]
        public void AddBook_WhenBookAlreadyExists_ListStaysUnchanged()
        {
            var tempfile = Path.GetTempFileName();
            var library = new Library(new JsonFileBookStorage(tempfile), new ConsoleOutputHandler());
            library.AddBook("name", "author", "category", "language", 2000, "9999-01-101-0");

            library.AddBook("name", "author", "category", "language", 2000, "9999-01-101-0");

            Assert.Single(library.GetBooks());
            File.Delete(tempfile);
        }

        [Fact]
        public void AddBook_WhenInputIsValid_WorksProperly()
        {
            var tempfile = Path.GetTempFileName();
            var library = new Library(new JsonFileBookStorage(tempfile), new ConsoleOutputHandler());
            var list = Enumerable.Range(1, 5).Select(x => new Book()
            {
                Name = $"Book {x}",
                Author = $"Author {x}",
                Category = $"Category {x}",
                Language = $"Language {x}",
                PublicationYear = (ushort)(2010 + x),
                ISBN = $"9999-01-101-{x}"
            });

            foreach (Book book in list)
                library.AddBook(book.Name, book.Author, book.Category, book.Language, book.PublicationYear, book.ISBN);

            Assert.Equal(5, library.GetBooks().Count());
            File.Delete(tempfile);
        }

        #endregion

        #region TakeBook Tests

        [Theory]
        [InlineData (0, 0)]
        [InlineData (-1, 10)]
        [InlineData (10, -10)]
        public void TakeBook_WhenPeriodIsZeroOrLessDays_DoesNotGiveBook(int months, int days)
        {
            var tempfile = Path.GetTempFileName();
            var library = new Library(new JsonFileBookStorage(tempfile), new ConsoleOutputHandler());
            library.AddBook("name", "author", "category", "language", 2000, "9999-01-101-0");

            library.TakeBook("9999-01-101-0", "readerID0001", months, days);
            var bookAttemptedToTake = library.GetBooks().ToList().Find(x => x.ISBN == "9999-01-101-0");

            Assert.Null(bookAttemptedToTake.Reader);

            File.Delete(tempfile);
        }

        [Fact]
        public void TakeBook_WhenPeriodIsLongerThanTwoMonths_DoesNotGiveBook()
        {
            var tempfile = Path.GetTempFileName();
            var library = new Library(new JsonFileBookStorage(tempfile), new ConsoleOutputHandler());
            library.AddBook("name", "author", "category", "language", 2000, "9999-01-101-0");

            library.TakeBook("9999-01-101-0", "readerID0001", 2, 1);
            var bookAttemptedToTake = library.GetBooks().ToList().Find(x => x.ISBN == "9999-01-101-0");

            Assert.Null(bookAttemptedToTake.Reader);
            File.Delete(tempfile);
        }

        [Fact]
        public void TakeBook_WhenBookIsAlreadyTaken_DoesNotGiveBook()
        {
            var tempfile = Path.GetTempFileName();
            var library = new Library(new JsonFileBookStorage(tempfile), new ConsoleOutputHandler());
            library.AddBook("name", "author", "category", "language", 2000, "9999-01-101-0");
            library.TakeBook("9999-01-101-0", "readerID0001", 1, 0);

            library.TakeBook("9999-01-101-0", "readerID0002", 0, 14);
            var bookAttemptedToTake = library.GetBooks().ToList().Find(x => x.ISBN == "9999-01-101-0");

            Assert.NotEqual("readerID0002", bookAttemptedToTake.Reader);
            File.Delete(tempfile);
        }

        [Fact]
        public void TakeBook_WhenInputIsValid_WorksProperly()
        {
            var tempfile = Path.GetTempFileName();
            var library = new Library(new JsonFileBookStorage(tempfile), new ConsoleOutputHandler());
            library.AddBook("name", "author", "category", "language", 2000, "9999-01-101-0");

            library.TakeBook("9999-01-101-0", "readerID0001", 1, 5);
            var bookAttemptedToTake = library.GetBooks().ToList().Find(x => x.ISBN == "9999-01-101-0");

            Assert.Equal("readerID0001", bookAttemptedToTake.Reader);
            Assert.Equal(DateTime.Now.ToString("yyyy-MM-dd"), bookAttemptedToTake.DateTaken);
            Assert.Equal(DateTime.Now.AddMonths(1).AddDays(5).ToString("yyyy-MM-dd"), bookAttemptedToTake.ReturnDate);
            File.Delete(tempfile);
        }

        [Fact]
        public void TakeBook_WhenPersonAlreadyHasTakenThreeBooks_DoesNotGiveBook()
        {
            var tempfile = Path.GetTempFileName();
            var library = new Library(new JsonFileBookStorage(tempfile), new ConsoleOutputHandler());
            var list = Enumerable.Range(1, 5).Select(x => new Book()
            {
                Name = $"Book {x}",
                Author = $"Author {x}",
                Category = $"Category {x}",
                Language = $"Language {x}",
                PublicationYear = (ushort)(2010 + x),
                ISBN = $"9999-01-101-{x}"
            });
            foreach (Book book in list)
                library.AddBook(book.Name, book.Author, book.Category, book.Language, book.PublicationYear, book.ISBN);
            library.TakeBook("9999-01-101-1", "readerID0001", 1, 0);
            library.TakeBook("9999-01-101-2", "readerID0001", 1, 0);
            library.TakeBook("9999-01-101-3", "readerID0001", 1, 0);

            library.TakeBook("9999-01-101-4", "readerID0001", 1, 0);
            var bookAttemptedToTake = library.GetBooks().ToList().Find(x => x.ISBN == "9999-01-101-4");

            Assert.NotEqual("readerID0001", bookAttemptedToTake.Reader);
            File.Delete(tempfile);
        }

        #endregion

        #region ReturnBook Tests

        [Fact]
        public void ReturnBook_WhenInputIsValid_ReaderAndDatesGetResetToNull()
        {
            var tempfile = Path.GetTempFileName();
            var library = new Library(new JsonFileBookStorage(tempfile), new ConsoleOutputHandler());
            library.AddBook("name", "author", "category", "language", 2000, "9999-01-101-0");
            library.TakeBook("9999-01-101-0", "readerID0001", 1, 0);

            library.ReturnBook("9999-01-101-0");
            var bookReturned = library.GetBooks().ToList().Find(x => x.ISBN == "9999-01-101-0");

            Assert.Null(bookReturned.Reader);
            Assert.Null(bookReturned.DateTaken);
            Assert.Null(bookReturned.ReturnDate);
            File.Delete(tempfile);
        }

        #endregion

        #region DeleteBook Tests

        [Fact]
        public void DeleteBook_WhenInputIsValid_DeletesBook()
        {
            var tempfile = Path.GetTempFileName();
            var library = new Library(new JsonFileBookStorage(tempfile), new ConsoleOutputHandler());
            var list = Enumerable.Range(1, 5).Select(x => new Book()
            {
                Name = $"Book {x}",
                Author = $"Author {x}",
                Category = $"Category {x}",
                Language = $"Language {x}",
                PublicationYear = (ushort)(2010 + x),
                ISBN = $"9999-01-101-{x}"
            });
            foreach (Book book in list)
                library.AddBook(book.Name, book.Author, book.Category, book.Language, book.PublicationYear, book.ISBN);

            library.DeleteBook("9999-01-101-4");
            var books = library.GetBooks();

            Assert.True(books.Count(x => x.ISBN != "9999-01-101-4") == books.Count());
            File.Delete(tempfile);
        }

        #endregion
    }
}
