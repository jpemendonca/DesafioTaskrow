using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioTaskrow.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GruposSolicitantes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GrupoSolicitantePaiId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GruposSolicitantes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GruposSolicitantes_GruposSolicitantes_GrupoSolicitantePaiId",
                        column: x => x.GrupoSolicitantePaiId,
                        principalTable: "GruposSolicitantes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GruposSolicitantes_GrupoSolicitantePaiId",
                table: "GruposSolicitantes",
                column: "GrupoSolicitantePaiId");

            migrationBuilder.CreateIndex(
                name: "IX_GruposSolicitantes_Id",
                table: "GruposSolicitantes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GruposSolicitantes");
        }
    }
}
