namespace WebApplicationDocLab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrateDataBasaForce1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.User_Info", name: "DoctorTypes_Id", newName: "DoctorType_Id");
            RenameIndex(table: "dbo.User_Info", name: "IX_DoctorTypes_Id", newName: "IX_DoctorType_Id");
            AlterColumn("dbo.User_Info", "Status", c => c.String());
            AlterColumn("dbo.User_Info", "UserType", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User_Info", "UserType", c => c.Int(nullable: false));
            AlterColumn("dbo.User_Info", "Status", c => c.Int(nullable: false));
            RenameIndex(table: "dbo.User_Info", name: "IX_DoctorType_Id", newName: "IX_DoctorTypes_Id");
            RenameColumn(table: "dbo.User_Info", name: "DoctorType_Id", newName: "DoctorTypes_Id");
        }
    }
}
