namespace XamarinEvolve.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSessionLevel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sessions", "Level", c => c.String(maxLength: 3, defaultValue: "200"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sessions", "Level");
        }
    }
}
