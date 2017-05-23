namespace XamarinEvolve.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnrichConferenceFeedback : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ConferenceFeedbacks", "DeviceOS", c => c.String(nullable: true));
            AddColumn("dbo.ConferenceFeedbacks", "AppVersion", c => c.String(nullable: true));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ConferenceFeedbacks", "AppVersion");
            DropColumn("dbo.ConferenceFeedbacks", "DeviceOS");
        }
    }
}
