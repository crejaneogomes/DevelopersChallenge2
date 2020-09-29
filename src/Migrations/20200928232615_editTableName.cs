using Microsoft.EntityFrameworkCore.Migrations;

namespace src.Migrations
{
    public partial class editTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OFXTransactions",
                table: "OFXTransactions");

            migrationBuilder.RenameTable(
                name: "OFXTransactions",
                newName: "OFXTransactionModels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OFXTransactionModels",
                table: "OFXTransactionModels",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OFXTransactionModels",
                table: "OFXTransactionModels");

            migrationBuilder.RenameTable(
                name: "OFXTransactionModels",
                newName: "OFXTransactions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OFXTransactions",
                table: "OFXTransactions",
                column: "Id");
        }
    }
}
