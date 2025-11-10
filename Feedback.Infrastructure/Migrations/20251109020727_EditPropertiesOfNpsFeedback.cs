using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Feedback.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EditPropertiesOfNpsFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Feedbacks",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Feedbacks");
        }
    }
}
