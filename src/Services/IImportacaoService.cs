using System.IO;
using System.Threading.Tasks;

namespace EmpresaX.POS.API.Services
{
    public interface IImportacaoService
    {
        Task ProcessarExtratoBancario(Stream fileStream);
    }
}

