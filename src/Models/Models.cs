using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }

    public class User : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
        
        public string Phone { get; set; } = string.Empty;
        public string Document { get; set; } = string.Empty;
        public string Photo { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.User;
        public UserStatus Status { get; set; } = UserStatus.Active;
        public DateTime? LastLogin { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
    }

    public class ContaPagar : BaseEntity
    {
        [Required]
        public string Fornecedor { get; set; } = string.Empty;
        
        [Required]
        public string Descricao { get; set; } = string.Empty;
        
        [Required]
        public decimal Valor { get; set; }
        
        [Required]
        public DateTime Vencimento { get; set; }
        
        public string Categoria { get; set; } = string.Empty;
        public ContaStatus Status { get; set; } = ContaStatus.Pendente;
        public DateTime? DataPagamento { get; set; }
        public decimal? ValorPago { get; set; }
        public string Observacoes { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    public class Activity : BaseEntity
    {
        [Required]
        public string Type { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;
        public string UserAgent { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }

    public enum UserRole
    {
        User = 1,
        Financeiro = 2,
        Vendas = 3,
        Admin = 4
    }

    public enum UserStatus
    {
        Active = 1,
        Inactive = 2,
        Blocked = 3
    }

    public enum ContaStatus
    {
        Pendente = 1,
        Paga = 2,
        Vencida = 3,
        Cancelada = 4
    }

    public enum BlockedType
    {
        SemBloqueio = 0,
        UsuarioInexistente = 1,
        SemPermissao = 2,
        ContaInativa = 3,
        AcessoSuspenso = 4,
        LimiteTentativas = 5,
        ForaHorario = 6,
        DispositivoNaoAutorizado = 7,
        SessaoExpirada = 8,
        AutenticacaoObrigatoria = 9,
        PendenciasFinanceiras = 10,
        AprovacaoNecessaria = 11,
        ModuloManutencao = 12,
        LicencaExpirada = 13,
        LimiteUsuarios = 14
    }
}


