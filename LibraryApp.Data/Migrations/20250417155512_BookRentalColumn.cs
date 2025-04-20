using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class BookRentalColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRental_Books_BookId",
                table: "BookRental");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRental_Users_UserId",
                table: "BookRental");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookRental",
                table: "BookRental");

            migrationBuilder.RenameTable(
                name: "BookRental",
                newName: "BookRents");

            migrationBuilder.RenameIndex(
                name: "IX_BookRental_BookId",
                table: "BookRents",
                newName: "IX_BookRents_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookRents",
                table: "BookRents",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookRents_Books_BookId",
                table: "BookRents",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRents_Users_UserId",
                table: "BookRents",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRents_Books_BookId",
                table: "BookRents");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRents_Users_UserId",
                table: "BookRents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookRents",
                table: "BookRents");

            migrationBuilder.RenameTable(
                name: "BookRents",
                newName: "BookRental");

            migrationBuilder.RenameIndex(
                name: "IX_BookRents_BookId",
                table: "BookRental",
                newName: "IX_BookRental_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookRental",
                table: "BookRental",
                columns: new[] { "UserId", "BookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookRental_Books_BookId",
                table: "BookRental",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRental_Users_UserId",
                table: "BookRental",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
