using System.Collections.Generic;

namespace Visma_s_Book_Library_Manager
{
    /// <summary>
    /// Interface for classes who handle various message output
    /// </summary>
    public interface IOutputHandler
    {
        /// <summary>
        /// Outputs an error message
        /// </summary>
        /// <param name="message"></param>
        public void PrintError(string message);

        /// <summary>
        /// Outputs a confirmation message
        /// </summary>
        /// <param name="message"></param>
        public void PrintConfirmation(string message);

        /// <summary>
        /// Displays all the books and their properties
        /// </summary>
        /// <param name="books"></param>
        public void DisplayData(List<Book> books);
    }
}
