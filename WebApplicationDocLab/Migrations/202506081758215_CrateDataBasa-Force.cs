namespace WebApplicationDocLab.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CrateDataBasaForce : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User_Info", "Department", c => c.String(maxLength: 100));
        }

        public override void Down()
        {
            AlterColumn("dbo.User_Info", "Department", c => c.String(maxLength: 50));
        }
    }
}
