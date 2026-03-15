using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarCostCalculator.Data.EF.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarExpense",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarInsurance = table.Column<decimal>(type: "decimal(18,2)", nullable: false, precision: 18, scale: 2),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Fuel = table.Column<decimal>(type: "decimal(18,2)", nullable: false, precision: 18, scale: 2),
                    Inspection = table.Column<decimal>(type: "decimal(18,2)", nullable: false, precision: 18, scale: 2),
                    KilometersDriven = table.Column<decimal>(type: "decimal(18,2)", nullable: false, precision: 18, scale: 2),
                    OAMTC = table.Column<decimal>(type: "decimal(18,2)", nullable: false, precision: 18, scale: 2),
                    Other = table.Column<decimal>(type: "decimal(18,2)", nullable: false, precision: 18, scale: 2),
                    Registration = table.Column<decimal>(type: "decimal(18,2)", nullable: false, precision: 18, scale: 2),
                    Service = table.Column<decimal>(type: "decimal(18,2)", nullable: false, precision: 18, scale: 2),
                    Vignette = table.Column<decimal>(type: "decimal(18,2)", nullable: false, precision: 18, scale: 2)
                },
                constraints: table => table.PrimaryKey("PK_CarExpense", x => x.Id));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarExpense");
        }
    }
}
