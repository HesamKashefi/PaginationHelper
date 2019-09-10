using System;

namespace PaginationHelper
{
    /// <summary>
    /// Sets defaults for pagination
    /// </summary>
    public class PageConfig : IPageConfig
    {
        public PageConfig()
        {

        }
        public PageConfig(int pageSize)
        {
            if (pageSize < 1)
            {
                throw new ArgumentException("pageSize cannot be less than 1", nameof(pageSize));
            }
            PageSize = pageSize;
        }

        public int PageSize { get; } = 25;
    }
}
