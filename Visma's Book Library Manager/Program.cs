using System.CommandLine;
using System.CommandLine.Invocation;

namespace Visma_s_Book_Library_Manager
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instantiate a library
            var library = new Library(new JsonFileBookStorage("books.json"), new ConsoleOutputHandler());

            // Create subcommands and their handlers
            var add = new Command("add", "Adds a book to the library")
            {
                new Argument<string>("name", "The name of the book"),
                new Argument<string>("author","The author(s) of the book"),
                new Argument<string>("category","The category of the book"),
                new Argument<string>("language","The language of the book"),
                new Argument<ushort>("year","The year the book was published"),
                new Argument<string>("isbn","The ISBN of the book"),
            };
            add.Handler = CommandHandler.Create<string, string, string, string, ushort, string>(library.AddBook);

            var take = new Command("take", "Lends a book to a reader")
            {
                new Argument<string>("isbn", "The ISBN of the book to lend"),
                new Argument<string>("reader", "The ID of the reader"),
                new Argument<int>("months", "The amount of months to lend the book"),
                new Argument<int>("days", "The amount of days to lend the book")
            };
            take.Handler = CommandHandler.Create<string, string, int, int>(library.TakeBook);

            var ret = new Command("return", "Returns the book to the library")
            {
                new Argument<string>("isbn", "The ISBN of the book to return")
            };
            ret.Handler = CommandHandler.Create<string>(library.ReturnBook);

            var list = new Command("list", "Lists all the books in the library")
            {
                new Option<string>("--byAuthor", "Filter by author"),
                new Option<string>("--byCategory", "Filter by category"),
                new Option<string>("--byLanguage", "Filter by language"),
                new Option<string>("--byISBN", "Filter by ISBN"),
                new Option<string>("--byName", "Filter by name"),
                new Option<string>("--byStatus", "Filter by status (Taken/Available)")
            };
            list.Handler = CommandHandler.Create<string, string, string, string, string, string>(library.ListBooks);

            var delete = new Command("delete", "Deletes a book from the library")
            {
                new Argument<string>("isbn", "The ISBN of the book to delete")
            };
            delete.Handler = CommandHandler.Create<string>(library.DeleteBook);

            // Create root command and add subcommands
            var cmd = new RootCommand("A book library manager for Visma")
            {
                add,
                take,
                ret,
                list,
                delete
            };
            cmd.Invoke(args);
        }
    }
}
