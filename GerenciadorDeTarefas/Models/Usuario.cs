using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GerenciadorDeTarefas.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome é obrigatório")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Nome deve conter entre 3 e 20 caracteres")]
        public string Nome { get; set; }



        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }


        [BindNever]
        [ValidateNever]
        public string SenhaHash { get; set; }



        [NotMapped]
        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "A senha deve conter entre 6 e 50 caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }


        public List<Tarefa> Tarefas { get; set; } = new List<Tarefa>();

    }
}
