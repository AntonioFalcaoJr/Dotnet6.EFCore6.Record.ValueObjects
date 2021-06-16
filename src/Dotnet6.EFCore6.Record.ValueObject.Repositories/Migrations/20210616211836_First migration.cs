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
                    Street_City_State_Country_Initials = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    Street_City_State_Country_Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Street_City_State_Initials = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    Street_City_State_Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Street_Name = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    Street_Number = table.Column<int>(type: "int", nullable: true),
                    ZipCode = table.Column<string>(type: "varchar(32)", unicode: false, maxLength: 32, nullable: false),
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
                    { new Guid("5fb17362-5582-4784-a474-55f8674dcebb"), 31, "Mozelle" },
                    { new Guid("7d6bfebd-7f3c-477a-930d-f384dbdb178c"), 51, "Hipolito" },
                    { new Guid("99836a44-c19e-4121-b7a0-1df1335b25e5"), 48, "Filiberto" },
                    { new Guid("1e7e2bc7-8394-4892-bd26-0996974efe6f"), 58, "Carlee" },
                    { new Guid("4dc3c3ca-d991-4466-a423-982d2eecbe61"), 52, "Lindsay" },
                    { new Guid("1c6eaa44-e44a-4396-a61d-465be5b3b807"), 65, "Shana" },
                    { new Guid("4e6ef913-24c0-4af6-b7d5-9b20b2e2f2e1"), 79, "Madisen" },
                    { new Guid("c08070ab-566f-42bd-a32d-dcf19b4f0ec1"), 22, "Danielle" },
                    { new Guid("47cb5470-ad76-4193-91d2-89a8379ca482"), 34, "Kelton" },
                    { new Guid("362f9e3a-164c-4e75-a401-0bb90371f73e"), 66, "Emelia" }
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
