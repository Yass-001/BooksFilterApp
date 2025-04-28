using BooksFilterApp.DBContext;
using BooksFilterApp.Models;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksFilterApp.Filter
{
    public class BooksFilterService
    {
        private List<BookFromCsv> _booksFoundList = new List<BookFromCsv>();
        private Filter _filter = new Filter();
        private BooksDBContext _dbContext;

        public BooksFilterService(Filter filter, BooksDBContext dbContext)
        {
            _filter = filter;
            _dbContext = dbContext;
            FilterAsync().Wait();
        }

        public async Task DisplayFilteredBooksToConsoleAsync()
        {
            Console.WriteLine($"Books found : {_booksFoundList.Count}.");
            foreach (var book in _booksFoundList)
            {
                Console.WriteLine(book.Title.ToString());
            }
        }

        public async Task ExportToFileAsync()
        {
            string fileName = DateTime.Now.ToString("s").Replace(":", " ");

            using (var writer = new StreamWriter($"{fileName}_books_found.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                await Task.Run(() => csv.WriteRecords(_booksFoundList));
            }
        }

        private async Task FilterAsync()
        {
            var query = _dbContext.Books
                .Include(b => b.Genre)
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .AsQueryable();

            if (!string.IsNullOrEmpty(_filter.Title))
            {
                var titleFromFilter = _filter.Title.ToLower().Trim();
                query = query.Where(p => p.Title.ToLower() == titleFromFilter);
            }

            if (!string.IsNullOrEmpty(_filter.Genre))
            {
                var genreFromFilter = _filter.Genre.ToLower().Trim();
                var foundGenreID = (await _dbContext.Genres
                    .FirstOrDefaultAsync(g => g.Name.ToLower() == genreFromFilter))
                    .Id;

                if (foundGenreID != Guid.Empty)
                {
                    query = query.Where(book => book.GenreId == foundGenreID);
                }
            }

            if (!string.IsNullOrEmpty(_filter.Author))
            {
                var authorFromFilter = _filter.Author.ToLower().Trim();
                var foundAuthorID = (await _dbContext.Authors
                    .FirstOrDefaultAsync(a => a.Name.ToLower() == authorFromFilter))
                    .Id;

                if (foundAuthorID != Guid.Empty)
                {
                    query = query.Where(book => book.AuthorId == foundAuthorID);
                }
            }

            if (!string.IsNullOrEmpty(_filter.Publisher))
            {
                var publisherFromFilter = _filter.Publisher.ToLower().Trim();
                var foundPublisherID = (await _dbContext.Publishers
                    .FirstOrDefaultAsync(p => p.Name.ToLower() == publisherFromFilter))
                    .Id;

                if (foundPublisherID != Guid.Empty)
                {
                    query = query.Where(book => book.PublisherId == foundPublisherID);
                }
            }

            if (_filter.MoreThanPages.HasValue && _filter.MoreThanPages > 0)
            {
                query = query.Where(p => p.Pages > _filter.MoreThanPages);
            }

            if (_filter.LessThanPages.HasValue && _filter.LessThanPages > 0)
            {
                query = query.Where(p => p.Pages < _filter.LessThanPages);
            }

            if (_filter.PublishedBefore.HasValue)
            {
                query = query.Where(p => p.ReleaseDate < _filter.PublishedBefore);
            }

            if (_filter.PublishedAfter.HasValue)
            {
                query = query.Where(p => p.ReleaseDate > _filter.PublishedAfter);
            }

            var books = (await query
                .AsNoTracking()
                .ToListAsync())
                .Select(book => new BookFromCsv
                {
                    Title = book.Title,
                    Pages = book.Pages.ToString(),
                    ReleaseDate = book.ReleaseDate.ToString("yyyy-MM-dd"),
                    Genre = book.Genre?.Name,
                    Author = book.Author?.Name,
                    Publisher = book.Publisher?.Name
                });

            _booksFoundList.AddRange(books);
        }
    }
}
