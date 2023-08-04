using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VerbTrainer.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesLowercase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conjugations_Tenses_TenseId",
                table: "Conjugations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conjugations_Verbs_VerbId",
                table: "Conjugations");

            migrationBuilder.DropForeignKey(
                name: "FK_Verbs_Binyans_BinyanId",
                table: "Verbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Verbs",
                table: "Verbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenses",
                table: "Tenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Conjugations",
                table: "Conjugations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Binyans",
                table: "Binyans");

            migrationBuilder.RenameTable(
                name: "Verbs",
                newName: "verbs");

            migrationBuilder.RenameTable(
                name: "Tenses",
                newName: "tenses");

            migrationBuilder.RenameTable(
                name: "Conjugations",
                newName: "conjugations");

            migrationBuilder.RenameTable(
                name: "Binyans",
                newName: "binyans");

            migrationBuilder.RenameIndex(
                name: "IX_Verbs_BinyanId",
                table: "verbs",
                newName: "IX_verbs_BinyanId");

            migrationBuilder.RenameIndex(
                name: "IX_Conjugations_TenseId",
                table: "conjugations",
                newName: "IX_conjugations_TenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_verbs",
                table: "verbs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tenses",
                table: "tenses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_conjugations",
                table: "conjugations",
                columns: new[] { "VerbId", "TenseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_binyans",
                table: "binyans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_conjugations_tenses_TenseId",
                table: "conjugations",
                column: "TenseId",
                principalTable: "tenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_conjugations_verbs_VerbId",
                table: "conjugations",
                column: "VerbId",
                principalTable: "verbs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_verbs_binyans_BinyanId",
                table: "verbs",
                column: "BinyanId",
                principalTable: "binyans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_conjugations_tenses_TenseId",
                table: "conjugations");

            migrationBuilder.DropForeignKey(
                name: "FK_conjugations_verbs_VerbId",
                table: "conjugations");

            migrationBuilder.DropForeignKey(
                name: "FK_verbs_binyans_BinyanId",
                table: "verbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_verbs",
                table: "verbs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tenses",
                table: "tenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_conjugations",
                table: "conjugations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_binyans",
                table: "binyans");

            migrationBuilder.RenameTable(
                name: "verbs",
                newName: "Verbs");

            migrationBuilder.RenameTable(
                name: "tenses",
                newName: "Tenses");

            migrationBuilder.RenameTable(
                name: "conjugations",
                newName: "Conjugations");

            migrationBuilder.RenameTable(
                name: "binyans",
                newName: "Binyans");

            migrationBuilder.RenameIndex(
                name: "IX_verbs_BinyanId",
                table: "Verbs",
                newName: "IX_Verbs_BinyanId");

            migrationBuilder.RenameIndex(
                name: "IX_conjugations_TenseId",
                table: "Conjugations",
                newName: "IX_Conjugations_TenseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Verbs",
                table: "Verbs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenses",
                table: "Tenses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Conjugations",
                table: "Conjugations",
                columns: new[] { "VerbId", "TenseId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Binyans",
                table: "Binyans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Conjugations_Tenses_TenseId",
                table: "Conjugations",
                column: "TenseId",
                principalTable: "Tenses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Conjugations_Verbs_VerbId",
                table: "Conjugations",
                column: "VerbId",
                principalTable: "Verbs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Verbs_Binyans_BinyanId",
                table: "Verbs",
                column: "BinyanId",
                principalTable: "Binyans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
