using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Visma_s_Book_Library_Manager.Tests
{
    public class JsonFileBookStorageTests
    {
        [Fact]
        public void Read_WhenFileDoesntExist_ReturnsEmptyList()
        {
            var tempfile = Path.GetTempFileName();
            var storage = new JsonFileBookStorage(tempfile);

            var actual = storage.Read();

            Assert.NotNull(actual);
            Assert.Empty(actual);
            File.Delete(tempfile);
        }

        [Fact]
        public void Read_WhenFileIsEmpty_ReturnsEmptyList()
        {
            var tempfile = Path.GetTempFileName();
            var storage = new JsonFileBookStorage(tempfile);

            var actual = storage.Read();

            Assert.NotNull(actual);
            Assert.Empty(actual);
            File.Delete(tempfile);
        }

        [Fact]
        public void Write_WhenBookListIsEmpty_WritesEmptyList()
        {
            var tempfile = Path.GetTempFileName();
            var storage = new JsonFileBookStorage(tempfile);

            storage.Write(new List<Book>());

            Assert.Empty(storage.Read());
            File.Delete(tempfile);
        }

        [Fact]
        public void Write_WhenBookListIsNull_WritesEmptyList()
        {
            var tempfile = Path.GetTempFileName();
            var storage = new JsonFileBookStorage(tempfile);
            
            storage.Write(null);
            var actual = storage.Read();

            Assert.NotNull(actual);
            Assert.Empty(actual);
            File.Delete(tempfile);
        }

        [Fact]
        public void ReadAndWrite_ShouldWorkWithNormalList()
        {
            var tempfile = Path.GetTempFileName();
            var storage = new JsonFileBookStorage(tempfile);
            var bookList = Enumerable.Range(1, 5)
                                    .Select(x => new Book(){Name = $"Book{x}"}).ToList();

            storage.Write(bookList);

            Assert.True(Enumerable.SequenceEqual(bookList.Select(book => book.Name), storage.Read().Select(book => book.Name)));
            File.Delete(tempfile);
        }
    }
}
