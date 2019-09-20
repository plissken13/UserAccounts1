namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class namename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignModels", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignModels", "ImageUrl");
        }
    }
}
