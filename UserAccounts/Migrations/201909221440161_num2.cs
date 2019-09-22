namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class num2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CampaignModels", "CreatedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CampaignModels", "CreatedOn");
        }
    }
}
