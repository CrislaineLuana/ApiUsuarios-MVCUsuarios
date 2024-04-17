namespace ApiUsuarios_.Model
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Sobrenome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public DateTime DataAlteracao { get; set; } = DateTime.Now;
        public byte[] SenhaSalt { get; set; } 
        public byte[] SenhaHash { get; set; }
    }
}
