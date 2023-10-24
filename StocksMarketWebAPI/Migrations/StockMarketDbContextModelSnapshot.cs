﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StocksMarketWebAPI.Context;

#nullable disable

namespace StocksMarketWebAPI.Migrations
{
    [DbContext(typeof(StockMarketDbContext))]
    partial class StockMarketDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StocksMarketWebAPI.Entities.MainBoard", b =>
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
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.MoneyCard", b =>
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

            modelBuilder.Entity("StocksMarketWebAPI.Entities.Portfolio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<float>("Balance")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Portfolios");
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.PortfolioStock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

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

            modelBuilder.Entity("StocksMarketWebAPI.Entities.PortfolioUser", b =>
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
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("Unit")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.StockBuyAndSale", b =>
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

            modelBuilder.Entity("StocksMarketWebAPI.Entities.StockPrice", b =>
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

            modelBuilder.Entity("StocksMarketWebAPI.Entities.User", b =>
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
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.UserMoneyCard", b =>
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

            modelBuilder.Entity("StocksMarketWebAPI.Entities.PortfolioStock", b =>
                {
                    b.HasOne("StocksMarketWebAPI.Entities.Portfolio", "Portfolio")
                        .WithMany("PortfoliosStocks")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StocksMarketWebAPI.Entities.Stock", "Stock")
                        .WithMany("PortfolioStocks")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.PortfolioUser", b =>
                {
                    b.HasOne("StocksMarketWebAPI.Entities.Portfolio", "Portfolio")
                        .WithOne("PortfolioUsers")
                        .HasForeignKey("StocksMarketWebAPI.Entities.PortfolioUser", "PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StocksMarketWebAPI.Entities.User", "User")
                        .WithOne("PortfolioUser")
                        .HasForeignKey("StocksMarketWebAPI.Entities.PortfolioUser", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.StockBuyAndSale", b =>
                {
                    b.HasOne("StocksMarketWebAPI.Entities.Portfolio", "Portfolio")
                        .WithMany("StockBuyAndSales")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StocksMarketWebAPI.Entities.Stock", "Stock")
                        .WithMany("StockBuyAndSales")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.StockPrice", b =>
                {
                    b.HasOne("StocksMarketWebAPI.Entities.Stock", "Stock")
                        .WithMany("StockPrices")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.UserMoneyCard", b =>
                {
                    b.HasOne("StocksMarketWebAPI.Entities.MoneyCard", "MoneyCard")
                        .WithOne("UserMoneyCard")
                        .HasForeignKey("StocksMarketWebAPI.Entities.UserMoneyCard", "MoneyCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StocksMarketWebAPI.Entities.User", "User")
                        .WithMany("UserMoneyCards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MoneyCard");

                    b.Navigation("User");
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.MoneyCard", b =>
                {
                    b.Navigation("UserMoneyCard")
                        .IsRequired();
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.Portfolio", b =>
                {
                    b.Navigation("PortfolioUsers")
                        .IsRequired();

                    b.Navigation("PortfoliosStocks");

                    b.Navigation("StockBuyAndSales");
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.Stock", b =>
                {
                    b.Navigation("PortfolioStocks");

                    b.Navigation("StockBuyAndSales");

                    b.Navigation("StockPrices");
                });

            modelBuilder.Entity("StocksMarketWebAPI.Entities.User", b =>
                {
                    b.Navigation("PortfolioUser")
                        .IsRequired();

                    b.Navigation("UserMoneyCards");
                });
#pragma warning restore 612, 618
        }
    }
}
