using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicService.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArtistTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    CoverUrl = table.Column<string>(type: "text", nullable: false),
                    ArtistTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Artists_ArtistTypes_ArtistTypeId",
                        column: x => x.ArtistTypeId,
                        principalTable: "ArtistTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Releases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReleaseTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CoverUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Releases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Releases_ReleaseTypes_ReleaseTypeId",
                        column: x => x.ReleaseTypeId,
                        principalTable: "ReleaseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ArtistUsers",
                columns: table => new
                {
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtistUsers", x => new { x.ArtistId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ArtistUsers_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseArtists",
                columns: table => new
                {
                    ReleaseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseArtists", x => new { x.ReleaseId, x.ArtistId });
                    table.ForeignKey(
                        name: "FK_ReleaseArtists_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReleaseArtists_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    ReleaseId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracks_Releases_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Releases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagTracks",
                columns: table => new
                {
                    TrackId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagTracks", x => new { x.TagId, x.TrackId });
                    table.ForeignKey(
                        name: "FK_TagTracks_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagTracks_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackArtists",
                columns: table => new
                {
                    TrackId = table.Column<Guid>(type: "uuid", nullable: false),
                    ArtistId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackArtists", x => new { x.TrackId, x.ArtistId });
                    table.ForeignKey(
                        name: "FK_TrackArtists_Artists_ArtistId",
                        column: x => x.ArtistId,
                        principalTable: "Artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackArtists_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ArtistTypes",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("20af1149-c5c6-40e2-868e-755dff6be93d"), "Band", "band" },
                    { new Guid("84e328a2-56d2-4099-8c34-c12c41056fef"), "Musician", "musician" }
                });

            migrationBuilder.InsertData(
                table: "ReleaseTypes",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("3558d638-e3cf-4a4a-aadf-ce216afd670a"), "Other", "other" },
                    { new Guid("53b200e5-fcc6-4528-a2ab-3c0ad6a4065a"), "Album", "album" },
                    { new Guid("983a3e85-3c5c-46ac-b067-6824ed955fbb"), "Soundtrack", "soundtrack" },
                    { new Guid("a841d69b-56a5-4b80-8ad9-78b32ba5c124"), "EP", "ep" },
                    { new Guid("c7f13388-a018-493c-9ff8-03c575167ee9"), "Single", "single" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("a6370b10-0b0b-4ba2-9480-cd18621f2d25"), "Electronic", "Electronic" },
                    { new Guid("a758b7a5-8fc1-4a8f-ad0b-93b7805588bc"), "Lo-Fi", "lo-fi" },
                    { new Guid("b3a4fb15-15f1-4317-8283-d8a02e6ed4e3"), "Shoegaze", "shoegaze" },
                    { new Guid("c1fd5974-8c54-4134-874f-c935a8cd5d04"), "Rock", "rock" },
                    { new Guid("c2372919-3a7e-4bca-b9dd-00dc108f5325"), "Pop", "pop" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTypes_Name",
                table: "ArtistTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArtistTypes_Slug",
                table: "ArtistTypes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artists_ArtistTypeId",
                table: "Artists",
                column: "ArtistTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Artists_Name",
                table: "Artists",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Artists_Slug",
                table: "Artists",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseArtists_ArtistId",
                table: "ReleaseArtists",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseTypes_Name",
                table: "ReleaseTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseTypes_Slug",
                table: "ReleaseTypes",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Releases_ReleaseTypeId",
                table: "Releases",
                column: "ReleaseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Releases_Slug",
                table: "Releases",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagTracks_TrackId",
                table: "TagTracks",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Slug",
                table: "Tags",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrackArtists_ArtistId",
                table: "TrackArtists",
                column: "ArtistId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_ReleaseId",
                table: "Tracks",
                column: "ReleaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_Slug",
                table: "Tracks",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtistUsers");

            migrationBuilder.DropTable(
                name: "ReleaseArtists");

            migrationBuilder.DropTable(
                name: "TagTracks");

            migrationBuilder.DropTable(
                name: "TrackArtists");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "ArtistTypes");

            migrationBuilder.DropTable(
                name: "Releases");

            migrationBuilder.DropTable(
                name: "ReleaseTypes");
        }
    }
}
