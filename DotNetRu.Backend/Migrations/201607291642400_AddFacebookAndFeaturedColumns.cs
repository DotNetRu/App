namespace XamarinEvolve.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFacebookAndFeaturedColumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Speakers", "FacebookProfileName", c => c.String(nullable: true));
            AddColumn("dbo.Speakers", "IsFeatured", c => c.Boolean(nullable: false, defaultValue: false));
        }

        public override void Down()
        {
            DropColumn("dbo.Speakers", "IsFeatured");
            DropColumn("dbo.Speakers", "FacebookProfileName");
        }
    }
}
