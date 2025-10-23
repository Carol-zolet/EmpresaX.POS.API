using Microsoft.AspNetCore.Mvc;

namespace EmpresaX.POS.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected object CreatePaginatedResponse<T>(IEnumerable<T> data, int take = 50, int skip = 0)
        {
            var dataList = data.ToList();
            var totalItems = dataList.Count;
            var paginatedData = dataList.Skip(skip).Take(take).ToList();
            
            // Adicionar header com total (padrão EVO) - CORRIGIDO: usando Append em vez de Add
            Response.Headers.Append("X-Total-Count", totalItems.ToString());
            
            return new
            {
                data = paginatedData,
                pagination = new
                {
                    total = totalItems,
                    take = take,
                    skip = skip,
                    hasNext = (skip + take) < totalItems,
                    hasPrevious = skip > 0,
                    totalPages = (int)Math.Ceiling((double)totalItems / take),
                    currentPage = (skip / take) + 1
                }
            };
        }
    }
}

