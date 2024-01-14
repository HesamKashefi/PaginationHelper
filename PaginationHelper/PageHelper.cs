using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaginationHelper
{
    /// <summary>
    /// Used to create pagination
    /// </summary>
    public partial class PageHelper : IPageHelper
    {
        #region Private Fields
        private readonly Func<int, int, int> _countFrom =
            (pageSize, pageNumber) => pageNumber == 1 ? 0 : pageSize * pageNumber - pageSize;
        #endregion

        #region Dependencies

        private readonly IPageConfig _pageConfig;
        private readonly IMapper _mapper;
        private readonly IUrlHelper _urlHelper;

        #endregion

        #region Ctor
        public PageHelper(IOptions<PageConfig> options, IUrlHelper urlHelper, IMapper mapper)
        {
            _pageConfig = options.Value;
            _urlHelper = urlHelper;
            _mapper = mapper;
        }
        #endregion

        #region Implementation of IPageHelper

        /// <inheritdoc />
        public async Task<Envelope<T[]>> GetPageAsync<T>(
            IQueryable<T> items,
            PaginationDto paginationDto,
            CancellationToken cancellationToken = default) where T : class
        {
            var pagination = paginationDto ?? throw new ArgumentNullException(nameof(paginationDto));
            pagination.PageSize = pagination.PageSize ?? _pageConfig.PageSize;

            var countFrom = _countFrom(pagination.PageSize.Value, pagination.Page);

            var records = await items
                .Skip(countFrom)
                .Take(pagination.PageSize.Value)
                .AsNoTracking()
                .ToArrayAsync(cancellationToken);

            var numberOfRecords = await items.CountAsync(cancellationToken);

            return GetEnvelope(records, pagination.Page, pagination.PageSize.Value, numberOfRecords);
        }

        public Envelope<T[]> GetPage<T>(IEnumerable<T> items, PaginationDto paginationDto) where T : class
        {
            var pagination = paginationDto ?? throw new ArgumentNullException(nameof(paginationDto));
            pagination.PageSize = pagination.PageSize ?? _pageConfig.PageSize;

            var countFrom = _countFrom(pagination.PageSize.Value, pagination.Page);

            var records = items
                .Skip(countFrom)
                .Take(pagination.PageSize.Value)
                .ToArray();

            var numberOfRecords = items.Count();

            return GetEnvelope(records, pagination.Page, pagination.PageSize.Value, numberOfRecords);
        }

        /// <inheritdoc />
        public async Task<Envelope<TTarget[]>> GetProjectedPageAsync<TSource, TTarget>(
            IQueryable<TSource> items,
            PaginationDto paginationDto,
            CancellationToken cancellationToken = default)
            where TSource : class
            where TTarget : class
        {
            var pagination = paginationDto ?? throw new ArgumentNullException(nameof(paginationDto));
            pagination.PageSize = pagination.PageSize ?? _pageConfig.PageSize;

            var countFrom = _countFrom(pagination.PageSize.Value, pagination.Page);
            var records = await items
                .Skip(countFrom)
                .Take(pagination.PageSize.Value)
                .AsNoTracking()
                .ProjectTo<TTarget>(_mapper.ConfigurationProvider)
                .ToArrayAsync(cancellationToken);

            var numberOfRecords = await items.CountAsync(cancellationToken);

            return GetEnvelope(records, pagination.Page, pagination.PageSize.Value, numberOfRecords);
        }

        public Envelope<TTarget[]> GetProjectedPage<TSource, TTarget>(IEnumerable<TSource> items, PaginationDto paginationDto)
            where TSource : class
            where TTarget : class
        {
            var pagination = paginationDto ?? throw new ArgumentNullException(nameof(paginationDto));
            pagination.PageSize = pagination.PageSize ?? _pageConfig.PageSize;

            var countFrom = _countFrom(pagination.PageSize.Value, pagination.Page);
            var records = items
                .Skip(countFrom)
                .Take(pagination.PageSize.Value)
                .ToArray();

            var mappedRecords = _mapper.Map<TTarget[]>(records);

            var numberOfRecords = items.Count();

            return GetEnvelope(mappedRecords, pagination.Page, pagination.PageSize.Value, numberOfRecords);
        }

        /// <inheritdoc />
        public Pagination GetPagination(Pager pager)
        {
            var routeName = _urlHelper.ActionContext.ActionDescriptor.AttributeRouteInfo.Name;
            var routeValues = _urlHelper.ActionContext.ActionDescriptor.RouteValues;

            routeValues[_pageConfig.PageSizeParameterName] = pager.PageSize.ToString();

            var pagination = new Pagination
            {
                Self = _urlHelper.Link(routeName, new RouteValueDictionary(routeValues)
                {
                    [_pageConfig.PageParameterName] = pager.CurrentPage
                }),
                Pager = pager
            };

            if (pager.CurrentPage < pager.NumberOfPages)
            {
                pagination.NextPage = _urlHelper.Link(routeName, new RouteValueDictionary(routeValues)
                {
                    [_pageConfig.PageParameterName] = pager.CurrentPage + 1
                });
            }

            if (pager.CurrentPage > 1)
            {
                pagination.PrevPage = _urlHelper.Link(routeName, new RouteValueDictionary(routeValues)
                {
                    [_pageConfig.PageParameterName] = pager.CurrentPage - 1
                });
            }

            if (pager.NumberOfPages > 1)
            {
                pagination.FirstPage = _urlHelper.Link(routeName, new RouteValueDictionary(routeValues)
                {
                    [_pageConfig.PageParameterName] = 1
                });

                pagination.LastPage = _urlHelper.Link(routeName, new RouteValueDictionary(routeValues)
                {
                    [_pageConfig.PageParameterName] = pager.NumberOfPages
                });
            }

            return pagination;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Wraps data in an envelope
        /// </summary>
        /// <typeparam name="T">Type of data elements</typeparam>
        /// <param name="records">current page's records</param>
        /// <param name="page">Current page</param>
        /// <param name="pageSize">Size of the page</param>
        /// <param name="numberOfRecords">Total records count</param>
        /// <returns>Envelope containing data</returns>
        private Envelope<T[]> GetEnvelope<T>(T[] records, int page, int pageSize, int numberOfRecords) where T : class
        {
            var numberOfPages = GetPagingCount(numberOfRecords, pageSize);
            var pager = new Pager
            {
                NumberOfPages = numberOfPages,
                CurrentPage = page,
                TotalRecords = numberOfRecords,
                PageSize = pageSize
            };

            return new Envelope<T[]>
            {
                Data = records,
                Meta = new Meta
                {
                    Links = GetPagination(pager),
                    Count = records.Count()
                }
            };
        }

        /// <summary>
        /// Calculates number of total pages
        /// </summary>
        /// <param name="count">Total records</param>
        /// <param name="pageSize">Page size of the request</param>
        /// <returns>Number of total pages</returns>
        private static int GetPagingCount(int count, int pageSize)
        {
            var extraCount = count % pageSize > 0 ? 1 : 0;
            return count < pageSize ? 1 : count / pageSize + extraCount;
        }

        #endregion
    }
}
