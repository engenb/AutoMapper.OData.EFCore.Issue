using LeaseCrunch.GeneralLedger.Infrastructure.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Linq;

namespace OData.Configuration;

internal class SwaggerConfigurator :
    IConfigureOptions<SwaggerGenOptions>,
    IConfigureOptions<SwaggerOptions>,
    IConfigureOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _apiVersions;

    public SwaggerConfigurator(IApiVersionDescriptionProvider apiVersions)
    {
        _apiVersions = apiVersions ?? throw new ArgumentNullException(nameof(apiVersions));
    }

    public void Configure(SwaggerGenOptions options)
    {
        options.DocumentFilter<VersionFilter>();
        options.DocumentFilter<ODataPreferHeaderFilter>();
        options.OperationFilter<DefaultResponseFilter>();

        foreach (var apiVersionDesc in _apiVersions.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                apiVersionDesc.GroupName,
                new OpenApiInfo
                {
                    Title = "API",
                    Version = apiVersionDesc.ApiVersion.ToString()
                });
        }
    }

    public void Configure(SwaggerOptions options)
    {
        options.RouteTemplate = "api/swagger/{documentName}/swagger.json";
    }

    public void Configure(SwaggerUIOptions options)
    {
        options.RoutePrefix = "api/explorer";
        options.ConfigObject.DisplayRequestDuration = true;

        foreach (var groupName in _apiVersions.ApiVersionDescriptions
                     .Reverse()
                     .Select(x => x.GroupName))
            options.SwaggerEndpoint(
                $"/api/swagger/{groupName}/swagger.json",
                $"API {groupName}");
    }
}