namespace XamarinEvolve.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLinkedInToSponsor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sponsors", "FacebookProfileName", c => c.String());
            AddColumn("dbo.Sponsors", "LinkedInUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sponsors", "LinkedInUrl");
            DropColumn("dbo.Sponsors", "FacebookProfileName");
        }
    }
}
