﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using rateit.DataAccess.DbContexts;

#nullable disable

namespace rateit.Migrations
{
    [DbContext(typeof(UserContext))]
    [Migration("20221031134827_test4")]
    partial class test4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("rateit.DataAccess.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Categories", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.NotedProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("NotedProducts", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Ean")
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Producer")
                        .HasColumnType("text");

                    b.Property<int>("RateCount")
                        .HasColumnType("integer");

                    b.Property<int>("RateSum")
                        .HasColumnType("integer");

                    b.Property<int>("Sponsor")
                        .HasColumnType("integer");

                    b.Property<Guid>("SubcategoryId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Id");

                    b.HasIndex("SubcategoryId");

                    b.ToTable("Products", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.RatedProduct", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rate")
                        .HasColumnType("integer");

                    b.HasKey("UserId");

                    b.HasIndex("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("RatedProducts", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.Subcategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Id");

                    b.ToTable("Subcategories", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccountType")
                        .HasColumnType("integer");

                    b.Property<int>("AddedProduct")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("HaveAvatar")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("ResetPassKey")
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("rateit.DataAccess.Entities.ViewedProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("UserId");

                    b.ToTable("ViewedProducts", (string)null);
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
                    b.Navigation("NotedProducts");

                    b.Navigation("RatedProducts");

                    b.Navigation("ViewedProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
