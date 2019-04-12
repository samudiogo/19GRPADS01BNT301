using TP3WebApi.Models;

namespace TP3WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spgetAllFriends : DbMigration
    {
        public override void Up()
        {
            var sp = @"CREATE PROCEDURE [dbo].[GetAllFriends]
                        AS
	                        BEGIN
		                        SET NOCOUNT ON;
		                        SELECT Id, [Name], LastName, Email, Phone, BirthDate FROM [dbo].[Friends]
	                        END";

            using (var db  = new Tp3DbContext())
            {
                db.Database.ExecuteSqlCommand(sp);
            }

        }
        
        public override void Down()
        {
        }
    }
}
