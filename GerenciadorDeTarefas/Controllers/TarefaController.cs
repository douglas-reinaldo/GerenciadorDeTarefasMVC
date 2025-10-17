using GerenciadorDeTarefas.Filters;
using Microsoft.AspNetCore.Mvc;
using GerenciadorDeTarefas.Services;
using GerenciadorDeTarefas.Models;
using GerenciadorDeTarefas.Models.ViewModels;
using GerenciadorDeTarefas.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            try
            {
                var tarefas = await _tarefaService.GetTarefasAsync(userId);
                return View(tarefas);
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return View(new List<Tarefa>());
            }

        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            TarefaFormViewModel viewModel = new TarefaFormViewModel();
            viewModel.statusLista = new SelectList(Enum.GetValues<Status>());
            viewModel.prioridadeLista = new SelectList(Enum.GetValues<Prioridade>());

            return View(viewModel);
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(Tarefa tarefa)
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
                await _tarefaService.AddTarefaAsync(tarefa, userId);

                TempData["Success"] = "Tarefa criada com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {               
                ModelState.AddModelError(string.Empty, ex.Message);
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
        public async Task<IActionResult> Edit(int? Id)
        {

            if (Id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                Tarefa tarefa = await _tarefaService.ObterTarefaPorIdAsync(Id.Value);
                var viewModel = new TarefaFormViewModel();

                int usuarioLogadoId = HttpContext.Session.GetInt32("UserId").Value;

                if (tarefa.UsuarioId != usuarioLogadoId)
                {
                    return RedirectToAction(nameof(Index));
                }
                ;
                viewModel.tarefa = tarefa;
                viewModel.statusLista = new SelectList(Enum.GetValues<Status>());
                viewModel.prioridadeLista = new SelectList(Enum.GetValues<Prioridade>());

                return View(viewModel);
            }
            catch (InvalidOperationException ex) 
            {
                return RedirectToAction(nameof(Index));
            }
        }



        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Edit(Tarefa tarefa)
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
                var tarefaExistente = await _tarefaService.ObterTarefaPorIdAsync(tarefa.Id);

                int usuarioLogadoId = HttpContext.Session.GetInt32("UserId").Value;
                if (tarefaExistente.UsuarioId != usuarioLogadoId)
                {
                    return RedirectToAction(nameof(Index));
                }

                tarefaExistente.Titulo = tarefa.Titulo;
                tarefaExistente.Descricao = tarefa.Descricao;
                tarefaExistente.Status = tarefa.Status;
                tarefaExistente.Prioridade = tarefa.Prioridade;

                await _tarefaService.AtualizarTarefa(tarefaExistente);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
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
        public async Task<IActionResult> Details(int? Id)
        {
            try
            {
                Tarefa tarefa = await _tarefaService.ObterTarefaPorIdAsync(Id.Value);

                if (tarefa.UsuarioId != HttpContext.Session.GetInt32("UserId").Value)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(tarefa);
            }
            catch (InvalidOperationException ex)
            {
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Delete(int? Id)
        {
            try
            {
                Tarefa tarefa = await _tarefaService.ObterTarefaPorIdAsync(Id.Value);

                if (tarefa.UsuarioId != HttpContext.Session.GetInt32("UserId").Value)
                {
                    return RedirectToAction(nameof(Index));
                }

                await _tarefaService.DeletarTarefa(tarefa);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }


    }
}
