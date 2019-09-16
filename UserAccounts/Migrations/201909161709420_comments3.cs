namespace UserAccounts.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class comments3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CommentsModels", "AuthorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CommentsModels", "AuthorId", c => c.String());
        }
    }
}
