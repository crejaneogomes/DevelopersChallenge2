using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class editTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Challenge",
                table: "Challenge");

            migrationBuilder.RenameTable(
                name: "Challenge",
                newName: "OFXTransactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OFXTransactions",
                table: "OFXTransactions",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OFXTransactions",
                table: "OFXTransactions");

            migrationBuilder.RenameTable(
                name: "OFXTransactions",
                newName: "Challenge");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Challenge",
                table: "Challenge",
                column: "Id");
        }
    }
}
