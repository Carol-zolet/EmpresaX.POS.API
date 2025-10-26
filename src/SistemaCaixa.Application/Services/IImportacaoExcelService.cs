using System.Threading;
using System.Threading.Tasks;
using SistemaCaixa.Application.DTOs;

namespace SistemaCaixa.Application.Services;

public interface IImportacaoExcelService
{
    Task<ImportacaoExcelResultDto> ImportarPlanilhaAsync(
        Stream fileStream,
        string fileName,
        string operador,
        CancellationToken cancellationToken = default
    );
}
