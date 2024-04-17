using System.ComponentModel.DataAnnotations;

namespace ApiUsuarios_.Dto.Usuario
{
    public class UsuarioEdicaoDto
    {
        public int Id { get; set; }
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

    }
}
