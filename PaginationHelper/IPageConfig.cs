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
    }
}
