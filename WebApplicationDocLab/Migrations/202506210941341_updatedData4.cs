namespace WebApplicationDocLab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedData4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookingAppointments", "ActualTime", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.BookingAppointments", "SerialNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.BookingAppointments", "BookType", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BookingAppointments", "BookType", c => c.String(nullable: false));
            DropColumn("dbo.BookingAppointments", "SerialNumber");
            DropColumn("dbo.BookingAppointments", "ActualTime");
        }
    }
}
