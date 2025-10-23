using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using EmpresaX.POS.Infrastructure.Data;
using EmpresaX.POS.API.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

// ===============================================================
// 1. CRIA��O DO BUILDER E CONFIGURA��O INICIAL
// ===============================================================
var builder = WebApplication.CreateBuilder(args);

// Configura��o do Serilog
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

// Em desenvolvimento, utilize Kestrel na porta 5245 com HTTPS local.
// Em produção (Azure/App Service/containers), não force a porta: respeite ASPNETCORE_URLS.
if (builder.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureKestrel(options =>
        options.ListenAnyIP(5245, listenOptions => listenOptions.UseHttps()));
}


// ===============================================================
// 2. CONFIGURA��O DE SERVI�OS (Inje��o de Depend�ncia)
// ===============================================================

// Garante que a string de conex�o existe antes de continuar
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("A string de conex�o 'DefaultConnection' n�o foi encontrada.");

// Adiciona o DbContext para PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ICaixaService, CaixaService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));

// Configura��o do Swagger com suporte para autentica��o JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "EmpresaX POS API", 
        Version = "v1",
        Description = "API RESTful para sistema de Ponto de Venda com gestão financeira completa",
        Contact = new OpenApiContact
        {
            Name = "EmpresaX Support",
            Email = "support@empresax.com"
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira 'Bearer' [espaço] e então seu token JWT. Exemplo: 'Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...'"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
    
    // Habilitar comentários XML para documentação detalhada
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    
    // Tags para organização dos endpoints
    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    c.DocInclusionPredicate((name, api) => true);
});

// Configura��o do CORS
builder.Services.AddCors(options =>
{
    var policyName = builder.Configuration["Cors:PolicyName"] ?? "DefaultPolicy";
    options.AddPolicy(policyName, policy => {
        var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? new[] { "http://localhost:3000" };
        policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
    });
});

// Configura��o de Autentica��o JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        var jwtKey = builder.Configuration["Jwt:Key"] ?? "MinhaChaveSecretaSuperSegura123456789!";
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

// Registro dos seus servi�os de neg�cio
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IImportacaoService, ImportacaoService>();
builder.Services.AddScoped<IContaService, ContaService>();
// builder.Services.AddScoped<IAuthService, AuthService>();

// Servi�os de Monitoramento
builder.Services.AddSingleton<PerformanceMetricsService>();

// Servi�o de Health Checks
builder.Services.AddHealthChecks()
   .AddNpgSql(connectionString, name: "PostgreSQL");


// ===============================================================
// 3. CONSTRU��O DA APLICA��O
// ===============================================================
var app = builder.Build();


// ===============================================================
// 4. CONFIGURA��O DO PIPELINE HTTP
// ===============================================================

// Middleware de tratamento de exceções (deve ser o primeiro)
app.UseMiddleware<EmpresaX.POS.API.Middleware.ExceptionHandlingMiddleware>();

// Middleware de logging de requisições
app.UseMiddleware<EmpresaX.POS.API.Middleware.RequestLoggingMiddleware>();

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmpresaX POS API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "EmpresaX POS API - Documentação";
    });
}

app.UseHttpsRedirection();

var policyNameToUse = builder.Configuration["Cors:PolicyName"] ?? "DefaultPolicy";
app.UseCors(policyNameToUse);

// A ordem � importante: Autentica��o ANTES de Autoriza��o
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).WithTags("Health");


// ===============================================================
// 5. EXECU��O DA APLICA��O
// ===============================================================
try
{
    Log.Information("?? Iniciando EmpresaX POS API...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "?? Falha cr�tica na inicializa��o da API");
}
finally
{
    Log.CloseAndFlush();
}

// Torna a classe Program acessível para testes de integração
public partial class Program { }



