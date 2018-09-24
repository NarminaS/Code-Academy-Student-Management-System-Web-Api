﻿// <auto-generated />
using System;
using CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CodeAcademy.CoreWebApi.Migrations
{
    [DbContext(typeof(AppIdentityDbContext))]
    [Migration("20180923132134_filepublicid")]
    partial class filepublicid
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime>("BirthDate");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("FacultyId");

                    b.Property<byte>("GenderId");

                    b.Property<bool>("IsBlocked");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("LoginToken");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int>("PhotoId");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("Surname");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("UserType")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.HasIndex("GenderId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PhotoId");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("UserType").HasValue("AppIdentityUser");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Certificate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppIdentityUserId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Number");

                    b.HasKey("Id");

                    b.HasIndex("AppIdentityUserId");

                    b.ToTable("Certificates");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppIdentityUserId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<bool>("IsApproved");

                    b.Property<int>("PostId");

                    b.Property<int?>("QuestionId");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("AppIdentityUserId");

                    b.HasIndex("PostId");

                    b.HasIndex("QuestionId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Faculty", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded");

                    b.Property<int>("LessonHour");

                    b.Property<string>("Name");

                    b.Property<int>("PhotoId");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<bool>("IsMain");

                    b.Property<string>("PublicId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded");

                    b.Property<int>("FacultyId");

                    b.Property<DateTime>("LessonEndDate");

                    b.Property<int>("LessonHourId");

                    b.Property<DateTime>("LessonStartDate");

                    b.Property<int>("LessonStatusId");

                    b.Property<string>("Name");

                    b.Property<int>("PhotoId");

                    b.Property<int>("RoomId");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.HasIndex("LessonHourId");

                    b.HasIndex("LessonStatusId");

                    b.HasIndex("PhotoId");

                    b.HasIndex("RoomId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Language", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.LeftNavItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IconClassname");

                    b.Property<bool>("IsVisible");

                    b.Property<string>("Name");

                    b.Property<int>("PhotoId");

                    b.Property<string>("RouterLink");

                    b.HasKey("Id");

                    b.HasIndex("PhotoId");

                    b.ToTable("LeftNavItems");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.LessonHour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("BeginHour");

                    b.Property<byte>("BeginMinute");

                    b.Property<byte>("EndHour");

                    b.Property<byte>("EndMinute");

                    b.Property<bool>("Friday");

                    b.Property<bool>("Monday");

                    b.Property<string>("Name");

                    b.Property<bool>("Saturday");

                    b.Property<bool>("Sunday");

                    b.Property<bool>("Thursday");

                    b.Property<bool>("Tuesday");

                    b.Property<bool>("Wednesday");

                    b.HasKey("Id");

                    b.ToTable("LessonHours");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.LessonStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("LessonStatuses");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppIdentityUserId");

                    b.Property<int?>("CommentId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<int>("PostId");

                    b.HasKey("Id");

                    b.HasIndex("AppIdentityUserId");

                    b.HasIndex("CommentId");

                    b.HasIndex("PostId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.MentorGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppIdentityUserId");

                    b.Property<int>("GroupId");

                    b.Property<string>("MentorId");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("MentorId");

                    b.ToTable("MentorGroups");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppIdentityUserId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<int?>("FacultyId1");

                    b.Property<int>("FacutlyId");

                    b.Property<int>("PhotoId");

                    b.Property<string>("PostType")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AppIdentityUserId");

                    b.HasIndex("FacultyId1");

                    b.ToTable("Posts");

                    b.HasDiscriminator<string>("PostType").HasValue("Post");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.PostTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("PostId");

                    b.Property<int>("TagId");

                    b.HasKey("ID");

                    b.HasIndex("PostId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTags");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<byte>("Capacity");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FacultyId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("FacultyId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.TeacherGroup", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppIdentityUserId");

                    b.Property<DateTime>("DateAdded");

                    b.Property<int>("GroupId");

                    b.Property<string>("TeacherId");

                    b.HasKey("ID");

                    b.HasIndex("GroupId");

                    b.HasIndex("TeacherId");

                    b.ToTable("TeacherGroups");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.Entities.Gender", b =>
                {
                    b.Property<byte>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Genders");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.Entities.Photo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateAdded");

                    b.Property<string>("Description");

                    b.Property<bool>("IsMain");

                    b.Property<string>("PublicId");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Student", b =>
                {
                    b.HasBaseType("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser");

                    b.Property<int?>("CertificateId");

                    b.Property<int>("GroupId");

                    b.Property<bool>("IsMentor");

                    b.Property<int>("LessonStatusId");

                    b.HasIndex("CertificateId");

                    b.HasIndex("GroupId");

                    b.HasIndex("LessonStatusId");

                    b.ToTable("Student");

                    b.HasDiscriminator().HasValue("Student");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Teacher", b =>
                {
                    b.HasBaseType("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser");


                    b.ToTable("Teacher");

                    b.HasDiscriminator().HasValue("Teacher");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Article", b =>
                {
                    b.HasBaseType("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Post");

                    b.Property<string>("HeadText");

                    b.Property<bool>("IsApproved");

                    b.Property<string>("Text");

                    b.HasIndex("PhotoId");

                    b.ToTable("Article");

                    b.HasDiscriminator().HasValue("Article");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Book", b =>
                {
                    b.HasBaseType("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Post");

                    b.Property<string>("Author");

                    b.Property<string>("Description");

                    b.Property<int?>("FacultyId");

                    b.Property<int>("FileId");

                    b.Property<bool>("IsApproved")
                        .HasColumnName("Book_IsApproved");

                    b.Property<int>("LanguageId");

                    b.Property<string>("Name");

                    b.Property<int>("Pages");

                    b.Property<int>("Year");

                    b.HasIndex("FacultyId");

                    b.HasIndex("FileId");

                    b.HasIndex("LanguageId");

                    b.HasIndex("PhotoId")
                        .HasName("IX_Posts_PhotoId1");

                    b.ToTable("Book");

                    b.HasDiscriminator().HasValue("Book");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Link", b =>
                {
                    b.HasBaseType("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Post");

                    b.Property<string>("HeadText")
                        .HasColumnName("Link_HeadText");

                    b.Property<string>("LinkUrl");

                    b.HasIndex("PhotoId");

                    b.ToTable("Link");

                    b.HasDiscriminator().HasValue("Link");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Question", b =>
                {
                    b.HasBaseType("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Post");

                    b.Property<string>("HeadText")
                        .HasColumnName("Question_HeadText");

                    b.Property<string>("Text")
                        .HasColumnName("Question_Text");

                    b.HasIndex("PhotoId");

                    b.ToTable("Question");

                    b.HasDiscriminator().HasValue("Question");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Faculty", "Faculty")
                        .WithMany("Users")
                        .HasForeignKey("FacultyId");

                    b.HasOne("CodeAcademy.CoreWebApi.Entities.Gender", "Gender")
                        .WithMany("Users")
                        .HasForeignKey("GenderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.Entities.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Certificate", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("AppIdentityUserId");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Comment", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("AppIdentityUserId");

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Post", "Post")
                        .WithMany()
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Question")
                        .WithMany("Comments")
                        .HasForeignKey("QuestionId");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Faculty", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.Entities.Photo", "Photo")
                        .WithMany("Faculties")
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Group", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Faculty", "Faculty")
                        .WithMany("Groups")
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.LessonHour", "LessonHour")
                        .WithMany("Groups")
                        .HasForeignKey("LessonHourId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.LessonStatus", "LessonStatus")
                        .WithMany()
                        .HasForeignKey("LessonStatusId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.Entities.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Room", "Room")
                        .WithMany("Groups")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.LeftNavItem", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.Entities.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Like", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser", "UserId")
                        .WithMany("Likes")
                        .HasForeignKey("AppIdentityUserId");

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Comment")
                        .WithMany("Likes")
                        .HasForeignKey("CommentId");

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.MentorGroup", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Group", "Group")
                        .WithMany("MentorGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Student", "Mentor")
                        .WithMany("MentorGroups")
                        .HasForeignKey("MentorId");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Post", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser", "User")
                        .WithMany("Posts")
                        .HasForeignKey("AppIdentityUserId");

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Faculty", "Faculty")
                        .WithMany("Posts")
                        .HasForeignKey("FacultyId1");
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.PostTag", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Post", "Post")
                        .WithMany("PostTags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Tag", "Tag")
                        .WithMany("PostTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Tag", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.TeacherGroup", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Group", "Group")
                        .WithMany("TeacherGroups")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Teacher", "Teacher")
                        .WithMany("TeacherGroups")
                        .HasForeignKey("TeacherId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.AppIdentity.AppIdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Student", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Certificate", "Certificate")
                        .WithMany()
                        .HasForeignKey("CertificateId");

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Group", "Group")
                        .WithMany("Students")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.LessonStatus", "LessonStatus")
                        .WithMany("Students")
                        .HasForeignKey("LessonStatusId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Article", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.Entities.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Book", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Faculty")
                        .WithMany("Books")
                        .HasForeignKey("FacultyId");

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.File", "File")
                        .WithMany()
                        .HasForeignKey("FileId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Language", "Language")
                        .WithMany("Books")
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CodeAcademy.CoreWebApi.Entities.Photo", "Photo")
                        .WithMany("Books")
                        .HasForeignKey("PhotoId")
                        .HasConstraintName("FK_Posts_Photos_PhotoId1")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Link", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.Entities.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CodeAcademy.CoreWebApi.DataAccessLayer.Entities.Question", b =>
                {
                    b.HasOne("CodeAcademy.CoreWebApi.Entities.Photo", "Photo")
                        .WithMany()
                        .HasForeignKey("PhotoId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
