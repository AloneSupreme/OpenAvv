using Microsoft.EntityFrameworkCore;
using OpenAvv.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAvv.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace OpenAvv.Data
{
    public class OpenAvvDbContext : IdentityDbContext<OpenAvvUser, OpenAvvRole, string>
    {
        public OpenAvvDbContext(DbContextOptions<OpenAvvDbContext> options) : base(options)
        {
        }

        public DbSet<Story> Stories { get; set; }

        private new DbSet<OpenAvvUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Story>().ToTable("Story");
        }

        //public DbSet<OpenAvv.ViewModels.StoryVM> StoryVM { get; set; }
    }
}
