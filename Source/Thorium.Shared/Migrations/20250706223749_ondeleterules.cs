using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thorium.Shared.Migrations
{
    /// <inheritdoc />
    public partial class ondeleterules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nodes_Tasks_CurrentTaskId",
                table: "Nodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Nodes_LinedUpOnNodeId",
                table: "Tasks");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_JobId",
                table: "Tasks",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_JobId",
                table: "Operations",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nodes_Tasks_CurrentTaskId",
                table: "Nodes",
                column: "CurrentTaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Jobs_JobId",
                table: "Operations",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Jobs_JobId",
                table: "Tasks",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Nodes_LinedUpOnNodeId",
                table: "Tasks",
                column: "LinedUpOnNodeId",
                principalTable: "Nodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nodes_Tasks_CurrentTaskId",
                table: "Nodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Jobs_JobId",
                table: "Operations");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Jobs_JobId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Nodes_LinedUpOnNodeId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_JobId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Operations_JobId",
                table: "Operations");

            migrationBuilder.AddForeignKey(
                name: "FK_Nodes_Tasks_CurrentTaskId",
                table: "Nodes",
                column: "CurrentTaskId",
                principalTable: "Tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Nodes_LinedUpOnNodeId",
                table: "Tasks",
                column: "LinedUpOnNodeId",
                principalTable: "Nodes",
                principalColumn: "Id");
        }
    }
}
