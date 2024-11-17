using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MangaScraper.Data.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Generi",
                columns: table => new
                {
                    NameId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GenereEnum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Generi", x => x.NameId);
                });

            migrationBuilder.CreateTable(
                name: "Mangas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CopertinaUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stato = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Autore = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Artista = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Trama = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mangas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MangaGenere",
                columns: table => new
                {
                    MangaId = table.Column<int>(type: "int", nullable: false),
                    GenereId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaGenere", x => new { x.MangaId, x.GenereId });
                    table.ForeignKey(
                        name: "FK_MangaGenere_Generi_GenereId",
                        column: x => x.GenereId,
                        principalTable: "Generi",
                        principalColumn: "NameId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaGenere_Mangas_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Mangas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Volumi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumVolume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MangaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volumi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Volumi_Mangas_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Mangas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Capitoli",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumCapitolo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VolumeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Capitoli", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Capitoli_Volumi_VolumeId",
                        column: x => x.VolumeId,
                        principalTable: "Volumi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ImagePositions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PathImg = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CapitoloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagePositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImagePositions_Capitoli_CapitoloId",
                        column: x => x.CapitoloId,
                        principalTable: "Capitoli",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Generi",
                columns: new[] { "NameId", "GenereEnum" },
                values: new object[,]
                {
                    { "ArtiMarziali", 0 },
                    { "Avventura", 1 },
                    { "Azione", 2 },
                    { "Commedia", 3 },
                    { "Doujinshi", 4 },
                    { "Drammatico", 5 },
                    { "Ecchi", 6 },
                    { "Fantasy", 7 },
                    { "GenderBender", 8 },
                    { "Harem", 9 },
                    { "Hentai", 10 },
                    { "Horror", 11 },
                    { "Josei", 12 },
                    { "Lolicon", 13 },
                    { "Maturo", 14 },
                    { "Mecha", 15 },
                    { "Mistero", 16 },
                    { "Psicologico", 17 },
                    { "Romantico", 18 },
                    { "Scifi", 19 },
                    { "Scolastico", 20 },
                    { "Seinen", 21 },
                    { "Shotacon", 22 },
                    { "Shoujo", 23 },
                    { "ShoujoAi", 24 },
                    { "Shounen", 25 },
                    { "ShounenAi", 26 },
                    { "SliceofLife", 27 },
                    { "Smut", 28 },
                    { "Soprannaturale", 29 },
                    { "Sport", 30 },
                    { "Storico", 31 },
                    { "Tragico", 32 },
                    { "Yaoi", 33 },
                    { "Yuri", 34 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Capitoli_VolumeId",
                table: "Capitoli",
                column: "VolumeId");

            migrationBuilder.CreateIndex(
                name: "IX_ImagePositions_CapitoloId",
                table: "ImagePositions",
                column: "CapitoloId");

            migrationBuilder.CreateIndex(
                name: "IX_MangaGenere_GenereId",
                table: "MangaGenere",
                column: "GenereId");

            migrationBuilder.CreateIndex(
                name: "IX_Volumi_MangaId",
                table: "Volumi",
                column: "MangaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagePositions");

            migrationBuilder.DropTable(
                name: "MangaGenere");

            migrationBuilder.DropTable(
                name: "Capitoli");

            migrationBuilder.DropTable(
                name: "Generi");

            migrationBuilder.DropTable(
                name: "Volumi");

            migrationBuilder.DropTable(
                name: "Mangas");
        }
    }
}
