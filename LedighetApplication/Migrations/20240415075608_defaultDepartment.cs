using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LedighetApplication.Migrations
{
    /// <inheritdoc />
    public partial class defaultDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Departments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 1,
                column: "IsDefault",
                value: false);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 2,
                column: "IsDefault",
                value: false);

            migrationBuilder.UpdateData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 3,
                column: "IsDefault",
                value: false);

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "DepartmentId", "IsDefault", "Name" },
                values: new object[] { 4, true, "Not Chosen" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "DepartmentId",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Departments");
        }
    }
}
