namespace Urok1_povtor_metanit.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Books", "Author");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Books", "Author", c => c.String());
        }
    }
}
