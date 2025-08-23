using Microsoft.AspNetCore.Mvc;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Services;
using Microsoft.AspNetCore.Identity;
using GerenciadorDeTarefas.DTO;
using System.Reflection.Metadata.Ecma335;

namespace GerenciadorDeTarefas.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;


        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View(new Usuario());
        }

        [HttpPost]
        public IActionResult Cadastro(Usuario usuario) 
        {
            
            _usuarioService.AdicionarUsuario(usuario);
            return RedirectToAction(nameof(Cadastro));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginRequestDTO());
        }

        [HttpPost("Login")]
        public IActionResult Login(LoginRequestDTO request) 
        {
            var usuario = _usuarioService.Autenticar(request.Email, request.Senha);
            if (usuario == null) 
            {
                return Unauthorized("Email ou senha incorretos");
            }

            HttpContext.Session.SetInt32("UserId", usuario.Id);
            HttpContext.Session.SetString("UserName", usuario.Nome);

            return Ok();
        }
        
    }
}
