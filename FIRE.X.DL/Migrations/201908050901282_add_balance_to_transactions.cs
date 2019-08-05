namespace FIRE.X.DL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_balance_to_transactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "Balance", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "Balance");
        }
    }
}
