using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Linq;

namespace LeaseCrunch.Core.OData.AspNetCore.ApiVersioning.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IMvcBuilder AddODataApiVersioning(this IMvcBuilder mvc)
    {
        mvc.Services.AddODataApiVersioning();
        return mvc;
    }

    public static IServiceCollection AddODataApiVersioning(this IServiceCollection services)
    {
        services.TryAddSingleton<ODataApiVersioningModelConvention>();

        services.AddOptions<ODataOptions>()
            .PostConfigure<ODataApiVersioningModelConvention>((opts, convention) =>
            {
                var attributeRoutingConventions = opts.Conventions.OfType<AttributeRoutingConvention>().ToArray();

                foreach (var attributeRoutingConvention in attributeRoutingConventions)
                {
                    opts.Conventions.Remove(attributeRoutingConvention);
                }

                opts.Conventions.Add(convention);
            });

        return services;
    }
}