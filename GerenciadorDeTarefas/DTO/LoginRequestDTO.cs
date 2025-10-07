using System.ComponentModel.DataAnnotations;

namespace GerenciadorDeTarefas.DTO
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "A senha deve conter entre 6 e 50 caracteres")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}
