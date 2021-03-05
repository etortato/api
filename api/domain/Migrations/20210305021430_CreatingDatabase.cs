using Microsoft.EntityFrameworkCore.Migrations;

namespace domain.Migrations
{
    public partial class CreatingDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "TimeOfDays",
                columns: table => new
                {
                    TimeOfDayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeOfDays", x => x.TimeOfDayId);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    DishId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanHaveMultiple = table.Column<bool>(type: "bit", nullable: false),
                    TimeOfDayId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.DishId);
                    table.ForeignKey(
                        name: "FK_Dishes_TimeOfDays_TimeOfDayId",
                        column: x => x.TimeOfDayId,
                        principalTable: "TimeOfDays",
                        principalColumn: "TimeOfDayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDishes",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    DishId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDishes", x => new { x.DishId, x.OrderId });
                    table.ForeignKey(
                        name: "FK_OrderDishes_Dishes_DishId",
                        column: x => x.DishId,
                        principalTable: "Dishes",
                        principalColumn: "DishId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDishes_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TimeOfDays",
                columns: new[] { "TimeOfDayId", "Name" },
                values: new object[] { 1, "morning" });

            migrationBuilder.InsertData(
                table: "TimeOfDays",
                columns: new[] { "TimeOfDayId", "Name" },
                values: new object[] { 2, "night" });

            migrationBuilder.InsertData(
                table: "Dishes",
                columns: new[] { "DishId", "CanHaveMultiple", "Name", "Number", "TimeOfDayId" },
                values: new object[,]
                {
                    { 1, false, "eggs", 1, 1 },
                    { 2, false, "toast", 2, 1 },
                    { 3, true, "coffee", 3, 1 },
                    { 4, false, "steak", 1, 2 },
                    { 5, true, "potato", 2, 2 },
                    { 6, false, "wine", 3, 2 },
                    { 7, false, "cake", 4, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_TimeOfDayId",
                table: "Dishes",
                column: "TimeOfDayId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDishes_OrderId",
                table: "OrderDishes",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDishes");

            migrationBuilder.DropTable(
                name: "Dishes");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "TimeOfDays");
        }
    }
}
