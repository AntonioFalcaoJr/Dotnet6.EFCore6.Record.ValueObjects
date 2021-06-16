using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dotnet6.EFCore6.Record.ValueObject.Repositories.Migrations
{
    public partial class Firstmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Street_City_Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Street_City_State_Country_Initials = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Street_City_State_Country_Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Street_City_State_Initials = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Street_City_State_Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Street_Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Street_Number = table.Column<int>(type: "int", nullable: true),
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Age", "Name" },
                values: new object[,]
                {
                    { new Guid("00958d32-8a83-45c8-8300-9d1b8ea05afc"), 76, "Avery" },
                    { new Guid("4f35bfdd-d49b-442d-971f-fdd0cf2e86ee"), 60, "Vida" },
                    { new Guid("4a8b006c-2e55-4339-9a02-3bd38f3a9571"), 23, "Toby" },
                    { new Guid("f3dfb700-c4ae-4239-a1d6-ffe8f2c2c47e"), 43, "Lilly" },
                    { new Guid("835042ea-1bdc-439a-90f0-36f871b1890a"), 37, "Madisen" },
                    { new Guid("6e017e62-a84a-4c42-bb6b-af6ceaa3603c"), 34, "Jane" },
                    { new Guid("c2822b57-94bb-4c65-a85f-19508df6ade0"), 18, "Corrine" },
                    { new Guid("21adaba7-0925-4b02-973b-40004d0b8958"), 69, "Margarette" },
                    { new Guid("04226e8f-acf0-4eeb-b4d1-e53f4ee8063a"), 62, "German" },
                    { new Guid("2d129e75-f6f4-4385-ba7e-077aeeb0cbb2"), 43, "Mortimer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_PersonId",
                table: "Addresses",
                column: "PersonId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
