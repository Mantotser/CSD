namespace CSD.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeintPropertytoDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StatsPerDays", "DriveTimePerDay", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StatsPerDays", "DriveTimePerDay", c => c.Int(nullable: false));
        }
    }
}
