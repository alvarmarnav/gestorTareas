using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorTareas.Migrations
{
    /// <inheritdoc />
    public partial class AñadirIndiceUnicoALinkedtasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinkedTasks_TaskId",
                table: "LinkedTasks");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedTasks_TaskId_DependsOnTaskId",
                table: "LinkedTasks",
                columns: new[] { "TaskId", "DependsOnTaskId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LinkedTasks_TaskId_DependsOnTaskId",
                table: "LinkedTasks");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedTasks_TaskId",
                table: "LinkedTasks",
                column: "TaskId");
        }
    }
}
