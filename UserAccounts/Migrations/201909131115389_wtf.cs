namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class wtf : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CampaignModels", "UserViewModel_Id", "dbo.UserViewModels");
            DropIndex("dbo.CampaignModels", new[] { "UserViewModel_Id" });
            DropColumn("dbo.CampaignModels", "UserViewModel_Id");
            DropTable("dbo.UserViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserViewModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        Status = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.CampaignModels", "UserViewModel_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.CampaignModels", "UserViewModel_Id");
            AddForeignKey("dbo.CampaignModels", "UserViewModel_Id", "dbo.UserViewModels", "Id");
        }
    }
}
