namespace TP3WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class spCrud : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.Friend_Insert",
                p => new
                    {
                        Id = p.Guid(),
                        Name = p.String(),
                        LastName = p.String(),
                        Email = p.String(),
                        Phone = p.String(),
                        BirthDate = p.DateTime(),
                    },
                body:
                    @"INSERT [dbo].[Friends]([Id], [Name], [LastName], [Email], [Phone], [BirthDate])
                      VALUES (@Id, @Name, @LastName, @Email, @Phone, @BirthDate)"
            );
            
            CreateStoredProcedure(
                "dbo.Friend_Update",
                p => new
                    {
                        Id = p.Guid(),
                        Name = p.String(),
                        LastName = p.String(),
                        Email = p.String(),
                        Phone = p.String(),
                        BirthDate = p.DateTime(),
                    },
                body:
                    @"UPDATE [dbo].[Friends]
                      SET [Name] = @Name, [LastName] = @LastName, [Email] = @Email, [Phone] = @Phone, [BirthDate] = @BirthDate
                      WHERE ([Id] = @Id)"
            );
            
            CreateStoredProcedure(
                "dbo.Friend_Delete",
                p => new
                    {
                        Id = p.Guid(),
                    },
                body:
                    @"DELETE [dbo].[Friends]
                      WHERE ([Id] = @Id)"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Friend_Delete");
            DropStoredProcedure("dbo.Friend_Update");
            DropStoredProcedure("dbo.Friend_Insert");
        }
    }
}
