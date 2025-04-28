using BooksFilterApp.DBContext;
using BooksFilterApp.Filter;

namespace BooksFilterTestProject
{
    public class BooksFilterServiceTests
    {
        [Fact]
        public async Task DisplayFilteredBooksToConsoleAsync_ShouldDisplayBooks()
        {
            // Arrange
            var filter = new Filter { Title = "TestTitle" };
            var booksFiltered = new BooksFilterService(filter, new BooksDBContext());
            using StringWriter stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            // Act
            await booksFiltered.DisplayFilteredBooksToConsoleAsync();

            // Assert
            string actual = stringWriter.ToString();
            bool checkForNullOrEmpty = string.IsNullOrEmpty(actual);

            Assert.Contains("Books found :", actual);
            Assert.False(checkForNullOrEmpty);
        }

        [Fact]
        public async Task ExportToFileAsync_ShouldCreateFile()
        {
            // Arrange
            var filter = new Filter { Title = "TestTitle" };
            var booksFiltered = new BooksFilterService(filter, new BooksDBContext());
            var fileName = DateTime.Now.ToString("s").Replace(":", " ") + "_books_found.csv";

            // Act
            await booksFiltered.ExportToFileAsync();

            // Assert
            Assert.True(File.Exists(fileName));

            File.Delete(fileName);
        }
    }
}

