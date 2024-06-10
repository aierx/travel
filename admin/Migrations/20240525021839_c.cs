using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    public partial class c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "filter",
                table: "comment",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "OzUlc5PmPp8bvGlzqEXncb+ngu+PUcT6n3w77e7Z8+Gcl7ErBLxDbiUEbtmOZG+QgYIbB3k8Vxn5oxqYpx5YdQ==:rf7ukwHbLb0utQ8zR3s3YnMll1nDzh2tqQxBs2nk2p07gSFViFXfhw+bRvfwSwpLAT1xAT3+kXiS2kaYPZg+xQ==");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "filter",
                table: "comment");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "passwd",
                value: "zaglbSrrE2uQhnvGxLS/v2MnVCCal52B8A9g04SeO3vaUV0lBcc8dMt3Qtq4cr1Qt9NsGh0xmqVrAOXzOyd6zg==:SSLRnAPA7TQsbmKk91lnTOTzqOihUDc0R9OaemQCH608GoBhcey5mdt9H99g+Dl+JaYr5aszxy5BpvtEnmcVwg==");
        }
    }
}
