using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicService.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverUrlToTrackModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArtistTypes",
                keyColumn: "Id",
                keyValue: new Guid("24b29830-dec4-47ca-8f8d-dc88c2833325"));

            migrationBuilder.DeleteData(
                table: "ArtistTypes",
                keyColumn: "Id",
                keyValue: new Guid("8b3e0710-7088-49ff-a2f6-1398ad70a070"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("41b08d67-cf66-4c7e-9bbe-edb88571c54a"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("58c68224-84b6-419c-90b3-c8163f70e2c6"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("8f0cdc1c-0d7d-457c-9e10-f8623343a92c"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("e39e393b-b399-40df-a9b0-e367b402ec08"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("e600ecc8-d0c3-44fe-8d02-dd287d0c9a5e"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("614f9aac-825d-4319-b42b-ae94327df3f4"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("8a7275d3-9142-41e6-815d-9ed91d428fbe"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("c2bae778-33a2-442a-927d-3616211402d8"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("e7102cd5-efa8-4f06-ac5a-e60427d63c75"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("ea9a7166-dc82-477c-9ced-7382475c1510"));

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                table: "Tracks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "ArtistTypes",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("627789f8-6120-47f2-aaa3-deb85ffb79b3"), "Musician", "musician" },
                    { new Guid("72fb8b0e-c5e3-4f13-89ee-2e51fef84a74"), "Band", "band" }
                });

            migrationBuilder.InsertData(
                table: "ReleaseTypes",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("648fb1ea-b2f0-441e-8eb6-72e2d2015a79"), "Soundtrack", "soundtrack" },
                    { new Guid("7fdd6237-6f4c-49f8-9c15-09c9fe10e297"), "EP", "ep" },
                    { new Guid("c324de6a-2ee4-481c-97fe-ac06e89b3642"), "Album", "album" },
                    { new Guid("d18f7426-f75b-4ce6-a25e-27ffa26a18c6"), "Single", "single" },
                    { new Guid("ed6f65fd-87b0-4f06-8236-5f42f0e823c0"), "Other", "other" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("07ca573c-8aa7-45ad-a8b3-ceeb12876571"), "Electronic", "Electronic" },
                    { new Guid("39e89b8b-33a6-4381-9476-7e9c34d7aa94"), "Pop", "pop" },
                    { new Guid("455b9a55-5569-4d8b-8150-ef6fc8cbcb09"), "Lo-Fi", "lo-fi" },
                    { new Guid("5124a6d7-80b8-42b8-bba6-daa8642e7b14"), "Shoegaze", "shoegaze" },
                    { new Guid("fece3b9e-4b08-4b2d-8111-f86b049d5cfa"), "Rock", "rock" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArtistTypes",
                keyColumn: "Id",
                keyValue: new Guid("627789f8-6120-47f2-aaa3-deb85ffb79b3"));

            migrationBuilder.DeleteData(
                table: "ArtistTypes",
                keyColumn: "Id",
                keyValue: new Guid("72fb8b0e-c5e3-4f13-89ee-2e51fef84a74"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("648fb1ea-b2f0-441e-8eb6-72e2d2015a79"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("7fdd6237-6f4c-49f8-9c15-09c9fe10e297"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("c324de6a-2ee4-481c-97fe-ac06e89b3642"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("d18f7426-f75b-4ce6-a25e-27ffa26a18c6"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("ed6f65fd-87b0-4f06-8236-5f42f0e823c0"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("07ca573c-8aa7-45ad-a8b3-ceeb12876571"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("39e89b8b-33a6-4381-9476-7e9c34d7aa94"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("455b9a55-5569-4d8b-8150-ef6fc8cbcb09"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("5124a6d7-80b8-42b8-bba6-daa8642e7b14"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("fece3b9e-4b08-4b2d-8111-f86b049d5cfa"));

            migrationBuilder.DropColumn(
                name: "CoverUrl",
                table: "Tracks");

            migrationBuilder.InsertData(
                table: "ArtistTypes",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("24b29830-dec4-47ca-8f8d-dc88c2833325"), "Musician", "musician" },
                    { new Guid("8b3e0710-7088-49ff-a2f6-1398ad70a070"), "Band", "band" }
                });

            migrationBuilder.InsertData(
                table: "ReleaseTypes",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("41b08d67-cf66-4c7e-9bbe-edb88571c54a"), "Other", "other" },
                    { new Guid("58c68224-84b6-419c-90b3-c8163f70e2c6"), "Album", "album" },
                    { new Guid("8f0cdc1c-0d7d-457c-9e10-f8623343a92c"), "Single", "single" },
                    { new Guid("e39e393b-b399-40df-a9b0-e367b402ec08"), "EP", "ep" },
                    { new Guid("e600ecc8-d0c3-44fe-8d02-dd287d0c9a5e"), "Soundtrack", "soundtrack" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("614f9aac-825d-4319-b42b-ae94327df3f4"), "Pop", "pop" },
                    { new Guid("8a7275d3-9142-41e6-815d-9ed91d428fbe"), "Lo-Fi", "lo-fi" },
                    { new Guid("c2bae778-33a2-442a-927d-3616211402d8"), "Electronic", "Electronic" },
                    { new Guid("e7102cd5-efa8-4f06-ac5a-e60427d63c75"), "Shoegaze", "shoegaze" },
                    { new Guid("ea9a7166-dc82-477c-9ced-7382475c1510"), "Rock", "rock" }
                });
        }
    }
}
