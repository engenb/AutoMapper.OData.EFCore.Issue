using Domain.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Domain;

public static class WebApplicationBuilderExtensions
{
    public static TBuilder AddDomainServices<TBuilder>(this TBuilder builder)
        where TBuilder : IHostApplicationBuilder
    {
        builder.Services
            .AddAutoMapper(Assembly.GetExecutingAssembly())
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))

            .AddDbContext<TestDbContext>((sp, efCore) => efCore
                .UseSqlServer(
                    builder.Configuration.GetConnectionString(nameof(TestDbContext)),
                    sqlServer => sqlServer.EnableRetryOnFailure()));

        return builder;
    }
}