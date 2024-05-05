using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.OData.Query;
using System.Linq;

namespace LeaseCrunch.Core.OData.AspNetCore.ApiVersioning.ApiExplorer;

public class ODataApiVersioningDescriptionProvider : IApiDescriptionProvider
{
    public int Order => int.MaxValue;

    private readonly IModelMetadataProvider modelMetadataProvider;

    public ODataApiVersioningDescriptionProvider(IModelMetadataProvider modelMetadataProvider)
    {
        this.modelMetadataProvider = modelMetadataProvider;
    }

    public void OnProvidersExecuting(ApiDescriptionProviderContext context)
    {
    }

    public void OnProvidersExecuted(ApiDescriptionProviderContext context)
    {
        foreach (var parameterDescriptions in context.Results.Select(x => x.ParameterDescriptions))
        {
            var odataQueryOptionsParameter = parameterDescriptions
                .SingleOrDefault(p => p.Type is {IsConstructedGenericType: true}
                                      && p.Type.GetGenericTypeDefinition() == typeof(ODataQueryOptions<>));

            if (odataQueryOptionsParameter != null)
            {
                parameterDescriptions.Remove(odataQueryOptionsParameter);

                parameterDescriptions.Add(BuildQueryParameter<string>("$select"));
                parameterDescriptions.Add(BuildQueryParameter<string>("$expand"));
                parameterDescriptions.Add(BuildQueryParameter<string>("$filter"));
                parameterDescriptions.Add(BuildQueryParameter<string>("$orderby"));
                parameterDescriptions.Add(BuildQueryParameter<int>("$top"));
                parameterDescriptions.Add(BuildQueryParameter<int>("$skip"));
                parameterDescriptions.Add(BuildQueryParameter<bool>("$count"));
            }
        }
    }

    private ApiParameterDescription BuildQueryParameter<TParameter>(string name)
    {
        var bindingInfo = new BindingInfo
        {
            BindingSource = BindingSource.Query
        };

        return new ApiParameterDescription
        {
#if NET5_0_OR_GREATER
            BindingInfo = bindingInfo,
#endif
            DefaultValue = null,
            IsRequired = false,
            Name = name,
            ModelMetadata = modelMetadataProvider.GetMetadataForType(typeof(TParameter)),
            ParameterDescriptor = new ControllerParameterDescriptor
            {
                BindingInfo = bindingInfo,
                Name = name,
                ParameterType = typeof(TParameter)
            },
            Source = BindingSource.Query,
            Type = typeof(TParameter)
        };
    }
}