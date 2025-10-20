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
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(UsuarioService usuarioService, ILogger<UsuarioController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View(new Usuario());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Cadastro(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            try
            {
                var usuarioExistente = await _usuarioService.ObterPorEmailAsync(usuario.Email);
                if (usuarioExistente != null)
                {
                    ModelState.AddModelError("Email", "Email já cadastrado");
                    return View(usuario);
                }

                await _usuarioService.AdicionarUsuarioAsync(usuario);
                HttpContext.Session.SetInt32("UserId", usuario.Id);
                HttpContext.Session.SetString("UserName", usuario.Nome);


                return RedirectToAction("Index", "Tarefa");
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(usuario);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginRequestDTO());
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
                return View(request);

            try
            {
                var usuario = await _usuarioService.Autenticar(request.Email, request.Senha);
                if (usuario == null)
                {

                    ModelState.AddModelError(string.Empty, "Email ou senha incorretos");
                    return View(request);
                }

                HttpContext.Session.SetInt32("UserId", usuario.Id);
                HttpContext.Session.SetString("UserName", usuario.Nome);

                return RedirectToAction("Index", "Tarefa");
            }
            catch (InvalidOperationException e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(request);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return View(request);
            }
        }



        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var usuarioId = HttpContext.Session.GetInt32("UserId");
            if (usuarioId == null)
            {
                return RedirectToAction(nameof(Login));

            }
            try
            {
                var usuario = await _usuarioService.ObterPorIdAsync(usuarioId.Value);
                return View(usuario);
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction(nameof(Login));

            }
            catch (ArgumentException)
            {
                return RedirectToAction(nameof(Login));
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult LogoutConfirmado()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public async Task<IActionResult> Detalhes()
        {
            var usuarioId = HttpContext.Session.GetInt32("UserId");
            if (usuarioId == null)
            {
                return RedirectToAction(nameof(Login));
                
            }
            try
            {
                var usuario = await _usuarioService.ObterPorIdAsync(usuarioId.Value);
                return View(usuario);
            }
            catch (ArgumentException e)
            {
                _logger.LogError(e, "ID de usuário inválido na sessão: {UserId}", usuarioId);
                return RedirectToAction(nameof(Index), nameof(Tarefa));
            }
            catch(InvalidOperationException e)
            {
                _logger.LogError(e, "Erro ao carregar detalhes do usuário {UserId}", usuarioId);
                return RedirectToAction(nameof(Index), nameof(Tarefa));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Erro inesperado ao carregar detalhes do usuário {UserId}", usuarioId);
                return RedirectToAction(nameof(Index), nameof(Tarefa));
            }


        }




    }
}
