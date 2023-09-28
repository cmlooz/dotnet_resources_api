using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourcesAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial_model_create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Para eliminar las tablas si ya existen y que tomen los cambios en diseño
            //Down(migrationBuilder);

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    rowid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    class_id = table.Column<int>(type: "int", nullable: false),
                    class_text = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    //class_order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    class_order = table.Column<int>(type: "int", nullable: false),
                    createdby = table.Column<string>(type: "varchar(50)", nullable: false),
                    //createdon = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                    createdon = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.rowid);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    rowid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    filename = table.Column<string>(type: "varchar(500)", nullable: false),
                    filedata = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    class_id = table.Column<int>(type: "int", nullable: false),
                    //class_order = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    class_order = table.Column<int>(type: "int", nullable: false),
                    createdby = table.Column<string>(type: "varchar(50)", nullable: false),
                    //createdon = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()")
                    createdon = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.rowid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
