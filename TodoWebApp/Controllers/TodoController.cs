using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Interfaces;

namespace TodoWebApp.Controllers
{
    public class TodoController : Controller
    {
        private readonly ITodoRepository _repository;
        public TodoController(ITodoRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

    }
}