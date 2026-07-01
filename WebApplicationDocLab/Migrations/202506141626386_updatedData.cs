namespace WebApplicationDocLab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedData : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Medicines", "ImageUrl", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Medicines", "ImageUrl", c => c.String(nullable: false));
        }
    }
}
