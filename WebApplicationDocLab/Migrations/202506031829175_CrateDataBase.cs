namespace WebApplicationDocLab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CrateDataBase : DbMigration
    {
        public override void Up()
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
            
            CreateTable(
                "dbo.Doctor_Details",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DoctorId = c.Int(nullable: false),
                        Day = c.String(nullable: false),
                        TimeStart = c.String(nullable: false),
                        TimeEnd = c.String(nullable: false),
                        ConsultingFees = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User_Info", t => t.DoctorId, cascadeDelete: true)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.User_Info",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        DateOfBirth = c.DateTime(nullable: false),
                        Title = c.String(),
                        Address = c.String(nullable: false, maxLength: 200),
                        Gender = c.Int(nullable: false),
                        Phone = c.String(nullable: false),
                        BloodGroup = c.Int(nullable: false),
                        NID = c.String(maxLength: 20),
                        RegistrationNo = c.String(maxLength: 20),
                        Department = c.String(maxLength: 50),
                        DocType = c.String(),
                        Email = c.String(nullable: false, maxLength: 100),
                        Password = c.String(nullable: false, maxLength: 100),
                        ConfirmPassword = c.String(nullable: false, maxLength: 100),
                        Image = c.String(maxLength: 200),
                        Status = c.Int(nullable: false),
                        UserType = c.Int(nullable: false),
                        createdBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        DoctorTypes_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctor_Type", t => t.DoctorTypes_Id)
                .Index(t => t.DoctorTypes_Id);
            
            CreateTable(
                "dbo.Doctor_Type",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Medicine_List",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicinID = c.String(),
                        PrescriptionId = c.Int(nullable: false),
                        Created_at = c.DateTime(nullable: false),
                        Medicine_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medicines", t => t.Medicine_Id)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId)
                .Index(t => t.Medicine_Id);
            
            CreateTable(
                "dbo.Medicines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicineName = c.String(nullable: false, maxLength: 100),
                        Description = c.String(nullable: false, maxLength: 500),
                        MedicineCategory = c.String(nullable: false, maxLength: 50),
                        ImageUrl = c.String(nullable: false),
                        Category = c.String(nullable: false, maxLength: 50),
                        Created_at = c.DateTime(nullable: false),
                        Prescription_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.Prescription_Id)
                .Index(t => t.Prescription_Id);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientId = c.Int(nullable: false),
                        DoctorId = c.Int(nullable: false),
                        PrescriptionDate = c.DateTime(nullable: false),
                        Notes = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Doctor_Details", t => t.DoctorId)
                .ForeignKey("dbo.User_Info", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId)
                .Index(t => t.DoctorId);
            
            CreateTable(
                "dbo.Test_List",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestId = c.Int(nullable: false),
                        PrescriptionId = c.Int(nullable: false),
                        Created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .ForeignKey("dbo.Tests", t => t.TestId, cascadeDelete: true)
                .Index(t => t.TestId)
                .Index(t => t.PrescriptionId);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TestName = c.String(nullable: false, maxLength: 100),
                        Category = c.String(nullable: false, maxLength: 50),
                        Created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Test_List", "TestId", "dbo.Tests");
            DropForeignKey("dbo.Test_List", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.Medicine_List", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.Prescriptions", "PatientId", "dbo.User_Info");
            DropForeignKey("dbo.Medicines", "Prescription_Id", "dbo.Prescriptions");
            DropForeignKey("dbo.Prescriptions", "DoctorId", "dbo.Doctor_Details");
            DropForeignKey("dbo.Medicine_List", "Medicine_Id", "dbo.Medicines");
            DropForeignKey("dbo.Doctor_Details", "DoctorId", "dbo.User_Info");
            DropForeignKey("dbo.User_Info", "DoctorTypes_Id", "dbo.Doctor_Type");
            DropIndex("dbo.Test_List", new[] { "PrescriptionId" });
            DropIndex("dbo.Test_List", new[] { "TestId" });
            DropIndex("dbo.Prescriptions", new[] { "DoctorId" });
            DropIndex("dbo.Prescriptions", new[] { "PatientId" });
            DropIndex("dbo.Medicines", new[] { "Prescription_Id" });
            DropIndex("dbo.Medicine_List", new[] { "Medicine_Id" });
            DropIndex("dbo.Medicine_List", new[] { "PrescriptionId" });
            DropIndex("dbo.User_Info", new[] { "DoctorTypes_Id" });
            DropIndex("dbo.Doctor_Details", new[] { "DoctorId" });
            DropTable("dbo.Tests");
            DropTable("dbo.Test_List");
            DropTable("dbo.Prescriptions");
            DropTable("dbo.Medicines");
            DropTable("dbo.Medicine_List");
            DropTable("dbo.Doctor_Type");
            DropTable("dbo.User_Info");
            DropTable("dbo.Doctor_Details");
            DropTable("dbo.BookIngs");
        }
    }
}
