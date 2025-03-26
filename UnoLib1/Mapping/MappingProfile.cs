using AutoMapper;
using UnoLib1.Dao;
using UnoLib1.Entity;

namespace UnoLib1.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserEntity, UserDao>().ReverseMap();
        CreateMap<TestEntity, TestDao>().ReverseMap();
        CreateMap<CredsEntity, CredsDao>().ReverseMap();
    }
}
