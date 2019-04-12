namespace Assessment.Data.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InitialSqlFunctions : DbMigration
    {
        public override void Up()
        {
            var fnTotalOfFriends = @"
                CREATE FUNCTION [dbo].[TotalOfFriendsOf](@FriendId varchar(255))
                RETURNS INT
                AS
                    BEGIN
                    DECLARE @count int;
                    SELECT @count = count(*) from [dbo].Friendship where MainFriendId = @FriendId

                    RETURN @count;
                END
";
            Sql(fnTotalOfFriends);

            var fnTotalOfContries = @"
                CREATE FUNCTION [dbo].[TotalOfContries]()
                RETURNS INT
                AS
                    BEGIN
                    DECLARE @count int;
                    SELECT @count = count(*) from [dbo].Country

                    RETURN @count;
                END
";
            Sql(fnTotalOfContries);
            var fnTotalOfStates = @"
                CREATE FUNCTION [dbo].[TotalOfStateOf](@CountryId varchar(255))
                RETURNS INT
                AS
                    BEGIN
                    DECLARE @count int;
                    SELECT @count = count(*) from [dbo].State where Country_Id = @CountryId

                    RETURN @count;
                END
";
            Sql(fnTotalOfStates);

        }

        public override void Down()
        {
            Sql("DROP FUNCTION IF EXISTS [dbo].TotalOfFriendOf;");
            Sql("DROP FUNCTION IF EXISTS [dbo].TotalOfContries;");
            Sql("DROP FUNCTION IF EXISTS [dbo].TotalOfStateOf;");
        }
    }
}
