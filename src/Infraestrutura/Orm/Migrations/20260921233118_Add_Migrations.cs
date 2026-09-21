using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GeradorCertificados.Infraestrutura.Orm.Migrations
{
    /// <inheritdoc />
    public partial class Add_Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBCertificados_TBSolicitacoesCertificados_SolicitacaoCertif~",
                table: "TBCertificados");

            migrationBuilder.DropIndex(
                name: "IX_TBCertificados_SolicitacaoCertificadosId",
                table: "TBCertificados");

            migrationBuilder.DropColumn(
                name: "SolicitacaoCertificadosId",
                table: "TBCertificados");

            migrationBuilder.AddColumn<Guid>(
                name: "SolicitacaoId",
                table: "TBCertificados",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TBCertificados_SolicitacaoId",
                table: "TBCertificados",
                column: "SolicitacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_TBCertificados_TBSolicitacoesCertificados_SolicitacaoId",
                table: "TBCertificados",
                column: "SolicitacaoId",
                principalTable: "TBSolicitacoesCertificados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBCertificados_TBSolicitacoesCertificados_SolicitacaoId",
                table: "TBCertificados");

            migrationBuilder.DropIndex(
                name: "IX_TBCertificados_SolicitacaoId",
                table: "TBCertificados");

            migrationBuilder.DropColumn(
                name: "SolicitacaoId",
                table: "TBCertificados");

            migrationBuilder.AddColumn<Guid>(
                name: "SolicitacaoCertificadosId",
                table: "TBCertificados",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBCertificados_SolicitacaoCertificadosId",
                table: "TBCertificados",
                column: "SolicitacaoCertificadosId");

            migrationBuilder.AddForeignKey(
                name: "FK_TBCertificados_TBSolicitacoesCertificados_SolicitacaoCertif~",
                table: "TBCertificados",
                column: "SolicitacaoCertificadosId",
                principalTable: "TBSolicitacoesCertificados",
                principalColumn: "Id");
        }
    }
}
