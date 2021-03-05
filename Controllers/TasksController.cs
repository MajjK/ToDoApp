using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;

namespace ToDoApp.Controllers
{
    public class TasksController : Controller
    {
        private ToDoDatabaseContext DbContext;

        public TasksController(ToDoDatabaseContext context)
        {
            DbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await DbContext.Tasks.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await DbContext.Tasks
                .Include(s => s.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.TaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            return View(task);
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
                DbContext.Tasks.Add(task);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(task);
        }

        public ActionResult Edit(int? id)
        {
            ToDoApp.Models.Task task = DbContext.Tasks.Find(id);
            return View(task);
        }
    }
}
