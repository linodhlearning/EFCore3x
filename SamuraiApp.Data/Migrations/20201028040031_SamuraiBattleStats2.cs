using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class SamuraiBattleStats2 : Migration
    {
        /*Custom added the function in migration
        add-migration SamuraiBattleStats2 -Context SamuraiContext
         */
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"create function dbo.EarliestBattleFought(@SamuraiId INT)
            RETURNS CHAR(30) AS
            BEGIN
             DECLARE @ret CHAR(30)
             SELECT TOP 1 @ret=Name
             FROM Battles b where b.Id IN(SELECT BattleId FROM SamuraiBattle WHERE SamuraiId=@SamuraiId)
             ORDER BY StartDate
             RETURN @ret
            END
            ");
            migrationBuilder.Sql(
            @"CREATE VIEW dbo.SamuariBattleStats
                AS
                SELECT s.Name
                , COUNT(sb.BattleId) AS NoOfBattles,
                dbo.EarliestBattleFought(sb.SamuraiId) AS EarliestBattleName
                FROM dbo.SamuraiBattle sb 
                INNER JOIN dbo.Samurais s ON s.Id=sb.SamuraiId
                GROUP BY s.Name, sb.SamuraiId
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW dbo.SamuariBattleStats ");
            migrationBuilder.Sql("DROP Function dbo.EarliestBattleFought ");
        }
    }
}
