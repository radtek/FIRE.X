namespace FIRE.X.DL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adding_transactions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.Int(nullable: false),
                        TransactionId = c.String(maxLength: 4000),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Transactions");
        }
    }
}
