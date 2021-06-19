using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visma_s_Book_Library_Manager
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
        List<Book> Read();

        /// <summary>
        /// Updates the book storage with a new list of books
        /// </summary>
        /// <param name="books"></param>
        void Write(IEnumerable<Book> books);
    }
}
