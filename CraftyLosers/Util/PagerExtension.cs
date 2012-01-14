using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace CraftyLosers.Util
{
    public static class PagerExtension
    {
        /// <summary>
        /// Returns a block of Html that displays a paging control.
        /// </summary>
        /// <typeparam name="TModel">The type of model.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="pagedList">The <see cref="PagedList{TModel}"/> to use to build the pager.</param>
        /// <returns>A block of HTML that contains the pager controls.</returns>
        public static MvcHtmlString BootstrapPager<TModel>(this HtmlHelper html, PagedList<TModel> pagedList) where TModel : class
        {
            if (pagedList == null)
            {
                throw new ArgumentNullException("pagedList");
            }

            return BootstrapPager(html, pagedList, pagedList.ItemsPerPage /* numberOfPagesToShow */);
        }

        /// <summary>
        /// Returns a block of Html that displays a paging control.
        /// </summary>
        /// <typeparam name="TModel">The type of model.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="pagedList">The <see cref="PagedList{TModel}"/> to use to build the pager.</param>
        /// <param name="numberOfPagesToShow">The number of pages to show in the paging control.</param>
        /// <returns>A block of HTML that contains the pager controls.</returns>
        public static MvcHtmlString BootstrapPager<TModel>(this HtmlHelper html, PagedList<TModel> pagedList, int numberOfPagesToShow) where TModel : class
        {
            return BootstrapPager(html, pagedList, numberOfPagesToShow /* numberOfPagesToShow */, 2 /* numberOfPagesInEdge */);
        }

        /// <summary>
        /// Returns a block of Html that displays a paging control.
        /// </summary>
        /// <typeparam name="TModel">The type of model.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends.</param>
        /// <param name="pagedList">The <see cref="PagedList{TModel}"/> to use to build the pager.</param>
        /// <param name="numberOfPagesToShow">The number of pages to show in the paging control.</param>
        /// <param name="numberOfPagesInEdge">The number of edge pages to show (beginning and end pages) in the paging control.</param>
        /// <returns>A block of HTML that contains the pager controls.</returns>
        public static MvcHtmlString BootstrapPager<TModel>(this HtmlHelper html, PagedList<TModel> pagedList, int numberOfPagesToShow, int numberOfPagesInEdge) where TModel : class
        {
            return MvcHtmlString.Create(BootstrapPagerHelper(html, pagedList, numberOfPagesToShow, numberOfPagesInEdge));
        }

        internal static string BootstrapPagerHelper<TModel>(HtmlHelper html, PagedList<TModel> pagedList, int numberOfPagesToShow, int numberOfPagesInEdge) where TModel : class
        {
            IDictionary<string, object> values = html.ViewContext.RouteData.Values;
            HttpRequestBase httpRequest = html.ViewContext.RequestContext.HttpContext.Request;

            string pageParamName = "page";

            string routeName = null;
            string actionName = null;
            string controllerName = null;

            int pageCount = pagedList.PageCount;
            int currentPage = pagedList.CurrentPage;

            Func<string, int, string> getPageLink = (text, page) =>
            {
                RouteValueDictionary newValues = new RouteValueDictionary();

                foreach (KeyValuePair<string, object> pair in values)
                {
                    if (!pair.Key.Equals("controller", StringComparison.OrdinalIgnoreCase) &&
                        !pair.Key.Equals("action", StringComparison.OrdinalIgnoreCase))
                    {
                        newValues[pair.Key] = pair.Value;
                    }
                }

                foreach (string key in httpRequest.QueryString.Keys)
                {
                    if (!newValues.ContainsKey(key))
                    {
                        newValues.Add(key, httpRequest.QueryString[key].ToString());
                    }
                }

                if (page > 0)
                {
                    newValues[pageParamName] = page;
                }

                string link;

                if (!String.IsNullOrEmpty(routeName))
                {
                    link = html.RouteLink(text, routeName, newValues).ToHtmlString();
                }
                else
                {
                    actionName = actionName ?? values["action"].ToString();
                    controllerName = controllerName ?? values["controller"].ToString();

                    string url = new UrlHelper(httpRequest.RequestContext).Action(actionName, controllerName, newValues);
                    link = String.Format(CultureInfo.CurrentCulture, "<a href=\"{0}\">{1}</a>", url, text);
                }

                return link;
            };

            var pagerHtml = new StringBuilder();

            if (pageCount > 1)
            {
                pagerHtml.Append("<div class=\"pagination\"> <ul>");

                double half = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(numberOfPagesToShow) / 2));

                int start = Convert.ToInt32((currentPage > half) ? Math.Max(Math.Min((currentPage - half), (pageCount - numberOfPagesToShow)), 0) : 0);
                int end = Convert.ToInt32((currentPage > half) ? Math.Min(currentPage + half, pageCount) : Math.Min(numberOfPagesToShow, pageCount));

                pagerHtml.Append(currentPage > 1 ? String.Format(CultureInfo.CurrentCulture, " <li class=\"prev\">{0}</li>", getPageLink("&larr; Previous", currentPage - 1)) : " <li class=\"prev disabled\"><a href=\"#\">&larr; Previous</a></li>");

                if (start > 0)
                {
                    int startingEnd = Math.Min(numberOfPagesInEdge, start);

                    for (int i = 0; i < startingEnd; i++)
                    {
                        int page = i + 1;
                        pagerHtml.Append(String.Format(CultureInfo.CurrentCulture, " <li>{0}</li>", getPageLink(page.ToString(CultureInfo.CurrentCulture), page)));
                    }

                    if (numberOfPagesInEdge < start)
                    {
                        pagerHtml.Append(" <li class=\"disabled\"><a href=\"#\">...</a></li>");
                    }
                }

                for (int i = start; i < end; i++)
                {
                    int page = i + 1;
                    pagerHtml.Append(page == currentPage ? String.Format(CultureInfo.CurrentCulture, " <li class=\"active\"><a href=\"#\">{0}</a></li>", page) : String.Format(CultureInfo.CurrentCulture, " <li>{0}</li>", getPageLink(page.ToString(CultureInfo.CurrentCulture), page)));
                }

                if (end < pageCount)
                {
                    if ((pageCount - numberOfPagesInEdge) > end)
                    {
                        pagerHtml.Append(" <li class=\"disabled\"><a href=\"#\">...</a></li>");
                    }

                    int endingStart = Math.Max(pageCount - numberOfPagesInEdge, end);

                    for (int i = endingStart; i < pageCount; i++)
                    {
                        int page = i + 1;
                        pagerHtml.Append(String.Format(" <li>{0}</li>", getPageLink(page.ToString(CultureInfo.CurrentCulture), page)));
                    }
                }

                pagerHtml.Append(currentPage < pageCount ? String.Format(CultureInfo.CurrentCulture, " <li class=\"next\">{0}</li>", getPageLink("Next &rarr;", currentPage + 1)) : " <li class=\"next disabled\"><a href=\"#\">Next &rarr;</a></li>");
                pagerHtml.Append(" </ul> </div>");
            }

            return pagerHtml.ToString();
        }
    }

    /// <summary>
    /// Represents a <see cref="PagedList{TModel}"/> that will hold a collection of items to be displayed in pages.
    /// </summary>
    /// <typeparam name="TModel">The type of model to use.</typeparam>
    public class PagedList<TModel> where TModel : class
    {
        /// <summary>
        /// Creates a new instance of a <see cref="PagedList{TModel}"/> that will hold a collection of items to be displayed in pages.
        /// </summary>
        /// <param name="items">The collection of items.</param>
        /// <param name="currentPage">The current page.</param>
        /// <param name="itemsPerPage">The total number of items displayed on a page.</param>
        /// <param name="totalCount">The total number of items contained in the collection.</param>
        public PagedList(IEnumerable<TModel> items, int currentPage, int itemsPerPage, int totalCount)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            Items = new List<TModel>(items);
            CurrentPage = currentPage;
            ItemsPerPage = itemsPerPage;
            TotalCount = totalCount;
        }

        /// <summary>
        /// Gets the collection of items.
        /// </summary>
        public IList<TModel> Items
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current page.
        /// </summary>
        public int CurrentPage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total number of items displayed on a page.
        /// </summary>
        public int ItemsPerPage
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total number of items contained in the collection.
        /// </summary>
        public int TotalCount
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the total number of pages contained in the collection.
        /// </summary>
        public int PageCount
        {
            get
            {
                return PageCalculator.TotalPages(TotalCount, ItemsPerPage);
            }
        }

        /// <summary>
        /// Provides calculations that are needed to build a Pager.
        /// </summary>
        public static class PageCalculator
        {
            /// <summary>
            /// Calculates the total number of pages based on the total number of items and the items per page allowed.
            /// </summary>
            /// <param name="total">The total number of items.</param>
            /// <param name="itemPerPage">The items per page allowed.</param>
            /// <returns></returns>
            public static int TotalPages(int total, int itemPerPage)
            {
                if ((total == 0) || (itemPerPage == 0))
                {
                    return 1;
                }

                if ((total % itemPerPage) == 0)
                {
                    return total / itemPerPage;
                }

                double result = Convert.ToDouble(total / itemPerPage);

                result = Math.Ceiling(result);

                return Convert.ToInt32(result) + 1;
            }

            /// <summary>
            /// Calculates a start index based on the provided page number and the items per page allowed.
            /// </summary>
            /// <param name="page">The current page number.</param>
            /// <param name="itemPerPage">The items per page allowed.</param>
            /// <returns></returns>
            public static int StartIndex(int? page, int itemPerPage)
            {
                return (page.HasValue && (page.Value > 1)) ? ((page.Value - 1) * itemPerPage) : 0;
            }
        }
    }
}