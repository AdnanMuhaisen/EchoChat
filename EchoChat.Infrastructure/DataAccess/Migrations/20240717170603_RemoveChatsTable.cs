using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EchoChat.Infrastructure.DataAccess.Migrations;

/// <inheritdoc />
public partial class RemoveChatsTable : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Chats");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Chats",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ReceiverId = table.Column<int>(type: "int", nullable: false),
                SenderId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Chats", x => x.Id);
            });
    }
}
