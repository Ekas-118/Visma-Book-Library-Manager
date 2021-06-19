using ConsoleTables;
using System;
using System.Collections.Generic;

namespace Visma_s_Book_Library_Manager
{
    /// <summary>
    /// Class responsible for message output to the console
    /// </summary>
    public class ConsoleOutputHandler : IOutputHandler
    {
        /// <summary>
        /// Prints a red error message to the console
        /// </summary>
        /// <param name="message">The error message</param>
        public void PrintError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a green confirmation message to the console
        /// </summary>
        /// <param name="message">The confirmation message</param>
        public void PrintConfirmation(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        /// <summary>
        /// Prints a table of all books and their properties to the console
        /// </summary>
        /// <param name="books"></param>
        public void DisplayData(List<Book> books)
        {
            // Create and print table to the console
            var table = new ConsoleTable("Name", "Author", "Category", "Language", "Publication year", "ISBN", "Reader ID", "Date taken", "Return date");
            foreach (var book in books)
            {
                table.AddRow(book.Name, book.Author, book.Category, book.Language, book.PublicationYear, book.ISBN, book.Reader, book.DateTaken, book.ReturnDate);
            }
            table.Write(Format.MarkDown);
            Console.WriteLine($"[{books.Count} results]");
        }
    }
}
