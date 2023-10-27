using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LPCR.Monitor.Web.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobScheduleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobScheduleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobStatusTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobStatusTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    ProcessorName = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "BIT", nullable: false),
                    ScheduleTypeId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Schedule = table.Column<string>(type: "VARCHAR(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_JobScheduleTypes_ScheduleTypeId",
                        column: x => x.ScheduleTypeId,
                        principalTable: "JobScheduleTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SystemLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogTypeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SystemLogs_LogTypes_LogTypeId",
                        column: x => x.LogTypeId,
                        principalTable: "LogTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobRuns",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Started = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Completed = table.Column<DateTime>(type: "DATETIME", nullable: true),
                    Paylaod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanRetry = table.Column<bool>(type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRuns_JobStatusTypes_StatusId",
                        column: x => x.StatusId,
                        principalTable: "JobStatusTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobRuns_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobRunLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobRunId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogTypeId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "DATETIME", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobRunLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobRunLogs_JobRuns_JobRunId",
                        column: x => x.JobRunId,
                        principalTable: "JobRuns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobRunLogs_LogTypes_LogTypeId",
                        column: x => x.LogTypeId,
                        principalTable: "LogTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "JobScheduleTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Scheduled" },
                    { 2, "OnDemand" }
                });

            migrationBuilder.InsertData(
                table: "JobStatusTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Unknown" },
                    { 2, "Pending" },
                    { 3, "Queued" },
                    { 4, "Running" },
                    { 5, "Completed" },
                    { 6, "Errored" },
                    { 7, "Canceled" }
                });

            migrationBuilder.InsertData(
                table: "LogTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Information" },
                    { 2, "Warning" },
                    { 3, "Error" },
                    { 4, "Critical" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobRunLogs_JobRunId",
                table: "JobRunLogs",
                column: "JobRunId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRunLogs_LogTypeId",
                table: "JobRunLogs",
                column: "LogTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRuns_JobId",
                table: "JobRuns",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobRuns_StatusId",
                table: "JobRuns",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_ScheduleTypeId",
                table: "Jobs",
                column: "ScheduleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SystemLogs_LogTypeId",
                table: "SystemLogs",
                column: "LogTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobRunLogs");

            migrationBuilder.DropTable(
                name: "SystemLogs");

            migrationBuilder.DropTable(
                name: "JobRuns");

            migrationBuilder.DropTable(
                name: "LogTypes");

            migrationBuilder.DropTable(
                name: "JobStatusTypes");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "JobScheduleTypes");
        }
    }
}
