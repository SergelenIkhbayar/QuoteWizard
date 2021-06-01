using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechLibrary.Models;

namespace TechLibrary.Contracts.Responses
{
    public class PaginationResponse
    {
        public PaginationResponse(int currentPage, int totalPage, List<BookResponse> bookResponses)
        {
            CurrentPage = currentPage;
            TotalPage = totalPage;
            BookResponses = bookResponses;
        }

        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public List<BookResponse> BookResponses { get; set; }        
    }
}