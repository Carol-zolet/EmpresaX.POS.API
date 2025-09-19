using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.OpenApi.Models;
using EmpresaX.POS.API.Services;

// ===============================================================
// 1. CRIAÇÃO DO BUILDER
// ===============================================================
var builder = WebApplication.CreateBuilder(args);


// ===============================================================
// 2. CONFIGURAÇÃO DE SERVIÇOS (Injeção de Dependência)
// ===============================================================

// Configuração do Kestrel e Serilog
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5245, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});
builder.Host.UseSerilog((context, logger) =>
{
    logger.ReadFrom.Configuration(context.Configuration);
});

// Adiciona serviços essenciais do ASP.NET Core
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuração do AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configuração do Swagger... (código do swagger continua o mesmo)
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

// Configuração do CORS... (código do CORS continua o mesmo)
builder.Services.AddCors(options =>
{
    var policyName = builder.Configuration["Cors:PolicyName"] ?? "DefaultPolicy";
    options.AddPolicy(policyName, policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? new[] { "http://localhost:3000" };
        policy.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configuração de Autenticação JWT... (código do JWT continua o mesmo)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"] ?? "MinhaChaveSecretaSuperSegura123456789!";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddAuthorization();

// Registro dos seus serviços de negócio
builder.Services.AddScoped<IProdutosService, ProdutoService>();
builder.Services.AddScoped<ICategoriasService, CategoriaService>(); // <-- Apenas uma linha, a correta
// TODO: Adicionar outros serviços aqui (IContaService, etc.)


// ===============================================================
// 3. CONSTRUÇÃO DA APLICAÇÃO (builder.Build)
// ===============================================================
var app = builder.Build();


// ===============================================================
// 4. CONFIGURAÇÃO DO PIPELINE HTTP (código do pipeline continua o mesmo)
// ===============================================================
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmpresaX POS API v1"));
}
// ... (resto do arquivo é o mesmo)
app.UseHttpsRedirection();
var policyNameToUse = builder.Configuration["Cors:PolicyName"] ?? "DefaultPolicy";
app.UseCors(policyNameToUse);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "healthy" })).WithTags("Health");

// ===============================================================
// 5. EXECUÇÃO DA APLICAÇÃO (código de execução continua o mesmo)
// ===============================================================
try
{
    Log.Information("🚀 Iniciando EmpresaX POS API...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "💥 Falha crítica na inicialização da API");
}
finally
{
    Log.CloseAndFlush();
}