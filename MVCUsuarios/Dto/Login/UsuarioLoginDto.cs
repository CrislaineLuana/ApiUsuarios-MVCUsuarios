using System.ComponentModel.DataAnnotations;

namespace MVCUsuarios.Dto.Login
{
    public class UsuarioLoginDto
    {
        [Required(ErrorMessage = "Digite o email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Digite a senha")]
        public string Senha { get; set; }
    }
}
