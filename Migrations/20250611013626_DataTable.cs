using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameCatalog.Migrations
{
    /// <inheritdoc />
    public partial class DataTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nome = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    DataLancamento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Desenvolvedor = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    CategoriaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Categories_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Descricao", "Nome" },
                values: new object[,]
                {
                    { 1, "Jogos de ação e aventura", "Ação" },
                    { 2, "Jogos de interpretação de papéis", "RPG" },
                    { 3, "Jogos de estratégia e simulação", "Estratégia" },
                    { 4, "Jogos de corrida e simulação de direção", "Corrida" },
                    { 5, "Jogos de esportes em geral", "Esportes" }
                });

            migrationBuilder.InsertData(
                table: "Games",
                columns: new[] { "Id", "CategoriaId", "DataLancamento", "Descricao", "Desenvolvedor", "Nome", "Preco" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2017, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Um épico jogo de aventura em mundo aberto", "Nintendo EPD", "The Legend of Zelda: Breath of the Wild", 59.99m },
                    { 2, 2, new DateTime(2015, 5, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), "RPG de fantasia medieval com escolhas morais complexas", "CD Projekt RED", "The Witcher 3: Wild Hunt", 39.99m },
                    { 3, 3, new DateTime(2016, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jogo de estratégia por turnos para construir impérios", "Firaxis Games", "Civilization VI", 49.99m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_CategoriaId",
                table: "Games",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
