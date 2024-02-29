using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MusicService.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorIdToTrackAndRelease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ArtistTypes",
                keyColumn: "Id",
                keyValue: new Guid("20af1149-c5c6-40e2-868e-755dff6be93d"));

            migrationBuilder.DeleteData(
                table: "ArtistTypes",
                keyColumn: "Id",
                keyValue: new Guid("84e328a2-56d2-4099-8c34-c12c41056fef"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("3558d638-e3cf-4a4a-aadf-ce216afd670a"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("53b200e5-fcc6-4528-a2ab-3c0ad6a4065a"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("983a3e85-3c5c-46ac-b067-6824ed955fbb"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("a841d69b-56a5-4b80-8ad9-78b32ba5c124"));

            migrationBuilder.DeleteData(
                table: "ReleaseTypes",
                keyColumn: "Id",
                keyValue: new Guid("c7f13388-a018-493c-9ff8-03c575167ee9"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("a6370b10-0b0b-4ba2-9480-cd18621f2d25"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("a758b7a5-8fc1-4a8f-ad0b-93b7805588bc"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("b3a4fb15-15f1-4317-8283-d8a02e6ed4e3"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("c1fd5974-8c54-4134-874f-c935a8cd5d04"));

            migrationBuilder.DeleteData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: new Guid("c2372919-3a7e-4bca-b9dd-00dc108f5325"));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Tracks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Releases",
                type: "text",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Releases");

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
        }
    }
}
