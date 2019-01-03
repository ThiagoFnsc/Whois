namespace Whois.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PesquisaWhois1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PesquisaWhois", "DataRegistro", c => c.DateTime());
            AlterColumn("dbo.PesquisaWhois", "UltimaAlteracao", c => c.DateTime());
            AlterColumn("dbo.PesquisaWhois", "ExpiraEm", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PesquisaWhois", "ExpiraEm", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PesquisaWhois", "UltimaAlteracao", c => c.DateTime(nullable: false));
            AlterColumn("dbo.PesquisaWhois", "DataRegistro", c => c.DateTime(nullable: false));
        }
    }
}
