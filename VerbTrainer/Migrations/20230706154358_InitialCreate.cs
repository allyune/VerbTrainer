using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VerbTrainer.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Binyans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Binyans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tenses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Verbs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Root = table.Column<string>(type: "text", nullable: false),
                    Meaning = table.Column<string>(type: "text", nullable: false),
                    BinyanId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verbs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Verbs_Binyans_BinyanId",
                        column: x => x.BinyanId,
                        principalTable: "Binyans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conjugations",
                columns: table => new
                {
                    VerbId = table.Column<Guid>(type: "uuid", nullable: false),
                    TenseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Transcription = table.Column<string>(type: "text", nullable: false),
                    Meaning = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conjugations", x => new { x.VerbId, x.TenseId });
                    table.ForeignKey(
                        name: "FK_Conjugations_Tenses_TenseId",
                        column: x => x.TenseId,
                        principalTable: "Tenses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Conjugations_Verbs_VerbId",
                        column: x => x.VerbId,
                        principalTable: "Verbs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conjugations_TenseId",
                table: "Conjugations",
                column: "TenseId");

            migrationBuilder.CreateIndex(
                name: "IX_Verbs_BinyanId",
                table: "Verbs",
                column: "BinyanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conjugations");

            migrationBuilder.DropTable(
                name: "Tenses");

            migrationBuilder.DropTable(
                name: "Verbs");

            migrationBuilder.DropTable(
                name: "Binyans");
        }
    }
}
