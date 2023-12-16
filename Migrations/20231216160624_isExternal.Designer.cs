﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using rateit.DataAccess.DbContexts;

#nullable disable

namespace rateit.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20231216160624_isExternal")]
    partial class isExternal
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("rateit.DataAccess.Entities.ActivateCode", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ActivateCodes", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.NotedProduct", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Note")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "ProductId");

                    b.HasIndex("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("NotedProducts", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Ean")
                        .HasColumnType("longtext");

                    b.Property<string>("Image")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("Producer")
                        .HasColumnType("longtext");

                    b.Property<int>("RateCount")
                        .HasColumnType("int");

                    b.Property<int>("RateSum")
                        .HasColumnType("int");

                    b.Property<int>("Sponsor")
                        .HasColumnType("int");

                    b.Property<Guid>("SubcategoryId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Id");

                    b.HasIndex("SubcategoryId");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.RatedProduct", b =>
                {
                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("Rate")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "UserId");

                    b.HasIndex("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RatedProducts", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.Subcategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Id");

                    b.ToTable("Subcategories", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("AccountType")
                        .HasColumnType("int");

                    b.Property<int?>("AddedProduct")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<bool>("HaveAvatar")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsExternal")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("ResetPassKey")
                        .HasColumnType("longtext");

                    b.Property<string>("Surname")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.ViewedProduct", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.HasKey("UserId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("ViewedProducts", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.ActivateCode", b =>
                {
                    b.HasOne("rateit.DataAccess.Entities.User", "User")
                        .WithOne("ActivateCode")
                        .HasForeignKey("rateit.DataAccess.Entities.ActivateCode", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.NotedProduct", b =>
                {
                    b.HasOne("rateit.DataAccess.Entities.Product", "Product")
                        .WithMany("NotedProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("rateit.DataAccess.Entities.User", "User")
                        .WithMany("NotedProducts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.Product", b =>
                {
                    b.HasOne("rateit.DataAccess.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("rateit.DataAccess.Entities.Subcategory", "Subcategory")
                        .WithMany("Products")
                        .HasForeignKey("SubcategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Subcategory");
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.RatedProduct", b =>
                {
                    b.HasOne("rateit.DataAccess.Entities.Product", "Product")
                        .WithMany("RatedProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("rateit.DataAccess.Entities.User", "User")
                        .WithMany("RatedProducts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.Subcategory", b =>
                {
                    b.HasOne("rateit.DataAccess.Entities.Category", "Category")
                        .WithMany("Subcategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.ViewedProduct", b =>
                {
                    b.HasOne("rateit.DataAccess.Entities.Product", "Product")
                        .WithMany("ViewedProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("rateit.DataAccess.Entities.User", "User")
                        .WithMany("ViewedProducts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("User");
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.Category", b =>
                {
                    b.Navigation("Products");

                    b.Navigation("Subcategories");
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.Product", b =>
                {
                    b.Navigation("NotedProducts");

                    b.Navigation("RatedProducts");

                    b.Navigation("ViewedProducts");
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.Subcategory", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.User", b =>
                {
                    b.Navigation("ActivateCode");

                    b.Navigation("NotedProducts");

                    b.Navigation("RatedProducts");

                    b.Navigation("ViewedProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
