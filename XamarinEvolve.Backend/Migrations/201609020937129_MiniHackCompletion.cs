namespace XamarinEvolve.Backend.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MiniHackCompletion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MiniHacks", "Score", c => c.Int(nullable: true));
            AddColumn("dbo.MiniHacks", "Category", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MiniHacks", "Category");
            DropColumn("dbo.MiniHacks", "Score");
        }
    }
}
