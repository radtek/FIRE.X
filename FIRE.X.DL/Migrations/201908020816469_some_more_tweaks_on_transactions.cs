namespace FIRE.X.DL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class some_more_tweaks_on_transactions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "LoanId", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Transactions", "Date", c => c.DateTime());
            DropColumn("dbo.Transactions", "LoadId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transactions", "LoadId", c => c.String(maxLength: 4000));
            AlterColumn("dbo.Transactions", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Transactions", "LoanId");
        }
    }
}
