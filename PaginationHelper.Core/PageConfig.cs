using System;

namespace PaginationHelper
{
    /// <summary>
    /// Sets defaults for pagination
    /// </summary>
    public class PageConfig : IPageConfig
    {
        public PageConfig() : this(25)
        {

        }

        public PageConfig(int pageSize, string pageParameterName = "page", string pageSizeParameterName = "pageSize")
        {
            if (pageSize < 1)
            {
                throw new ArgumentException("pageSize cannot be less than 1", nameof(pageSize));
            }
            PageSize = pageSize;
            PageParameterName = pageParameterName ?? throw new ArgumentNullException(nameof(pageParameterName));
            PageSizeParameterName = pageSizeParameterName ?? throw new ArgumentNullException(nameof(pageSizeParameterName));
        }

        /// <summary>
        /// Default page size of an envelope
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Parameter name for the page specified in the generated urls
        /// </summary>
        public string PageParameterName { get; }

        /// <summary>
        /// Parameter name for the pageSize specified in the generated urls
        /// </summary>
        public string PageSizeParameterName { get; }
    }
}
