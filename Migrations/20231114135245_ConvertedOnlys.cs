using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Student_Planner.Migrations
{
    /// <inheritdoc />
    public partial class ConvertedOnlys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "test",
                table: "Events",
                newName: "StartTime");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "EndTime",
                table: "Events",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "StartTime",
                table: "Events",
                newName: "test");
        }
    }
}
