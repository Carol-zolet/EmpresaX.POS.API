using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EmpresaX.POS.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    NumeroConta = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Agencia = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Banco = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Ativa = table.Column<bool>(type: "INTEGER", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FechamentosCaixa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContaId = table.Column<int>(type: "INTEGER", nullable: false),
                    DataFechamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SaldoInicial = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalEntradas = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    TotalSaidas = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SaldoFinal = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SaldoInformado = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Diferenca = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FechamentosCaixa", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FechamentosCaixa_Contas_ContaId",
                        column: x => x.ContaId,
                        principalTable: "Contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Movimentacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ContaId = table.Column<int>(type: "INTEGER", nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tipo = table.Column<string>(type: "TEXT", nullable: false),
                    Valor = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DataMovimentacao = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Observacoes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Conciliado = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumeroDocumento = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Origem = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DataRegistro = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimentacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movimentacoes_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movimentacoes_Contas_ContaId",
                        column: x => x.ContaId,
                        principalTable: "Contas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Ativa", "DataCriacao", "Descricao", "Nome", "Tipo" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Receitas de vendas", "Vendas", "Receita" },
                    { 2, true, new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Receitas de serviços", "Serviços", "Receita" },
                    { 3, true, new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pagamentos a fornecedores", "Fornecedores", "Despesa" },
                    { 4, true, new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Folha de pagamento", "Salários", "Despesa" },
                    { 5, true, new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Despesas com aluguel", "Aluguel", "Despesa" }
                });

            migrationBuilder.InsertData(
                table: "Contas",
                columns: new[] { "Id", "Agencia", "Ativa", "Banco", "DataCriacao", "Nome", "NumeroConta", "SaldoInicial", "Tipo" },
                values: new object[,]
                {
                    { 1, null, true, null, new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Caixa Principal", null, 5000m, "Caixa" },
                    { 2, "1234", true, "Santander", new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Banco Santander", "123456-7", 15000m, "Banco" },
                    { 3, "5678", true, "Itaú", new DateTime(2025, 9, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Banco Itaú", "987654-3", 8000m, "Banco" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FechamentosCaixa_ContaId",
                table: "FechamentosCaixa",
                column: "ContaId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_CategoriaId",
                table: "Movimentacoes",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Movimentacoes_ContaId",
                table: "Movimentacoes",
                column: "ContaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FechamentosCaixa");

            migrationBuilder.DropTable(
                name: "Movimentacoes");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "Contas");
        }
    }
}
