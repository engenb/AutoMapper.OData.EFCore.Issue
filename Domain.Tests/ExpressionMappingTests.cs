using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Domain.Components;
using Domain.Components.Foos;
using Domain.Enumerations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Domain.Tests;

public class ExpressionMappingTests
{
    private readonly IMapper _mapper;
    private readonly TestDbContext _dbContext;

    public ExpressionMappingTests()
    {
        _mapper = new MapperConfiguration(x => x.AddProfile<FooMaps>())
            .CreateMapper();

        _dbContext = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(nameof(ExpressionMappingTests))
            .Options);
    }

    [Fact]
    public async Task LinqWhere__Given_FooTypeThreeExists__When_FilterHas__Then_ReturnFoo()
    {
        var expectedFoo = new Foo { Id = Guid.NewGuid(), Type = FooType.Three };
        _dbContext.Set<Foo>().Add(expectedFoo);
        await _dbContext.SaveChangesAsync();

        Expression<Func<ApiModel.Foo, bool>> filter = x => x.Type.HasFlag(ApiModel.FooType.Three);

        var result = _mapper.MapExpression<Expression<Func<Foo, bool>>>(filter);

        var actualFoo = await _dbContext.Set<Foo>().Where(result).ToArrayAsync();

        actualFoo.Should().ContainSingle(x => x.Id == expectedFoo.Id)
            .Which
            .Type.Should().HaveSameValueAs(expectedFoo.Type);
    }
}