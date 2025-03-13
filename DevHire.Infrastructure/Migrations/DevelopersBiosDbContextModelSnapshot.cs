﻿// <auto-generated />
using System;
using DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DevHire.Infrastructure.Migrations
{
    [DbContext(typeof(DevelopersDbContext))]
    partial class DevelopersDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Entities.Developer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FavoriteLanguage")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("YearsOfExperience")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Developers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = new Guid("eb68e0e6-4661-4f4f-9b23-a1c658586231"),
                            FavoriteLanguage = "Angular",
                            FirstName = "Sivachandrakumar",
                            LastName = "Chandrasekaran",
                            YearsOfExperience = 12
                        },
                        new
                        {
                            Id = new Guid("cee5ef24-e34f-49df-a680-1f7f98a73c0d"),
                            FavoriteLanguage = "Typescript",
                            FirstName = "Roshan",
                            LastName = "Kumar",
                            YearsOfExperience = 10
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
