using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicService.Migrations
{
    /// <inheritdoc />
    public partial class AddFileRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArtistTypes",
                keyColumn: "Id",
                keyValue: new Guid("9e485c47-461c-4cc0-874a-e9d2d469461e"));

            migrationBuilder.DeleteData(
                table: "ArtistTypes",
                keyColumn: "Id",
                keyValue: new Guid("b861f3d7-c02f-4d7b-bfce-4e8a224e5afa"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("13d6da3a-1d10-4543-81b4-239df1a8f310"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("163dd204-d500-4769-b352-9cf921c66ff0"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("6b8f2f94-1ec3-4d65-84d1-141324014a38"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("e479477a-c26e-49a2-b1a4-f6b82597612f"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("f37c03aa-7500-44f3-9c3f-510a63c271ed"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("1d85e803-f3d9-4c08-a5b1-fdc6404280a4"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("2da9bb5b-1c82-48a8-b0ff-b5a531235c7d"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("85419c9c-1f96-44ad-a91f-6c1d8f590fc8"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("c74d5032-e3a9-4309-8cf5-b57e6f255149"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("e00ee89e-053e-467d-a6cf-0ebf2df80ff0"));

            migrationBuilder.AddColumn<Guid>(
                name: "AudioFileId",
                table: "Tracks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "AudioUrl",
                table: "Tracks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CoverFileId",
                table: "Releases",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CoverFileId",
                table: "Artists",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "AudioFileId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "AudioUrl",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "CoverFileId",
                table: "Releases");

            migrationBuilder.DropColumn(
                name: "CoverFileId",
                table: "Artists");

            migrationBuilder.InsertData(
                table: "ArtistTypes",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("9e485c47-461c-4cc0-874a-e9d2d469461e"), "Musician", "musician" },
                    { new Guid("b861f3d7-c02f-4d7b-bfce-4e8a224e5afa"), "Band", "band" }
                });

            migrationBuilder.InsertData(
                table: "ReleaseTypes",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("13d6da3a-1d10-4543-81b4-239df1a8f310"), "Soundtrack", "soundtrack" },
                    { new Guid("163dd204-d500-4769-b352-9cf921c66ff0"), "EP", "ep" },
                    { new Guid("6b8f2f94-1ec3-4d65-84d1-141324014a38"), "Single", "single" },
                    { new Guid("e479477a-c26e-49a2-b1a4-f6b82597612f"), "Album", "album" },
                    { new Guid("f37c03aa-7500-44f3-9c3f-510a63c271ed"), "Other", "other" }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("1d85e803-f3d9-4c08-a5b1-fdc6404280a4"), "Rock", "rock" },
                    { new Guid("2da9bb5b-1c82-48a8-b0ff-b5a531235c7d"), "Lo-Fi", "lo-fi" },
                    { new Guid("85419c9c-1f96-44ad-a91f-6c1d8f590fc8"), "Pop", "pop" },
                    { new Guid("c74d5032-e3a9-4309-8cf5-b57e6f255149"), "Shoegaze", "shoegaze" },
                    { new Guid("e00ee89e-053e-467d-a6cf-0ebf2df80ff0"), "Electronic", "Electronic" }
                });
        }
    }
}
