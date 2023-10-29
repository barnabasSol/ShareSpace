using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSpace.Server.Migrations
{
    /// <inheritdoc />
    public partial class PostTagFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_post_tags",
                table: "post_tags");

            migrationBuilder.RenameColumn(
                name: "tag_id",
                table: "post_tags",
                newName: "tag_name");

            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "post_tags",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_tags",
                table: "post_tags",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_post_tags_post_id",
                table: "post_tags",
                column: "post_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_post_tags",
                table: "post_tags");

            migrationBuilder.DropIndex(
                name: "IX_post_tags_post_id",
                table: "post_tags");

            migrationBuilder.DropColumn(
                name: "id",
                table: "post_tags");

            migrationBuilder.RenameColumn(
                name: "tag_name",
                table: "post_tags",
                newName: "tag_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_tags",
                table: "post_tags",
                columns: new[] { "post_id", "tag_id" });
        }
    }
}
