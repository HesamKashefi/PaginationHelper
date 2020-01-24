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
        /// <param name="pageConfig">Default configuration for pagination</param>
        /// <returns>Service Collection</returns>
        public static IServiceCollection AddPaginationHelper(this IServiceCollection services, IPageConfig pageConfig)
        {
            services.AddSingleton(pageConfig);
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
