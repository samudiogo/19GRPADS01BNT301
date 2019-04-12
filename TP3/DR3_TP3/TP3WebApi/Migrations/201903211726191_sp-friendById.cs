namespace TP3WebApi.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class spfriendById : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure("dbo.GetFriendByGuid", p => new { friendId = p.Guid() }, 
                body:
                @"
		            SELECT Id, [Name], LastName, Email, Phone, BirthDate FROM [dbo].[Friends] WHERE Id = @friendId
	            ");
        }

        public override void Down()
        {
            DropStoredProcedure("dbo.GetFriendByGuid");
        }
    }
}
