using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorTareas.Migrations
{
    /// <inheritdoc />
    public partial class CreacionInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserLastName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: true, defaultValue: 1),
                    Status = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DueTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelReason = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompositeTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompositeTaskType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompositeTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompositeTasks_Tasks_Id",
                        column: x => x.Id,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecurringTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecurrenceRule = table.Column<int>(type: "int", nullable: false),
                    RecurringTasksCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecurringTasks_Tasks_Id",
                        column: x => x.Id,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SimpleTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimpleTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SimpleTasks_Tasks_Id",
                        column: x => x.Id,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    UsersListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    tasksListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "LinkedTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LinkedTaskOrder = table.Column<int>(type: "int", nullable: false),
                    FKCompositeTaskId_Linked = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LinkedTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkedTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkedTasks_CompositeTasks_FKCompositeTaskId_Linked",
                        column: x => x.FKCompositeTaskId_Linked,
                        principalTable: "CompositeTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LinkedTasks_CompositeTasks_Id",
                        column: x => x.Id,
                        principalTable: "CompositeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LinkedTasks_LinkedTasks_LinkedTaskId",
                        column: x => x.LinkedTaskId,
                        principalTable: "LinkedTasks",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SubTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FKCompositeTaskId_Sub = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubTasks_CompositeTasks_FKCompositeTaskId_Sub",
                        column: x => x.FKCompositeTaskId_Sub,
                        principalTable: "CompositeTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubTasks_CompositeTasks_Id",
                        column: x => x.Id,
                        principalTable: "CompositeTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinkedTasks_FKCompositeTaskId_Linked",
                table: "LinkedTasks",
                column: "FKCompositeTaskId_Linked");

            migrationBuilder.CreateIndex(
                name: "IX_LinkedTasks_LinkedTaskId",
                table: "LinkedTasks",
                column: "LinkedTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_SubTasks_FKCompositeTaskId_Sub",
                table: "SubTasks",
                column: "FKCompositeTaskId_Sub");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserEmail",
                table: "Users",
                column: "UserEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_tasksListId",
                table: "UserTasks",
                column: "tasksListId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LinkedTasks");

            migrationBuilder.DropTable(
                name: "RecurringTasks");

            migrationBuilder.DropTable(
                name: "SimpleTasks");

            migrationBuilder.DropTable(
                name: "SubTasks");

            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.DropTable(
                name: "CompositeTasks");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
