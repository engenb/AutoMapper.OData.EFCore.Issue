using System;
using AutoMapper;
using AutoMapper.AspNet.OData;
using MediatR;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Components.Foos.Queries;

public record GetFoosODataQuery(ODataQueryOptions<ApiModel.Foo> QueryOptions) : IRequest<IEnumerable<ApiModel.Foo>>;

internal class GetFoosODataQueryHandler : IRequestHandler<GetFoosODataQuery, IEnumerable<ApiModel.Foo>>
{
    private readonly TestDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetFoosODataQueryHandler(TestDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<ApiModel.Foo>> Handle(GetFoosODataQuery query, CancellationToken ct)
    {
        var foos = await _dbContext.Set<Foo>()
            .AsNoTracking()
            .GetAsync(
                _mapper,
                query.QueryOptions,
                new QuerySettings { AsyncSettings = new AsyncSettings { CancellationToken = ct } });

        return foos.AsEnumerable();
    }
}
