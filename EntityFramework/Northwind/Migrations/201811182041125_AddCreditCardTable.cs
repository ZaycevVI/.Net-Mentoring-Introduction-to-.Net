namespace Northwind.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreditCardTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Employees", "ReportsTo", "dbo.Employees");
            DropForeignKey("dbo.Orders", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.EmployeeTerritories", "EmployeeID", "dbo.Employees");
            DropPrimaryKey("dbo.Employees");
            CreateTable(
                "dbo.CreditCards",
                c => new
                    {
                        CreditCardID = c.Int(nullable: false, identity: true),
                        ExpireDate = c.DateTime(),
                        CardHolder = c.String(),
                    })
                .PrimaryKey(t => t.CreditCardID);
            
            AlterColumn("dbo.Employees", "EmployeeID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Employees", "EmployeeID");
            CreateIndex("dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.Employees", "EmployeeID", "dbo.CreditCards", "CreditCardID");
            AddForeignKey("dbo.Employees", "ReportsTo", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.Orders", "EmployeeID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.EmployeeTerritories", "EmployeeID", "dbo.Employees", "EmployeeID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EmployeeTerritories", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Orders", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.Employees", "ReportsTo", "dbo.Employees");
            DropForeignKey("dbo.Employees", "EmployeeID", "dbo.CreditCards");
            DropIndex("dbo.Employees", new[] { "EmployeeID" });
            DropPrimaryKey("dbo.Employees");
            AlterColumn("dbo.Employees", "EmployeeID", c => c.Int(nullable: false, identity: true));
            DropTable("dbo.CreditCards");
            AddPrimaryKey("dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.EmployeeTerritories", "EmployeeID", "dbo.Employees", "EmployeeID", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "EmployeeID", "dbo.Employees", "EmployeeID");
            AddForeignKey("dbo.Employees", "ReportsTo", "dbo.Employees", "EmployeeID");
        }
    }
}
