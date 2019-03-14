namespace Urok1_povtor_metanit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Authors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Country = c.String(),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        PurchaseId = c.Int(nullable: false, identity: true),
                        Person = c.String(),
                        Adress = c.String(),
                        BookId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseId);
            
            CreateTable(
                "dbo.AuthorBook",
                c => new
                    {
                        AuthorId = c.Int(nullable: false),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AuthorId, t.BookId })
                .ForeignKey("dbo.Authors", t => t.AuthorId, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.AuthorId)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuthorBook", "BookId", "dbo.Books");
            DropForeignKey("dbo.AuthorBook", "AuthorId", "dbo.Authors");
            DropIndex("dbo.AuthorBook", new[] { "BookId" });
            DropIndex("dbo.AuthorBook", new[] { "AuthorId" });
            DropTable("dbo.AuthorBook");
            DropTable("dbo.Purchases");
            DropTable("dbo.Books");
            DropTable("dbo.Authors");
        }
    }
}
