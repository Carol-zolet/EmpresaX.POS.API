using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace SistemaCaixa.API.Controllers;

public partial class ImportacaoController
{
    [HttpGet("template")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    public IActionResult BaixarTemplate()
    {
        var workbook = CriarPlanilhaTemplate();
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var bytes = stream.ToArray();
        return File(
            bytes,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Template_Controle_Caixa.xlsx"
        );
    }

    private XLWorkbook CriarPlanilhaTemplate()
    {
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Controle Mensal");
        worksheet.Cell(1, 1).Value = "CONTROLE MENSAL [MÊS]";
        worksheet.Cell(1, 1).Style.Font.Bold = true;
        worksheet.Cell(1, 1).Style.Font.FontSize = 14;
        worksheet.Cell(3, 1).Value = "Dia";
        worksheet.Cell(3, 2).Value = "Descrição Geral";
        worksheet.Cell(3, 3).Value = "Classificação Conta";
        worksheet.Cell(3, 4).Value = "(+) Entradas R$";
        worksheet.Cell(3, 5).Value = "(-) Saídas R$";
        worksheet.Cell(3, 6).Value = "Saldo";
        var rangeHeader = worksheet.Range("A3:F3");
        rangeHeader.Style.Font.Bold = true;
        rangeHeader.Style.Fill.BackgroundColor = XLColor.LightGray;
        rangeHeader.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Cell(4, 4).Value = 0;
        worksheet.Cell(4, 4).Style.NumberFormat.Format = "R$ #,##0.00";
        worksheet.Cell(9, 1).Value = 1;
        worksheet.Cell(9, 2).Value = "Nome do Paciente";
        worksheet.Cell(9, 3).Value = "Unimed";
        worksheet.Cell(9, 4).Value = 100.00;
        worksheet.Cell(9, 4).Style.NumberFormat.Format = "R$ #,##0.00";
        worksheet.Column(1).Width = 8;
        worksheet.Column(2).Width = 40;
        worksheet.Column(3).Width = 25;
        worksheet.Column(4).Width = 18;
        worksheet.Column(5).Width = 18;
        worksheet.Column(6).Width = 18;
        return workbook;
    }
}
