using Microsoft.AspNetCore.Mvc;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Services;

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
        public IActionResult Index()
        {
            return View(new Usuario());
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario) 
        {
            _usuarioService.AdicionarUsuario(usuario);
            return RedirectToAction(nameof(Index));
        }
    }
}
