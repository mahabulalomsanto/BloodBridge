using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloodBridge.Migrations
{
    /// <inheritdoc />
    public partial class DonationStatusAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanDonate",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanDonate",
                table: "AspNetUsers");
        }
    }
}
