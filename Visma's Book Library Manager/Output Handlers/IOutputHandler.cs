namespace Visma_s_Book_Library_Manager
{
    /// <summary>
    /// Interface for classes who handle error/confirmation message output
    /// </summary>
    public interface IOutputHandler
    {
        /// <summary>
        /// Outputs an error message
        /// </summary>
        /// <param name="message"></param>
        void PrintError(string message);

        /// <summary>
        /// Outputs a confirmation message
        /// </summary>
        /// <param name="message"></param>
        void PrintConfirmation(string message);
    }
}
