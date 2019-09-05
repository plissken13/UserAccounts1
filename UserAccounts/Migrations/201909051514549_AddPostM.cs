namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostM : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Posts", newName: "PostModels");
            CreateTable(
                "dbo.WritePostViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Content = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WritePostViewModels");
            RenameTable(name: "dbo.PostModels", newName: "Posts");
        }
    }
}
