using Microsoft.EntityFrameworkCore.Migrations;

namespace Comics.DAL.Migrations
{
    public partial class ModelsEdited1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "Collections",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "BaseItem",
                newName: "Description");

            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Collections",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BaseItem",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CollectionId",
                table: "BaseItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CollectionId",
                table: "Comments",
                column: "CollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseItem_CollectionId",
                table: "BaseItem",
                column: "CollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseItem_Collections_CollectionId",
                table: "BaseItem",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Collections_CollectionId",
                table: "Comments",
                column: "CollectionId",
                principalTable: "Collections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseItem_Collections_CollectionId",
                table: "BaseItem");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Collections_CollectionId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_CollectionId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_BaseItem_CollectionId",
                table: "BaseItem");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Collections");

            migrationBuilder.DropColumn(
                name: "CollectionId",
                table: "BaseItem");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Collections",
                newName: "Desc");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "BaseItem",
                newName: "Desc");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "BaseItem",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
