namespace Koombu.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4Migration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "GroupId", "dbo.Groups");
            DropIndex("dbo.Posts", new[] { "GroupId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Posts", "GroupId");
            AddForeignKey("dbo.Posts", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
    }
}
