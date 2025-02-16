using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioTaskrow.Migrations
{
    /// <inheritdoc />
    public partial class MigracaoSolicitacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogLevel = table.Column<int>(type: "int", nullable: false),
                    Local = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Erro = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detalhes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposSolicitacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposSolicitacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LimitesGrupos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrupoSolicitanteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoSolicitacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Ano = table.Column<int>(type: "int", nullable: false),
                    Mes = table.Column<int>(type: "int", nullable: false),
                    LimiteMensal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LimitesGrupos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LimitesGrupos_GruposSolicitantes_GrupoSolicitanteId",
                        column: x => x.GrupoSolicitanteId,
                        principalTable: "GruposSolicitantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LimitesGrupos_TiposSolicitacoes_TipoSolicitacaoId",
                        column: x => x.TipoSolicitacaoId,
                        principalTable: "TiposSolicitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Solicitacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GrupoSolicitanteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoSolicitacaoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_GruposSolicitantes_GrupoSolicitanteId",
                        column: x => x.GrupoSolicitanteId,
                        principalTable: "GruposSolicitantes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_TiposSolicitacoes_TipoSolicitacaoId",
                        column: x => x.TipoSolicitacaoId,
                        principalTable: "TiposSolicitacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LimitesGrupos_GrupoSolicitanteId",
                table: "LimitesGrupos",
                column: "GrupoSolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_LimitesGrupos_Id",
                table: "LimitesGrupos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_LimitesGrupos_TipoSolicitacaoId",
                table: "LimitesGrupos",
                column: "TipoSolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_Id",
                table: "Logs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_GrupoSolicitanteId",
                table: "Solicitacoes",
                column: "GrupoSolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_Id",
                table: "Solicitacoes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_TipoSolicitacaoId",
                table: "Solicitacoes",
                column: "TipoSolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TiposSolicitacoes_Id",
                table: "TiposSolicitacoes",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LimitesGrupos");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "TiposSolicitacoes");
        }
    }
}
