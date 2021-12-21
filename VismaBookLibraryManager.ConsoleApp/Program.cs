using Autofac;
using System.CommandLine;
using System.CommandLine.Invocation;
using VismaBookLibraryManager.Core;

namespace VismaBookLibraryManager.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Configure IoC container
            var container = ConfigureContainer();

            // Get an instance of Library
            using var scope = container.BeginLifetimeScope();
            var library = scope.Resolve<Library>();

            // Create subcommands and their handlers
            Command add = GetAddCommand();
            add.Handler = CommandHandler.Create<string, string, string, string, ushort, string>(library.AddBook);

            Command take = GetTakeCommand();
            take.Handler = CommandHandler.Create<string, string, int, int>(library.TakeBook);

            Command ret = GetReturnCommand();
            ret.Handler = CommandHandler.Create<string>(library.ReturnBook);

            Command list = GetListCommand();
            list.Handler = CommandHandler.Create<string, string, string, string, string, string>(library.ListBooks);
            
            Command delete = GetDeleteCommand();
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

        private static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Library>().AsSelf();
            builder.RegisterType<JsonFileBookStorage>().As<IBookStorage>().WithParameter("filename", "books.json");
            builder.RegisterType<ConsoleOutputHandler>().As<IOutputHandler>();

            return builder.Build();
        }

        private static Command GetDeleteCommand()
        {
            return new Command("delete", "Deletes a book from the library")
                {
                    new Argument<string>("isbn", "The ISBN of the book to delete")
                };
        }

        private static Command GetListCommand()
        {
            return new Command("list", "Lists all the books in the library")
                {
                    new Option<string>("--byAuthor", "Filter by author"),
                    new Option<string>("--byCategory", "Filter by category"),
                    new Option<string>("--byLanguage", "Filter by language"),
                    new Option<string>("--byISBN", "Filter by ISBN"),
                    new Option<string>("--byName", "Filter by name"),
                    new Option<string>("--byStatus", "Filter by status (Taken/Available)")
                };
        }

        private static Command GetReturnCommand()
        {
            return new Command("return", "Returns the book to the library")
                {
                    new Argument<string>("isbn", "The ISBN of the book to return")
                };
        }

        private static Command GetTakeCommand()
        {
            return new Command("take", "Lends a book to a reader")
                {
                    new Argument<string>("isbn", "The ISBN of the book to lend"),
                    new Argument<string>("reader", "The ID of the reader"),
                    new Argument<int>("months", "The amount of months to lend the book"),
                    new Argument<int>("days", "The amount of days to lend the book")
                };
        }

        private static Command GetAddCommand()
        {
            return new Command("add", "Adds a book to the library")
                {
                    new Argument<string>("name", "The name of the book"),
                    new Argument<string>("author","The author(s) of the book"),
                    new Argument<string>("category","The category of the book"),
                    new Argument<string>("language","The language of the book"),
                    new Argument<ushort>("year","The year the book was published"),
                    new Argument<string>("isbn","The ISBN of the book"),
                };
        }
    }
}
