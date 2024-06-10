using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    public partial class p : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notLikeCount",
                table: "strategy");

            migrationBuilder.DropColumn(
                name: "like",
                table: "like");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "f25f3lCmx5Pnsw11ywJcYRMUun40hJf4xSNMbmDVxSOBJPqfppIjkFOANnCG1Rlsyg/j9jw/5h3dc/hRStsoBQ==:wk0Ae+Q/obriu/lQGuAivaaTu+FdoWwf1QduuogCAa+w6jUupUjw9T+vgjHyF+g0+O5OZdD6ZrtarZYFKfvUWA==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "notLikeCount",
                table: "strategy",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                value: "I7a0fS1gvXYvqTjfwlAaqdwLVE1gcLjM9X1pJLVOQh837O52zWAoV8qxIA8hsjLnEa4J5zU/Wt03COZnwcsQMA==:i5sx7rvBnQpAICQsmv5wqJVNq47JwuX4oeNYTrEfkpX2SvAf1NSI28SsDMd886F0D+QCv4eyfumSAG2eP4zKDQ==");
        }
    }
}
