using Microsoft.EntityFrameworkCore.Migrations;

namespace CompanyEmployess.Migrations
{
    public partial class AddedRolesToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "29cfded9-4554-495a-8a90-af3196f97b8a", "b0fa5d63-54d4-4865-8848-90d00d37a701", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "455bbc7b-22fe-42d8-a68f-caeaf3695c39", "460b3a78-7e52-4e10-b04d-95cb919347d9", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "29cfded9-4554-495a-8a90-af3196f97b8a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "455bbc7b-22fe-42d8-a68f-caeaf3695c39");
        }
    }
}
