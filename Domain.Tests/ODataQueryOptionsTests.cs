
using AutoFixture;
using AutoMapper;
using AutoMapper.AspNet.OData;
using Bogus;
using Domain.Components;
using Domain.Components.Foos;
using Domain.Enumerations;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;

namespace Domain.Tests;

public class ODataQueryOptionsTests : IDisposable
{
    private readonly Faker _faker;
    private readonly IMapper _mapper;
    private readonly TestDbContext _dbContext;

    private readonly SqliteConnection _connection;

    public ODataQueryOptionsTests()
    {
        _faker = new Faker();

        _mapper = new MapperConfiguration(x => x.AddProfile<FooMaps>())
            .CreateMapper();

        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _dbContext = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(_connection)
            .Options);

        _dbContext.Database.EnsureCreated();
    }

    [Fact]
    public async Task GetAsync__Given_FooTypeThreeExists__When_FilterHas__Then_ReturnFoo()
    {
        var expectedFoo = new Foo { Id = Guid.NewGuid(), Type = BuildFooType(Enum.GetValues<FooType>()) };
        _dbContext.Set<Foo>().Add(expectedFoo);

        var typesWithoutThree = Enum.GetValues<FooType>().ToList();
        typesWithoutThree.Remove(FooType.Three);
        var unexpectedFoos = Enumerable.Range(0, 99)
            .Select(i => new Foo { Id = Guid.NewGuid(), Type = BuildFooType(typesWithoutThree.ToArray()) });
        _dbContext.Set<Foo>().AddRange(unexpectedFoos);

        await _dbContext.SaveChangesAsync();

        var oDataQueryOptions = BuildODataQueryOptions("http://localhost/api/v1/Foos?%24filter=type%20has%20%27Three%27");

        var actualFoos = await _dbContext.Set<Foo>()
            .AsNoTracking()
            .GetAsync(_mapper, oDataQueryOptions);

        actualFoos.Should().ContainSingle(x => x.Id == expectedFoo.Id)
            .Which
            .Type.Should().HaveSameValueAs(expectedFoo.Type);
    }

    private FooType BuildFooType(params FooType[] availableTypes)
    {
        var types = _faker.PickRandom(availableTypes, _faker.Random.Int(1, availableTypes.Length - 1));
        return types.Skip(1).Aggregate(types.First(), (result, next) => result | next);
    }

    private static ODataQueryOptions<ApiModel.Foo> BuildODataQueryOptions(string uri)
    {
        var modelBuilder = new ODataConventionModelBuilder().EnableLowerCamelCase();
        var entityType = modelBuilder.EntitySet<ApiModel.Foo>("Foos").EntityType;
        entityType.HasKey(x => x.Id);
        entityType
            .Select()
            .Filter();
        var edmModel = modelBuilder.GetEdmModel();

        const string routePrefix = "api/v1";
        var entitySet = edmModel.EntityContainer.FindEntitySet("Foos");
        var oDataPath = new ODataPath(new EntitySetSegment(entitySet));

        var httpContext = new DefaultHttpContext();
        var httpRequest = httpContext.Request;

        httpContext.RequestServices = new ServiceCollection()
            .Configure<ODataOptions>(x => x.AddRouteComponents(routePrefix, edmModel))
            .BuildServiceProvider();

        httpRequest.Method = HttpMethods.Get;
        var requestUri = new Uri(uri);
        httpRequest.Scheme = requestUri.Scheme;
        httpRequest.Host = requestUri.IsDefaultPort ? new HostString(requestUri.Host) : new HostString(requestUri.Host, requestUri.Port);
        httpRequest.QueryString = new QueryString(requestUri.Query);
        httpRequest.Path = new PathString(requestUri.AbsolutePath);

        httpRequest.ODataFeature().Model = edmModel;
        httpRequest.ODataFeature().Path = oDataPath;
        httpRequest.ODataFeature().RoutePrefix = routePrefix;

        var queryContext = new ODataQueryContext(edmModel, typeof(ApiModel.Foo), new ODataPath());
        return new ODataQueryOptions<ApiModel.Foo>(queryContext, httpRequest);
    }

    public void Dispose() => _connection.Dispose();
}