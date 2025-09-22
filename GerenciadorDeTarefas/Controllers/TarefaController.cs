using GerenciadorDeTarefas.Filters;
using Microsoft.AspNetCore.Mvc;
using GerenciadorDeTarefas.Services;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.ViewModels;
using GerenciadorDeTarefas.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            TarefaFormViewModel viewModel = new TarefaFormViewModel();
            viewModel.statusLista = new SelectList(Enum.GetValues<Status>());

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Tarefa tarefa) 
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            _tarefaService.AddTarefa(tarefa, userId);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Edit(int? Id) 
        {

            if (Id == null) 
            {
                return RedirectToAction(nameof(Index));
            }

            Tarefa tarefa = _tarefaService.ObterTarefaPorId(Id.Value);
            var viewModel = new TarefaFormViewModel();

            viewModel.tarefa = tarefa;
            viewModel.statusLista = new SelectList(Enum.GetValues<Status>());

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Tarefa tarefa) 
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine($"{tarefa.Id}, {tarefa.Titulo}, {tarefa.Descricao}, {tarefa.DataCriacao}, {tarefa.UsuarioId}, {tarefa.Status}");
                var viewModel = new TarefaFormViewModel();
                viewModel.statusLista = new SelectList(Enum.GetValues<Status>());
                return View(viewModel);
            }

            var tarefaExistente = _tarefaService.ObterTarefaPorId(tarefa.Id);
            if (tarefaExistente == null) 
            {
                return NotFound("Tarefa não encontrada");
            }

            tarefaExistente.Titulo = tarefa.Titulo;
            tarefaExistente.Descricao = tarefa.Descricao;
            tarefaExistente.Status = tarefa.Status;


            _tarefaService.AtualizarTarefa(tarefaExistente);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int? Id) 
        {
            if (Id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            Tarefa tarefa = _tarefaService.ObterTarefaPorId(Id.Value);

            if (tarefa == null) 
            {
                return NotFound("Tarefa não encontrada");
            }
            if (tarefa.UsuarioId != HttpContext.Session.GetInt32("UserId").Value) 
            {
                
                return Unauthorized("Você não tem permissão para acessar essa tarefa.");
            }
            
            return View(tarefa);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(int? Id) 
        {
            if (Id == null) 
            {
                return RedirectToAction(nameof(Index));
            }

            Tarefa tarefa = _tarefaService.ObterTarefaPorId(Id.Value);
            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            if (tarefa.UsuarioId != HttpContext.Session.GetInt32("UserId").Value)
            {
                return Unauthorized("Você não tem permissão para acessar essa tarefa.");
            }

            _tarefaService.DeletarTarefa(tarefa);
            return RedirectToAction(nameof(Index));
        }


    }
}
