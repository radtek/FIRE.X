namespace FIRE.X.DL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class some_more_tweaks_2 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Transactions", new[] { "Source", "TransactionId" }, unique: true, name: "IX_UniqueTransaction");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Transactions", "IX_UniqueTransaction");
        }
    }
}
