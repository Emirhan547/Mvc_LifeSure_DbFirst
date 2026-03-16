using Mvc_LifeSure_DbFirst.Data.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Mvc_LifeSure_DbFirst.Data.Context
{
    public class AppDbContext : DbContext
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}