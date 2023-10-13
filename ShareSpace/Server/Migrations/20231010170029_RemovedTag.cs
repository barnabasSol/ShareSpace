using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSpace.Server.Migrations
{
    /// <inheritdoc />
    public partial class RemovedTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_post_tags_tags_tag_id",
                table: "post_tags");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropIndex(
                name: "IX_post_tags_tag_id",
                table: "post_tags");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "uuid_generate_v4()"),
                    name = table.Column<string>(type: "text", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.id);
                    table.ForeignKey(
                        name: "FK_tags_posts_PostId",
                        column: x => x.PostId,
                        principalTable: "posts",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_post_tags_tag_id",
                table: "post_tags",
                column: "tag_id");

            migrationBuilder.CreateIndex(
                name: "IX_tags_PostId",
                table: "tags",
                column: "PostId");

            migrationBuilder.AddForeignKey(
                name: "FK_post_tags_tags_tag_id",
                table: "post_tags",
                column: "tag_id",
                principalTable: "tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
