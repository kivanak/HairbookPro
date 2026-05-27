namespace HairbookPro.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitalHairBookPro : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppointmentLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AppointmentId = c.Int(nullable: false),
                        Action = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ServiceId = c.Int(nullable: false),
                        StylistId = c.Int(nullable: false),
                        AppointmentDate = c.DateTime(nullable: false),
                        Status = c.String(nullable: false, maxLength: 30),
                        Note = c.String(maxLength: 500),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Services", t => t.ServiceId, cascadeDelete: true)
                .ForeignKey("dbo.Stylists", t => t.StylistId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ServiceId)
                .Index(t => t.StylistId);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 120),
                        Description = c.String(maxLength: 1000),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DurationMinutes = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ServiceCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServiceCategories", t => t.ServiceCategoryId, cascadeDelete: true)
                .Index(t => t.ServiceCategoryId);
            
            CreateTable(
                "dbo.ServiceCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 80),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Stylists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Specialization = c.String(maxLength: 100),
                        Bio = c.String(maxLength: 1000),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StylistId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(maxLength: 1000),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stylists", t => t.StylistId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.StylistId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.BlogCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 80),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BlogPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 160),
                        Content = c.String(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        BlogCategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogCategories", t => t.BlogCategoryId, cascadeDelete: true)
                .Index(t => t.BlogCategoryId);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        BlogPostId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        Content = c.String(nullable: false, maxLength: 1000),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BlogPosts", t => t.BlogPostId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.BlogPostId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ContactMessages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false, maxLength: 100),
                        Email = c.String(nullable: false, maxLength: 150),
                        Message = c.String(nullable: false, maxLength: 1200),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeletedCommentLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentId = c.Int(nullable: false),
                        BlogPostId = c.Int(nullable: false),
                        UserId = c.String(),
                        Content = c.String(),
                        DeletedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GalleryImages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageUrl = c.String(nullable: false, maxLength: 250),
                        Title = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Comments", "BlogPostId", "dbo.BlogPosts");
            DropForeignKey("dbo.BlogPosts", "BlogCategoryId", "dbo.BlogCategories");
            DropForeignKey("dbo.Appointments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Reviews", "StylistId", "dbo.Stylists");
            DropForeignKey("dbo.Appointments", "StylistId", "dbo.Stylists");
            DropForeignKey("dbo.Services", "ServiceCategoryId", "dbo.ServiceCategories");
            DropForeignKey("dbo.Appointments", "ServiceId", "dbo.Services");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "BlogPostId" });
            DropIndex("dbo.BlogPosts", new[] { "BlogCategoryId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.Reviews", new[] { "StylistId" });
            DropIndex("dbo.Services", new[] { "ServiceCategoryId" });
            DropIndex("dbo.Appointments", new[] { "StylistId" });
            DropIndex("dbo.Appointments", new[] { "ServiceId" });
            DropIndex("dbo.Appointments", new[] { "UserId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.GalleryImages");
            DropTable("dbo.DeletedCommentLogs");
            DropTable("dbo.ContactMessages");
            DropTable("dbo.Comments");
            DropTable("dbo.BlogPosts");
            DropTable("dbo.BlogCategories");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Reviews");
            DropTable("dbo.Stylists");
            DropTable("dbo.ServiceCategories");
            DropTable("dbo.Services");
            DropTable("dbo.Appointments");
            DropTable("dbo.AppointmentLogs");
        }
    }
}
