using AutoMapper;
using EmpresaX.POS.Domain.Entities;
using EmpresaX.POS.API.Modelos.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Mapeamentos para Produto
        CreateMap<Produto, ProdutoDto>();
        CreateMap<CreateProdutoDto, Produto>();
        CreateMap<UpdateProdutoDto, Produto>();

        // Mapeamentos para Categoria
        CreateMap<Categoria, CategoriaDto>();
        CreateMap<CreateCategoriaDto, Categoria>();
    }
}

