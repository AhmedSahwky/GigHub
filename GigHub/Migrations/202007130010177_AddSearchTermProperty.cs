namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSearchTermProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Gigs", "SearchTerm", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Gigs", "SearchTerm");
        }
    }
}
