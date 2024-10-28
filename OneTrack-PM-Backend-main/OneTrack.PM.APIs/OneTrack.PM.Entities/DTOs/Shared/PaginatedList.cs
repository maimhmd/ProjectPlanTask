using System;
using System.Collections.Generic;
using System.Linq;

namespace OneTrack.PM.Entities.DTOs.Shared
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int TotalCount { get; }
        public int CurrentPage { get; }
        public int TotalPages { get; }

        public PaginatedList(List<T> items, int count, PagingDTO paging)
        {
            TotalCount = count;
            CurrentPage = paging.PageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)paging.PageSize);
            Items = items;
        }
    }
}
