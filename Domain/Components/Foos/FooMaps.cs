using AutoMapper;

namespace Domain.Components.Foos;

internal class FooMaps : Profile
{
    public FooMaps()
    {
        CreateMap<Foo, ApiModel.Foo>();
    }
}