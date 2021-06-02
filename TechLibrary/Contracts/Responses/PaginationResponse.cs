using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechLibrary.Models;

namespace TechLibrary.Contracts.Responses
{
    public class PaginationResponse
    {
        public PaginationResponse(int pageNumber, int pagination, List<BookResponse> bookResponses)
        {
            PageNumber = pageNumber;
            Pagination = pagination;
            BookResponses = bookResponses;
        }

        public int PageNumber { get; set; }
        public int Pagination { get; set; }
        public List<BookResponse> BookResponses { get; set; }        
    }
}