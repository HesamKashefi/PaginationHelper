using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaginationHelper
{
    /// <summary>
    /// A helper for data pagination
    /// </summary>
    public interface IPageHelper
    {
        /// <summary>
        /// Returns the data of the specified page
        /// </summary>
        /// <typeparam name="T">Type of data</typeparam>
        /// <param name="items">The IQueryable of the dataSource</param>
        /// <param name="paginationDto">Number of the page and page size</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The current page of the data</returns>
        Task<Envelope<T[]>> GetPageAsync<T>(
            IQueryable<T> items,
            PaginationDto paginationDto,
            CancellationToken cancellationToken = default) where T : class;

        /// <summary>
        /// Projects <paramref name="items"/> into the specified type and
        /// Returns the data of the specified page
        /// </summary>
        /// <typeparam name="TSource">Type of the source data</typeparam>
        /// <typeparam name="TTarget">Type of the data after projection</typeparam>
        /// <param name="items">The IQueryable of the dataSource</param>
        /// <param name="paginationDto">Number of the page and page size</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>The current page of the data</returns>
        Task<Envelope<TTarget[]>> GetProjectedPageAsync<TSource, TTarget>(
            IQueryable<TSource> items,
            PaginationDto paginationDto,
            CancellationToken cancellationToken = default)
            where TSource : class
            where TTarget : class;

        /// <summary>
        /// Generates pagination links
        /// </summary>
        /// <param name="pager">The pagination page stats data</param>
        /// <returns>Pagination data</returns>
        Pagination GetPagination(Pager pager);
    }
}
