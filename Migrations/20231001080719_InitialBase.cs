using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigRepair.Migrations
{
    /// <inheritdoc />
    public partial class InitialBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Masters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Masters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkKinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkKinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RepairObjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Address = table.Column<string>(type: "TEXT", nullable: true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairObjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepairObjects_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Unit = table.Column<string>(type: "TEXT", nullable: true),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    WorkKindID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkTypes_WorkKinds_WorkKindID",
                        column: x => x.WorkKindID,
                        principalTable: "WorkKinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClientId = table.Column<int>(type: "INTEGER", nullable: true),
                    RepairObjectId = table.Column<int>(type: "INTEGER", nullable: true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Amount = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Accounts_RepairObjects_RepairObjectId",
                        column: x => x.RepairObjectId,
                        principalTable: "RepairObjects",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BigRepairData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RepairObjectId = table.Column<int>(type: "INTEGER", nullable: true),
                    MasterId = table.Column<int>(type: "INTEGER", nullable: true),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    WorkTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Count = table.Column<double>(type: "REAL", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BigRepairData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BigRepairData_Masters_MasterId",
                        column: x => x.MasterId,
                        principalTable: "Masters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BigRepairData_RepairObjects_RepairObjectId",
                        column: x => x.RepairObjectId,
                        principalTable: "RepairObjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BigRepairData_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RepairObjectId = table.Column<int>(type: "INTEGER", nullable: true),
                    WorkTypeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Count = table.Column<double>(type: "REAL", nullable: false),
                    Amount = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkLists_RepairObjects_RepairObjectId",
                        column: x => x.RepairObjectId,
                        principalTable: "RepairObjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkLists_WorkTypes_WorkTypeId",
                        column: x => x.WorkTypeId,
                        principalTable: "WorkTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ClientId",
                table: "Accounts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_RepairObjectId",
                table: "Accounts",
                column: "RepairObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BigRepairData_MasterId",
                table: "BigRepairData",
                column: "MasterId");

            migrationBuilder.CreateIndex(
                name: "IX_BigRepairData_RepairObjectId",
                table: "BigRepairData",
                column: "RepairObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BigRepairData_WorkTypeId",
                table: "BigRepairData",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RepairObjects_ClientId",
                table: "RepairObjects",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLists_RepairObjectId",
                table: "WorkLists",
                column: "RepairObjectId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkLists_WorkTypeId",
                table: "WorkLists",
                column: "WorkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTypes_WorkKindID",
                table: "WorkTypes",
                column: "WorkKindID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "BigRepairData");

            migrationBuilder.DropTable(
                name: "WorkLists");

            migrationBuilder.DropTable(
                name: "Masters");

            migrationBuilder.DropTable(
                name: "RepairObjects");

            migrationBuilder.DropTable(
                name: "WorkTypes");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "WorkKinds");
        }
    }
}
