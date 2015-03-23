using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Project.Web.Extensions.Pagination
{
    public class Pagination
    {
        private readonly ViewContext _viewContext;

        private readonly int _pageSize;
        
        private readonly int _currentPageNumber;
        
        private readonly long _totalCount;
        
        private readonly AjaxOptions _ajaxOptions;

        public Pagination(ViewContext viewContext, int currentPageNumber, int pageSize, long totalCount, AjaxOptions ajaxOptions)
        {
            _viewContext = viewContext;
            _pageSize = pageSize;
            _currentPageNumber = currentPageNumber;
            _totalCount = totalCount;
            _ajaxOptions = ajaxOptions;
        }

        public HtmlString RenderHtml()
        {
            const int numberOfPagesToDisplay = 20;
            var pageCount = (int) Math.Ceiling(_totalCount / (double) _pageSize);

            var stringBuilder = new StringBuilder();

            // Previous
            stringBuilder.Append(_currentPageNumber > 1 ? GeneratePageLink("&lt;", _currentPageNumber - 1) : "<span class=\"disabled\">&lt;</span>");

            var start = 1;
            var end = pageCount;

            if (pageCount > numberOfPagesToDisplay)
            {
                var middle = (int) Math.Ceiling(numberOfPagesToDisplay / 2d) - 1;
                var below = _currentPageNumber - middle;
                var above = _currentPageNumber + middle;

                if (below < 4)
                {
                    above = numberOfPagesToDisplay;
                    below = 1;
                }
                else if (above > (pageCount - 3))
                {
                    above = pageCount;
                    below = pageCount - (numberOfPagesToDisplay - 1);
                }

                start = below;
                end = above;
            }

            if (start > 3)
            {
                stringBuilder.Append(GeneratePageLink("1", 1));
                stringBuilder.Append(GeneratePageLink("2", 2));
                stringBuilder.Append("...");
            }

            for (var i = start; i <= end; i++)
            {
                if (i == _currentPageNumber || (_currentPageNumber <= 0 && i == 0))
                {
                    stringBuilder.AppendFormat("<span class=\"current\">{0}</span>", i);
                }
                else
                {
                    stringBuilder.Append(GeneratePageLink(i.ToString(), i));
                }
            }

            if (end < (pageCount - 2))
            {
                stringBuilder.Append("...");
                stringBuilder.Append(GeneratePageLink((pageCount - 1).ToString(), pageCount - 1));
                stringBuilder.Append(GeneratePageLink(pageCount.ToString(), pageCount));
            }

            // Next
            stringBuilder.Append(_currentPageNumber < pageCount ? GeneratePageLink("&gt;", (_currentPageNumber + 1)) : "<span class=\"disabled\">&gt;</span>");

            return new HtmlString(stringBuilder.ToString());
        }

        private string GeneratePageLink(string linkText, int pageNumber)
        {
            var virtualPath = HttpContext.Current.Request.Path;
            virtualPath = virtualPath.EndsWith("/") ? virtualPath.Remove(virtualPath.LastIndexOf('/')) : virtualPath;

            var stringBuilder = new StringBuilder("<a");

            if (_ajaxOptions != null)
                foreach (var ajaxOption in _ajaxOptions.ToUnobtrusiveHtmlAttributes())
                    stringBuilder.AppendFormat(" {0}=\"{1}\"", ajaxOption.Key, ajaxOption.Value);

            stringBuilder.AppendFormat(" href=\"{0}/?page={1}\">{2}</a>", virtualPath, pageNumber, linkText);

            return stringBuilder.ToString();
        }
    }
}