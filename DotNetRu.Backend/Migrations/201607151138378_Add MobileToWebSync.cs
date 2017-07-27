namespace XamarinEvolve.Backend.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;

    public partial class AddMobileToWebSync : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MobileToWebSyncs",
                c => new
                {
                    Id = c.String(nullable: false, maxLength: 128,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                {
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Id")
                                },
                            }),
                    UserId = c.String(nullable: false),
                    TempCode = c.String(nullable: false, maxLength: 5),
                    Expires = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .Index(t => t.TempCode, unique: true);
        }

        public override void Down()
        {
            DropTable("dbo.MobileToWebSyncs");
        }
    }
}
