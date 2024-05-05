using AutoMapper;
using Bogus;
using Domain;
using LeaseCrunch.Core.OData.AspNetCore.ApiVersioning.ApiExplorer.DependencyInjection;
using LeaseCrunch.Core.OData.AspNetCore.ApiVersioning.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OData.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Exceptions.MsSqlServer.Destructurers;
using System;

Randomizer.Seed = new Random(12345);

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, config) =>
{
    var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Debug);

    config
        .MinimumLevel.ControlledBy(levelSwitch)
        .Enrich.FromLogContext()
        .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder()
            .WithDefaultDestructurers()
            .WithDestructurers([new DbUpdateExceptionDestructurer(), new SqlExceptionDestructurer()]))
        .WriteTo.Seq(
            serverUrl: "http://localhost:5341",
            apiKey: "",
            controlLevelSwitch: levelSwitch)
        .WriteTo.Console();
});

builder.Services
    .ConfigureOptions<ODataConfigurator>()
    .ConfigureOptions<SwaggerConfigurator>()
    
    .AddRouting()
    .AddApiVersioning()
    .AddVersionedApiExplorer()
    .AddSwaggerGen()
    
    .AddControllers()
    .AddOData()
    .AddODataApiVersioning()
    .AddODataApiVersioningApiExplorer();

builder
    .AddDomainServices();

var app = builder.Build();

app.Services.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();

var environment = app.Services.GetRequiredService<IHostEnvironment>();

if (environment.IsDevelopment())
{
    app.UseODataRouteDebug("api/$odata");
}

app
    .MapWhen(IsApiRoute, api =>
        api
            .UseSerilogRequestLogging()
            .UseRouting()
            .UseEndpoints(endpoints => endpoints.MapControllers()))
    .UseSwagger()
    .UseSwaggerUI();

app.Run();
return;

static bool IsApiRoute(HttpContext ctx) =>
    ctx.Request.Path.Value?.StartsWith(
        "/api/v",
        StringComparison.OrdinalIgnoreCase) == true;