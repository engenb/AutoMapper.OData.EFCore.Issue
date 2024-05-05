using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Options;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;

namespace OData.Configuration;

internal class ODataConfigurator : IConfigureOptions<ODataOptions>
{
    private readonly Lazy<IEdmModel> _model;
    private IEdmModel Model => _model.Value;

    public ODataConfigurator()
    {
        _model = new Lazy<IEdmModel>(BuildModel);
    }

    public void Configure(ODataOptions options)
    {
        options.AddRouteComponents("api/v1", Model);
    }

    private static IEdmModel BuildModel()
    {
        var builder = new ODataConventionModelBuilder().EnableLowerCamelCase();

        BuildFooModel(builder);

        return builder.GetEdmModel();
    }

    private static void BuildFooModel(ODataModelBuilder builder)
    {
        var entityType = builder.EntitySet<ApiModel.Foo>("Foos").EntityType;

        entityType.HasKey(x => x.Id);

        entityType
            .Select()
            .Filter();
    }
}