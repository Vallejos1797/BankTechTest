using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UsersAndUserSps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Tabla Users
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    // Si quieres tamaños fijos, en el DbContext agrega HasMaxLength(32/16)
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "user"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "SYSUTCDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            // SP: GetAll
            migrationBuilder.Sql("""
                IF OBJECT_ID('dbo.usp_User_GetAll','P') IS NOT NULL DROP PROCEDURE dbo.usp_User_GetAll;
                EXEC('CREATE PROCEDURE dbo.usp_User_GetAll AS
                BEGIN
                  SET NOCOUNT ON;
                  SELECT Id, Username, Email, PasswordHash, PasswordSalt, Role, CreatedAt
                  FROM dbo.Users
                  ORDER BY Id DESC;
                END');
            """);

            // SP: GetById
            migrationBuilder.Sql("""
                IF OBJECT_ID('dbo.usp_User_GetById','P') IS NOT NULL DROP PROCEDURE dbo.usp_User_GetById;
                EXEC('CREATE PROCEDURE dbo.usp_User_GetById @Id INT AS
                BEGIN
                  SET NOCOUNT ON;
                  SELECT Id, Username, Email, PasswordHash, PasswordSalt, Role, CreatedAt
                  FROM dbo.Users WHERE Id=@Id;
                END');
            """);

            // SP: GetByUsername
            migrationBuilder.Sql("""
                IF OBJECT_ID('dbo.usp_User_GetByUsername','P') IS NOT NULL DROP PROCEDURE dbo.usp_User_GetByUsername;
                EXEC('CREATE PROCEDURE dbo.usp_User_GetByUsername @Username NVARCHAR(50) AS
                BEGIN
                  SET NOCOUNT ON;
                  SELECT Id, Username, Email, PasswordHash, PasswordSalt, Role, CreatedAt
                  FROM dbo.Users WHERE Username=@Username;
                END');
            """);

            // SP: Create (devuelve el Id)
            migrationBuilder.Sql("""
                IF OBJECT_ID('dbo.usp_User_Create','P') IS NOT NULL DROP PROCEDURE dbo.usp_User_Create;
                EXEC('CREATE PROCEDURE dbo.usp_User_Create
                  @Username NVARCHAR(50), @Email NVARCHAR(120),
                  @PasswordHash VARBINARY(8000), @PasswordSalt VARBINARY(8000),
                  @Role NVARCHAR(20)
                AS
                BEGIN
                  SET NOCOUNT ON;
                  INSERT INTO dbo.Users (Username, Email, PasswordHash, PasswordSalt, Role)
                  VALUES (@Username, @Email, @PasswordHash, @PasswordSalt, @Role);
                  SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;
                END');
            """);

            // SP: Update
            migrationBuilder.Sql("""
                IF OBJECT_ID('dbo.usp_User_Update','P') IS NOT NULL DROP PROCEDURE dbo.usp_User_Update;
                EXEC('CREATE PROCEDURE dbo.usp_User_Update
                  @Id INT, @Email NVARCHAR(120),
                  @PasswordHash VARBINARY(8000), @PasswordSalt VARBINARY(8000),
                  @Role NVARCHAR(20)
                AS
                BEGIN
                  SET NOCOUNT ON;
                  UPDATE dbo.Users
                  SET Email=@Email, PasswordHash=@PasswordHash, PasswordSalt=@PasswordSalt, Role=@Role
                  WHERE Id=@Id;
                END');
            """);

            // SP: Delete
            migrationBuilder.Sql("""
                IF OBJECT_ID('dbo.usp_User_Delete','P') IS NOT NULL DROP PROCEDURE dbo.usp_User_Delete;
                EXEC('CREATE PROCEDURE dbo.usp_User_Delete @Id INT AS
                BEGIN
                  SET NOCOUNT ON;
                  DELETE FROM dbo.Users WHERE Id=@Id;
                END');
            """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Primero elimina SPs, luego tabla
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_User_Delete;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_User_Update;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_User_Create;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_User_GetByUsername;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_User_GetById;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_User_GetAll;");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
