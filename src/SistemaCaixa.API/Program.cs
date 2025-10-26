using MediatR;
using Microsoft.EntityFrameworkCore;
using SistemaCaixa.API.Filters;
using SistemaCaixa.API.Middlewares;
using SistemaCaixa.Application.UseCases.AbrirCaixa;
using SistemaCaixa.Application.UseCases.FecharCaixa;
using SistemaCaixa.Domain.Repositories;
using SistemaCaixa.Infrastructure.Data;
using SistemaCaixa.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICaixaRepository, CaixaRepository>();
builder.Services.AddScoped<IConciliacaoRepository, ConciliacaoRepository>();
builder.Services.AddMediatR(typeof(AbrirCaixaCommand).Assembly);

var app = builder.Build();

// Middlewares
app.UseMiddleware<ErrorHandlingMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
