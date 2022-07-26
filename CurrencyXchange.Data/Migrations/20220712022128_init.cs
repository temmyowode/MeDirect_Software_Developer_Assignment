using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyXchange.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionHistory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<Guid>(nullable: false),
                    CurrencyFrom = table.Column<string>(nullable: true),
                    CurrencyTo = table.Column<string>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    TransDate = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionHistory");
        }
    }
}
