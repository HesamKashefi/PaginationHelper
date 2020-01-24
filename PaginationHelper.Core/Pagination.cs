namespace PaginationHelper
{
    /// <summary>
    /// Container of pagination data
    /// </summary>
    public class Pagination
    {
        public Pagination()
        {

        }

        public Pagination(Pager pager)
        {
            Pager = pager;
        }

        /// <summary>
        /// Pager of the pagination
        /// </summary>
        public Pager Pager { get; set; }
        
        /// <summary>
        /// Current page's link
        /// </summary>
        public string Self { get; set; }
        
        /// <summary>
        /// Next page's link
        /// </summary>
        public string NextPage { get; set; }

        /// <summary>
        /// Previous page's link
        /// </summary>
        public string PrevPage { get; set; }

        /// <summary>
        /// First page's link
        /// </summary>
        public string FirstPage { get; set; }

        /// <summary>
        /// Last page's link
        /// </summary>
        public string LastPage { get; set; }
    }
}
