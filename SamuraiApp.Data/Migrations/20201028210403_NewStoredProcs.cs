using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class NewStoredProcs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROC dbo.SamuarisWithQuote
            @Text Varchar(20)
            AS 
            Select s.Id,s.Name,s.ClanId
            FROM Samuaris s
            where EXISTS(select 1 from Quotes q where q.SamuraiId=s.Id AND q.Text LIKE '%'+@Text+'%')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE dbo.SamuarisWithQuote");
        }
    }
}
