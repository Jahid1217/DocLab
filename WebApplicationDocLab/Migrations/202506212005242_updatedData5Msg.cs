namespace WebApplicationDocLab.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedData5Msg : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    SenderId = c.Int(nullable: false),
                    ReceiverId = c.Int(nullable: false),
                    Content = c.String(nullable: false, maxLength: 1000),
                    SentAt = c.DateTime(nullable: false),
                })
            .PrimaryKey(t => t.Id)
            .ForeignKey("dbo.User_Info", t => t.SenderId, cascadeDelete: true) // Keep cascade here
            .ForeignKey("dbo.User_Info", t => t.ReceiverId, cascadeDelete: false) // Disable cascade here
            .Index(t => t.SenderId)
            .Index(t => t.ReceiverId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.User_Info");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.User_Info");
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropTable("dbo.Messages");
        }

    }
}
