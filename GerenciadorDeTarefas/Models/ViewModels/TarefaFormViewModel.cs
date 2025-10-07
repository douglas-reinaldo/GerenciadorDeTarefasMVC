using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GerenciadorDeTarefas.Models.ViewModels
{
    public class TarefaFormViewModel
    {
        public Tarefa tarefa { get; set; } = new Tarefa();
        public SelectList statusLista { get; set; }
    }
}
