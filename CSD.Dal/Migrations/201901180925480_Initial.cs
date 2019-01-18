namespace CSD.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LicensePlate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StatsPerDays",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DriveTimePerDay = c.Int(nullable: false),
                        PenaltyCountPerDay = c.Int(nullable: false),
                        KilometersPerDay = c.Int(nullable: false),
                        AccidentsPerDay = c.Int(nullable: false),
                        DayCount = c.Int(nullable: false),
                        DriverId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Drivers", t => t.DriverId, cascadeDelete: true)
                .Index(t => t.DriverId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StatsPerDays", "DriverId", "dbo.Drivers");
            DropIndex("dbo.StatsPerDays", new[] { "DriverId" });
            DropTable("dbo.StatsPerDays");
            DropTable("dbo.Drivers");
        }
    }
}
