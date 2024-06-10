using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    public partial class g : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "like",
                table: "like",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "5QAmkUaXxVdRQuRtRFC1Uw2Bb39omyJkQrjH7Ovn0zICo+euqW+kixybkHaGQT7qVdV067Ox2L6nfiLcFWkO2g==:Zw/0iP6yeN3ufb6QFocv3yVHwg0F7sEA715voZH7PDPNHSWuU8QSWjbrowgJZXxDubBMDQKYSjutsJUscTY3uA==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "like",
                table: "like");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "S0YQUoN70G07EKp8rPmkn7DcRLwgoVWkTrA+9A1QJ/PtCk3BVpAp6dVgQX/jU9I+aBL37cj2gsMJo4qydo40RQ==:gOL44Ev7upQb9ZoQ1IesTg1DmIuzEIjXHncxzaiIaKChatcELv2LryKp0yw98o91jk75jqW/qxbKdTHrga2OgQ==");
        }
    }
}
