namespace PaginationHelper
{
    /// <summary>
    /// Page configuration
    /// </summary>
    public interface IPageConfig
    {
        /// <summary>
        /// Default page size of an envelope
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Parameter name for the page specified in the generated urls
        /// </summary>
        string PageParameterName { get; }

        /// <summary>
        /// Parameter name for the pageSize specified in the generated urls
        /// </summary>
        string PageSizeParameterName { get; }
    }
}
