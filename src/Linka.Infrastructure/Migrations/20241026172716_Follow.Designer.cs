﻿// <auto-generated />
using System;
using Linka.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Linka.Infrastructure.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20241026172716_Follow")]
    partial class Follow
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EventJobVolunteer", b =>
                {
                    b.Property<Guid>("JobsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VolunteersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("JobsId", "VolunteersId");

                    b.HasIndex("VolunteersId");

                    b.ToTable("EventJobVolunteer");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Neighborhood")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("ImageBytes")
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Linka.Domain.Entities.EventJob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MaxVolunteers")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventJobs");
                });

            modelBuilder.Entity("Linka.Domain.Entities.JobVolunteerActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("CheckIn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CheckOut")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("JobId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("VolunteerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("VolunteerId");

                    b.ToTable("JobVolunterActivities");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ProfilePictureBytes")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ProfilePictureExtension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TradingName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AssociatedOrganizationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("ImageBytes")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("AssociatedOrganizationId");

                    b.HasIndex("AuthorId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Linka.Domain.Entities.PostComment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PostId");

                    b.ToTable("PostComments");
                });

            modelBuilder.Entity("Linka.Domain.Entities.PostLike", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostLikes");
                });

            modelBuilder.Entity("Linka.Domain.Entities.PostShare", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PostId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostShares");
                });

            modelBuilder.Entity("Linka.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Volunteer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AllTimePoints")
                        .HasColumnType("int");

                    b.Property<string>("CPF")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<byte[]>("ProfilePictureBytes")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ProfilePictureExtension")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("UserId");

                    b.ToTable("Volunteers");
                });

            modelBuilder.Entity("EventJobVolunteer", b =>
                {
                    b.HasOne("Linka.Domain.Entities.EventJob", null)
                        .WithMany()
                        .HasForeignKey("JobsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Linka.Domain.Entities.Volunteer", null)
                        .WithMany()
                        .HasForeignKey("VolunteersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Linka.Domain.Entities.Event", b =>
                {
                    b.HasOne("Linka.Domain.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Linka.Domain.Entities.Organization", "Organization")
                        .WithMany("Events")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("Linka.Domain.Entities.EventJob", b =>
                {
                    b.HasOne("Linka.Domain.Entities.Event", "Event")
                        .WithMany("Jobs")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Linka.Domain.Entities.JobVolunteerActivity", b =>
                {
                    b.HasOne("Linka.Domain.Entities.EventJob", "Job")
                        .WithMany()
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Linka.Domain.Entities.Volunteer", "Volunteer")
                        .WithMany()
                        .HasForeignKey("VolunteerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Job");

                    b.Navigation("Volunteer");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Organization", b =>
                {
                    b.HasOne("Linka.Domain.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Linka.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Post", b =>
                {
                    b.HasOne("Linka.Domain.Entities.Organization", "AssociatedOrganization")
                        .WithMany()
                        .HasForeignKey("AssociatedOrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Linka.Domain.Entities.User", "Author")
                        .WithMany("Posts")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("AssociatedOrganization");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Linka.Domain.Entities.PostComment", b =>
                {
                    b.HasOne("Linka.Domain.Entities.User", "Author")
                        .WithMany("Comments")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Linka.Domain.Entities.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Post");
                });

            modelBuilder.Entity("Linka.Domain.Entities.PostLike", b =>
                {
                    b.HasOne("Linka.Domain.Entities.Post", "Post")
                        .WithMany("Likes")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Linka.Domain.Entities.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Linka.Domain.Entities.PostShare", b =>
                {
                    b.HasOne("Linka.Domain.Entities.Post", "Post")
                        .WithMany("Shares")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Linka.Domain.Entities.User", "User")
                        .WithMany("Shares")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Volunteer", b =>
                {
                    b.HasOne("Linka.Domain.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Linka.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Event", b =>
                {
                    b.Navigation("Jobs");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Organization", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("Linka.Domain.Entities.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("Shares");
                });

            modelBuilder.Entity("Linka.Domain.Entities.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("Posts");

                    b.Navigation("Shares");
                });
#pragma warning restore 612, 618
        }
    }
}
