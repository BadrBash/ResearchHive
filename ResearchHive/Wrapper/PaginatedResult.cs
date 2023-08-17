using System;
using System.Collections.Generic;

namespace ResearchHive.Wrapper
{
    public class PaginatedResult<T> : Result
    {
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;
        public List<T> Data { get; set; }

        public PaginatedResult()
        {
        }

        public PaginatedResult(List<T> data)
        {
            Data = data;
        }

        public PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0,
            int page = 0, int pageSize = 0)
        {
            Data = data;
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Messages = messages;
        }

        public PaginatedResult(List<T> data = default, int count = 0, int page = 1, int pageSize = 10)
        {
            Data = data;
            CurrentPage = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }

    }
}