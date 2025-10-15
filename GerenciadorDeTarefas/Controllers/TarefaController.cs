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
            viewModel.prioridadeLista = new SelectList(Enum.GetValues<Prioridade>());

            return View(viewModel);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Tarefa tarefa)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new TarefaFormViewModel
                {
                    tarefa = tarefa,
                    statusLista = new SelectList(Enum.GetValues<Status>()),
                    prioridadeLista = new SelectList(Enum.GetValues<Prioridade>())
                };
                return View(viewModel);
            }

            try
            {
                int userId = HttpContext.Session.GetInt32("UserId").Value;
                _tarefaService.AddTarefa(tarefa, userId);

                TempData["Success"] = "Tarefa criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {               
                ModelState.AddModelError(string.Empty, "Ocorreu um erro ao criar a tarefa. Tente novamente.");
            }

            var errorViewModel = new TarefaFormViewModel
            {
                tarefa = tarefa,
                statusLista = new SelectList(Enum.GetValues<Status>()),
                prioridadeLista = new SelectList(Enum.GetValues<Prioridade>())
            };
            return View(errorViewModel);
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

            int usuarioLogadoId = HttpContext.Session.GetInt32("UserId").Value;
            if (tarefa == null)
            {
                return NotFound("Tarefa não encontrada");
            }
            if (tarefa.UsuarioId != usuarioLogadoId)
            {
                return Unauthorized("Você não tem permissão para editar essa tarefa.");
            };
            viewModel.tarefa = tarefa;
            viewModel.statusLista = new SelectList(Enum.GetValues<Status>());
            viewModel.prioridadeLista = new SelectList(Enum.GetValues<Prioridade>());

            return View(viewModel);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Tarefa tarefa)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new TarefaFormViewModel
                {
                    tarefa = tarefa,
                    statusLista = new SelectList(Enum.GetValues<Status>()),
                    prioridadeLista = new SelectList(Enum.GetValues<Prioridade>())
                };
                return View(viewModel);
            }

            var tarefaExistente = _tarefaService.ObterTarefaPorId(tarefa.Id);
            if (tarefaExistente == null)
            {
                return NotFound("Tarefa não encontrada");
            }

            int usuarioLogadoId = HttpContext.Session.GetInt32("UserId").Value;
            if (tarefaExistente.UsuarioId != usuarioLogadoId)
            {
                return Unauthorized("Você não tem permissão para editar essa tarefa.");
            }


            tarefaExistente.Titulo = tarefa.Titulo;
            tarefaExistente.Descricao = tarefa.Descricao;
            tarefaExistente.Status = tarefa.Status;
            tarefaExistente.Prioridade = tarefa.Prioridade;

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
