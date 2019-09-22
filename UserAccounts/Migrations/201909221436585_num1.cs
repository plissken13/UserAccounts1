namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class num1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CampaignModels", "CreatedOn");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CampaignModels", "CreatedOn", c => c.DateTime(nullable: false));
        }
    }
}
