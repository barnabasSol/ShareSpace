using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSpace.Server.Migrations
{
    /// <inheritdoc />
    public partial class FUCKINGWORK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_post_images",
                table: "post_images");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "post_images",
                newName: "image_url");

            migrationBuilder.CreateIndex(
                name: "IX_post_images_post_id",
                table: "post_images",
                column: "post_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_post_images_post_id",
                table: "post_images");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "post_images",
                newName: "ImageUrl");

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_images",
                table: "post_images",
                column: "post_id");
        }
    }
}
