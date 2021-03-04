using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class TasksController : Controller
    {
        private ToDoDatabaseContext _context;

        public TasksController(ToDoDatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Tasks.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ToDoApp.Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(task);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(task);
        }
    }
}
