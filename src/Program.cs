using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using EmpresaX.POS.API.Data;
using EmpresaX.POS.API.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

// ===============================================================
// 1. CRIAÇÃO DO BUILDER E CONFIGURAÇÃO INICIAL
// ===============================================================
var builder = WebApplication.CreateBuilder(args);

// Configuração do Serilog
builder.Host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));

// Configuração do Kestrel para escutar na porta 5245 com HTTPS
builder.WebHost.ConfigureKestrel(options => options.ListenAnyIP(5245, listenOptions => listenOptions.UseHttps()));


// ===============================================================
// 2. CONFIGURAÇÃO DE SERVIÇOS (Injeção de Dependência)
// ===============================================================

// Garante que a string de conexão existe antes de continuar
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("A string de conexão 'DefaultConnection' não foi encontrada.");

// Adiciona o DbContext para PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(Program));

// Configuração do Swagger com suporte para autenticação JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmpresaX.POS.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira 'Bearer' [espaço] e então seu token."
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
});

// Configuração do CORS
builder.Services.AddCors(options =>
{
    var policyName = builder.Configuration["Cors:PolicyName"] ?? "DefaultPolicy";
    options.AddPolicy(policyName, policy => {
        var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? new[] { "http://localhost:3000" };
        policy.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod();
    });
});

// Configuração de Autenticação JWT
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

// Registro dos seus serviços de negócio
builder.Services.AddScoped<IProdutoService, ProdutoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
// builder.Services.AddScoped<IContaService, ContaService>();
// builder.Services.AddScoped<IAuthService, AuthService>();

// Serviço de Health Checks
builder.Services.AddHealthChecks()
   .AddNpgSql(connectionString, name: "PostgreSQL");


// ===============================================================
// 3. CONSTRUÇÃO DA APLICAÇÃO
// ===============================================================
var app = builder.Build();


// ===============================================================
// 4. CONFIGURAÇÃO DO PIPELINE HTTP
// ===============================================================
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var policyNameToUse = builder.Configuration["Cors:PolicyName"] ?? "DefaultPolicy";
app.UseCors(policyNameToUse);

// A ordem é importante: Autenticação ANTES de Autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).WithTags("Health");


// ===============================================================
// 5. EXECUÇÃO DA APLICAÇÃO
// ===============================================================
try
{
    Log.Information("?? Iniciando EmpresaX POS API...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "?? Falha crítica na inicialização da API");
}
finally
{
    Log.CloseAndFlush();
}
