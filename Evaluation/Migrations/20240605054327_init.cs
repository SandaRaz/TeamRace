using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Evaluation.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    PictureName = table.Column<string>(type: "text", nullable: false),
                    Profil = table.Column<string>(type: "text", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassementEquipeCategorie",
                columns: table => new
                {
                    EquipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    NomEquipe = table.Column<string>(type: "text", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false),
                    Rang = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    DateCourse = table.Column<DateTime>(type: "timestamp", nullable: false),
                    DureeHeure = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CsvEtape",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Etape = table.Column<string>(type: "text", nullable: false),
                    Longueur = table.Column<double>(type: "double precision", nullable: false),
                    NbCoureur = table.Column<int>(type: "integer", nullable: false),
                    Rang = table.Column<int>(type: "integer", nullable: false),
                    DateDepart = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsvEtape", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CsvPoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Classement = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsvPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CsvResultat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EtapeRang = table.Column<int>(type: "integer", nullable: false),
                    NumeroDossard = table.Column<int>(type: "integer", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Genre = table.Column<string>(type: "text", nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "timestamp", nullable: false),
                    Equipe = table.Column<string>(type: "text", nullable: false),
                    Arrivee = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CsvResultat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Equipe",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    MotDePasse = table.Column<string>(type: "text", nullable: false),
                    Profil = table.Column<string>(type: "text", nullable: false),
                    DateCreation = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PointEtape",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Rang = table.Column<int>(type: "integer", nullable: false),
                    Points = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PointEtape", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Etape",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Lieu = table.Column<string>(type: "text", nullable: false),
                    Longueur = table.Column<double>(type: "double precision", nullable: false),
                    NombreCoureur = table.Column<int>(type: "integer", nullable: false),
                    DateDepart = table.Column<DateTime>(type: "timestamp", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    RangEtape = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etape", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Etape_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coureur",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Genre = table.Column<string>(type: "text", nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "timestamp", nullable: false),
                    NumDossard = table.Column<int>(type: "integer", nullable: false),
                    EquipeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coureur", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coureur_Equipe_EquipeId",
                        column: x => x.EquipeId,
                        principalTable: "Equipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Penalite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EtapeId = table.Column<Guid>(type: "uuid", nullable: false),
                    EquipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TempsPenalite = table.Column<TimeSpan>(type: "interval", nullable: false),
                    Etat = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Penalite_Equipe_EquipeId",
                        column: x => x.EquipeId,
                        principalTable: "Equipe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Penalite_Etape_EtapeId",
                        column: x => x.EtapeId,
                        principalTable: "Etape",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoureurCategorie",
                columns: table => new
                {
                    CategorieId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoureurId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoureurCategorie", x => new { x.CategorieId, x.CoureurId });
                    table.ForeignKey(
                        name: "FK_CoureurCategorie_Categorie_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoureurCategorie_Coureur_CoureurId",
                        column: x => x.CoureurId,
                        principalTable: "Coureur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoureurEtape",
                columns: table => new
                {
                    CoureurId = table.Column<Guid>(type: "uuid", nullable: false),
                    EtapeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoureurEtape", x => new { x.CoureurId, x.EtapeId });
                    table.ForeignKey(
                        name: "FK_CoureurEtape_Coureur_CoureurId",
                        column: x => x.CoureurId,
                        principalTable: "Coureur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CoureurEtape_Etape_EtapeId",
                        column: x => x.EtapeId,
                        principalTable: "Etape",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resultat",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EtapeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CoureurId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateArrivee = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resultat_Coureur_CoureurId",
                        column: x => x.CoureurId,
                        principalTable: "Coureur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resultat_Etape_EtapeId",
                        column: x => x.EtapeId,
                        principalTable: "Etape",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coureur_EquipeId",
                table: "Coureur",
                column: "EquipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Coureur_NumDossard",
                table: "Coureur",
                column: "NumDossard",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoureurCategorie_CoureurId",
                table: "CoureurCategorie",
                column: "CoureurId");

            migrationBuilder.CreateIndex(
                name: "IX_CoureurEtape_EtapeId",
                table: "CoureurEtape",
                column: "EtapeId");

            migrationBuilder.CreateIndex(
                name: "IX_Etape_CourseId",
                table: "Etape",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalite_EquipeId",
                table: "Penalite",
                column: "EquipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Penalite_EtapeId",
                table: "Penalite",
                column: "EtapeId");

            migrationBuilder.CreateIndex(
                name: "IX_Resultat_CoureurId",
                table: "Resultat",
                column: "CoureurId");

            migrationBuilder.CreateIndex(
                name: "IX_Resultat_EtapeId",
                table: "Resultat",
                column: "EtapeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "ClassementEquipeCategorie");

            migrationBuilder.DropTable(
                name: "CoureurCategorie");

            migrationBuilder.DropTable(
                name: "CoureurEtape");

            migrationBuilder.DropTable(
                name: "CsvEtape");

            migrationBuilder.DropTable(
                name: "CsvPoints");

            migrationBuilder.DropTable(
                name: "CsvResultat");

            migrationBuilder.DropTable(
                name: "Penalite");

            migrationBuilder.DropTable(
                name: "PointEtape");

            migrationBuilder.DropTable(
                name: "Resultat");

            migrationBuilder.DropTable(
                name: "Categorie");

            migrationBuilder.DropTable(
                name: "Coureur");

            migrationBuilder.DropTable(
                name: "Etape");

            migrationBuilder.DropTable(
                name: "Equipe");

            migrationBuilder.DropTable(
                name: "Course");
        }
    }
}
