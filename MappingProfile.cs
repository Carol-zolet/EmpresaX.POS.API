using AutoMapper;
using InfraConta = EmpresaX.POS.Infrastructure.Conta;
using EmpresaX.POS.API.Controllers;

namespace EmpresaX.POS.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InfraConta, ContaDto>()
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.Saldo))
                .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => DateTime.Now.AddDays(30)));
        }
    }
}