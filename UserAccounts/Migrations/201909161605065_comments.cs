namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommentsModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        CampaignId = c.Int(nullable: false),
                        AuthorId = c.String(),
                        AuthorName = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignModels", t => t.CampaignId, cascadeDelete: true)
                .Index(t => t.CampaignId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CommentsModels", "CampaignId", "dbo.CampaignModels");
            DropIndex("dbo.CommentsModels", new[] { "CampaignId" });
            DropTable("dbo.CommentsModels");
        }
    }
}
