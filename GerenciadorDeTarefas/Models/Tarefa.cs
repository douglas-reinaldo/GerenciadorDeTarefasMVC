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
        [Required(ErrorMessage = "O Titulo é obrigatório")]
        [StringLength(100, ErrorMessage = "O Titulo só permite até 100 caracteres")]
        public string Titulo { get; set; }


        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "A Descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "A Descrição só permite até 100 caracteres")]
        public string Descricao { get; set; }


        [Display(Name = "Data de Criação")]
        [BindNever]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime DataCriacao { get; set; }


        [Display(Name = "Status da Tarefa")]
        [Required(ErrorMessage = "É necessario definir um status")]
        [EnumDataType(typeof(Status), ErrorMessage = "Status Inválido")]
        public Status Status { get; set; }

        [BindNever]
        public int UsuarioId { get; set; }

        [BindNever]
        [ValidateNever]
        public Usuario Usuario { get; set; }


        [Display(Name = "Prioridadde da Tarefa")]
        [Required(ErrorMessage = "É necessario definir uma prioridade")]
        public Prioridade Prioridade { get; set; }


    }
}
