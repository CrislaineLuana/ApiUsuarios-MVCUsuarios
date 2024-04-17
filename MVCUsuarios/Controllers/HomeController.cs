using Microsoft.AspNetCore.Mvc;
using MVCUsuarios.Dto.Login;
using MVCUsuarios.Dto.Usuario;
using MVCUsuarios.Models;
using MVCUsuarios.Services.Sessao;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace MVCUsuarios.Controllers
{
    public class HomeController : Controller
    {

        Uri baseUrl = new Uri("https://localhost:7100/api");
        
        private readonly HttpClient _httpClient;
        private readonly ISessaoInterface _sessaoInterface;

        public HomeController(HttpClient httpClient, ISessaoInterface sessaoInterface)
        {

            _httpClient = httpClient;
            _sessaoInterface = sessaoInterface;
            _httpClient.BaseAddress = baseUrl;

        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {  
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _sessaoInterface.RemoverSessao();
            return RedirectToAction("Login");
        }


        [HttpGet]
        public async Task<IActionResult> Registrar()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditarUsuario(int id)
        {

            UsuarioModel usuario = _sessaoInterface.BuscarSessao();

            if (usuario == null)
            {
                TempData["MensagemErro"] = "É necessário estar logado para acessar essa página";
                return RedirectToAction("Login");
            }

            ResponseModelMvc<UsuarioModel> usuarios = new ResponseModelMvc<UsuarioModel>();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress + "/Usuario/" + Convert.ToInt32(id)))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);
                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    usuarios = JsonConvert.DeserializeObject<ResponseModelMvc<UsuarioModel>>(data);
                }

                var usuarioEdicaoDto = new UsuarioEdicaoDto
                {
                    Id = usuarios.Dados.Id,
                    Nome = usuarios.Dados.Nome,
                    Sobrenome = usuarios.Dados.Sobrenome,
                    Email = usuarios.Dados.Email,
                    Usuario = usuarios.Dados.Usuario
                };


                return View(usuarioEdicaoDto);


            };

          
        }


        [HttpGet]
        public async Task<IActionResult> RemoverUsuario(int id)
        {
            UsuarioModel usuario = _sessaoInterface.BuscarSessao();

            if (usuario == null)
            {
                TempData["MensagemErro"] = "É necessário estar logado para acessar essa página";
                return RedirectToAction("Login");
            }

            ResponseModelMvc<UsuarioModel> usuarios = new ResponseModelMvc<UsuarioModel>();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Delete, _httpClient.BaseAddress + "/Usuario?id=" + Convert.ToInt32(id)))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);
               
                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    usuarios = JsonConvert.DeserializeObject<ResponseModelMvc<UsuarioModel>>(data);
                }

                TempData["MensagemSucesso"] = usuarios.Mensagem;
                return RedirectToAction("ListarUsuarios");


            };

        }


        [HttpGet]
        public async Task<IActionResult> ListarUsuarios()
        {
            UsuarioModel usuario = _sessaoInterface.BuscarSessao();

            if(usuario == null)
            {
                TempData["MensagemErro"] = "É necessário estar logado para acessar essa página";
                return RedirectToAction("Login");
            }

          
            ResponseModelMvc<List<UsuarioModel>> usuarios = new ResponseModelMvc<List<UsuarioModel>>();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, _httpClient.BaseAddress + "/Usuario"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);
                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    usuarios = JsonConvert.DeserializeObject<ResponseModelMvc<List<UsuarioModel>>>(data);
                }

                return View(usuarios.Dados);


            };

               

            
        }


        [HttpPost]
        public async Task<IActionResult> EditarUsuario(UsuarioEdicaoDto usuarioEdicaoDto)
        {
            UsuarioModel usuario = _sessaoInterface.BuscarSessao();

            if (usuario == null)
            {
                TempData["MensagemErro"] = "É necessário estar logado para acessar essa página";
                return RedirectToAction("Login");
            }

            ResponseModelMvc<UsuarioModel> usuarios = new ResponseModelMvc<UsuarioModel>();

            using (var requestMessage = new HttpRequestMessage(HttpMethod.Put, _httpClient.BaseAddress + "/Usuario"))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", usuario.Token);
  
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(usuarioEdicaoDto), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.SendAsync(requestMessage);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    usuarios = JsonConvert.DeserializeObject<ResponseModelMvc<UsuarioModel>>(data);
                }

                TempData["MensagemSucesso"] = usuarios.Mensagem;
                return RedirectToAction("ListarUsuarios");



            };




        }



        [HttpPost]
        public async Task<IActionResult> Registrar(UsuarioCriacaoDto usuarioCriacaoDto)
        {

            if (ModelState.IsValid)
            {
                ResponseModelMvc<UsuarioModel> usuarios = new ResponseModelMvc<UsuarioModel>();

                var httpContent = new StringContent(JsonConvert.SerializeObject(usuarioCriacaoDto), Encoding.UTF8, "application/json");
                

                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Login/register", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    usuarios = JsonConvert.DeserializeObject<ResponseModelMvc<UsuarioModel>>(data);
                }

                if (usuarios.Status == false)
                {
                    return View(usuarioCriacaoDto);
                }


                TempData["MensagemSucesso"] = usuarios.Mensagem;                
                return RedirectToAction("Login");
                
            }
            else
            {
                TempData["MensagemErro"] = "Ocorreu um erro no processo, procure pelo suporte!";
                return View(usuarioCriacaoDto);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLoginDto)
        {

            if(ModelState.IsValid)
            {
                ResponseModelMvc<UsuarioModel> usuarios = new ResponseModelMvc<UsuarioModel>();

                var httpContent = new StringContent(JsonConvert.SerializeObject(usuarioLoginDto), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Login/login", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    usuarios = JsonConvert.DeserializeObject<ResponseModelMvc<UsuarioModel>>(data);
                }

                if (usuarios.Status == false)
                {
                    return RedirectToAction("Login");
                }

                _sessaoInterface.CriarSessao(usuarios.Dados);

                return RedirectToAction("ListarUsuarios");
            }
            else
            {
                return View(usuarioLoginDto);
            }
           
        }


    }
}
