﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;
using Microsoft.EntityFrameworkCore;
// Pozostawianie Zmian w User/Task po wygenerowaniu nowego modelu

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
