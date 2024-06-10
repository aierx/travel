using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    public partial class b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "filter",
                table: "comment",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "MNTZ1nfduORLWlqZRwaQFNHzt0Jgu7HRV1PFVllZy1+OWsSXwg3VZCX37z+UiqZZ1+RhpkS949UC0JwaJj5pbQ==:X7Jk1wGj7bxbYOzrXJnSwqGQEs1V72D1acN+Pl/1bwhAyiLJDqIZ014Oah0beDGAdV3EtGWTe7hPfaSxnaU1Cw==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "comment",
                keyColumn: "filter",
                keyValue: null,
                column: "filter",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "filter",
                table: "comment",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "OzUlc5PmPp8bvGlzqEXncb+ngu+PUcT6n3w77e7Z8+Gcl7ErBLxDbiUEbtmOZG+QgYIbB3k8Vxn5oxqYpx5YdQ==:rf7ukwHbLb0utQ8zR3s3YnMll1nDzh2tqQxBs2nk2p07gSFViFXfhw+bRvfwSwpLAT1xAT3+kXiS2kaYPZg+xQ==");
        }
    }
}
