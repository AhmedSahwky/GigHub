namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFollowing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Follwoings",
                c => new
                    {
                        FollowerId = c.String(nullable: false, maxLength: 128),
                        FolloweeId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.FollowerId, t.FolloweeId })
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerId)
                .ForeignKey("dbo.AspNetUsers", t => t.FolloweeId)
                .Index(t => t.FollowerId)
                .Index(t => t.FolloweeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Follwoings", "FolloweeId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Follwoings", "FollowerId", "dbo.AspNetUsers");
            DropIndex("dbo.Follwoings", new[] { "FolloweeId" });
            DropIndex("dbo.Follwoings", new[] { "FollowerId" });
            DropTable("dbo.Follwoings");
        }
    }
}
