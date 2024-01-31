using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuqueBot.Migrations
{
    /// <inheritdoc />
    public partial class QueueUserrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRating_Users_UserId",
                table: "UserRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QueueTelegramUser",
                table: "QueueTelegramUser");

            migrationBuilder.DropIndex(
                name: "IX_QueueTelegramUser_UsersId",
                table: "QueueTelegramUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRating",
                table: "UserRating");

            migrationBuilder.RenameTable(
                name: "UserRating",
                newName: "Ratings");

            migrationBuilder.RenameIndex(
                name: "IX_UserRating_UserId",
                table: "Ratings",
                newName: "IX_Ratings_UserId");

            migrationBuilder.AlterColumn<long>(
                name: "UsersId",
                table: "QueueTelegramUser",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<long>(
                name: "QueuesId",
                table: "QueueTelegramUser",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<int>(
                name: "Position",
                table: "QueueTelegramUser",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QueueTelegramUser",
                table: "QueueTelegramUser",
                columns: new[] { "UsersId", "QueuesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QueueTelegramUser_QueuesId",
                table: "QueueTelegramUser",
                column: "QueuesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Users_UserId",
                table: "Ratings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Users_UserId",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_QueueTelegramUser",
                table: "QueueTelegramUser");

            migrationBuilder.DropIndex(
                name: "IX_QueueTelegramUser_QueuesId",
                table: "QueueTelegramUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "QueueTelegramUser");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "UserRating");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_UserId",
                table: "UserRating",
                newName: "IX_UserRating_UserId");

            migrationBuilder.AlterColumn<long>(
                name: "QueuesId",
                table: "QueueTelegramUser",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<long>(
                name: "UsersId",
                table: "QueueTelegramUser",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AddPrimaryKey(
                name: "PK_QueueTelegramUser",
                table: "QueueTelegramUser",
                columns: new[] { "QueuesId", "UsersId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRating",
                table: "UserRating",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_QueueTelegramUser_UsersId",
                table: "QueueTelegramUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRating_Users_UserId",
                table: "UserRating",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
