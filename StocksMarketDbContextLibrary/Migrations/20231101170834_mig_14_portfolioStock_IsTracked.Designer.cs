﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockMarketDbContextLibrary.Context;

#nullable disable

namespace StockMarketDbContextLibrary.Migrations
{
    [DbContext(typeof(StockMarketDbContext))]
    [Migration("20231101170834_mig_14_portfolioStock_IsTracked")]
    partial class mig_14_portfolioStock_IsTracked
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.MainBoard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Balance")
                        .HasColumnType("real");

                    b.Property<float>("CommissionRate")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("MainBoards");

                    b.HasData(
                        new
                        {
                            Id = 5,
                            Balance = 0f,
                            CommissionRate = 5f
                        });
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.MoneyCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Balance")
                        .HasColumnType("real");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("UsedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("MoneyCards");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.Portfolio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Balance")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Portfolios");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Balance = 0f
                        });
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.PortfolioStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool?>("IsTracked")
                        .HasColumnType("bit");

                    b.Property<int>("PortfolioId")
                        .HasColumnType("int");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PortfolioId");

                    b.HasIndex("StockId");

                    b.ToTable("PortfolioStock");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.PortfolioUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("PortfolioId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PortfolioId")
                        .IsUnique();

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("PortfolioUser");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PortfolioId = 1,
                            UserId = 1
                        });
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.StockBuyAndSale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<int>("PortfolioId")
                        .HasColumnType("int");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PortfolioId");

                    b.HasIndex("StockId");

                    b.ToTable("StockBuyAndSale");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.StockPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.ToTable("StockPrices");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tel")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@gmail.com",
                            Name = "admin",
                            Password = "admin",
                            Role = "Admin",
                            Tel = "05350449876"
                        });
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.UserMoneyCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MoneyCardId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MoneyCardId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("UsersMoneyCard");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.PortfolioStock", b =>
                {
                    b.HasOne("StockMarketEntitiesLibrary.Entities.Portfolio", "Portfolio")
                        .WithMany("PortfoliosStocks")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StockMarketEntitiesLibrary.Entities.Stock", "Stock")
                        .WithMany("PortfolioStocks")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.PortfolioUser", b =>
                {
                    b.HasOne("StockMarketEntitiesLibrary.Entities.Portfolio", "Portfolio")
                        .WithOne("PortfolioUsers")
                        .HasForeignKey("StockMarketEntitiesLibrary.Entities.PortfolioUser", "PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StockMarketEntitiesLibrary.Entities.User", "User")
                        .WithOne("PortfolioUser")
                        .HasForeignKey("StockMarketEntitiesLibrary.Entities.PortfolioUser", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.StockBuyAndSale", b =>
                {
                    b.HasOne("StockMarketEntitiesLibrary.Entities.Portfolio", "Portfolio")
                        .WithMany("StockBuyAndSales")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StockMarketEntitiesLibrary.Entities.Stock", "Stock")
                        .WithMany("StockBuyAndSales")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.StockPrice", b =>
                {
                    b.HasOne("StockMarketEntitiesLibrary.Entities.Stock", "Stock")
                        .WithMany("StockPrices")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.UserMoneyCard", b =>
                {
                    b.HasOne("StockMarketEntitiesLibrary.Entities.MoneyCard", "MoneyCard")
                        .WithOne("UserMoneyCard")
                        .HasForeignKey("StockMarketEntitiesLibrary.Entities.UserMoneyCard", "MoneyCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StockMarketEntitiesLibrary.Entities.User", "User")
                        .WithMany("UserMoneyCards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MoneyCard");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.MoneyCard", b =>
                {
                    b.Navigation("UserMoneyCard")
                        .IsRequired();
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.Portfolio", b =>
                {
                    b.Navigation("PortfolioUsers")
                        .IsRequired();

                    b.Navigation("PortfoliosStocks");

                    b.Navigation("StockBuyAndSales");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.Stock", b =>
                {
                    b.Navigation("PortfolioStocks");

                    b.Navigation("StockBuyAndSales");

                    b.Navigation("StockPrices");
                });

            modelBuilder.Entity("StockMarketEntitiesLibrary.Entities.User", b =>
                {
                    b.Navigation("PortfolioUser")
                        .IsRequired();

                    b.Navigation("UserMoneyCards");
                });
#pragma warning restore 612, 618
        }
    }
}
