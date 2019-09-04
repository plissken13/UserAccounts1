namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.CreateCampaignViewModels");
        }
        
        public override void Down()
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
            
        }
    }
}
