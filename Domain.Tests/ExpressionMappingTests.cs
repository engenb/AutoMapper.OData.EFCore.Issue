using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Domain.Components;
using Domain.Components.Foos;
using Domain.Enumerations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Data.Sqlite;

namespace Domain.Tests;

public class ExpressionMappingTests : IDisposable
{
    private readonly IMapper _mapper;
    private readonly TestDbContext _dbContext;

    private readonly SqliteConnection _connection;

    public ExpressionMappingTests()
    {
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
    public async Task LinqWhere__Given_FooTypeThreeExists__When_MappedFilterHas__Then_ReturnFoo()
    {
        Expression<Func<Foo, bool>> expected = x => x.Type.HasFlag(FooType.Three);

        var expectedFoo = new Foo { Id = Guid.NewGuid(), Type = FooType.Three };
        _dbContext.Set<Foo>().Add(expectedFoo);
        await _dbContext.SaveChangesAsync();

        Expression<Func<ApiModel.Foo, bool>> filter = x => x.Type.HasFlag(ApiModel.FooType.Three);

        var actual = _mapper.MapExpression<Expression<Func<Foo, bool>>>(filter);

        // I can't seem find anything to distinguish between actual/expected
        actual.Should().BeEquivalentTo(expected);

        var actualFoo = await _dbContext.Set<Foo>()
            .Where(actual)
            .ToArrayAsync();

        actualFoo.Should().ContainSingle(x => x.Id == expectedFoo.Id)
            .Which
            .Type.Should().HaveSameValueAs(expectedFoo.Type);
    }

    [Fact]
    public async Task LinqWhere__Given_FooTypeThreeExists__When_DirectFilterHas__Then_ReturnFoo()
    {
        var expectedFoo = new Foo { Id = Guid.NewGuid(), Type = FooType.Three };
        _dbContext.Set<Foo>().Add(expectedFoo);
        await _dbContext.SaveChangesAsync();

        Expression<Func<Foo, bool>> filter = x => x.Type.HasFlag(FooType.Three);

        var actualFoo = await _dbContext.Set<Foo>()
            .Where(filter)
            .ToArrayAsync();
        
        actualFoo.Should().ContainSingle(x => x.Id == expectedFoo.Id)
            .Which
            .Type.Should().HaveSameValueAs(expectedFoo.Type);
    }

    public void Dispose() => _connection.Dispose();
}