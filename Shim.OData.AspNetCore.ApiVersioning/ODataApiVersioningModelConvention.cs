using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.OData.Routing;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.AspNetCore.OData.Routing.Parser;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OData;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace LeaseCrunch.Core.OData.AspNetCore.ApiVersioning;

public class ODataApiVersioningModelConvention : AttributeRoutingConvention
{
    private ApiVersioningOptions ApiVersioningOptions => _apiVersioningOptions.Value;
    private readonly IOptions<ApiVersioningOptions> _apiVersioningOptions;
    private readonly IODataPathTemplateParser _templateParser;
    private readonly ILogger _logger;

    private readonly Regex _apiVersionPattern;

    public ODataApiVersioningModelConvention(
        IODataPathTemplateParser templateParser,
        IOptions<ApiVersioningOptions> apiVersioningOptions,
        ILoggerFactory loggerFactory):
        base(loggerFactory.CreateLogger<AttributeRoutingConvention>(), templateParser)
    {
        _templateParser = templateParser;
        _apiVersioningOptions = apiVersioningOptions;
        _logger = loggerFactory.CreateLogger<ODataApiVersioningModelConvention>();

        _apiVersionPattern = new Regex($"(^.*)({{.*:{ApiVersioningOptions.RouteConstraintName}}})(.*$)");
    }

    /// <inheritdoc />
    public override bool AppliesToAction(ODataControllerActionContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        // Be noted, the validation checks (non OData controller, non OData action) are done before calling this method.
        var controllerModel = context.Controller;
        var actionModel = context.Action;

        var isODataController = controllerModel.Attributes.Any(a => a is ODataAttributeRoutingAttribute);
        var isODataAction = actionModel.Attributes.Any(a => a is ODataAttributeRoutingAttribute);

        // At least one of controller or action has "ODataRoutingAttribute"
        // The best way is to derive your controller from ODataController.
        if (!isODataController && !isODataAction) return false;

        // TODO: Which one is better? input from context or inject from constructor?
        var prefixes = context.Options.RouteComponents.Keys;

        // Loop through all attribute routes defined on the controller.
        var controllerSelectors = controllerModel.Selectors.Where(sm => sm.AttributeRouteModel != null).ToList();
        if (controllerSelectors.Count == 0)
        {
            // If no controller route template, we still need to go through action to process the action route template.
            controllerSelectors.Add(null);
        }

        // In order to avoiding polluting the action selectors, we use a Dictionary to save the intermediate results.
        var updatedSelectors = new Dictionary<SelectorModel, IList<SelectorModel>>();
        foreach (var actionSelector in actionModel.Selectors)
        {
            if (actionSelector.AttributeRouteModel is {IsAbsoluteTemplate: true})
            {
                ProcessAttributeModel(actionSelector.AttributeRouteModel, prefixes, context, actionSelector, actionModel, controllerModel, updatedSelectors);
            }
            else
            {
                foreach (var controllerSelector in controllerSelectors)
                {
                    var combinedRouteModel = AttributeRouteModel.CombineAttributeRouteModel(controllerSelector?.AttributeRouteModel, actionSelector.AttributeRouteModel);
                    ProcessAttributeModel(combinedRouteModel, prefixes, context, actionSelector, actionModel, controllerModel, updatedSelectors);
                }
            }
        }

        // remove the old one.
        foreach (var selector in updatedSelectors)
        {
            actionModel.Selectors.Remove(selector.Key);
        }

        // add new one.
        foreach (var newSelector in updatedSelectors.SelectMany(selector => selector.Value))
        {
            actionModel.Selectors.Add(newSelector);
        }

        // let's just return false to let this action go to other conventions.
        return false;
    }

    private void ProcessAttributeModel(
        AttributeRouteModel? attributeRouteModel,
        IEnumerable<string> prefixes,
        ODataControllerActionContext context,
        SelectorModel actionSelector,
        ActionModel actionModel,
        ControllerModel controllerModel,
        IDictionary<SelectorModel, IList<SelectorModel>> updatedSelectors)
    {
        // not an attribute routing, skip it.
        if (attributeRouteModel == null) return;

        var routeTemplates = GenerateEffectiveRoutes(actionModel, attributeRouteModel);

        foreach (var routeTemplate in routeTemplates)
        {
            var prefix = FindRelatedODataPrefix(
                routeTemplate,
                prefixes,
                out var newRouteTemplate);

            if (prefix == null) continue;

            var model = context.Options.RouteComponents[prefix].EdmModel;
            var sp = context.Options.RouteComponents[prefix].ServiceProvider;

            var newSelectorModel = CreateActionSelectorModel(
                prefix,
                model,
                sp,
                newRouteTemplate,
                actionSelector,
                attributeRouteModel.Template,
                actionModel.ActionName,
                controllerModel.ControllerName);

            if (newSelectorModel == null) continue;

            if (!updatedSelectors.TryGetValue(actionSelector, out var selectors))
            {
                selectors = new List<SelectorModel>();
                updatedSelectors[actionSelector] = selectors;
            }

            selectors.Add(newSelectorModel);
        }
    }

    private SelectorModel? CreateActionSelectorModel(
        string prefix,
        IEdmModel model,
        IServiceProvider sp,
        string? routeTemplate,
        SelectorModel actionSelectorModel,
        string? originalTemplate,
        string actionName,
        string controllerName)
    {
        try
        {
            // Due the uri parser, it will throw exception if the route template is not a OData path.
            var pathTemplate = _templateParser.Parse(model, routeTemplate, sp);
            if (pathTemplate != null)
            {
                // Create a new selector model?
                var newSelectorModel = new SelectorModel(actionSelectorModel);
                // Shall we remove any certain attributes/metadata?
                ClearMetadata(newSelectorModel);

                // Add OData routing metadata
                var odataMetadata = new ODataRoutingMetadata(prefix, model, pathTemplate);
                newSelectorModel.EndpointMetadata.Add(odataMetadata);

                // replace the attribute routing template using absolute routing template to avoid appending any controller route template
                newSelectorModel.AttributeRouteModel = new AttributeRouteModel
                {
                    Template = $"/{originalTemplate}" // add a "/" to make sure it's absolute template, don't combine with controller
                };

                return newSelectorModel;
            }

            return null;
        }
        catch (ODataException ex)
        {
            // use the logger to log the wrong odata attribute template. Shall we log the others?

            // Whether we throw exception or mark it as warning is a design pattern.
            _logger.LogWarning(
                ex,
                "The path template '{OriginalTemplate}' on the action '{ActionName}' in controller '{ControllerName}' is not a valid OData path template. {ExceptionMessage}",
                originalTemplate,
                actionName,
                controllerName,
                ex.Message);
            return null;
        }
    }

    private static void ClearMetadata(SelectorModel selectorModel)
    {
        // remove the unused metadata
        for (var i = selectorModel.EndpointMetadata.Count - 1; i >= 0; i--)
        {
            if (selectorModel.EndpointMetadata[i] is IRouteTemplateProvider)
            {
                selectorModel.EndpointMetadata.RemoveAt(i);
            }
        }
    }

    private IEnumerable<string> GenerateEffectiveRoutes(ActionModel action, AttributeRouteModel attributeRouteModel)
    {
        // test if route template contains an ApiVersion parameter
        if (_apiVersionPattern.IsMatch(attributeRouteModel.Template!))
        {
            var supportedVersions = new List<ApiVersion>();

            // check the action first for [ApiVersion]
            supportedVersions.AddRange(GetApiVersions(action));

            if (!supportedVersions.Any())
            {
                // if no explicit action [ApiVersion]s are defined, the framework falls back
                // to the controller-level
                supportedVersions.AddRange(GetApiVersions(action.Controller));
            }

            // this probably isn't bullet-proof
            if (!supportedVersions.Any() && ApiVersioningOptions.AssumeDefaultVersionWhenUnspecified)
            {
                // no [ApiVersion]s found on action or controller,
                // add the default api version
                supportedVersions.Add(ApiVersioningOptions.DefaultApiVersion);
            }

            return supportedVersions
                .Select(x => _apiVersionPattern.Replace(attributeRouteModel.Template!, $"${{1}}{x}${{3}}"))
                .ToArray();
        }

        return new [] {attributeRouteModel.Template}!;
    }

    private static IEnumerable<ApiVersion> GetApiVersions(ICommonModel model) => model
        .Attributes
        .OfType<ApiVersionAttribute>()
        .SelectMany(attr => attr.Versions);

    private static string? FindRelatedODataPrefix(
        string routeTemplate,
        IEnumerable<string> prefixes,
        out string? newRouteTemplate)
    {
        if (routeTemplate.StartsWith('/'))
        {
            routeTemplate = routeTemplate[1..];
        }
        else if (routeTemplate.StartsWith("~/", StringComparison.Ordinal))
        {
            routeTemplate = routeTemplate[2..];
        }

        // the input route template could be:
        // #1) odata/Customers/{key}
        // #2) orders({key})
        // So, #1 matches the "odata" prefix route
        //     #2 matches the non-odata prefix route
        // Since #1 and #2 can be considered starting with "",
        // In order to avoiding ambiguous, let's compare non-empty route prefix first,
        // If no match, then compare empty route prefix.
        string? emptyPrefix = null;
        foreach (var prefix in prefixes)
        {
            if (string.IsNullOrEmpty(prefix))
            {
                emptyPrefix = prefix;
            }
            else if (routeTemplate.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                // we hit: "odata/Customers/{key}" scenario, let's remove the "odata" route prefix
                newRouteTemplate = routeTemplate[prefix.Length..];

                // why do like this: because the input route template could be "odata", after remove the prefix, it's empty string.
                if (newRouteTemplate.StartsWith("/", StringComparison.Ordinal))
                {
                    newRouteTemplate = newRouteTemplate[1..];
                }

                return prefix;
            }
        }

        // we are here because no non-empty prefix matches.
        if (emptyPrefix != null)
        {
            // So, if we have empty prefix route, it could match all OData route template.
            newRouteTemplate = routeTemplate;
            return emptyPrefix;
        }

        newRouteTemplate = null;
        return null;
    }
}