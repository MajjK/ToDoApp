using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Controllers
{
    public class TasksController : Controller
    {
        private ToDoDatabaseContext DbContext;

        public TasksController(ToDoDatabaseContext context)
        {
            DbContext = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["FinishSortParm"] = String.IsNullOrEmpty(sortOrder) ? "finish" : "";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["CurrentFilter"] = searchString;

            var tasks = from s in DbContext.Tasks
                        select s;
            if (!String.IsNullOrEmpty(searchString))
                tasks = tasks.Where(s => s.Objective.Contains(searchString) || s.ClosingDate.Value.ToString().Contains(searchString));//Problem z data
            switch (sortOrder)
            {
                case "finish":
                    tasks = tasks.OrderBy(s => s.Finished).ThenBy(s => s.ClosingDate);
                    break;
                case "date":
                    tasks = tasks.OrderBy(s => s.ClosingDate).ThenByDescending(s => s.Finished);
                    break;
                case "date_desc":
                    tasks = tasks.OrderByDescending(s => s.ClosingDate).ThenByDescending(s => s.Finished);
                    break;
                default:
                    tasks = tasks.OrderByDescending(s => s.Finished).ThenBy(s => s.ClosingDate);
                    break;
            }
            return View(await tasks.AsNoTracking().ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Objective, Description, ClosingDate")] ToDoApp.Models.Task task)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DbContext.Add(task);
                    await DbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(task);
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await DbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var taskToUpdate = await DbContext.Tasks.FirstOrDefaultAsync(s => s.TaskId == id);
            if (await TryUpdateModelAsync<ToDoApp.Models.Task>(taskToUpdate, "",
                s => s.Objective, s => s.Description, s => s.AdditionDate, s => s.ClosingDate, s => s.Finished))
            {
                try
                {
                    await DbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(taskToUpdate);
        }

        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await DbContext.Tasks.AsNoTracking().FirstOrDefaultAsync(s => s.TaskId == id);
            if (task == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await DbContext.Tasks.FindAsync(id);
            if (task == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                DbContext.Tasks.Remove(task);
                await DbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }
    }
}
