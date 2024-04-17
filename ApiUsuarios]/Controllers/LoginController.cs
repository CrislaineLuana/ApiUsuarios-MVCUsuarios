using ApiUsuarios_.Dto.Login;
using ApiUsuarios_.Dto.Usuario;
using ApiUsuarios_.Services.Usuario;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiUsuarios_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly IUsuarioInterface _usuarioInterface;
        public LoginController(IUsuarioInterface usuarioInterface)
        {
            _usuarioInterface = usuarioInterface;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegistrarUsuario(UsuarioCriacaoDto usuarioCriacaoDto)
        {

            var usuario = await _usuarioInterface.RegistrarUsuario(usuarioCriacaoDto);
            return Ok(usuario);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLogin)
        {

            var resposta = await _usuarioInterface.Login(usuarioLogin);
            return Ok(resposta);
        }
    }
}
