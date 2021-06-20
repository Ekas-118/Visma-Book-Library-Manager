using System.Collections.Generic;

namespace VismaBookLibraryManager.Core
{
    /// <summary>
    /// Interface for classes that store information about books in a library
    /// </summary>
    public interface IBookStorage
    {
        /// <summary>
        /// Returns a list of all books in the book storage
        /// </summary>
        /// <returns></returns>
        public List<Book> Read();

        /// <summary>
        /// Updates the book storage with a new list of books
        /// </summary>
        /// <param name="books"></param>
        public void Write(IEnumerable<Book> books);
    }
}
