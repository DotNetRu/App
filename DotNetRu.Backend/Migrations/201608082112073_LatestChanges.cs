namespace XamarinEvolve.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LatestChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Speakers", "IsFeatured", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Speakers", "IsFeatured", c => c.Boolean(nullable: false));
        }
    }
}
