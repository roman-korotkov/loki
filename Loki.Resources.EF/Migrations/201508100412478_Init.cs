namespace Loki.Resources.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "loki.Resource",
                c => new
                    {
                        Culture = c.Int(nullable: false),
                        Id = c.Long(nullable: false, identity: true),
                        Set = c.String(nullable: false, maxLength: 256),
                        Key = c.String(nullable: false),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.Culture, t.Id });
            
        }
        
        public override void Down()
        {
            DropTable("loki.Resource");
        }
    }
}
