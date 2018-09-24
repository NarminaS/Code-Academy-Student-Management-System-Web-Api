using CodeAcademy.CoreWebApi.DataAccessLayer.Entities;
using CodeAcademy.CoreWebApi.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity
{
    public class AppIdentityDbContext:IdentityDbContext<AppIdentityUser,AppIdentityRole,string>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options):base(options) 
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>().HasDiscriminator<string>("PostType");
            builder.Entity<AppIdentityUser>().HasDiscriminator<string>("UserType");

            builder.Entity<PostTag>().HasKey(t => new { t.TagId, t.PostId });

            builder.Entity<PostTag>().HasOne(t => t.Tag)
                                     .WithMany(tp => tp.PostTags)
                                     .HasForeignKey(ti => ti.TagId);

            builder.Entity<PostTag>().HasOne(p => p.Post)
                         .WithMany(tp => tp.PostTags)
                         .HasForeignKey(pi => pi.PostId);
        }


        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<LessonHour> LessonHours { get; set; }
        public DbSet<LessonStatus> LessonStatuses { get; set; }
        public DbSet<LeftNavItem> LeftNavItems { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Link> Links { get; set; }

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<TeacherGroup> TeacherGroups { get; set; }
        public DbSet<MentorGroup> MentorGroups { get; set; }


        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Faculty> Faculties { get; set; }



    }
}
