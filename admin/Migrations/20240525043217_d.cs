using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    public partial class d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "strategy",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    userName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sceneryId = table.Column<long>(type: "bigint", nullable: false),
                    html = table.Column<string>(type: "longtext", maxLength: 2147483647, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    likeCount = table.Column<int>(type: "int", nullable: false),
                    notLikeCount = table.Column<int>(type: "int", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    ModifyTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    Creator = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modifer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_strategy", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "b7lzC85JVrQZkI8dPPg9aDB4rxuwuw8QVFaVdCKAM0D2DddV/5uhqoVFX7KeUdSn93Vxzgu5Unory05NgFZXeA==:KFFgKxit8QdORVbgmEJ/puD6Hcx6u/8E7sPiVfQhtPH1OPU0XSVUNLFzgtDHL8TMxTywXyIVfHo/FfmKpCP8ng==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "strategy");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "MNTZ1nfduORLWlqZRwaQFNHzt0Jgu7HRV1PFVllZy1+OWsSXwg3VZCX37z+UiqZZ1+RhpkS949UC0JwaJj5pbQ==:X7Jk1wGj7bxbYOzrXJnSwqGQEs1V72D1acN+Pl/1bwhAyiLJDqIZ014Oah0beDGAdV3EtGWTe7hPfaSxnaU1Cw==");
        }
    }
}
