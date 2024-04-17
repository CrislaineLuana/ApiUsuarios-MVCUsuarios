using MVCUsuarios.Models;

namespace MVCUsuarios.Services.Sessao
{
    public interface ISessaoInterface
    {
        UsuarioModel BuscarSessao();
        void CriarSessao(UsuarioModel usuario);
        void RemoverSessao();
    }
}
