namespace WebApplicationDocLab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateBookingsTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Medicines", "Prescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Doctor_Details", "DoctorId", "dbo.User_Info");
            DropForeignKey("dbo.Prescriptions", "DoctorId", "dbo.Doctor_Details");
            DropForeignKey("dbo.Prescriptions", "PatientId", "dbo.User_Info");
            DropIndex("dbo.Doctor_Details", new[] { "DoctorId" });
            DropIndex("dbo.Medicines", new[] { "Prescription_Id" });
            DropIndex("dbo.Prescriptions", new[] { "PatientId" });
            DropIndex("dbo.Prescriptions", new[] { "DoctorId" });
            CreateTable(
                "dbo.BookingAppointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        Booking_Date = c.DateTime(nullable: false),
                        BookType = c.String(nullable: false),
                        Created_at = c.DateTime(nullable: false),
                        User_Info_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User_Info", t => t.User_Info_Id)
                .Index(t => t.User_Info_Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentMethod = c.String(),
                        TransactionId = c.String(),
                        Status = c.String(),
                        Created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Medical_History",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        Record_Type = c.String(),
                        Description = c.String(),
                        Created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Doctor_Details", "User_Info_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Medicines", "GenericName", c => c.String(nullable: false));
            AddColumn("dbo.Medicines", "BrandName", c => c.String());
            AddColumn("dbo.Prescriptions", "Doctor_Details_Id", c => c.Int());
            AddColumn("dbo.Prescriptions", "Medicine_Id", c => c.Int());
            AddColumn("dbo.Prescriptions", "User_Info_Id", c => c.Int());
            CreateIndex("dbo.Doctor_Details", "User_Info_Id");
            CreateIndex("dbo.Prescriptions", "Doctor_Details_Id");
            CreateIndex("dbo.Prescriptions", "Medicine_Id");
            CreateIndex("dbo.Prescriptions", "User_Info_Id");
            AddForeignKey("dbo.Prescriptions", "Medicine_Id", "dbo.Medicines", "Id");
            AddForeignKey("dbo.Doctor_Details", "User_Info_Id", "dbo.User_Info", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Prescriptions", "Doctor_Details_Id", "dbo.Doctor_Details", "Id");
            AddForeignKey("dbo.Prescriptions", "User_Info_Id", "dbo.User_Info", "Id");
            DropColumn("dbo.Medicines", "Prescription_Id");
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.BookIngs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PictureId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        Booking_Data = c.DateTime(nullable: false),
                        BookType = c.String(nullable: false),
                        Created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Medicines", "Prescription_Id", c => c.Int());
            DropForeignKey("dbo.Prescriptions", "User_Info_Id", "dbo.User_Info");
            DropForeignKey("dbo.Prescriptions", "Doctor_Details_Id", "dbo.Doctor_Details");
            DropForeignKey("dbo.Doctor_Details", "User_Info_Id", "dbo.User_Info");
            DropForeignKey("dbo.Prescriptions", "Medicine_Id", "dbo.Medicines");
            DropForeignKey("dbo.BookingAppointments", "User_Info_Id", "dbo.User_Info");
            DropIndex("dbo.Prescriptions", new[] { "User_Info_Id" });
            DropIndex("dbo.Prescriptions", new[] { "Medicine_Id" });
            DropIndex("dbo.Prescriptions", new[] { "Doctor_Details_Id" });
            DropIndex("dbo.Doctor_Details", new[] { "User_Info_Id" });
            DropIndex("dbo.BookingAppointments", new[] { "User_Info_Id" });
            DropColumn("dbo.Prescriptions", "User_Info_Id");
            DropColumn("dbo.Prescriptions", "Medicine_Id");
            DropColumn("dbo.Prescriptions", "Doctor_Details_Id");
            DropColumn("dbo.Medicines", "BrandName");
            DropColumn("dbo.Medicines", "GenericName");
            DropColumn("dbo.Doctor_Details", "User_Info_Id");
            DropTable("dbo.Medical_History");
            DropTable("dbo.Payments");
            DropTable("dbo.BookingAppointments");
            CreateIndex("dbo.Prescriptions", "DoctorId");
            CreateIndex("dbo.Prescriptions", "PatientId");
            CreateIndex("dbo.Medicines", "Prescription_Id");
            CreateIndex("dbo.Doctor_Details", "DoctorId");
            AddForeignKey("dbo.Prescriptions", "PatientId", "dbo.User_Info", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Prescriptions", "DoctorId", "dbo.Doctor_Details", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Doctor_Details", "DoctorId", "dbo.User_Info", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Medicines", "Prescription_Id", "dbo.Prescriptions", "Id");
        }
    }
}
