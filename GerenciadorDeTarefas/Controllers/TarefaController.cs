using GerenciadorDeTarefas.Filters;
using Microsoft.AspNetCore.Mvc;
using GerenciadorDeTarefas.Services;
using GerenciadorDeTarefas.Models;

namespace GerenciadorDeTarefas.Controllers
{
    [SessionAuthorizeAttribute]
    public class TarefaController : Controller
    {

        private readonly TarefaService _tarefaService;

        public TarefaController(TarefaService tarefaService)
        {
            _tarefaService = tarefaService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            var tarefas = _tarefaService.GetTarefas(userId);

            return View(tarefas);
        }


        [HttpGet]
        public IActionResult Create() 
        {
            return View(new Tarefa());
        }

        [HttpPost]
        public IActionResult Create(Tarefa tarefa) 
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            _tarefaService.AddTarefa(tarefa, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}
