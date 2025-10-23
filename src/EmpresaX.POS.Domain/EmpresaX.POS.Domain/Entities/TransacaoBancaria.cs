using System;
using EmpresaX.POS.Domain.Shared;

namespace EmpresaX.POS.Domain.Entities
{
    public class TransacaoBancaria : Entity
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public string Tipo { get; set; } // "Crédito" ou "Débito"
        public string Status { get; set; } = "NaoReconciliado";
    }
}
