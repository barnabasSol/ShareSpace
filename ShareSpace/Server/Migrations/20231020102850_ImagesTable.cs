using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSpace.Server.Migrations
{
    /// <inheritdoc />
    public partial class ImagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "posts");

            migrationBuilder.CreateTable(
                name: "post_images",
                columns: table => new
                {
                    post_id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_post_images", x => x.post_id);
                    table.ForeignKey(
                        name: "FK_post_images_posts_post_id",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "post_images");

            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "posts",
                type: "text",
                nullable: true);
        }
    }
}
