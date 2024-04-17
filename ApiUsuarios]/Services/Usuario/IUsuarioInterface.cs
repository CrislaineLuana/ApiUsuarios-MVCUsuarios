using ApiUsuarios_.Dto.Login;
using ApiUsuarios_.Dto.Usuario;
using ApiUsuarios_.Model;
using Azure;

namespace ApiUsuarios_.Services.Usuario
{
    public interface IUsuarioInterface
    {

        Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDto usuarioCriacaoDto);
        Task<ResponseModel<List<UsuarioModel>>> ListarUsuarios();
        Task<ResponseModel<UsuarioModel>> BuscarUsuarioPorId(int id);
        Task<ResponseModel<UsuarioModel>> RemoverUsuario(int id);
        Task<ResponseModel<UsuarioModel>> EditarUsuario(UsuarioEdicaoDto usuarioEdicaoDto);
        Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLogin);
    }
}
