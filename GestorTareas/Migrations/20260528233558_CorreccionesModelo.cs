using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorTareas.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionesModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "Tasks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId1",
                table: "Tasks",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_UserId1",
                table: "Tasks",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_UserId1",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_UserId1",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Tasks");

            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    UsersListId = table.Column<int>(type: "int", nullable: false),
                    tasksListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => new { x.UsersListId, x.tasksListId });
                    table.ForeignKey(
                        name: "FK_UserTasks_Tasks_tasksListId",
                        column: x => x.tasksListId,
                        principalTable: "Tasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserTasks_Users_UsersListId",
                        column: x => x.UsersListId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_tasksListId",
                table: "UserTasks",
                column: "tasksListId");
        }
    }
}
