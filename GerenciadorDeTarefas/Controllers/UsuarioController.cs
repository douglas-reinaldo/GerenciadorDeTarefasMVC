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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Cadastro(Usuario usuario) 
        {
            if (!ModelState.IsValid) 
            {
              
                return View(usuario);
            }

            var usuarioExistente = _usuarioService.obterPorEmail(usuario.Email);
            if (usuarioExistente != null) 
            {
                ModelState.AddModelError("Email", "Email já cadastrado");
                return View(usuario);
            }

            if (_usuarioService.SenhaJaExiste(usuario.Senha)) 
            {
                ModelState.AddModelError("Senha", "Senha já cadastrada");
                return View(usuario);
            }

            _usuarioService.AdicionarUsuario(usuario);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginRequestDTO());
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Login(LoginRequestDTO request) 
        {
            if (!ModelState.IsValid) 
            {
                return View(request);
            }
            var usuario = _usuarioService.Autenticar(request.Email, request.Senha);
            if (usuario == null) 
            {
                return Unauthorized("Email ou senha incorretos");
            }

            HttpContext.Session.SetInt32("UserId", usuario.Id);
            HttpContext.Session.SetString("UserName", usuario.Nome);

            return RedirectToAction("Index", "Tarefa");
        }


        [HttpGet]
        public IActionResult Logout() 
        {
            var usuarioId = HttpContext.Session.GetInt32("UserId");
            if (usuarioId != null) 
            {
                
                var usuario = _usuarioService.obterPorId(usuarioId.Value);
                return View(usuario);
            }
            return RedirectToAction(nameof(Login));
            
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult LogoutConfirmado() 
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Detalhes() 
        {
            var usuarioId = HttpContext.Session.GetInt32("UserId");
            if (usuarioId != null)
            {
                var usuario = _usuarioService.obterPorId(usuarioId.Value);
                return View(usuario);
            }
            return RedirectToAction(nameof(Login));
        }


      

    }
}
