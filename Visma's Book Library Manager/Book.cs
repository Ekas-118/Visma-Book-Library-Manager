namespace Visma_s_Book_Library_Manager
{
    /// <summary>
    /// Class for storing the properties of a book
    /// </summary>
    public class Book
    {
        /// <summary>
        /// The name of the book
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The book author(s)
        /// </summary>
        public string Author { get; set; }

        public string Category { get; set; }

        public string Language { get; set; }

        /// <summary>
        /// The year when the book was published
        /// </summary>
        public ushort PublicationYear { get; set; }

        public string ISBN { get; set; }

        /// <summary>
        /// ID of the reader if the book is taken,
        /// null if the book is available
        /// </summary>
        public string Reader { get; set; }

        /// <summary>
        /// Date when the book was taken,
        /// null if the book is available
        /// </summary>
        public string DateTaken { get; set; }

        /// <summary>
        /// Date of the last day the book can be returned,
        /// null if the book is available
        /// </summary>
        public string ReturnDate { get; set; }
    }
}
