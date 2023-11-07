namespace website_projexts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class life : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        CategoryID = c.Int(nullable: false),
                        ProjectName = c.String(nullable: false, maxLength: 30),
                        ShortDescription = c.String(nullable: false, maxLength: 50),
                        LongDescription = c.String(nullable: false, maxLength: 2000),
                        Goal = c.Int(nullable: false),
                        Raised = c.Int(nullable: false),
                        PostedTime = c.DateTime(nullable: false),
                        ProjectImage = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        DonationID = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        Donated = c.Int(nullable: false),
                        DonationText = c.String(nullable: false, maxLength: 50),
                        DonationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.DonationID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.ProjectID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        UserName = c.String(nullable: false, maxLength: 30),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        UserImage = c.String(),
                    })
                .PrimaryKey(t => t.UserID);
            
            CreateTable(
                "dbo.Updates",
                c => new
                    {
                        UpdateId = c.Int(nullable: false, identity: true),
                        ProjectID = c.Int(nullable: false),
                        UpdateText = c.String(nullable: false, maxLength: 50),
                        UpdateTime = c.DateTime(nullable: false),
                        UpdateImage = c.String(),
                    })
                .PrimaryKey(t => t.UpdateId)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.ProjectID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Updates", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Projects", "UserID", "dbo.Users");
            DropForeignKey("dbo.Donations", "UserID", "dbo.Users");
            DropForeignKey("dbo.Donations", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.Projects", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Updates", new[] { "ProjectID" });
            DropIndex("dbo.Donations", new[] { "UserID" });
            DropIndex("dbo.Donations", new[] { "ProjectID" });
            DropIndex("dbo.Projects", new[] { "CategoryID" });
            DropIndex("dbo.Projects", new[] { "UserID" });
            DropTable("dbo.Updates");
            DropTable("dbo.Users");
            DropTable("dbo.Donations");
            DropTable("dbo.Projects");
            DropTable("dbo.Categories");
        }
    }
}
