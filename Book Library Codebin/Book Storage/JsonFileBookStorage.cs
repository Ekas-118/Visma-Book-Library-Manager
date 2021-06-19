using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Visma_s_Book_Library_Manager
{
    /// <summary>
    /// Class that uses a json file for storing books
    /// </summary>
    public class JsonFileBookStorage : IBookStorage
    {
        #region Private Fields

        /// <summary>
        /// The path of the book library json file
        /// </summary>
        private string _filename;

        #endregion

        #region Constructor

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="filename"></param>
        public JsonFileBookStorage(string filename)
        {
            _filename = filename;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a list of all books in the json file
        /// </summary>
        /// <returns></returns>
        public List<Book> Read()
        {
            var books = new List<Book>();

            if (!File.Exists(_filename))
                return books;

            var jsoncontent = File.ReadAllText(_filename);
            var bookContent = JsonConvert.DeserializeObject<List<Book>>(jsoncontent);

            if (bookContent == null)
                return books;

            try
            {
                books.AddRange(bookContent);
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not parse books from {_filename}: {ex.Message}", ex);
            }

            return books;
        }

        /// <summary>
        /// Updates the json file with a new list of books
        /// </summary>
        /// <param name="books"></param>
        public void Write(IEnumerable<Book> books)
        {
            books ??= new List<Book>();
            var booksJson = JsonConvert.SerializeObject(books);
            File.WriteAllText(_filename, booksJson);
        }

        #endregion
    }
}
