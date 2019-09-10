using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace PaginationHelper
{
    public static class PaginationHelperExtensions
    {
        /// <summary>
        /// Adds services for PageHelper
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <param name="defaultPageSize">Default page size</param>
        /// <returns>Service Collection</returns>
        public static IServiceCollection AddPaginationHelper(this IServiceCollection services, int defaultPageSize = 200)
        {
            services.AddSingleton<IPageConfig, PageConfig>(provider => new PageConfig(defaultPageSize));
            services.TryAddScoped<IPageHelper, PageHelper>();

            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services
            .TryAddScoped(x => x
                .GetRequiredService<IUrlHelperFactory>()
                .GetUrlHelper(x.GetRequiredService<IActionContextAccessor>().ActionContext));

            return services;
        }
    }
}
