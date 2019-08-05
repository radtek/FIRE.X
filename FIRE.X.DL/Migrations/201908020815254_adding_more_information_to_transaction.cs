namespace FIRE.X.DL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adding_more_information_to_transaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Transactions", "LoadId", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Transactions", "LoadId");
        }
    }
}
