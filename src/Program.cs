using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using Microsoft.OpenApi.Models;

// ---------------------------------------------------------------
// 1️⃣  Kestrel → escuta HTTPS na porta 5245
// ---------------------------------------------------------------
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    // Escuta *sempre* no IP da máquina (0.0.0.0), porta 5245, com HTTPS
    options.ListenAnyIP(5245, listenOptions =>
    {
        listenOptions.UseHttps(); // usa o cert dev criado no passo 1
    });
});

// ---------------------------------------------------------------
// 2️⃣  Logging (Serilog)
// ---------------------------------------------------------------
builder.Host.UseSerilog((hostingContext, logger) =>
{
    logger.ReadFrom.Configuration(hostingContext.Configuration);
});

// ---------------------------------------------------------------
// 3️⃣  Serviços (controllers, swagger, CORS, JWT, AutoMapper)
// ---------------------------------------------------------------
builder.Services.AddControllers();   // MVC controllers

// AutoMapper (adicionar esta linha)
builder.Services.AddAutoMapper(typeof(Program));

// Swagger com configuração corrigida
builder.Services.AddEndpointsApiExplorer(); // Necessário para minimal APIs
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EmpresaX.POS.API",
        Version = "v1",
        Description = "API para Sistema de Ponto de Venda",
        Contact = new OpenApiContact
        {
            Name = "EmpresaX",
            Email = "contato@empresax.com"
        }
    });
    
    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        BearerFormat = "JWT",
        Scheme = "bearer",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Digite 'Bearer' seguido do token JWT",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    
    c.AddSecurityDefinition("Bearer", scheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { scheme, Array.Empty<string>() }
    });
});

// CORS com configuração segura
builder.Services.AddCors(options =>
{
    var policyName = builder.Configuration["Cors:PolicyName"] ?? "DefaultPolicy";
    options.AddPolicy(policyName, policy =>
    {
        var origins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? 
                     new[] { "http://localhost:3000", "https://localhost:3001" };
        
        policy.WithOrigins(origins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtKey = builder.Configuration["Jwt:Key"] ?? "MinhaChaveSecretaSuperSegura123456789!";
        var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "EmpresaX.POS.API";
        var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "EmpresaX.POS.Client";
        
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// ---------------------------------------------------------------
// 4️⃣  Pipeline
// ---------------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmpresaX POS API v1");
        c.RoutePrefix = "swagger";
    });
}

var policyNameToUse = builder.Configuration["Cors:PolicyName"] ?? "DefaultPolicy";
app.UseCors(policyNameToUse);

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Endpoints de health check
app.MapGet("/health", () => new
{
    status = "healthy",
    timestamp = DateTime.UtcNow,
    version = "1.0.0",
    port = 5245
}).WithTags("Health");

try
{
    Log.Information("🚀 Iniciando EmpresaX POS API na porta 5245...");
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