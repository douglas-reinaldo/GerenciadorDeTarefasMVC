using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GerenciadorDeTarefas.Migrations
{
    /// <inheritdoc />
    public partial class RenameDataCriacao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCricao",
                table: "Tarefa",
                newName: "DataCriacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataCriacao",
                table: "Tarefa",
                newName: "DataCricao");
        }
    }
}
