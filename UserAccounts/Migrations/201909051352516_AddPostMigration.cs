namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        Campaign_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CampaignModels", t => t.Campaign_Id)
                .Index(t => t.Campaign_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "Campaign_Id", "dbo.CampaignModels");
            DropIndex("dbo.Posts", new[] { "Campaign_Id" });
            DropTable("dbo.Posts");
        }
    }
}
