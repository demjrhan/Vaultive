using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MediaContents",
                columns: table => new
                {
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    OriginalLanguage = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Duration = table.Column<int>(type: "INTEGER", nullable: false),
                    SubtitleOptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    AudioOptionId = table.Column<int>(type: "INTEGER", nullable: false),
                    PosterImage = table.Column<string>(type: "TEXT", nullable: true),
                    BackgroundImage = table.Column<string>(type: "TEXT", nullable: true),
                    Discriminator = table.Column<string>(type: "TEXT", maxLength: 13, nullable: false),
                    Topics = table.Column<string>(type: "TEXT", nullable: true),
                    Genres = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaContents", x => x.Title);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Firstname = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Lastname = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Nickname = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AudioOptions",
                columns: table => new
                {
                    MediaTitle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioOptions", x => x.MediaTitle);
                    table.ForeignKey(
                        name: "FK_AudioOptions_MediaContents_MediaTitle",
                        column: x => x.MediaTitle,
                        principalTable: "MediaContents",
                        principalColumn: "Title",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StreamingServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    LogoImage = table.Column<string>(type: "TEXT", nullable: false),
                    MediaContentTitle = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StreamingServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StreamingServices_MediaContents_MediaContentTitle",
                        column: x => x.MediaContentTitle,
                        principalTable: "MediaContents",
                        principalColumn: "Title");
                });

            migrationBuilder.CreateTable(
                name: "SubtitleOptions",
                columns: table => new
                {
                    MediaTitle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubtitleOptions", x => x.MediaTitle);
                    table.ForeignKey(
                        name: "FK_SubtitleOptions_MediaContents_MediaTitle",
                        column: x => x.MediaTitle,
                        principalTable: "MediaContents",
                        principalColumn: "Title",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WatchDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    TimeLeftOf = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    MediaTitle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchHistories", x => x.Id);
                    table.UniqueConstraint("AK_WatchHistories_UserId_MediaTitle", x => new { x.UserId, x.MediaTitle });
                    table.ForeignKey(
                        name: "FK_WatchHistories_MediaContents_MediaTitle",
                        column: x => x.MediaTitle,
                        principalTable: "MediaContents",
                        principalColumn: "Title",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WatchHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AudioLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Language = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MediaTitle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AudioLanguages_AudioOptions_MediaTitle",
                        column: x => x.MediaTitle,
                        principalTable: "AudioOptions",
                        principalColumn: "MediaTitle",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaContentStreamingServices",
                columns: table => new
                {
                    MediaTitle = table.Column<string>(type: "TEXT", nullable: false),
                    StreamingServiceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaContentStreamingServices", x => new { x.MediaTitle, x.StreamingServiceId });
                    table.ForeignKey(
                        name: "FK_MediaContentStreamingServices_MediaContents_MediaTitle",
                        column: x => x.MediaTitle,
                        principalTable: "MediaContents",
                        principalColumn: "Title",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MediaContentStreamingServices_StreamingServices_StreamingServiceId",
                        column: x => x.StreamingServiceId,
                        principalTable: "StreamingServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DefaultPrice = table.Column<double>(type: "REAL", nullable: false),
                    StreamingServiceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_StreamingServices_StreamingServiceId",
                        column: x => x.StreamingServiceId,
                        principalTable: "StreamingServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubtitleLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Language = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MediaTitle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubtitleLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubtitleLanguages_SubtitleOptions_MediaTitle",
                        column: x => x.MediaTitle,
                        principalTable: "SubtitleOptions",
                        principalColumn: "MediaTitle",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rating = table.Column<double>(type: "REAL", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    MediaTitle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_MediaContents_MediaTitle",
                        column: x => x.MediaTitle,
                        principalTable: "MediaContents",
                        principalColumn: "Title",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_WatchHistories_UserId_MediaTitle",
                        columns: x => new { x.UserId, x.MediaTitle },
                        principalTable: "WatchHistories",
                        principalColumns: new[] { "UserId", "MediaTitle" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionConfirmations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PaymentMethod = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubscriptionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionConfirmations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionConfirmations_Subscriptions_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubscriptionConfirmations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioLanguages_MediaTitle",
                table: "AudioLanguages",
                column: "MediaTitle");

            migrationBuilder.CreateIndex(
                name: "IX_MediaContents_OriginalLanguage",
                table: "MediaContents",
                column: "OriginalLanguage");

            migrationBuilder.CreateIndex(
                name: "IX_MediaContents_ReleaseDate",
                table: "MediaContents",
                column: "ReleaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_MediaContentStreamingServices_StreamingServiceId",
                table: "MediaContentStreamingServices",
                column: "StreamingServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MediaTitle",
                table: "Reviews",
                column: "MediaTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId_MediaTitle",
                table: "Reviews",
                columns: new[] { "UserId", "MediaTitle" });

            migrationBuilder.CreateIndex(
                name: "IX_StreamingServices_MediaContentTitle",
                table: "StreamingServices",
                column: "MediaContentTitle");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionConfirmations_SubscriptionId",
                table: "SubscriptionConfirmations",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionConfirmations_UserId",
                table: "SubscriptionConfirmations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_StreamingServiceId",
                table: "Subscriptions",
                column: "StreamingServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_SubtitleLanguages_MediaTitle",
                table: "SubtitleLanguages",
                column: "MediaTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Nickname",
                table: "Users",
                column: "Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WatchHistories_MediaTitle",
                table: "WatchHistories",
                column: "MediaTitle");

            migrationBuilder.CreateIndex(
                name: "IX_WatchHistories_TimeLeftOf",
                table: "WatchHistories",
                column: "TimeLeftOf");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioLanguages");

            migrationBuilder.DropTable(
                name: "MediaContentStreamingServices");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "SubscriptionConfirmations");

            migrationBuilder.DropTable(
                name: "SubtitleLanguages");

            migrationBuilder.DropTable(
                name: "AudioOptions");

            migrationBuilder.DropTable(
                name: "WatchHistories");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "SubtitleOptions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "StreamingServices");

            migrationBuilder.DropTable(
                name: "MediaContents");
        }
    }
}
