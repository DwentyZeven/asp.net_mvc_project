using System;
using System.Collections.Generic;

namespace Project.Web.Extensions.Pagination
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public int CurrentPageIndex { get; private set; }

        public int CurrentPageNumber { get { return CurrentPageIndex + 1; } }

        public int PageSize { get; private set; }

        public long TotalCount { get; private set; }

        public PagedList(IEnumerable<T> source, int currentPageIndex, int pageSize, long totalCount)
        {
            if (currentPageIndex < 0)
                throw new ArgumentOutOfRangeException("currentPageIndex", "Value can not be below 0.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException("pageSize", "Value can not be less than 1.");
            if (totalCount < 0)
                throw new ArgumentOutOfRangeException("totalCount", "Value can not be below 0.");

            CurrentPageIndex = currentPageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            AddRange(source);
        }
    }
}