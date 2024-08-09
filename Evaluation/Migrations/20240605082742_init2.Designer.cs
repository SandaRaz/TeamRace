﻿// <auto-generated />
using System;
using Evaluation.Models.Cnx;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Evaluation.Migrations
{
    [DbContext(typeof(PsqlContext))]
    [Migration("20240605082742_init2")]
    partial class init2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CoureurCategorie", b =>
                {
                    b.Property<Guid>("CategorieId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CoureurId")
                        .HasColumnType("uuid");

                    b.HasKey("CategorieId", "CoureurId");

                    b.HasIndex("CoureurId");

                    b.ToTable("CoureurCategorie");
                });

            modelBuilder.Entity("CoureurEtape", b =>
                {
                    b.Property<Guid>("CoureurId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EtapeId")
                        .HasColumnType("uuid");

                    b.HasKey("CoureurId", "EtapeId");

                    b.HasIndex("EtapeId");

                    b.ToTable("CoureurEtape");
                });

            modelBuilder.Entity("Evaluation.Models.Authentication.Admin", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("timestamp");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PictureName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Profil")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Admin");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Categorie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("timestamp");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categorie");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Coureur", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateNaissance")
                        .HasColumnType("timestamp");

                    b.Property<Guid>("EquipeId")
                        .HasColumnType("uuid");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumDossard")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("EquipeId");

                    b.HasIndex("NumDossard")
                        .IsUnique();

                    b.ToTable("Coureur");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCourse")
                        .HasColumnType("timestamp");

                    b.Property<TimeSpan>("DureeHeure")
                        .HasColumnType("interval");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Equipe", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreation")
                        .HasColumnType("timestamp");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MotDePasse")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Profil")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Equipe");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Etape", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateDepart")
                        .HasColumnType("timestamp");

                    b.Property<string>("Lieu")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Longueur")
                        .HasColumnType("double precision");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NombreCoureur")
                        .HasColumnType("integer");

                    b.Property<int>("RangEtape")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Etape");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Penalite", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("EquipeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EtapeId")
                        .HasColumnType("uuid");

                    b.Property<int>("Etat")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("TempsPenalite")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("EquipeId");

                    b.HasIndex("EtapeId");

                    b.ToTable("Penalite");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.PointEtape", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<int>("Rang")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("PointEtape");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Resultat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CoureurId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateArrivee")
                        .HasColumnType("timestamp");

                    b.Property<Guid>("EtapeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CoureurId");

                    b.HasIndex("EtapeId");

                    b.ToTable("Resultat");
                });

            modelBuilder.Entity("Evaluation.Models.MappingFile.CsvEtape", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateDepart")
                        .HasColumnType("timestamp");

                    b.Property<string>("Etape")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("Longueur")
                        .HasColumnType("double precision");

                    b.Property<int>("NbCoureur")
                        .HasColumnType("integer");

                    b.Property<int>("Rang")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("CsvEtape");
                });

            modelBuilder.Entity("Evaluation.Models.MappingFile.CsvPoints", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Classement")
                        .HasColumnType("integer");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("CsvPoints");
                });

            modelBuilder.Entity("Evaluation.Models.MappingFile.CsvResultat", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Arrivee")
                        .HasColumnType("timestamp");

                    b.Property<DateTime>("DateNaissance")
                        .HasColumnType("timestamp");

                    b.Property<string>("Equipe")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("EtapeRang")
                        .HasColumnType("integer");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumeroDossard")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("CsvResultat");
                });

            modelBuilder.Entity("Evaluation.Models.MappingView.Classement", b =>
                {
                    b.Property<Guid>("CoureurId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateArrivee")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateArriveePenalisee")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("DateDepart")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EquipeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("EtapeId")
                        .HasColumnType("uuid");

                    b.Property<string>("GenreCoureur")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("LongueurEtape")
                        .HasColumnType("double precision");

                    b.Property<string>("NomCoureur")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NomEquipe")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NomEtape")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NombreCoureur")
                        .HasColumnType("integer");

                    b.Property<int>("NumDossard")
                        .HasColumnType("integer");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<int>("Rang")
                        .HasColumnType("integer");

                    b.Property<int>("RangEtape")
                        .HasColumnType("integer");

                    b.ToTable((string)null);

                    b.ToView("Classement", (string)null);
                });

            modelBuilder.Entity("Evaluation.Models.MappingView.ClassementGeneraleEquipe", b =>
                {
                    b.Property<Guid>("EquipeId")
                        .HasColumnType("uuid");

                    b.Property<string>("NomEquipe")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<int>("Rang")
                        .HasColumnType("integer");

                    b.ToTable("ClassementEquipeCategorie");
                });

            modelBuilder.Entity("Evaluation.Models.MappingView.ResultatAvecPenalite", b =>
                {
                    b.Property<Guid>("CoureurId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateArrivee")
                        .HasColumnType("timestamp");

                    b.Property<Guid>("EtapeId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.HasIndex("CoureurId");

                    b.HasIndex("EtapeId");

                    b.ToTable((string)null);

                    b.ToView("ResultatAvecPenalite", (string)null);
                });

            modelBuilder.Entity("CoureurCategorie", b =>
                {
                    b.HasOne("Evaluation.Models.Entity.Categorie", null)
                        .WithMany()
                        .HasForeignKey("CategorieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Evaluation.Models.Entity.Coureur", null)
                        .WithMany()
                        .HasForeignKey("CoureurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoureurEtape", b =>
                {
                    b.HasOne("Evaluation.Models.Entity.Coureur", null)
                        .WithMany()
                        .HasForeignKey("CoureurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Evaluation.Models.Entity.Etape", null)
                        .WithMany()
                        .HasForeignKey("EtapeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Coureur", b =>
                {
                    b.HasOne("Evaluation.Models.Entity.Equipe", "Equipe")
                        .WithMany("Coureurs")
                        .HasForeignKey("EquipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipe");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Etape", b =>
                {
                    b.HasOne("Evaluation.Models.Entity.Course", "Course")
                        .WithMany("Etapes")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Penalite", b =>
                {
                    b.HasOne("Evaluation.Models.Entity.Equipe", "Equipe")
                        .WithMany("Penalites")
                        .HasForeignKey("EquipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Evaluation.Models.Entity.Etape", "Etape")
                        .WithMany("Penalites")
                        .HasForeignKey("EtapeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipe");

                    b.Navigation("Etape");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Resultat", b =>
                {
                    b.HasOne("Evaluation.Models.Entity.Coureur", "Coureur")
                        .WithMany("Resultats")
                        .HasForeignKey("CoureurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Evaluation.Models.Entity.Etape", "Etape")
                        .WithMany("Resultats")
                        .HasForeignKey("EtapeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coureur");

                    b.Navigation("Etape");
                });

            modelBuilder.Entity("Evaluation.Models.MappingView.ResultatAvecPenalite", b =>
                {
                    b.HasOne("Evaluation.Models.Entity.Coureur", "Coureur")
                        .WithMany()
                        .HasForeignKey("CoureurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Evaluation.Models.Entity.Etape", "Etape")
                        .WithMany()
                        .HasForeignKey("EtapeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coureur");

                    b.Navigation("Etape");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Coureur", b =>
                {
                    b.Navigation("Resultats");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Course", b =>
                {
                    b.Navigation("Etapes");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Equipe", b =>
                {
                    b.Navigation("Coureurs");

                    b.Navigation("Penalites");
                });

            modelBuilder.Entity("Evaluation.Models.Entity.Etape", b =>
                {
                    b.Navigation("Penalites");

                    b.Navigation("Resultats");
                });
#pragma warning restore 612, 618
        }
    }
}
