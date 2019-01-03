namespace Whois.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PesquisaWhois2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PesquisaWhois", "NameServers", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PesquisaWhois", "NameServers");
        }
    }
}
