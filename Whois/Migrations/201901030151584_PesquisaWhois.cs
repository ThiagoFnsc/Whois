namespace Whois.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PesquisaWhois : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PesquisaWhois",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Dominio = c.String(),
                        DataPesquisa = c.DateTime(nullable: false),
                        Registrado = c.Boolean(nullable: false),
                        DataRegistro = c.DateTime(nullable: false),
                        UltimaAlteracao = c.DateTime(nullable: false),
                        ExpiraEm = c.DateTime(nullable: false),
                        JSON = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PesquisaWhois");
        }
    }
}
