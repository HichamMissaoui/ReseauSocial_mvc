namespace Koombu.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5Migration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GroupMembers", "UserFirstName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GroupMembers", "UserFirstName");
        }
    }
}
