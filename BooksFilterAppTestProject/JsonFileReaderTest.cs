using System;
using System.Collections.Generic;
using System.Linq;
using BooksFilterApp.Filter;
namespace BooksFilterTestProject
{
    public class JsonFileReaderTest
    {
        [Fact]
        public void Read_ShouldReturnFilterObject_WhenJsonIsValid()
        {
            var jsonFileReader = new JsonFileReader();
            // Act
            var filter = jsonFileReader.GetFilter();
            // Assert
            Assert.NotNull(filter);
            Assert.Equal("Test Title", filter.Title);
            Assert.Equal("Test Genre", filter.Genre);
            Assert.Equal("Test Author", filter.Author);
            Assert.Equal("Test Publisher", filter.Publisher);
            Assert.Equal(100, filter.MoreThanPages);
            Assert.Equal(200, filter.LessThanPages);
            Assert.Equal(new DateTime(2023, 1, 1), filter.PublishedBefore);
            Assert.Equal(new DateTime(2020, 1, 1), filter.PublishedAfter);
        }
    }
}