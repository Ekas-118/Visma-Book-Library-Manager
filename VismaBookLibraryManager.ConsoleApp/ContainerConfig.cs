using Autofac;
using VismaBookLibraryManager.Core;

namespace VismaBookLibraryManager.ConsoleApp
{
    public static class ContainerConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Library>().AsSelf();
            builder.RegisterType<JsonFileBookStorage>().As<IBookStorage>().WithParameter("filename", "books.json");
            builder.RegisterType<ConsoleOutputHandler>().As<IOutputHandler>();

            return builder.Build();
        }
    }
}
