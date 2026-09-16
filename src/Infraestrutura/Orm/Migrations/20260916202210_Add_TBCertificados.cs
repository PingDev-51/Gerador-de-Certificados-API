using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeradorCertificados.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_TBCertificados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Curso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: true),
                    CargaHoraria = table.Column<long>(type: "bigint", nullable: true),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curso", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBSolicitacoesCertificados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CursoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CaminhoZip = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataSolicitacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBSolicitacoesCertificados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBSolicitacoesCertificados_Curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Curso",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TBCertificados",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SolicitacaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    NomeAluno = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    NomeCurso = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CargaHoraria = table.Column<long>(type: "bigint", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CaminhoArquivo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    DataGeracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBCertificados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBCertificados_TBSolicitacoesCertificados_SolicitacaoId",
                        column: x => x.SolicitacaoId,
                        principalTable: "TBSolicitacoesCertificados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBCertificados_SolicitacaoId",
                table: "TBCertificados",
                column: "SolicitacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TBSolicitacoesCertificados_CursoId_Status",
                table: "TBSolicitacoesCertificados",
                columns: new[] { "CursoId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBCertificados");

            migrationBuilder.DropTable(
                name: "TBSolicitacoesCertificados");

            migrationBuilder.DropTable(
                name: "Curso");
        }
    }
}
