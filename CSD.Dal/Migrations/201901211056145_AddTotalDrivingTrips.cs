namespace CSD.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTotalDrivingTrips : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StatsPerDays", "DrivingTrips", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StatsPerDays", "DrivingTrips");
        }
    }
}
