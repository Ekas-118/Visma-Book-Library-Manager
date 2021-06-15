using System;

namespace Visma_s_Book_Library_Manager
{
    /// <summary>
    /// Class for error/confirmation message output to the console
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
    }
}
