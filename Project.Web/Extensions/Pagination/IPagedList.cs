using System.Collections.Generic;

namespace Project.Web.Extensions.Pagination
{
    public interface IPagedList<T> : IList<T>
    {
        int CurrentPageIndex { get; }

        int CurrentPageNumber { get; }
        
        int PageSize { get; }

        long TotalCount { get; }
    }
}
