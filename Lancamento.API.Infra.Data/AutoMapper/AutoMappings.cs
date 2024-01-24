using AutoMapper;
using Lancamento.API.Domain.Entities;
using Lancamento.API.Domain.Models;

namespace Lancamento.API.Infra.Data.AutoMapper
{
    public class AutoMappings : Profile
    {
        public AutoMappings()
        {
            CreateMap<UsuarioAddModel, Usuario>()
                    .ReverseMap();
            CreateMap<UsuarioModel, Usuario>()
                    .ReverseMap();
            CreateMap<LactoAddModel, Lacto>()
                    .ReverseMap();
            CreateMap<LactoModel, Lacto>()
                    .ReverseMap();
        }
    }
}
