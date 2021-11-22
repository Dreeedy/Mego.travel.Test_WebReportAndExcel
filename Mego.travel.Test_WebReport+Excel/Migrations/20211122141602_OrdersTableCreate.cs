using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mego.travel.Test_WebReport_Excel.Migrations
{
    public partial class OrdersTableCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "Date", "Price" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 10, 1, 8, 59, 59, 0, DateTimeKind.Unspecified), 560 },
                    { 16, new DateTime(2021, 11, 8, 16, 59, 59, 0, DateTimeKind.Unspecified), 5500 },
                    { 15, new DateTime(2021, 11, 8, 16, 59, 59, 0, DateTimeKind.Unspecified), 4999 },
                    { 14, new DateTime(2021, 11, 7, 16, 59, 59, 0, DateTimeKind.Unspecified), 5000 },
                    { 13, new DateTime(2021, 11, 6, 15, 59, 59, 0, DateTimeKind.Unspecified), 4560 },
                    { 12, new DateTime(2021, 11, 5, 14, 59, 59, 0, DateTimeKind.Unspecified), 3200 },
                    { 11, new DateTime(2021, 11, 5, 13, 59, 59, 0, DateTimeKind.Unspecified), 4004 },
                    { 10, new DateTime(2021, 11, 5, 12, 59, 59, 0, DateTimeKind.Unspecified), 3003 },
                    { 9, new DateTime(2021, 11, 4, 11, 59, 59, 0, DateTimeKind.Unspecified), 2005 },
                    { 8, new DateTime(2021, 11, 3, 10, 59, 59, 0, DateTimeKind.Unspecified), 2033 },
                    { 7, new DateTime(2021, 11, 2, 9, 59, 59, 0, DateTimeKind.Unspecified), 2002 },
                    { 6, new DateTime(2021, 11, 1, 8, 59, 59, 0, DateTimeKind.Unspecified), 1001 },
                    { 5, new DateTime(2021, 11, 25, 18, 59, 59, 0, DateTimeKind.Unspecified), 330 },
                    { 4, new DateTime(2021, 10, 18, 11, 59, 59, 0, DateTimeKind.Unspecified), 710 },
                    { 3, new DateTime(2021, 10, 10, 10, 59, 59, 0, DateTimeKind.Unspecified), 700 },
                    { 2, new DateTime(2021, 10, 5, 9, 59, 59, 0, DateTimeKind.Unspecified), 550 },
                    { 17, new DateTime(2021, 11, 8, 16, 59, 59, 0, DateTimeKind.Unspecified), 6000 },
                    { 18, new DateTime(2021, 11, 8, 16, 59, 59, 0, DateTimeKind.Unspecified), 6500 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
