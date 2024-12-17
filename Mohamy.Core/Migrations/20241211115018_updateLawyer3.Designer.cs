﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mohamy.Core;

#nullable disable

namespace Mohamy.Core.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241211115018_updateLawyer3")]
    partial class updateLawyer3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaim", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogin", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRole", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ApplicationData.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ArName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("Role", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ApplicationData.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BeneficiaryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IBAN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("ProfileId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("RegistrationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("lawyerLicenseId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("professionalAccreditation")
                        .HasColumnType("bit");

                    b.Property<int?>("yearsExperience")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("ProfileId");

                    b.HasIndex("lawyerLicenseId")
                        .IsUnique()
                        .HasFilter("[lawyerLicenseId] IS NOT NULL");

                    b.ToTable("Users", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ChatData.Chat", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagesId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUpdated")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReceiverId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ImagesId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Chats", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ConsultingData.Consulting", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("CustomerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUpdated")
                        .HasColumnType("bit");

                    b.Property<string>("LawyerId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OrderNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("PriceService")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("statusConsulting")
                        .HasColumnType("int");

                    b.Property<string>("subConsultingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("voiceConsulting")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("LawyerId");

                    b.HasIndex("subConsultingId");

                    b.ToTable("Consultings", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ConsultingData.RequestConsulting", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConsultingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUpdated")
                        .HasColumnType("bit");

                    b.Property<string>("LawyerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("PriceService")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("statusRequestConsulting")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("ConsultingId");

                    b.HasIndex("LawyerId");

                    b.ToTable("RequestConsultings", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ConsultingData.mainConsulting", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUpdated")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("iconId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("service")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("iconId");

                    b.ToTable("MainConsultings", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ConsultingData.subConsulting", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUpdated")
                        .HasColumnType("bit");

                    b.Property<string>("MainConsultingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("iconId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("MainConsultingId");

                    b.HasIndex("iconId");

                    b.ToTable("SubConsultings", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.Files.Images", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConsultingId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pathId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ConsultingId");

                    b.HasIndex("pathId");

                    b.ToTable("Images", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.Files.Paths", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Paths", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.LawyerData.Experience", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("End")
                        .HasColumnType("datetime2");

                    b.Property<string>("LawyerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.Property<string>("subConsultingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("LawyerId");

                    b.HasIndex("subConsultingId");

                    b.ToTable("Experiences", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.LawyerData.Specialties", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LawyerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("subConsultingId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("LawyerId");

                    b.HasIndex("subConsultingId");

                    b.ToTable("Specialties", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.LawyerData.graduationCertificate", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Collage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("End")
                        .HasColumnType("datetime2");

                    b.Property<string>("LawyerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.Property<string>("University")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LawyerId");

                    b.ToTable("graduationCertificate", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.LawyerData.lawyerLicense", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("End")
                        .HasColumnType("datetime2");

                    b.Property<string>("LawyerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LicenseNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Start")
                        .HasColumnType("datetime2");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LawyerLicenses", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.Notification.Notification", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUpdated")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications", "dbo");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.Others.Evaluation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("EvaluatedId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EvaluatorId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsUpdated")
                        .HasColumnType("bit");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("EvaluatedId");

                    b.HasIndex("EvaluatorId");

                    b.ToTable("Evaluations", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ApplicationData.ApplicationUser", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.Files.Images", "Profile")
                        .WithMany()
                        .HasForeignKey("ProfileId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mohamy.Core.Entity.LawyerData.lawyerLicense", "lawyerLicense")
                        .WithOne("Lawyer")
                        .HasForeignKey("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "lawyerLicenseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Profile");

                    b.Navigation("lawyerLicense");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ChatData.Chat", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.Files.Images", "Images")
                        .WithMany()
                        .HasForeignKey("ImagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Images");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ConsultingData.Consulting", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "Lawyer")
                        .WithMany()
                        .HasForeignKey("LawyerId");

                    b.HasOne("Mohamy.Core.Entity.ConsultingData.subConsulting", "subConsulting")
                        .WithMany()
                        .HasForeignKey("subConsultingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Lawyer");

                    b.Navigation("subConsulting");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ConsultingData.RequestConsulting", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ConsultingData.Consulting", "Consulting")
                        .WithMany("RequestConsultings")
                        .HasForeignKey("ConsultingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "Lawyer")
                        .WithMany("RequestConsultings")
                        .HasForeignKey("LawyerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Consulting");

                    b.Navigation("Lawyer");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ConsultingData.mainConsulting", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.Files.Images", "Icon")
                        .WithMany()
                        .HasForeignKey("iconId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Icon");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ConsultingData.subConsulting", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ConsultingData.mainConsulting", "MainConsulting")
                        .WithMany("SubConsultings")
                        .HasForeignKey("MainConsultingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mohamy.Core.Entity.Files.Images", "Icon")
                        .WithMany()
                        .HasForeignKey("iconId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Icon");

                    b.Navigation("MainConsulting");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.Files.Images", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ConsultingData.Consulting", null)
                        .WithMany("Files")
                        .HasForeignKey("ConsultingId");

                    b.HasOne("Mohamy.Core.Entity.Files.Paths", "path")
                        .WithMany("Images")
                        .HasForeignKey("pathId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("path");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.LawyerData.Experience", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "Lawyer")
                        .WithMany("Experiences")
                        .HasForeignKey("LawyerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mohamy.Core.Entity.ConsultingData.subConsulting", "subConsulting")
                        .WithMany()
                        .HasForeignKey("subConsultingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Lawyer");

                    b.Navigation("subConsulting");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.LawyerData.Specialties", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "Lawyer")
                        .WithMany("Specialties")
                        .HasForeignKey("LawyerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Mohamy.Core.Entity.ConsultingData.subConsulting", "subConsulting")
                        .WithMany()
                        .HasForeignKey("subConsultingId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Lawyer");

                    b.Navigation("subConsulting");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.LawyerData.graduationCertificate", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "Lawyer")
                        .WithMany("graduationCertificates")
                        .HasForeignKey("LawyerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lawyer");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.Notification.Notification", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.Others.Evaluation", b =>
                {
                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "Evaluated")
                        .WithMany("EvaluationsReceived")
                        .HasForeignKey("EvaluatedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mohamy.Core.Entity.ApplicationData.ApplicationUser", "Evaluator")
                        .WithMany()
                        .HasForeignKey("EvaluatorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Evaluated");

                    b.Navigation("Evaluator");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ApplicationData.ApplicationUser", b =>
                {
                    b.Navigation("EvaluationsReceived");

                    b.Navigation("Experiences");

                    b.Navigation("RequestConsultings");

                    b.Navigation("Specialties");

                    b.Navigation("graduationCertificates");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ConsultingData.Consulting", b =>
                {
                    b.Navigation("Files");

                    b.Navigation("RequestConsultings");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.ConsultingData.mainConsulting", b =>
                {
                    b.Navigation("SubConsultings");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.Files.Paths", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("Mohamy.Core.Entity.LawyerData.lawyerLicense", b =>
                {
                    b.Navigation("Lawyer")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
