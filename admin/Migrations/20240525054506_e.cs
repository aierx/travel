using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    public partial class e : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "like",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sceneryId = table.Column<long>(type: "bigint", nullable: false),
                    userId = table.Column<long>(type: "bigint", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    ModifyTime = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    Creator = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    modifer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_like", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "S0YQUoN70G07EKp8rPmkn7DcRLwgoVWkTrA+9A1QJ/PtCk3BVpAp6dVgQX/jU9I+aBL37cj2gsMJo4qydo40RQ==:gOL44Ev7upQb9ZoQ1IesTg1DmIuzEIjXHncxzaiIaKChatcELv2LryKp0yw98o91jk75jqW/qxbKdTHrga2OgQ==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "like");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "b7lzC85JVrQZkI8dPPg9aDB4rxuwuw8QVFaVdCKAM0D2DddV/5uhqoVFX7KeUdSn93Vxzgu5Unory05NgFZXeA==:KFFgKxit8QdORVbgmEJ/puD6Hcx6u/8E7sPiVfQhtPH1OPU0XSVUNLFzgtDHL8TMxTywXyIVfHo/FfmKpCP8ng==");
        }
    }
}
