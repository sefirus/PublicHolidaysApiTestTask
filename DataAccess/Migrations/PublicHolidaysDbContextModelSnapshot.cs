﻿// <auto-generated />
using System;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccess.Migrations
{
    [DbContext(typeof(PublicHolidaysDbContext))]
    partial class PublicHolidaysDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Entities.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CountryCode2Digit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CountryCode3Digit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ToDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Core.Entities.Holiday", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CountryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("Day")
                        .HasColumnType("int");

                    b.Property<int?>("DayOfWeek")
                        .HasColumnType("int");

                    b.Property<int>("HolidayType")
                        .HasColumnType("int");

                    b.Property<int?>("Month")
                        .HasColumnType("int");

                    b.Property<int>("PatternType")
                        .HasColumnType("int");

                    b.Property<int?>("WeekOfMonth")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("Core.Entities.LocalizedTextEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HolidayId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Lang")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("HolidayId");

                    b.ToTable("LocalizedTextEntries");
                });

            modelBuilder.Entity("Core.Enums.HolidayEffectivePeriod", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("EndYear")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HolidayId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("StartYear")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("HolidayId");

                    b.ToTable("HolidayEffectivePeriods");
                });

            modelBuilder.Entity("Core.Enums.HolidayOccurrence", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("HolidayId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("HolidayId");

                    b.ToTable("HolidayOccurrences");
                });

            modelBuilder.Entity("Core.Entities.Holiday", b =>
                {
                    b.HasOne("Core.Entities.Country", "Country")
                        .WithMany("Holidays")
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Core.Entities.LocalizedTextEntry", b =>
                {
                    b.HasOne("Core.Entities.Holiday", "Holiday")
                        .WithMany("Names")
                        .HasForeignKey("HolidayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Holiday");
                });

            modelBuilder.Entity("Core.Enums.HolidayEffectivePeriod", b =>
                {
                    b.HasOne("Core.Entities.Holiday", "Holiday")
                        .WithMany("EffectivePeriods")
                        .HasForeignKey("HolidayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Holiday");
                });

            modelBuilder.Entity("Core.Enums.HolidayOccurrence", b =>
                {
                    b.HasOne("Core.Entities.Holiday", "Holiday")
                        .WithMany("Occurrences")
                        .HasForeignKey("HolidayId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Holiday");
                });

            modelBuilder.Entity("Core.Entities.Country", b =>
                {
                    b.Navigation("Holidays");
                });

            modelBuilder.Entity("Core.Entities.Holiday", b =>
                {
                    b.Navigation("EffectivePeriods");

                    b.Navigation("Names");

                    b.Navigation("Occurrences");
                });
#pragma warning restore 612, 618
        }
    }
}
