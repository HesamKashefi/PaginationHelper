using System.ComponentModel.DataAnnotations;

namespace PaginationHelper
{
    /// <summary>
    /// Used to retrieve pagination data from Query String
    /// </summary>
    public class PaginationDto
    {
        public PaginationDto()
        {
        }

        public PaginationDto(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }

        /// <summary>
        /// Number of the page
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Size of the page
        /// </summary>
        [Range(1, 200)]
        public int? PageSize { get; set; } = 20;
    }
}
