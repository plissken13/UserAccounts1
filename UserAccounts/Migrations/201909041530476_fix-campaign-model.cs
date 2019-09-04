namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixcampaignmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreateCampaignViewModels",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 100),
                        Sum = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            AddColumn("dbo.CampaignModels", "Description", c => c.String());
            AlterColumn("dbo.CampaignModels", "OwnerId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CampaignModels", "OwnerId", c => c.Int(nullable: false));
            DropColumn("dbo.CampaignModels", "Description");
            DropTable("dbo.CreateCampaignViewModels");
        }
    }
}
