using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    public partial class k : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "strategyId",
                table: "like",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "I7a0fS1gvXYvqTjfwlAaqdwLVE1gcLjM9X1pJLVOQh837O52zWAoV8qxIA8hsjLnEa4J5zU/Wt03COZnwcsQMA==:i5sx7rvBnQpAICQsmv5wqJVNq47JwuX4oeNYTrEfkpX2SvAf1NSI28SsDMd886F0D+QCv4eyfumSAG2eP4zKDQ==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "strategyId",
                table: "like");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "5QAmkUaXxVdRQuRtRFC1Uw2Bb39omyJkQrjH7Ovn0zICo+euqW+kixybkHaGQT7qVdV067Ox2L6nfiLcFWkO2g==:Zw/0iP6yeN3ufb6QFocv3yVHwg0F7sEA715voZH7PDPNHSWuU8QSWjbrowgJZXxDubBMDQKYSjutsJUscTY3uA==");
        }
    }
}
