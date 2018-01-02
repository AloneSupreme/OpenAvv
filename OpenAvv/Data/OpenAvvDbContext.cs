using Microsoft.EntityFrameworkCore;
using OpenAvv.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenAvv.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OpenAvv.Data.Models.CommentSystem;
using OpenAvv.Data.Models.LikeSystem;
using OpenAvv.Data.Models.ImageSystem;

namespace OpenAvv.Data
{
    public class OpenAvvDbContext : IdentityDbContext<OpenAvvUser, OpenAvvRole, string>
    {
        public OpenAvvDbContext(DbContextOptions<OpenAvvDbContext> options) : base(options)
        {
        }

        public DbSet<Story> Stories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reply> Replies { get; set; }
        private new DbSet<OpenAvvUser> Users { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }
        public DbSet<ReplyLike> ReplyLikes { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Story>().ToTable("Story");
            modelBuilder.Entity<Comment>().ToTable("Comment");
            modelBuilder.Entity<Reply>().ToTable("Reply");
            modelBuilder.Entity<Image>().ToTable("Image");
        }

        

        //public DbSet<OpenAvv.ViewModels.CommentViewModel> CommentViewModel { get; set; }

        //public DbSet<OpenAvv.ViewModels.StoryVM> StoryVM { get; set; }
    }
}
