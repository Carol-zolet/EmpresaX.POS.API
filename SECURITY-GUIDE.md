# 🔒 Guia de Segurança e Validação - Backend

## 📋 Revisão de Segurança

Este guia documenta as melhorias de segurança e validação implementadas no backend.

---

## 🛡️ Validações de DTOs

### 1. **Data Annotations Recomendadas**

#### CreateContaDto
```csharp
using System.ComponentModel.DataAnnotations;

public class CreateContaDto
{
    [Required(ErrorMessage = "O fornecedor é obrigatório")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O fornecedor deve ter entre 3 e 200 caracteres")]
    public string Fornecedor { get; set; } = string.Empty;

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [StringLength(500, MinimumLength = 5, ErrorMessage = "A descrição deve ter entre 5 e 500 caracteres")]
    public string Descricao { get; set; } = string.Empty;

    [Required(ErrorMessage = "O valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "A data de vencimento é obrigatória")]
    [DataType(DataType.Date)]
    public DateTime Vencimento { get; set; }

    [Required(ErrorMessage = "O status é obrigatório")]
    [RegularExpression("^(Pendente|Paga|Vencida)$", ErrorMessage = "Status inválido")]
    public string Status { get; set; } = "Pendente";

    [Required(ErrorMessage = "A categoria é obrigatória")]
    [StringLength(100, ErrorMessage = "A categoria deve ter no máximo 100 caracteres")]
    public string Categoria { get; set; } = string.Empty;
}
```

#### UpdateContaDto
```csharp
public class UpdateContaDto
{
    [Required]
    public int Id { get; set; }

    [StringLength(200, MinimumLength = 3)]
    public string? Fornecedor { get; set; }

    [StringLength(500, MinimumLength = 5)]
    public string? Descricao { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? Valor { get; set; }

    [DataType(DataType.Date)]
    public DateTime? Vencimento { get; set; }

    [RegularExpression("^(Pendente|Paga|Vencida)$")]
    public string? Status { get; set; }

    [StringLength(100)]
    public string? Categoria { get; set; }
}
```

---

## 🔐 Autenticação e Autorização

### 1. **JWT Bearer Authentication**

**Program.cs:**
```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("FinanceiroAccess", policy => policy.RequireRole("Admin", "Financeiro"));
});
```

### 2. **Proteção de Endpoints**

```csharp
[Authorize] // Requer autenticação
[ApiController]
[Route("api/v1/[controller]")]
public class ContasController : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin,Financeiro")] // Requer role específica
    public async Task<ActionResult<IEnumerable<ContaDto>>> GetAll()
    {
        // ...
    }

    [HttpPost]
    [Authorize(Policy = "FinanceiroAccess")] // Requer policy
    public async Task<ActionResult<ContaDto>> Create([FromBody] CreateContaDto dto)
    {
        // ...
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Somente Admin pode deletar
    public async Task<IActionResult> Delete(int id)
    {
        // ...
    }
}
```

---

## 🔒 Proteções Adicionais

### 1. **Rate Limiting**

```csharp
// Instalar: dotnet add package Microsoft.AspNetCore.RateLimiting

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = TimeSpan.FromMinutes(1);
        options.PermitLimit = 100;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 10;
    });

    options.AddSlidingWindowLimiter("sliding", options =>
    {
        options.Window = TimeSpan.FromMinutes(1);
        options.PermitLimit = 100;
        options.SegmentsPerWindow = 4;
    });
});

// No pipeline
app.UseRateLimiter();

// No controller
[EnableRateLimiting("fixed")]
[HttpPost]
public async Task<ActionResult> Create([FromBody] CreateContaDto dto)
{
    // ...
}
```

### 2. **CORS Seguro**

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("Production", policy =>
    {
        policy.WithOrigins(
            "https://empresax.com.br",
            "https://www.empresax.com.br"
        )
        .AllowedMethods("GET", "POST", "PUT", "DELETE")
        .AllowedHeaders("Content-Type", "Authorization")
        .AllowCredentials()
        .SetIsOriginAllowedToAllowWildcardSubdomains();
    });

    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// No pipeline
app.UseCors(app.Environment.IsDevelopment() ? "Development" : "Production");
```

### 3. **Input Sanitization**

```csharp
public static class StringExtensions
{
    public static string Sanitize(this string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        // Remove caracteres perigosos
        var sanitized = input
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#x27;")
            .Replace("/", "&#x2F;");

        return sanitized.Trim();
    }
}

// Uso no controller
public async Task<ActionResult> Create([FromBody] CreateContaDto dto)
{
    dto.Fornecedor = dto.Fornecedor.Sanitize();
    dto.Descricao = dto.Descricao.Sanitize();
    // ...
}
```

### 4. **SQL Injection Prevention**

```csharp
// ✅ BOM - Usando EF Core com parâmetros
public async Task<Conta> GetByFornecedor(string fornecedor)
{
    return await _context.Contas
        .Where(c => c.Fornecedor == fornecedor)
        .FirstOrDefaultAsync();
}

// ❌ RUIM - SQL direto sem parâmetros (NUNCA FAÇA ISSO!)
public async Task<Conta> GetByFornecedor(string fornecedor)
{
    var sql = $"SELECT * FROM Contas WHERE Fornecedor = '{fornecedor}'";
    return await _context.Contas.FromSqlRaw(sql).FirstOrDefaultAsync();
}

// ✅ BOM - Se precisar usar SQL direto, use parâmetros
public async Task<Conta> GetByFornecedor(string fornecedor)
{
    var sql = "SELECT * FROM Contas WHERE Fornecedor = {0}";
    return await _context.Contas
        .FromSqlRaw(sql, fornecedor)
        .FirstOrDefaultAsync();
}
```

---

## 🔍 Auditoria e Logging

### 1. **Audit Trail**

```csharp
public class AuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }
}

public class Conta : AuditableEntity
{
    public int Id { get; set; }
    public string Fornecedor { get; set; } = string.Empty;
    // ... outras propriedades
}

// No DbContext
public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
{
    var entries = ChangeTracker.Entries<AuditableEntity>();
    var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    foreach (var entry in entries)
    {
        if (entry.State == EntityState.Added)
        {
            entry.Entity.CreatedAt = DateTime.UtcNow;
            entry.Entity.CreatedBy = userId ?? "System";
        }
        else if (entry.State == EntityState.Modified)
        {
            entry.Entity.UpdatedAt = DateTime.UtcNow;
            entry.Entity.UpdatedBy = userId ?? "System";
        }
    }

    return base.SaveChangesAsync(cancellationToken);
}
```

### 2. **Logging Sensível**

```csharp
// ❌ NUNCA logue senhas ou tokens
_logger.LogInformation("Login attempt: {Email} with password {Password}", email, password);

// ✅ Logue apenas informações seguras
_logger.LogInformation("Login attempt: {Email}", email);

// ✅ Use níveis apropriados
_logger.LogWarning("Failed login attempt for {Email} from IP {IP}", email, ipAddress);
_logger.LogError(exception, "Error processing payment for order {OrderId}", orderId);
```

---

## 🧪 Validações Customizadas

### 1. **FluentValidation**

```csharp
// Instalar: dotnet add package FluentValidation.AspNetCore

public class CreateContaDtoValidator : AbstractValidator<CreateContaDto>
{
    public CreateContaDtoValidator()
    {
        RuleFor(x => x.Fornecedor)
            .NotEmpty().WithMessage("O fornecedor é obrigatório")
            .Length(3, 200).WithMessage("O fornecedor deve ter entre 3 e 200 caracteres")
            .Must(BeValidFornecedor).WithMessage("Fornecedor inválido");

        RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("O valor deve ser maior que zero")
            .LessThan(1000000).WithMessage("O valor não pode exceder R$ 1.000.000,00");

        RuleFor(x => x.Vencimento)
            .NotEmpty().WithMessage("A data de vencimento é obrigatória")
            .Must(BeValidDate).WithMessage("Data de vencimento inválida");
    }

    private bool BeValidFornecedor(string fornecedor)
    {
        // Validação customizada
        return !fornecedor.Contains("<script>", StringComparison.OrdinalIgnoreCase);
    }

    private bool BeValidDate(DateTime vencimento)
    {
        return vencimento.Date >= DateTime.Today;
    }
}

// Registro no Program.cs
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateContaDtoValidator>();
```

---

## 📝 Checklist de Segurança

### Autenticação e Autorização
- [ ] JWT configurado com chaves fortes
- [ ] Tokens com expiração apropriada
- [ ] Refresh tokens implementados
- [ ] Roles e policies definidas
- [ ] Endpoints protegidos adequadamente

### Validação de Entrada
- [ ] Data Annotations em todos os DTOs
- [ ] FluentValidation para regras complexas
- [ ] Sanitização de strings
- [ ] Validação de tipos e ranges
- [ ] Tratamento de valores nulos

### Proteção Contra Ataques
- [ ] SQL Injection prevenido (EF Core com parâmetros)
- [ ] XSS prevenido (sanitização de HTML)
- [ ] CSRF tokens implementados
- [ ] Rate limiting configurado
- [ ] CORS configurado corretamente

### Logs e Auditoria
- [ ] Logs estruturados com Serilog
- [ ] Informações sensíveis não logadas
- [ ] Audit trail implementado
- [ ] Monitoramento de falhas de autenticação
- [ ] Alertas para atividades suspeitas

### HTTPS e Comunicação
- [ ] HTTPS obrigatório em produção
- [ ] HSTS configurado
- [ ] Certificados SSL válidos
- [ ] Redirect HTTP → HTTPS

### Dados Sensíveis
- [ ] Senhas com hash (BCrypt/Argon2)
- [ ] Dados sensíveis criptografados em repouso
- [ ] Secrets em variáveis de ambiente
- [ ] Connection strings protegidas
- [ ] API keys rotacionadas regularmente

---

## 🎯 Próximos Passos

### Implementações Pendentes
1. ⏳ Adicionar FluentValidation aos DTOs
2. ⏳ Implementar Rate Limiting
3. ⏳ Configurar CORS para produção
4. ⏳ Adicionar Audit Trail completo
5. ⏳ Implementar refresh tokens

### Testes de Segurança
- [ ] Testes de penetração
- [ ] Análise de vulnerabilidades (OWASP ZAP)
- [ ] Code review focado em segurança
- [ ] Validação de compliance (LGPD)

---

**Última Atualização:** 20 de outubro de 2025
