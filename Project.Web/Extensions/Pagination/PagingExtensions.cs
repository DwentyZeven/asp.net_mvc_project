using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Project.Web.Extensions.Pagination
{
    public static class PagingExtensions
    {
        public static HtmlString Pagination(this AjaxHelper ajaxHelper, int currentPageNumber, int pageSize, long totalCount, AjaxOptions ajaxOptions)
        {
            if (totalCount <= pageSize)
                return new HtmlString(string.Empty);

            var pagination = new Pagination(ajaxHelper.ViewContext, currentPageNumber, pageSize, totalCount, ajaxOptions);
            return pagination.RenderHtml();
        }

        public static HtmlString Pagination(this HtmlHelper htmlHelper, int currentPageNumber, int pageSize, long totalCount)
        {
            if (totalCount <= pageSize)
                return new HtmlString(string.Empty);

            var pagination = new Pagination(htmlHelper.ViewContext, currentPageNumber, pageSize, totalCount, null);
            return pagination.RenderHtml();
        }

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int currentPageIndex, int pageSize, long totalCount)
        {
            return new PagedList<T>(source, currentPageIndex, pageSize, totalCount);
        }
    }
}