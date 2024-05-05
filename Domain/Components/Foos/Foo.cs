using Bogus;
using Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace Domain.Components.Foos;

internal class Foo
{
    public Guid Id { get; set; }
    public FooType Type { get; set; }
}

internal class FooConfigurator : IEntityTypeConfiguration<Foo>
{
    public void Configure(EntityTypeBuilder<Foo> builder)
    {
        var faker = new Faker<Foo>();
        faker.UseSeed(12345);

        faker
            .RuleFor(x => x.Id, x => x.Random.Guid())
            .RuleFor(x => x.Type, x =>
            {
                var types = x.PickRandom(Enum.GetValues<FooType>(), x.Random.Int(1, 4));
                return types.Skip(1).Aggregate(types.First(), (result, next) => result | next);
            });

        builder.HasData(faker.Generate(100));
    }
}