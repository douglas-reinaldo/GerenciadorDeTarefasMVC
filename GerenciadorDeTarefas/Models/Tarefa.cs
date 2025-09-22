using GerenciadorDeTarefas.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeTarefas.Models
{
    public class Tarefa
    {
        public int Id { get; set; }


        [Display(Name = "Titulo da Tarefa")]
        public string Titulo { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }


        [Display(Name = "Data de Criação")]
        [BindNever]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Status da Tarefa")]
        public Status Status { get; set; }

        [BindNever]
        public int UsuarioId { get; set; }

        [BindNever]
        [ValidateNever]
        public Usuario Usuario { get; set; }


    }
}
