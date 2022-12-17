namespace PaginationHelper
{
    /// <summary>
    /// Extra information about the current set of data
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// The status code of the response
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// Number of items being returned
        /// </summary>
        public int? Count { get; set; }

        /// <summary>
        /// Pagination links of the current resource
        /// </summary>
        public Pagination Links { get; set; }
    }
}
