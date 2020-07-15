﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrigemDestino.Persistence;

namespace OrigemDestino.Migrations
{
    [DbContext(typeof(ODContext))]
    [Migration("20200410161130_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3");

            modelBuilder.Entity("OrigemDestino.Core.Frequenter", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Frequenter");
                });

            modelBuilder.Entity("OrigemDestino.Core.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("X")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Y")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("OrigemDestino.Core.LocationFrequenter", b =>
                {
                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FrequenterId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LocationId", "FrequenterId");

                    b.HasIndex("FrequenterId");

                    b.ToTable("LocationFrequenter");
                });

            modelBuilder.Entity("OrigemDestino.Core.LocationFrequenter", b =>
                {
                    b.HasOne("OrigemDestino.Core.Frequenter", "Frequenter")
                        .WithMany("LocationFrequenters")
                        .HasForeignKey("FrequenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OrigemDestino.Core.Location", "Location")
                        .WithMany("LocationFrequenters")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}