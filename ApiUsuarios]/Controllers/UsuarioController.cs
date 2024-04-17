using ApiUsuarios_.Dto.Usuario;
using ApiUsuarios_.Services.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiUsuarios_.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioInterface _usuarioInterface;
        public UsuarioController(IUsuarioInterface usuarioInterface)
        {
            _usuarioInterface = usuarioInterface;
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarUsuarioPorId(int id)
        {

            var usuario = await _usuarioInterface.BuscarUsuarioPorId(id);
            return Ok(usuario);
        }


        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {

            var usuario = await _usuarioInterface.ListarUsuarios();
            return Ok(usuario);
        }


        [HttpPut]
        public async Task<IActionResult> EditarUsuario(UsuarioEdicaoDto usuarioEdicaoDto)
        {

            var usuario = await _usuarioInterface.EditarUsuario(usuarioEdicaoDto);
            return Ok(usuario);
        }

        [HttpDelete]
        public async Task<IActionResult> RemoverUsuario(int id)
        {

            var usuario = await _usuarioInterface.RemoverUsuario(id);
            return Ok(usuario);
        }

    }
}
