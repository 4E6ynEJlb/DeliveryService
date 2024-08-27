using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Goods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Article = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    AverageCookingTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    AlreadyCooked = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: true),
                    AlreadyRated = table.Column<int>(type: "int", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Adress = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Adress);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _Name = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    TelegramId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Bonuses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    _Login = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    _Password = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Number = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CVV = table.Column<short>(type: "smallint", nullable: false),
                    Valid = table.Column<DateOnly>(type: "date", nullable: false),
                    Holder = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Number);
                    table.ForeignKey(
                        name: "FK_Cards_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Number",
                table: "Cards",
                column: "Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_UserId",
                table: "Cards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Article",
                table: "Goods",
                column: "Article",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Goods_Id",
                table: "Goods",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_Adress",
                table: "Restaurants",
                column: "Adress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_Login",
                table: "Restaurants",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_Password",
                table: "Restaurants",
                column: "Password",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users__Login",
                table: "Users",
                column: "_Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users__Password",
                table: "Users",
                column: "_Password",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Id",
                table: "Users",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TelegramId",
                table: "Users",
                column: "TelegramId",
                unique: true,
                filter: "[TelegramId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Goods");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
