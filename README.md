# Summary

This repository is a very specific, paired-down sample project to reproduce an exception.

The project has a basic domain with a single `Foo` entity that is mapped to a `Foo` api model/dto.

```cs
[Flags]
public enum FooType
{
    One =   0b0001,
    Two =   0b0010,
    Three = 0b0100,
    Four =  0b1000
}

public class Foo
{
    public Guid Id { get; set; }
    public FooType Type { get; set; }
}
```

An OData GET request with query `$filter=type has 'Three'` throws exception:

```
System.InvalidOperationException: The LINQ expression 'DbSet<Foo>()
    .Where(f => f.Type.HasFlag((Enum)Three))' could not be translated. Additional information: Translation of method 'System.Enum.HasFlag' failed. If this method can be mapped to your custom function, see https://go.microsoft.com/fwlink/?linkid=2132413 for more information. Either rewrite the query in a form that can be translated, or switch to client evaluation explicitly by inserting a call to 'AsEnumerable', 'AsAsyncEnumerable', 'ToList', or 'ToListAsync'. See https://go.microsoft.com/fwlink/?linkid=2101038 for more information.
   at Microsoft.EntityFrameworkCore.Query.QueryableMethodTranslatingExpressionVisitor.Translate(Expression expression)
   at Microsoft.EntityFrameworkCore.Query.RelationalQueryableMethodTranslatingExpressionVisitor.Translate(Expression expression)
   at Microsoft.EntityFrameworkCore.Query.QueryCompilationContext.CreateQueryExecutor[TResult](Expression query)
   at Microsoft.EntityFrameworkCore.Storage.Database.CompileQuery[TResult](Expression query, Boolean async)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.CompileQueryCore[TResult](IDatabase database, Expression query, IModel model, Boolean async)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.<>c__DisplayClass12_0`1.<ExecuteAsync>b__0()
   at Microsoft.EntityFrameworkCore.Query.Internal.CompiledQueryCache.GetOrAddQuery[TResult](Object cacheKey, Func`1 compiler)
   at Microsoft.EntityFrameworkCore.Query.Internal.QueryCompiler.ExecuteAsync[TResult](Expression query, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryProvider.ExecuteAsync[TResult](Expression expression, CancellationToken cancellationToken)
   at Microsoft.EntityFrameworkCore.Query.Internal.EntityQueryable`1.GetAsyncEnumerator(CancellationToken cancellationToken)
   at System.Runtime.CompilerServices.ConfiguredCancelableAsyncEnumerable`1.GetAsyncEnumerator()
   at Microsoft.EntityFrameworkCore.EntityFrameworkQueryableExtensions.ToListAsync[TSource](IQueryable`1 source, CancellationToken cancellationToken)
   at AutoMapper.AspNet.OData.QueryableExtensions.GetAsync[TModel,TData](IQueryable`1 query, IMapper mapper, Expression`1 filter, Expression`1 queryFunc, ICollection`1 includeProperties, AsyncSettings asyncSettings)
   at AutoMapper.AspNet.OData.QueryableExtensions.GetAsync[TModel,TData](IQueryable`1 query, IMapper mapper, ODataQueryOptions`1 options, QuerySettings querySettings)
   at Domain.Components.Foos.Queries.GetFoosODataQueryHandler.Handle(GetFoosODataQuery query, CancellationToken ct) in C:\Projects\AutoMapper.OData.EFCore.Issue\Domain\Components\Foos\Queries\GetFoosOData.cs:line 29
   ~~~ Truncated ~~~
```

I have included a couple unit tests in this solution that demonstrate my (failed) attempts to reproduce this bug. These tests are not a perfect facsimale as they use a different database provider.

# Project Setup

This sample relies on MSSQL to reproduce the issue

```ps
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Password1" -p 1433:1433 --name automapper-odata-efcore -d mcr.microsoft.com/mssql/server:2022-latest
```

Initialize DB

```ps
dotnet ef database update --startup-project OData --project Domain
```

Run

```ps
dotnet run --project OData
```

This project hosts a Swagger site at http://localhost:5199/api/explorer.
