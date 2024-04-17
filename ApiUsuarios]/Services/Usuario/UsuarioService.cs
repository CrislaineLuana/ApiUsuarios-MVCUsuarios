using ApiUsuarios_.Data;
using ApiUsuarios_.Dto.Login;
using ApiUsuarios_.Dto.Usuario;
using ApiUsuarios_.Model;
using ApiUsuarios_.Services.Senha;
using Azure;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace ApiUsuarios_.Services.Usuario
{
    public class UsuarioService : IUsuarioInterface
    {
        private readonly AppDbContext _context;
        private readonly ISenhaInterface _senhaInterface;

        public UsuarioService(AppDbContext context, ISenhaInterface senhaInterface)
        {
            _context = context;
            _senhaInterface = senhaInterface;
        }

        public async Task<ResponseModel<UsuarioModel>> BuscarUsuarioPorId(int id)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    response.Mensagem = "Não localizado!";
                    return response;
                }

                response.Dados = usuario;
                response.Mensagem = "Usuário localizado";

                return response;

            } catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> EditarUsuario(UsuarioEdicaoDto usuarioEdicaoDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuarioBanco = await _context.Usuarios.FindAsync(usuarioEdicaoDto.Id);

                if (usuarioBanco == null)
                {
                    response.Mensagem = "Não localizado!";
                    return response;
                }

                usuarioBanco.Nome = usuarioEdicaoDto.Nome;
                usuarioBanco.Sobrenome = usuarioEdicaoDto.Sobrenome;
                usuarioBanco.Email = usuarioEdicaoDto.Email;
                usuarioBanco.Usuario = usuarioEdicaoDto.Usuario;

                _context.Update(usuarioBanco);
                await _context.SaveChangesAsync();


                response.Mensagem = "Usuário Editado com sucesso!";
                response.Dados = usuarioBanco;

                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }
    

        public async Task<ResponseModel<List<UsuarioModel>>> ListarUsuarios()
        {
            ResponseModel<List<UsuarioModel>> response = new ResponseModel<List<UsuarioModel>>();
            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();

                response.Dados = usuarios;

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioCriacaoDto usuarioCriacaoDto)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {


                if (!VerificaSeExisteEmailUsuario(usuarioCriacaoDto))
                {
                    response.Mensagem = "Email/Usuário já cadastrado!";
                    return response;
                }

                _senhaInterface.CriarSenhaHash(usuarioCriacaoDto.Senha, out byte[] senhaHash, out byte[] senhaSalt);


                UsuarioModel usuario = new UsuarioModel()
                {
                    Usuario = usuarioCriacaoDto.Usuario,
                    Email = usuarioCriacaoDto.Email,
                    Nome = usuarioCriacaoDto.Nome,
                    Sobrenome = usuarioCriacaoDto.Sobrenome,
                    SenhaHash = senhaHash,
                    SenhaSalt = senhaSalt
                };

                _context.Add(usuario);
                await _context.SaveChangesAsync();


                response.Mensagem = "Usuário Cadastrado com Sucesso!";
                response.Dados = usuario;
                return response;

            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public bool VerificaSeExisteEmailUsuario(UsuarioCriacaoDto usuarioCriacaoDto)
        {
            var usuario =  _context.Usuarios.FirstOrDefault(u => u.Email == usuarioCriacaoDto.Email || u.Usuario == usuarioCriacaoDto.Usuario);

            if (usuario != null)
            {
                return false;
            }

            return true;
        
        }

        public async Task<ResponseModel<UsuarioModel>> RemoverUsuario(int id)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);

                if (usuario == null)
                {
                    response.Mensagem = "Não localizado!";
                    return response;
                }

                response.Dados = usuario;
                response.Mensagem = "Usuário Removido com Sucesso!";

                _context.Remove(usuario);
                await _context.SaveChangesAsync();

                return response;
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                response.Status = false;
                return response;
            }
        }

        public async Task<ResponseModel<UsuarioModel>> Login(UsuarioLoginDto usuarioLogin)
        {
            ResponseModel<UsuarioModel> response = new ResponseModel<UsuarioModel>();

            try
            {

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(userBanco => userBanco.Email == usuarioLogin.Email);

                if (usuario == null)
                {
                    response.Mensagem = "Credenciais inválidas!";
                    response.Status = false;
                    return response;
                }

                if (!_senhaInterface.VerificaSenhaHash(usuarioLogin.Senha, usuario.SenhaHash, usuario.SenhaSalt))
                {
                    response.Mensagem = "Credenciais inválidas!";
                    response.Status = false;
                    return response;
                }

                var token = _senhaInterface.CriarToken(usuario);

                usuario.Token = token;

                _context.Update(usuario);
                _context.SaveChangesAsync();
                
                response.Dados = usuario;
                response.Mensagem = "Usuário logado com sucesso!";

                
            }
            catch (Exception ex)
            {
                response.Dados = null;
                response.Mensagem = ex.Message;
                response.Status = false;
            }


            return response;
        }
    }
}
