using BooksFilterApp.CSVDataHandling;
using BooksFilterApp.DBContext;

namespace BooksFilterAppTestProject
{
    public class FileImportTests
    {
        [Fact]
        public async Task HandleCsvFile_ShouldParseAndSaveData()
        {
            // Arrange
            var filePath = @"c:\books.csv";
            using var dbContext = new BooksDBContext();
            var bookDataSearchService = new BookDataSearchService(dbContext);
            var fileHandler = new FileImportService(filePath, dbContext, bookDataSearchService);
            var expectedCount = 41;
            var expectedPages = 336;
            var expectedReleaseDate = new DateTime(1960, 07, 11);

            // Act
            await fileHandler.ProcessImportFile();

            // Assert
            var booksInDb = dbContext.Books.ToList();
            var authorsInDb = dbContext.Authors.ToList();
            var firstBook = booksInDb.First();
            var Author = authorsInDb.First();
            Assert.NotEmpty(booksInDb);
            Assert.Equal(expectedCount, booksInDb.Count);
            Assert.Equal("To Kill a Mockingbird", firstBook.Title);
            Assert.Equal("Aldous Huxley", Author.Name);
            Assert.Equal(expectedPages, firstBook.Pages);
            Assert.Equal(expectedReleaseDate, firstBook.ReleaseDate);
        }
    }
}

