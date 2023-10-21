using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareSpace.Server.Migrations
{
    /// <inheritdoc />
    public partial class FuckingWorkPlease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "id",
                table: "post_images",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_post_images",
                table: "post_images",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_post_images",
                table: "post_images");

            migrationBuilder.DropColumn(
                name: "id",
                table: "post_images");
        }
    }
}
