using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoonlightSquad.Migrations
{
    /// <inheritdoc />
    public partial class FixNewsCategoryFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_NewsCategories_NewsCategoryId",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_NewsCategoryId",
                table: "News");

            migrationBuilder.DropColumn(
                name: "NewsCategoryId",
                table: "News");

            migrationBuilder.CreateIndex(
                name: "IX_News_IdNewsCategory",
                table: "News",
                column: "IdNewsCategory");

            migrationBuilder.AddForeignKey(
                name: "FK_News_NewsCategories_IdNewsCategory",
                table: "News",
                column: "IdNewsCategory",
                principalTable: "NewsCategories",
                principalColumn: "NewsCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_News_NewsCategories_IdNewsCategory",
                table: "News");

            migrationBuilder.DropIndex(
                name: "IX_News_IdNewsCategory",
                table: "News");

            migrationBuilder.AddColumn<int>(
                name: "NewsCategoryId",
                table: "News",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_NewsCategoryId",
                table: "News",
                column: "NewsCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_News_NewsCategories_NewsCategoryId",
                table: "News",
                column: "NewsCategoryId",
                principalTable: "NewsCategories",
                principalColumn: "NewsCategoryId");
        }
    }
}
