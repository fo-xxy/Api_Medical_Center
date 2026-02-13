using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionalClaimImportId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_ClaimImports_ClaimImportsid",
                table: "Claims");

            migrationBuilder.RenameColumn(
                name: "ClaimImportsid",
                table: "Claims",
                newName: "claim_import_id");

            migrationBuilder.RenameIndex(
                name: "IX_Claims_ClaimImportsid",
                table: "Claims",
                newName: "IX_Claims_claim_import_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_ClaimImports_claim_import_id",
                table: "Claims",
                column: "claim_import_id",
                principalTable: "ClaimImports",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Claims_ClaimImports_claim_import_id",
                table: "Claims");

            migrationBuilder.RenameColumn(
                name: "claim_import_id",
                table: "Claims",
                newName: "ClaimImportsid");

            migrationBuilder.RenameIndex(
                name: "IX_Claims_claim_import_id",
                table: "Claims",
                newName: "IX_Claims_ClaimImportsid");

            migrationBuilder.AddForeignKey(
                name: "FK_Claims_ClaimImports_ClaimImportsid",
                table: "Claims",
                column: "ClaimImportsid",
                principalTable: "ClaimImports",
                principalColumn: "id");
        }
    }
}
