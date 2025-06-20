﻿// <auto-generated />
using System;
using HappeningService.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HappeningService.Migrations
{
    [DbContext(typeof(HappeningContext))]
    [Migration("20250423235235_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("HappeningService.Models.Happening", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime");

                    b.HasKey("Guid");

                    b.ToTable("Happenings");
                });

            modelBuilder.Entity("HappeningService.Models.HappeningArtist", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ArtistGuid")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("HappeningGuid")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Showtime")
                        .HasColumnType("datetime");

                    b.HasKey("Guid");

                    b.HasIndex("HappeningGuid");

                    b.ToTable("HappeningArtists");
                });

            modelBuilder.Entity("HappeningService.Models.HappeningArtist", b =>
                {
                    b.HasOne("HappeningService.Models.Happening", "Happening")
                        .WithMany("HappeningArtists")
                        .HasForeignKey("HappeningGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Happening");
                });

            modelBuilder.Entity("HappeningService.Models.Happening", b =>
                {
                    b.Navigation("HappeningArtists");
                });
#pragma warning restore 612, 618
        }
    }
}
