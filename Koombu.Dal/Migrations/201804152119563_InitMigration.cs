namespace Koombu.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 500),
                        Date = c.DateTime(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        PostId = c.Int(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.PostId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Content = c.String(maxLength: 500),
                        PictureUrl = c.String(maxLength: 200),
                        AttachementUrl = c.String(),
                        AttachementName = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        OwnerId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.PostId)
                .ForeignKey("dbo.Groups", t => t.GroupId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.GroupId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GroupName = c.String(nullable: false, maxLength: 50),
                        GroupDescription = c.String(maxLength: 200),
                        GroupPicture = c.String(),
                        GroupOwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(),
                        Department = c.String(),
                        Title = c.String(),
                        ProfilPicture = c.String(),
                        Password = c.String(maxLength: 20),
                        EmailAdress = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        User_UserId = c.Int(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Users", t => t.User_UserId)
                .Index(t => t.User_UserId);
            
            CreateTable(
                "dbo.PostLikes",
                c => new
                    {
                        PostId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PostId, t.UserId })
                .ForeignKey("dbo.Posts", t => t.PostId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.PostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserFollows",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        FollowedUserId = c.Int(nullable: false),
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId)
                .ForeignKey("dbo.Users", t => t.FollowedUserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.FollowedUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserFollows", "FollowedUserId", "dbo.Users");
            DropForeignKey("dbo.UserFollows", "UserId", "dbo.Users");
            DropForeignKey("dbo.PostLikes", "UserId", "dbo.Users");
            DropForeignKey("dbo.PostLikes", "PostId", "dbo.Posts");
            DropForeignKey("dbo.Comments", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Posts", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "User_UserId", "dbo.Users");
            DropForeignKey("dbo.Posts", "GroupId", "dbo.Groups");
            DropForeignKey("dbo.Comments", "PostId", "dbo.Posts");
            DropIndex("dbo.UserFollows", new[] { "FollowedUserId" });
            DropIndex("dbo.UserFollows", new[] { "UserId" });
            DropIndex("dbo.PostLikes", new[] { "UserId" });
            DropIndex("dbo.PostLikes", new[] { "PostId" });
            DropIndex("dbo.Users", new[] { "User_UserId" });
            DropIndex("dbo.Posts", new[] { "User_UserId" });
            DropIndex("dbo.Posts", new[] { "GroupId" });
            DropIndex("dbo.Comments", new[] { "User_UserId" });
            DropIndex("dbo.Comments", new[] { "PostId" });
            DropTable("dbo.UserFollows");
            DropTable("dbo.PostLikes");
            DropTable("dbo.Users");
            DropTable("dbo.Groups");
            DropTable("dbo.Posts");
            DropTable("dbo.Comments");
        }
    }
}
