using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveClaimImportRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_ClaimImports_claim_import_id",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_claim_import_id",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "claim_import_id",
                table: "Claims");

            migrationBuilder.AddColumn<int>(
                name: "ClaimImportsid",
                table: "Claims",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_ClaimImportsid",
                table: "Claims",
                column: "ClaimImportsid");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_ClaimImports_ClaimImportsid",
                table: "Claims",
                column: "ClaimImportsid",
                principalTable: "ClaimImports",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_ClaimImports_ClaimImportsid",
                table: "Claims");

            migrationBuilder.DropIndex(
                name: "IX_Claims_ClaimImportsid",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ClaimImportsid",
                table: "Claims");

            migrationBuilder.AddColumn<int>(
                name: "claim_import_id",
                table: "Claims",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Claims_claim_import_id",
                table: "Claims",
                column: "claim_import_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_ClaimImports_claim_import_id",
                table: "Claims",
                column: "claim_import_id",
                principalTable: "ClaimImports",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
