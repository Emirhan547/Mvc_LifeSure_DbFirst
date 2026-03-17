using Microsoft.AspNet.Identity.EntityFramework;
using Mvc_LifeSure_DbFirst.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Data.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, string, IdentityUserLogin, IdentityUserRole, IdentityUserClaim>
    {
        public AppDbContext()
            : base("name=AppDbContext")
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<About> Abouts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Data.Entities.Services> Services { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }

        public DbSet<InsurancePackage> InsurancePackages { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<PolicySaleData> PolicySaleDatas { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Identity için bu şart!

            // Policy - AppUser ilişkisi (Müşteri olarak)
            modelBuilder.Entity<Policy>()
                .HasRequired(p => p.User)
                .WithMany(u => u.Policies)
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(false);

            // AdminLog - AppUser ilişkisi (Admin olarak)
            modelBuilder.Entity<AdminLog>()
                .HasRequired(l => l.User)
                .WithMany(u => u.AdminLogs)
                .HasForeignKey(l => l.UserId)
                .WillCascadeOnDelete(false);

            // Policy - InsurancePackage ilişkisi
            modelBuilder.Entity<Policy>()
                .HasRequired(p => p.InsurancePackage)
                .WithMany(ip => ip.Policies)
                .HasForeignKey(p => p.InsurancePackageId)
                .WillCascadeOnDelete(false);

            // Tablo isimlerini düzenleyelim
            modelBuilder.Entity<AppUser>().ToTable("Users");
            modelBuilder.Entity<AppRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");

            // Index'ler
            modelBuilder.Entity<Policy>()
                .Property(p => p.PolicyNumber)
                .IsRequired();

            modelBuilder.Entity<InsurancePackage>()
                .Property(p => p.PackageName)
                .IsRequired();
        }

        // Identity için gerekli
        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}