using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoModel;
using TodoWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using TodoWebApp.Models.TodoViewModels;

namespace TodoWebApp.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        public TodoController(ITodoRepository repository, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            // Get currently logged -in user using userManager
            ApplicationUser currentUser = await
            _userManager.GetUserAsync(HttpContext.User);
            var todos = _repository.GetActive(new Guid(currentUser.Id));

            return View(todos);
        }

        public async Task<IActionResult> Todos()
        {
            // Get currently logged -in user using userManager
            ApplicationUser currentUser = await
            _userManager.GetUserAsync(HttpContext.User);
            var todos = _repository.GetActive(new Guid(currentUser.Id));

            return View(todos);
        }

        // GET: /Todo/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Todo/Add
        [HttpPost]
        public async Task<IActionResult> Add(AddTodoViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Get currently logged -in user using userManager
                ApplicationUser currentUser = await
                _userManager.GetUserAsync(HttpContext.User);

                _repository.Add(new TodoItem(model.Text, new Guid(currentUser.Id)));
                return RedirectToAction(nameof(Todos));
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
               return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Todo/Completed/{id?}
        [HttpGet]
        public async Task<IActionResult> Completed(String id = null)
        {
            // Get currently logged -in user using userManager
            ApplicationUser currentUser = await
                _userManager.GetUserAsync(HttpContext.User);

            if (id != null)
            {
                _repository.MarkAsCompleted(new Guid(id), new Guid(currentUser.Id));
                return RedirectToAction(nameof(Todos));
            } else
            {
                var todos = _repository.GetCompleted(new Guid(currentUser.Id));
                // If we got this far, something failed, redisplay form
                return View(todos);
            }
        }
    }
}