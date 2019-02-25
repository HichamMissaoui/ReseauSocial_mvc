namespace Koombu.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TroMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupMembers", "UserLastName", c => c.String());
            AddColumn("dbo.GroupMembers", "UserPicture", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupMembers", "UserPicture");
            DropColumn("dbo.GroupMembers", "UserLastName");
        }
    }
}
