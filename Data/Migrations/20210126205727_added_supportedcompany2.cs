using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuotesExchangeApp.Data.Migrations
{
    public partial class added_supportedcompany2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportedCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: true),
                    SourceId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportedCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportedCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportedCompanies_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupportedCompanies_CompanyId",
                table: "SupportedCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportedCompanies_SourceId",
                table: "SupportedCompanies",
                column: "SourceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportedCompanies");

        }
    }
}
