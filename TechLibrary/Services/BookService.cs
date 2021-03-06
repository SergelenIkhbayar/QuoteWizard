using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TechLibrary.Contracts.Responses;
using TechLibrary.Data;
using TechLibrary.Domain;
using TechLibrary.Models;

namespace TechLibrary.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookid);
        Task<PaginationResponse> PaginationAsync(int pageNumber, bool isTitle, string searchString);
        Task<BookResponse> SaveBook(int bookId, string title, string isbn, string publishedDate, string thumbnailUrl, string shortDesc, string longDescr);
    }

    public class BookService : IBookService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public BookService(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var queryable = _dataContext.Books.AsQueryable();

            return await queryable.ToListAsync();
        }

        public async Task<Book> GetBookByIdAsync(int bookid)
        {
            return await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookid);
        }

        public async Task<PaginationResponse> PaginationAsync(int pageNumber, bool isTitle = false, string searchString = null)
        {
            var queryable = _dataContext.Books.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
            {
                queryable = isTitle
                    ? queryable.Where(_ => _.Title.ToLower().Contains(searchString.ToLower()))
                    : queryable.Where(_ => _.ShortDescr.ToLower().Contains(searchString.ToLower()));
            }
            var totalBooks = await queryable.OrderBy(_ => _.BookId).ToListAsync();
            var books = totalBooks.Skip((pageNumber - 1) * 10).Take(10).ToList();
            var bookResponses = _mapper.Map<List<BookResponse>>(books);
            return new PaginationResponse(pageNumber, totalBooks.Count(), bookResponses);
        }

        public async Task<BookResponse> SaveBook(int bookId, string title, string isbn, string publishedDate, string thumbnailUrl, string shortDesc, string longDescr)
        {
            var book = await _dataContext.Books.SingleOrDefaultAsync(x => x.BookId == bookId);
            if (book == null)
            {
                book = new Book();
                _dataContext.Books.Add(book);
            }
            book.Title = title;
            book.ISBN = isbn;
            book.PublishedDate = publishedDate;
            book.ThumbnailUrl = thumbnailUrl;
            book.ShortDescr = shortDesc;
            book.LongDescr = longDescr;
            await _dataContext.SaveChangesAsync();
            return _mapper.Map<BookResponse>(book);
        }
    }
}