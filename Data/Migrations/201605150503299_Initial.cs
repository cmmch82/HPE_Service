namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Championships",
                c => new
                    {
                        ChampionshipId = c.Int(nullable: false, identity: true),
                        FirstPlace_MoveId = c.Int(),
                        SecondPlace_MoveId = c.Int(),
                    })
                .PrimaryKey(t => t.ChampionshipId)
                .ForeignKey("dbo.Moves", t => t.FirstPlace_MoveId)
                .ForeignKey("dbo.Moves", t => t.SecondPlace_MoveId)
                .Index(t => t.FirstPlace_MoveId)
                .Index(t => t.SecondPlace_MoveId);
            
            CreateTable(
                "dbo.Moves",
                c => new
                    {
                        MoveId = c.Int(nullable: false, identity: true),
                        PlayerId = c.Int(nullable: false),
                        PlayerStrategy = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MoveId)
                .ForeignKey("dbo.Users", t => t.PlayerId, cascadeDelete: true)
                .Index(t => t.PlayerId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        UserScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Championships", "SecondPlace_MoveId", "dbo.Moves");
            DropForeignKey("dbo.Championships", "FirstPlace_MoveId", "dbo.Moves");
            DropForeignKey("dbo.Moves", "PlayerId", "dbo.Users");
            DropIndex("dbo.Moves", new[] { "PlayerId" });
            DropIndex("dbo.Championships", new[] { "SecondPlace_MoveId" });
            DropIndex("dbo.Championships", new[] { "FirstPlace_MoveId" });
            DropTable("dbo.Users");
            DropTable("dbo.Moves");
            DropTable("dbo.Championships");
        }
    }
}
