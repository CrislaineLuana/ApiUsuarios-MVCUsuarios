using System.ComponentModel.DataAnnotations;

namespace MVCUsuarios.Dto.Usuario
{
    public class UsuarioCriacaoDto
    {
        [Required(ErrorMessage = "Digite o usuário")]
        public string Usuario { get; set; } = string.Empty;
        [Required(ErrorMessage = "Digite o nome")]
        public string Nome { get; set; } = string.Empty;
        [Required(ErrorMessage = "Digite o sobrenome")]
        public string Sobrenome { get; set; } = string.Empty;
        [Required(ErrorMessage = "Digite o email")]
        public string Email { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Digite a senha")]
        public string Senha { get; set; } = string.Empty;
        [Required(ErrorMessage = "Digite a cofirmação de senha"), Compare("Senha")]
        public string ConfirmaSenha { get; set; } = string.Empty;
    }
}
