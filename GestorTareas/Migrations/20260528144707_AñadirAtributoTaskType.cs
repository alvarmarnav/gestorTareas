using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorTareas.Migrations
{
    /// <inheritdoc />
    public partial class AñadirAtributoTaskType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskType",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskType",
                table: "Tasks");
        }
    }
}
