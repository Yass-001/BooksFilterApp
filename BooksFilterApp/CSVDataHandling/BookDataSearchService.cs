using BooksFilterApp.DBContext;
using BooksFilterApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BooksFilterApp.CSVDataHandling
{
    public class BookDataSearchService
    {
        private BooksDBContext _dbContext;

        public BookDataSearchService(BooksDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        public async Task<Genre> GetOrCreateGenre(string genreName)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name.ToLower() == genreName.ToLower().Trim());
            if (genre is null)
            {
                genre = new Genre { Name = genreName.Trim() };
                await _dbContext.Genres.AddAsync(genre);
                await _dbContext.SaveChangesAsync();
            }

            return genre;
        }

        public async Task<Author> GetOrCreateAuthor(string authorName)
        {
            var author = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Name.ToLower() == authorName.ToLower().Trim());
            if (author is null)
            {
                author = new Author { Name = authorName.Trim() };
                await _dbContext.Authors.AddAsync(author);
                await _dbContext.SaveChangesAsync();
            }

            return author;
        }

        public async Task<Publisher> GetOrCreatePublisher(string publisherName)
        {
            var publisher = await _dbContext.Publishers.FirstOrDefaultAsync(p => p.Name.ToLower() == publisherName.ToLower().Trim());
            if (publisher is null)
            {
                publisher = new Publisher { Name = publisherName.Trim() };
                await _dbContext.Publishers.AddAsync(publisher);
                await _dbContext.SaveChangesAsync();
            }

            return publisher;
        }
    }
}


