using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace TradeApp.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "trd");

            migrationBuilder.CreateTable(
                name: "Filter",
                schema: "trd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<long>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    Key = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filter", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "trd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<long>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    Logins = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "trd",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Id = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<long>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Widget",
                schema: "trd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<long>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Widget", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDashboard",
                schema: "trd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<long>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDashboard", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDashboard_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "trd",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDashboardWidget",
                schema: "trd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<long>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    WidgetId = table.Column<int>(nullable: true),
                    UserDashboardId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    XAxis = table.Column<int>(nullable: false),
                    YAxis = table.Column<int>(nullable: false),
                    Index = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDashboardWidget", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDashboardWidget_UserDashboard_UserDashboardId",
                        column: x => x.UserDashboardId,
                        principalSchema: "trd",
                        principalTable: "UserDashboard",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDashboardWidget_Widget_WidgetId",
                        column: x => x.WidgetId,
                        principalSchema: "trd",
                        principalTable: "Widget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDashboardWidgetFilter",
                schema: "trd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedById = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedById = table.Column<long>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    UserDashboardWidgetId = table.Column<int>(nullable: false),
                    FilterId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDashboardWidgetFilter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserDashboardWidgetFilter_Filter_FilterId",
                        column: x => x.FilterId,
                        principalSchema: "trd",
                        principalTable: "Filter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDashboardWidgetFilter_UserDashboardWidget_UserDashboard~",
                        column: x => x.UserDashboardWidgetId,
                        principalSchema: "trd",
                        principalTable: "UserDashboardWidget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserDashboard_UserId",
                schema: "trd",
                table: "UserDashboard",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDashboardWidget_UserDashboardId",
                schema: "trd",
                table: "UserDashboardWidget",
                column: "UserDashboardId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDashboardWidget_WidgetId",
                schema: "trd",
                table: "UserDashboardWidget",
                column: "WidgetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDashboardWidgetFilter_FilterId",
                schema: "trd",
                table: "UserDashboardWidgetFilter",
                column: "FilterId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDashboardWidgetFilter_UserDashboardWidgetId",
                schema: "trd",
                table: "UserDashboardWidgetFilter",
                column: "UserDashboardWidgetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tags",
                schema: "trd");

            migrationBuilder.DropTable(
                name: "UserDashboardWidgetFilter",
                schema: "trd");

            migrationBuilder.DropTable(
                name: "Filter",
                schema: "trd");

            migrationBuilder.DropTable(
                name: "UserDashboardWidget",
                schema: "trd");

            migrationBuilder.DropTable(
                name: "UserDashboard",
                schema: "trd");

            migrationBuilder.DropTable(
                name: "Widget",
                schema: "trd");

            migrationBuilder.DropTable(
                name: "User",
                schema: "trd");
        }
    }
}
