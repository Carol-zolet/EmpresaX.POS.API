using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmpresaX.POS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddProductMetadataJsonbColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "Produtos",
                type: "jsonb",
                nullable: false,
                defaultValue: "{}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "Produtos");
        }
    }
}
