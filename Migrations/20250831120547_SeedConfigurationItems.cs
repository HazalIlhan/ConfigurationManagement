using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConfigurationManagement.Migrations
{
    /// <inheritdoc />
    public partial class SeedConfigurationItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ConfigurationItems",
                columns: new[] { "Id", "ApplicationName", "IsActive", "Name", "Type", "Value" },
                values: new object[,]
                {
                    { 1, "SERVICE-A", true, "SiteName", "string", "soty.io" },
                    { 2, "SERVICE-B", true, "IsBasketEnabled", "bool", "true" },
                    { 3, "SERVICE-A", false, "MaxItemCount", "int", "50" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ConfigurationItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ConfigurationItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ConfigurationItems",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
