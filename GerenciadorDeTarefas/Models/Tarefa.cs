using GerenciadorDeTarefas.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GerenciadorDeTarefas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }

        [BindNever]
        public DateTime DataCriacao { get; set; }

        public Status Status { get; set; }

        [BindNever]
        public int UsuarioId { get; set; }

        [BindNever]
        [ValidateNever]
        public Usuario Usuario { get; set; }


    }
}
