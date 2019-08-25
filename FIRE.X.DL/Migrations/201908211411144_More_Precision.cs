namespace FIRE.X.DL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class More_Precision : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transactions", "Amount", c => c.Decimal(nullable: false, precision: 16, scale: 9));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Transactions", "Amount", c => c.Decimal(nullable: false, storeType: "money"));
        }
    }
}
