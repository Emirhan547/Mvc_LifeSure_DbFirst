namespace Mvc_LifeSure_DbFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
           
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.AdminLogs", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.Policies", "UserId", "dbo.Users");
            DropForeignKey("dbo.Policies", "InsurancePackageId", "dbo.InsurancePackages");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Policies", new[] { "InsurancePackageId" });
            DropIndex("dbo.Policies", new[] { "UserId" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.AdminLogs", new[] { "UserId" });
            DropTable("dbo.Testimonials");
            DropTable("dbo.Teams");
            DropTable("dbo.Sliders");
            DropTable("dbo.Services");
            DropTable("dbo.Roles");
            DropTable("dbo.PolicySaleDatas");
            DropTable("dbo.Features");
            DropTable("dbo.Faqs");
            DropTable("dbo.ContactMessages");
            DropTable("dbo.Blogs");
            DropTable("dbo.UserRoles");
            DropTable("dbo.InsurancePackages");
            DropTable("dbo.Policies");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.AdminLogs");
            DropTable("dbo.Abouts");
        }
    }
}
