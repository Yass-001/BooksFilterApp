using BooksFilterApp.CSVDataHandling;
using BooksFilterApp.DBContext;
using BooksFilterApp.Entities;
using CsvHelper.Configuration;
using CsvHelper;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BooksFilterApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksFilterApp.CSVDataHandling
{
    public class FileImportService
    {
        private string? _filePath = string.Empty;
        private List<BookFromCsv> _booksFromCSV = new List<BookFromCsv>();
        private BooksDBContext _dbContext;
        private ILogger _log;
        private BookDataSearchService _bookDataSearch;

        public FileImportService(string file, BooksDBContext dbContext, BookDataSearchService bookDataSearch)
        {
            if (!File.Exists(file))
            {
                throw new FileNotFoundException("File was not found.", nameof(file));
            }
            _filePath = file;
            _log = new LoggerConfiguration()
                    .WriteTo.File($"ImportLog-{DateTime.Now:yyyy-MM-dd}-{Path.GetFileName(file)}.log")
                    .CreateLogger();
            _dbContext = dbContext;
            _bookDataSearch = bookDataSearch;
        }

        public async Task ProcessImportFile()
        {
            _booksFromCSV = ParseCsvFile(_filePath);

            await SaveDataToDatabaseAsync(_booksFromCSV);
        }

        private List<BookFromCsv> ParseCsvFile(string filePath)
        {
            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                });

                var records = csv.GetRecords<BookFromCsv>().ToList();

                _log.Information($"Successfully parsed {records.Count} records from the CSV file.");

                return records;
            }
            catch (Exception ex)
            {
                _log.Information($"Error reading CSV file: {ex.Message}");

                return new List<BookFromCsv>();
            }
        }

        private async Task SaveDataToDatabaseAsync(List<BookFromCsv> books)
        {
            foreach (var book in books)
            {
                if (String.IsNullOrEmpty(book.Title))
                {
                    _log.Information($"Title is empty.");
                    continue;
                }

                if (String.IsNullOrEmpty(book.Genre))
                {
                    _log.Information($"Genre is empty for {book.Title}");
                    continue;
                }

                if (String.IsNullOrEmpty(book.Author))
                {
                    _log.Information($"Author is empty for {book.Title}");
                    continue;
                }

                if (String.IsNullOrEmpty(book.Publisher))
                {

                    _log.Information($"Publisher is empty for {book.Title}");
                    continue;
                }

                if (ParsePages(book.Pages) < 1)
                {
                    _log.Information($"Pages data is not correct for {book.Title}.");
                    continue;
                }

                if (ParseReleaseDate(book.ReleaseDate) == DateTime.MinValue)
                {
                    _log.Information($"Release date is not correct for {book.Title}.");
                    continue;
                }

                var genre = await _bookDataSearch.GetOrCreateGenre(book.Genre);

                if (genre.Id == Guid.Empty)
                {
                    _log.Information($"Genre is empty for {book.Title}.");
                    continue;
                }

                var author = await _bookDataSearch.GetOrCreateAuthor(book.Author);

                if (author.Id == Guid.Empty)
                {
                    _log.Information($"Author is empty for {book.Title}.");
                    continue;
                }

                var publisher = await _bookDataSearch.GetOrCreatePublisher(book.Publisher);

                if (publisher.Id == Guid.Empty)
                {
                    _log.Information($"Publisher is empty for {book.Title}.");
                    continue;
                }

                var exists = await _dbContext.Books
                    .AnyAsync(b => b.Title.ToLower() == book.Title.ToLower().Trim()
                          && b.AuthorId == author.Id
                          && b.PublisherId == publisher.Id
                          && b.GenreId == genre.Id
                    );

                if (!exists)
                {
                    var bookTemp = new Book
                    {
                        Title = book.Title.Trim(),
                        Pages = ParsePages(book.Pages),
                        ReleaseDate = ParseReleaseDate(book.ReleaseDate),
                        AuthorId = author.Id,
                        GenreId = genre.Id,
                        PublisherId = publisher.Id
                    };

                    await _dbContext.Books.AddAsync(bookTemp);

                    _log.Information($"Added: {book.Title}");
                }
                else
                {
                    _log.Information($"Duplicate skipped: {book.Title}");
                }
            }

            await _dbContext.SaveChangesAsync();

            _log.Information("Data saved to the database.");
        }

        private int ParsePages(string pages)
        {
            return int.TryParse(pages, out int result) ? result : int.MinValue;
        }

        private DateTime ParseReleaseDate(string releaseDate)
        {
            return DateTime.TryParse(releaseDate, out DateTime result) ? result : DateTime.MinValue;
        }
    }
}

