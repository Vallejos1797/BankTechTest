using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductStoredProcedures : Migration
    {
        /// <inheritdoc />
       protected override void Up(MigrationBuilder migrationBuilder)
{
    // GetAll
    migrationBuilder.Sql("""
        IF OBJECT_ID('dbo.usp_Product_GetAll','P') IS NOT NULL DROP PROCEDURE dbo.usp_Product_GetAll;
        EXEC('CREATE PROCEDURE dbo.usp_Product_GetAll AS
        BEGIN
            SET NOCOUNT ON;
            SELECT Id, Name, [Description], Logo, DateRelease, DateRevision
            FROM dbo.Products
            ORDER BY Id DESC;
        END');
    """);

    // GetById
    migrationBuilder.Sql("""
        IF OBJECT_ID('dbo.usp_Product_GetById','P') IS NOT NULL DROP PROCEDURE dbo.usp_Product_GetById;
        EXEC('CREATE PROCEDURE dbo.usp_Product_GetById @Id INT AS
        BEGIN
            SET NOCOUNT ON;
            SELECT Id, Name, [Description], Logo, DateRelease, DateRevision
            FROM dbo.Products WHERE Id = @Id;
        END');
    """);

    // Create -> devuelve el nuevo Id (para EF Core 8: SqlQuery<int>())
    migrationBuilder.Sql("""
        IF OBJECT_ID('dbo.usp_Product_Create','P') IS NOT NULL DROP PROCEDURE dbo.usp_Product_Create;
        EXEC('CREATE PROCEDURE dbo.usp_Product_Create
            @Name NVARCHAR(100),
            @Description NVARCHAR(255),
            @Logo NVARCHAR(255),
            @DateRelease DATETIME2,
            @DateRevision DATETIME2
        AS
        BEGIN
            SET NOCOUNT ON;
            INSERT INTO dbo.Products (Name,[Description],Logo,DateRelease,DateRevision)
            VALUES (@Name,@Description,@Logo,@DateRelease,@DateRevision);

            SELECT CAST(SCOPE_IDENTITY() AS INT) AS NewId;
        END');
    """);

    // Update
    migrationBuilder.Sql("""
        IF OBJECT_ID('dbo.usp_Product_Update','P') IS NOT NULL DROP PROCEDURE dbo.usp_Product_Update;
        EXEC('CREATE PROCEDURE dbo.usp_Product_Update
            @Id INT,
            @Name NVARCHAR(100),
            @Description NVARCHAR(255),
            @Logo NVARCHAR(255),
            @DateRelease DATETIME2,
            @DateRevision DATETIME2
        AS
        BEGIN
            SET NOCOUNT ON;
            UPDATE dbo.Products
            SET Name=@Name, [Description]=@Description, Logo=@Logo,
                DateRelease=@DateRelease, DateRevision=@DateRevision
            WHERE Id=@Id;
        END');
    """);

    // Delete
    migrationBuilder.Sql("""
        IF OBJECT_ID('dbo.usp_Product_Delete','P') IS NOT NULL DROP PROCEDURE dbo.usp_Product_Delete;
        EXEC('CREATE PROCEDURE dbo.usp_Product_Delete @Id INT AS
        BEGIN
            SET NOCOUNT ON;
            DELETE FROM dbo.Products WHERE Id=@Id;
        END');
    """);
}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_Product_Delete;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_Product_Update;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_Product_Create;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_Product_GetById;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.usp_Product_GetAll;");
        }
    }
}
