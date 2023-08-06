using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Musicfy.API.Migrations
{
    /// <inheritdoc />
    public partial class AddIsFeaturedToArtistTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Artists",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Artists");
        }
    }
}
