﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WMS.Models;

namespace WMS.Migrations
{
    [DbContext(typeof(WMSContext))]
    partial class WMSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
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
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

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

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WMS.Areas.Identity.Data.WMSUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("WMSUser");
                });

            modelBuilder.Entity("WMS.Models.Item", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ItemDetailsUPC");

                    b.Property<string>("StorageSpaceId");

                    b.HasKey("Id");

                    b.HasIndex("ItemDetailsUPC");

                    b.HasIndex("StorageSpaceId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("WMS.Models.ItemCount", b =>
                {
                    b.Property<int>("Count")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ItemUPC");

                    b.Property<string>("RequestId");

                    b.HasKey("Count");

                    b.HasIndex("ItemUPC");

                    b.HasIndex("RequestId");

                    b.ToTable("ItemCounts");
                });

            modelBuilder.Entity("WMS.Models.ItemDetails", b =>
                {
                    b.Property<string>("UPC")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("ImageURI");

                    b.Property<string>("Name");

                    b.Property<double>("Volume");

                    b.HasKey("UPC");

                    b.ToTable("ItemDetails");
                });

            modelBuilder.Entity("WMS.Models.Request", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("FirmId");

                    b.Property<bool>("Processed");

                    b.Property<DateTime>("RequestDate");

                    b.HasKey("Id");

                    b.HasIndex("FirmId");

                    b.ToTable("Requests");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Request");
                });

            modelBuilder.Entity("WMS.Models.StorageSpace", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Capacity");

                    b.Property<string>("FirmId");

                    b.Property<string>("WarehouseName");

                    b.HasKey("Id");

                    b.HasIndex("FirmId");

                    b.HasIndex("WarehouseName");

                    b.ToTable("StorageSpaces");
                });

            modelBuilder.Entity("WMS.Models.Warehouse", b =>
                {
                    b.Property<string>("Name")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Capacity");

                    b.HasKey("Name");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("WMS.Models.Employee", b =>
                {
                    b.HasBaseType("WMS.Areas.Identity.Data.WMSUser");

                    b.Property<string>("FullName");

                    b.Property<DateTime>("HireDate");

                    b.HasDiscriminator().HasValue("Employee");
                });

            modelBuilder.Entity("WMS.Models.Firm", b =>
                {
                    b.HasBaseType("WMS.Areas.Identity.Data.WMSUser");

                    b.Property<string>("FirmName");

                    b.HasDiscriminator().HasValue("Firm");
                });

            modelBuilder.Entity("WMS.Models.ExportRequest", b =>
                {
                    b.HasBaseType("WMS.Models.Request");

                    b.HasDiscriminator().HasValue("ExportRequest");
                });

            modelBuilder.Entity("WMS.Models.ImportRequest", b =>
                {
                    b.HasBaseType("WMS.Models.Request");

                    b.HasDiscriminator().HasValue("ImportRequest");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WMS.Areas.Identity.Data.WMSUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WMS.Areas.Identity.Data.WMSUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WMS.Areas.Identity.Data.WMSUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WMS.Areas.Identity.Data.WMSUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WMS.Models.Item", b =>
                {
                    b.HasOne("WMS.Models.ItemDetails", "ItemDetails")
                        .WithMany()
                        .HasForeignKey("ItemDetailsUPC");

                    b.HasOne("WMS.Models.StorageSpace")
                        .WithMany("Items")
                        .HasForeignKey("StorageSpaceId");
                });

            modelBuilder.Entity("WMS.Models.ItemCount", b =>
                {
                    b.HasOne("WMS.Models.ItemDetails", "Item")
                        .WithMany()
                        .HasForeignKey("ItemUPC");

                    b.HasOne("WMS.Models.Request")
                        .WithMany("Items")
                        .HasForeignKey("RequestId");
                });

            modelBuilder.Entity("WMS.Models.Request", b =>
                {
                    b.HasOne("WMS.Models.Firm", "Firm")
                        .WithMany()
                        .HasForeignKey("FirmId");
                });

            modelBuilder.Entity("WMS.Models.StorageSpace", b =>
                {
                    b.HasOne("WMS.Models.Firm", "Firm")
                        .WithMany("StorageSpaces")
                        .HasForeignKey("FirmId");

                    b.HasOne("WMS.Models.Warehouse")
                        .WithMany("StorageSpaces")
                        .HasForeignKey("WarehouseName");
                });
#pragma warning restore 612, 618
        }
    }
}
