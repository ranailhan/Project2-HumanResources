using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HumanResourcesDBFirst.Migrations
{
    /// <inheritdoc />
    public partial class AddSuggestionsAndReplySupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminReply",
                table: "WishSuggestions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WishSuggestions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Beklemede");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminReply",
                table: "WishSuggestions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WishSuggestions");
        }
    }
}
