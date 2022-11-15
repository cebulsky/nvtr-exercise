using AutoMapper;

using Jokes.Application;

namespace Jokes.Infrastructure
{
    public class JokeMappingProfile : Profile
    {
        public JokeMappingProfile()
        {
            CreateMap<Joke, JokeDbEntity>()
                .ForMember(dbEntity => dbEntity.ExternalResourceId, opt => opt.MapFrom(j => j.Id))
                .ForMember(dbEntity => dbEntity.Quote, opt => opt.MapFrom(j => j.Value))
                .ForMember(dbEntity => dbEntity.Checksum, opt => opt.MapFrom(j => StringHashCompute.ComputeHashChecksumFromText(j.Value)))
                .ReverseMap();

        }
    }
}
