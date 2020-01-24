namespace PaginationHelper
{
    /// <summary>
    ///  A container for page size, page counts, ...
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// Total number of pages
        /// </summary>
        public int NumberOfPages { get; set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Total number of records
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Size of the current page
        /// </summary>
        public int PageSize { get; set; }
    }
}
