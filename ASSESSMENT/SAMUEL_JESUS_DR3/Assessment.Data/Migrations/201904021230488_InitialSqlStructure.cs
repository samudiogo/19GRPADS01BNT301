namespace Assessment.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSqlStructure : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                CREATE TABLE SpLog
                (
                    ID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                    DESCRICAO VARCHAR(MAX),
                    DT DATETIME DEFAULT GETDATE(),
                )");

            CreateTable(
                "dbo.Country",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        PhotoUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.State",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        FlagPhotoUrl = c.String(),
                        Country_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Country", t => t.Country_Id, cascadeDelete: true)
                .Index(t => t.Country_Id);
            
            CreateTable(
                "dbo.Friend",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(nullable: false, maxLength: 255),
                        LastName = c.String(nullable: false, maxLength: 255),
                        PhotoUrl = c.String(nullable: false),
                        Email = c.String(nullable: false, maxLength: 255),
                        PhoneNumber = c.String(nullable: false, maxLength: 255),
                        BirthDate = c.DateTime(nullable: false),
                        State_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.State", t => t.State_Id, cascadeDelete: true)
                .Index(t => t.State_Id);
            
            CreateTable(
                "dbo.Friendship",
                c => new
                    {
                        MainFriendId = c.Guid(nullable: false),
                        ChildFriendId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.MainFriendId, t.ChildFriendId })
                .ForeignKey("dbo.Friend", t => t.MainFriendId)
                .ForeignKey("dbo.Friend", t => t.ChildFriendId)
                .Index(t => t.MainFriendId)
                .Index(t => t.ChildFriendId);
            
            CreateStoredProcedure(
                "dbo.Country_Insert",
                p => new
                    {
                        Id = p.Guid(),
                        Name = p.String(maxLength: 255),
                        PhotoUrl = p.String(),
                    },
                body:
                    @"IF @Id = '00000000-0000-0000-0000-000000000000'
	                    BEGIN
		                    SET @Id = NEWID();
	                    END
                        INSERT [dbo].[Country]([Id], [Name], [PhotoUrl])
                      VALUES (@Id, @Name, @PhotoUrl);
                    INSERT INTO SPLOG(DESCRICAO) VALUES('SP INSERT OF Country ID: ' + CONVERT(VARCHAR(MAX), @Id));"
            );
            
            CreateStoredProcedure(
                "dbo.Country_Update",
                p => new
                    {
                        Id = p.Guid(),
                        Name = p.String(maxLength: 255),
                        PhotoUrl = p.String(),
                    },
                body:
                    @"UPDATE [dbo].[Country]
                      SET [Name] = @Name, [PhotoUrl] = @PhotoUrl
                      WHERE ([Id] = @Id);
                    INSERT INTO SPLOG(DESCRICAO) VALUES('SP UPDATE OF Country ID: ' + CONVERT(VARCHAR(MAX), @Id));"
            );
            
            CreateStoredProcedure(
                "dbo.Country_Delete",
                p => new
                    {
                        Id = p.Guid(),
                    },
                body:
                    @"DELETE [dbo].[Country]
                      WHERE ([Id] = @Id);
                    INSERT INTO SPLOG(DESCRICAO) VALUES('SP DELETE OF Country ID: ' + CONVERT(VARCHAR(MAX), @Id));"
            );
            
            CreateStoredProcedure(
                "dbo.State_Insert",
                p => new
                    {
                        Id = p.Guid(),
                        Name = p.String(maxLength: 255),
                        FlagPhotoUrl = p.String(),
                        Country_Id = p.Guid(),
                    },
                body:
                    @"IF @Id = '00000000-0000-0000-0000-000000000000'
	                    BEGIN
		                    SET @Id = NEWID();
	                    END
                     INSERT [dbo].[State]([Id], [Name], [FlagPhotoUrl], [Country_Id])
                      VALUES (@Id, @Name, @FlagPhotoUrl, @Country_Id);
                     INSERT INTO SPLOG(DESCRICAO) VALUES('SP INSERT OF State ID: ' + CONVERT(VARCHAR(MAX), @Id));"
            );
            
            CreateStoredProcedure(
                "dbo.State_Update",
                p => new
                    {
                        Id = p.Guid(),
                        Name = p.String(maxLength: 255),
                        FlagPhotoUrl = p.String(),
                        Country_Id = p.Guid(),
                    },
                body:
                    @"UPDATE [dbo].[State]
                      SET [Name] = @Name, [FlagPhotoUrl] = @FlagPhotoUrl
                      WHERE ([Id] = @Id);
                     INSERT INTO SPLOG(DESCRICAO) VALUES('SP UPDATE OF State ID: ' + CONVERT(VARCHAR(MAX), @Id));"
            );
            
            CreateStoredProcedure(
                "dbo.State_Delete",
                p => new
                    {
                        Id = p.Guid(),
                        Country_Id = p.Guid(),
                    },
                body:
                    @"DELETE [dbo].[State]
                      WHERE (([Id] = @Id) AND (([Country_Id] = @Country_Id) OR ([Country_Id] IS NULL AND @Country_Id IS NULL)));
                     INSERT INTO SPLOG(DESCRICAO) VALUES('SP DELETE OF State ID: ' + CONVERT(VARCHAR(MAX), @Id));"
            );
            
            CreateStoredProcedure(
                "dbo.Friend_Insert",
                p => new
                    {
                        Id = p.Guid(),
                        Name = p.String(maxLength: 255),
                        LastName = p.String(maxLength: 255),
                        PhotoUrl = p.String(),
                        Email = p.String(maxLength: 255),
                        PhoneNumber = p.String(maxLength: 255),
                        BirthDate = p.DateTime(),
                        State_Id = p.Guid(),
                    },
                body:
                    @"
                    IF @Id = '00000000-0000-0000-0000-000000000000'
	                    BEGIN
		                    SET @Id = NEWID();
	                    END
                    INSERT [dbo].[Friend]([Id], [Name], [LastName], [PhotoUrl], [Email], [PhoneNumber], [BirthDate], [State_Id])
                      VALUES (@Id, @Name, @LastName, @PhotoUrl, @Email, @PhoneNumber, @BirthDate, @State_Id);
                     INSERT INTO SPLOG(DESCRICAO) VALUES('SP INSERT OF Friend ID: ' + CONVERT(VARCHAR(MAX), @Id));"
            );
            
            CreateStoredProcedure(
                "dbo.Friend_Update",
                p => new
                    {
                        Id = p.Guid(),
                        Name = p.String(maxLength: 255),
                        LastName = p.String(maxLength: 255),
                        PhotoUrl = p.String(),
                        Email = p.String(maxLength: 255),
                        PhoneNumber = p.String(maxLength: 255),
                        BirthDate = p.DateTime(),
                        State_Id = p.Guid(),
                    },
                body:
                    @"UPDATE [dbo].[Friend]
                      SET [Name] = @Name, [LastName] = @LastName, [PhotoUrl] = @PhotoUrl, [Email] = @Email, 
                        [PhoneNumber] = @PhoneNumber, [BirthDate] = @BirthDate, [State_Id] = @State_Id
                      WHERE ([Id] = @Id);
                     INSERT INTO SPLOG(DESCRICAO) VALUES('SP UPDATE OF Friend ID: ' + CONVERT(VARCHAR(MAX), @Id));"
            );
            
            CreateStoredProcedure(
                "dbo.Friend_Delete",
                p => new
                    {
                        Id = p.Guid(),
                        State_Id = p.Guid(),
                    },
                body:
                    @"DELETE [dbo].[Friend]
                      WHERE ([Id] = @Id);
                     INSERT INTO SPLOG(DESCRICAO) VALUES('SP DELETE OF Friend ID: ' + CONVERT(VARCHAR(MAX), @Id));"
            );
            
        }
        
        public override void Down()
        {
            DropStoredProcedure("dbo.Friend_Delete");
            DropStoredProcedure("dbo.Friend_Update");
            DropStoredProcedure("dbo.Friend_Insert");
            DropStoredProcedure("dbo.State_Delete");
            DropStoredProcedure("dbo.State_Update");
            DropStoredProcedure("dbo.State_Insert");
            DropStoredProcedure("dbo.Country_Delete");
            DropStoredProcedure("dbo.Country_Update");
            DropStoredProcedure("dbo.Country_Insert");
            DropForeignKey("dbo.Friend", "State_Id", "dbo.State");
            DropForeignKey("dbo.Friendship", "ChildFriendId", "dbo.Friend");
            DropForeignKey("dbo.Friendship", "MainFriendId", "dbo.Friend");
            DropForeignKey("dbo.State", "Country_Id", "dbo.Country");
            DropIndex("dbo.Friendship", new[] { "ChildFriendId" });
            DropIndex("dbo.Friendship", new[] { "MainFriendId" });
            DropIndex("dbo.Friend", new[] { "State_Id" });
            DropIndex("dbo.State", new[] { "Country_Id" });
            DropTable("dbo.Friendship");
            DropTable("dbo.Friend");
            DropTable("dbo.State");
            DropTable("dbo.Country");
            DropTable("dbo.SpLog");
        }
    }
}
