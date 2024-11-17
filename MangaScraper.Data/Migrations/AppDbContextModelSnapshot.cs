﻿// <auto-generated />
using MangaScraper.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MangaScraper.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.Capitolo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NumCapitolo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VolumeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VolumeId");

                    b.ToTable("Capitoli");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.Genere", b =>
                {
                    b.Property<string>("NameId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("GenereEnum")
                        .HasColumnType("int");

                    b.HasKey("NameId");

                    b.ToTable("Generi");

                    b.HasData(
                        new
                        {
                            NameId = "ArtiMarziali",
                            GenereEnum = 0
                        },
                        new
                        {
                            NameId = "Avventura",
                            GenereEnum = 1
                        },
                        new
                        {
                            NameId = "Azione",
                            GenereEnum = 2
                        },
                        new
                        {
                            NameId = "Commedia",
                            GenereEnum = 3
                        },
                        new
                        {
                            NameId = "Doujinshi",
                            GenereEnum = 4
                        },
                        new
                        {
                            NameId = "Drammatico",
                            GenereEnum = 5
                        },
                        new
                        {
                            NameId = "Ecchi",
                            GenereEnum = 6
                        },
                        new
                        {
                            NameId = "Fantasy",
                            GenereEnum = 7
                        },
                        new
                        {
                            NameId = "GenderBender",
                            GenereEnum = 8
                        },
                        new
                        {
                            NameId = "Harem",
                            GenereEnum = 9
                        },
                        new
                        {
                            NameId = "Hentai",
                            GenereEnum = 10
                        },
                        new
                        {
                            NameId = "Horror",
                            GenereEnum = 11
                        },
                        new
                        {
                            NameId = "Josei",
                            GenereEnum = 12
                        },
                        new
                        {
                            NameId = "Lolicon",
                            GenereEnum = 13
                        },
                        new
                        {
                            NameId = "Maturo",
                            GenereEnum = 14
                        },
                        new
                        {
                            NameId = "Mecha",
                            GenereEnum = 15
                        },
                        new
                        {
                            NameId = "Mistero",
                            GenereEnum = 16
                        },
                        new
                        {
                            NameId = "Psicologico",
                            GenereEnum = 17
                        },
                        new
                        {
                            NameId = "Romantico",
                            GenereEnum = 18
                        },
                        new
                        {
                            NameId = "Scifi",
                            GenereEnum = 19
                        },
                        new
                        {
                            NameId = "Scolastico",
                            GenereEnum = 20
                        },
                        new
                        {
                            NameId = "Seinen",
                            GenereEnum = 21
                        },
                        new
                        {
                            NameId = "Shotacon",
                            GenereEnum = 22
                        },
                        new
                        {
                            NameId = "Shoujo",
                            GenereEnum = 23
                        },
                        new
                        {
                            NameId = "ShoujoAi",
                            GenereEnum = 24
                        },
                        new
                        {
                            NameId = "Shounen",
                            GenereEnum = 25
                        },
                        new
                        {
                            NameId = "ShounenAi",
                            GenereEnum = 26
                        },
                        new
                        {
                            NameId = "SliceofLife",
                            GenereEnum = 27
                        },
                        new
                        {
                            NameId = "Smut",
                            GenereEnum = 28
                        },
                        new
                        {
                            NameId = "Soprannaturale",
                            GenereEnum = 29
                        },
                        new
                        {
                            NameId = "Sport",
                            GenereEnum = 30
                        },
                        new
                        {
                            NameId = "Storico",
                            GenereEnum = 31
                        },
                        new
                        {
                            NameId = "Tragico",
                            GenereEnum = 32
                        },
                        new
                        {
                            NameId = "Yaoi",
                            GenereEnum = 33
                        },
                        new
                        {
                            NameId = "Yuri",
                            GenereEnum = 34
                        });
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.ImagePosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CapitoloId")
                        .HasColumnType("int");

                    b.Property<string>("PathImg")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CapitoloId");

                    b.ToTable("ImagePositions");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.Manga", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Artista")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Autore")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CopertinaUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stato")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Trama")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Mangas");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.MangaGenere", b =>
                {
                    b.Property<int>("MangaId")
                        .HasColumnType("int");

                    b.Property<string>("GenereId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MangaId", "GenereId");

                    b.HasIndex("GenereId");

                    b.ToTable("MangaGenere");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.Volume", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MangaId")
                        .HasColumnType("int");

                    b.Property<string>("NumVolume")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MangaId");

                    b.ToTable("Volumi");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.Capitolo", b =>
                {
                    b.HasOne("MangaScraper.Data.Models.Domain.Volume", "Volume")
                        .WithMany("Capitoli")
                        .HasForeignKey("VolumeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Volume");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.ImagePosition", b =>
                {
                    b.HasOne("MangaScraper.Data.Models.Domain.Capitolo", "Capitolo")
                        .WithMany("ImgPositions")
                        .HasForeignKey("CapitoloId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Capitolo");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.MangaGenere", b =>
                {
                    b.HasOne("MangaScraper.Data.Models.Domain.Genere", "Genere")
                        .WithMany("MangaGenereList")
                        .HasForeignKey("GenereId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MangaScraper.Data.Models.Domain.Manga", "Manga")
                        .WithMany("MangaGenereList")
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genere");

                    b.Navigation("Manga");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.Volume", b =>
                {
                    b.HasOne("MangaScraper.Data.Models.Domain.Manga", "Manga")
                        .WithMany("Volumi")
                        .HasForeignKey("MangaId");

                    b.Navigation("Manga");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.Capitolo", b =>
                {
                    b.Navigation("ImgPositions");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.Genere", b =>
                {
                    b.Navigation("MangaGenereList");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.Manga", b =>
                {
                    b.Navigation("MangaGenereList");

                    b.Navigation("Volumi");
                });

            modelBuilder.Entity("MangaScraper.Data.Models.Domain.Volume", b =>
                {
                    b.Navigation("Capitoli");
                });
#pragma warning restore 612, 618
        }
    }
}
