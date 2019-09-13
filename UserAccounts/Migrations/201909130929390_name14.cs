namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class name14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CampaignModels", "UserViewModel_Id", "dbo.UserViewModels");
            DropIndex("dbo.CampaignModels", new[] { "UserViewModel_Id" });
            DropColumn("dbo.CampaignModels", "UserViewModel_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CampaignModels", "UserViewModel_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.CampaignModels", "UserViewModel_Id");
            AddForeignKey("dbo.CampaignModels", "UserViewModel_Id", "dbo.UserViewModels", "Id");
        }
    }
}
