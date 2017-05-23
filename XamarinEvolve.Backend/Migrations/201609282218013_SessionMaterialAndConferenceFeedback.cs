namespace XamarinEvolve.Backend.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class SessionMaterialAndConferenceFeedback : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConferenceFeedbacks",
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
                        UserId = c.String(),
                        Question1 = c.Int(nullable: false),
                        Question2 = c.Int(nullable: false),
                        Question3 = c.Int(nullable: false),
                        Question4 = c.Int(nullable: false),
                        Question5 = c.Int(nullable: false),
                        Question6 = c.Int(nullable: false),
                        Question7 = c.Int(nullable: false),
                        Question8 = c.Int(nullable: false),
                        Question9 = c.Int(nullable: false),
                        Question10 = c.Int(nullable: false),
                        RemoteId = c.String(),
                        Version = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion",
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Version")
                                },
                            }),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "CreatedAt")
                                },
                            }),
                        UpdatedAt = c.DateTimeOffset(precision: 7,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "UpdatedAt")
                                },
                            }),
                        Deleted = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "ServiceTableColumn",
                                    new AnnotationValues(oldValue: null, newValue: "Deleted")
                                },
                            }),
                    })
                .PrimaryKey(t => t.Id, clustered: false)
                .Index(t => t.CreatedAt, clustered: true);
            
            AddColumn("dbo.Sessions", "PresentationUrl", c => c.String(nullable:true));
            AddColumn("dbo.Sessions", "VideoUrl", c => c.String(nullable:true));
        }
        
        public override void Down()
        {
            DropIndex("dbo.ConferenceFeedbacks", new[] { "CreatedAt" });
            DropColumn("dbo.Sessions", "VideoUrl");
            DropColumn("dbo.Sessions", "PresentationUrl");
            DropTable("dbo.ConferenceFeedbacks",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "CreatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "CreatedAt" },
                        }
                    },
                    {
                        "Deleted",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Deleted" },
                        }
                    },
                    {
                        "Id",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Id" },
                        }
                    },
                    {
                        "UpdatedAt",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "UpdatedAt" },
                        }
                    },
                    {
                        "Version",
                        new Dictionary<string, object>
                        {
                            { "ServiceTableColumn", "Version" },
                        }
                    },
                });
        }
    }
}
