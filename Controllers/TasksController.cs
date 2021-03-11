using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using AutoMapper;
using ToDoApp.DB;
using ToDoApp.ViewModel;

namespace ToDoApp.Controllers
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<DB.Model.DbTask, ViewModel.Tasks.TaskViewModel>();
        }
    }

    public class TasksController : Controller
    {
        private ToDoDatabaseContext DbContext;
        private readonly IMapper _mapper;

        public TasksController(ToDoDatabaseContext context, IMapper mapper)
        {
            DbContext = context;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["FinishSortParm"] = String.IsNullOrEmpty(sortOrder) ? "finish" : "";
            ViewData["ObjectiveSortParm"] = sortOrder == "objective" ? "objective_desc" : "objective";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            ViewData["CurrentFilter"] = searchString;

            var tasks = this.GetSortedTasks(sortOrder, searchString);
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<ViewModel.Tasks.TaskViewModel> tasksViewModel = new List<ViewModel.Tasks.TaskViewModel>();
            foreach (var item in tasks)
            {
                ViewModel.Tasks.TaskViewModel taskViewModel = _mapper.Map<ViewModel.Tasks.TaskViewModel>(item);
                tasksViewModel.Add(taskViewModel);
            }

            return View(await tasksViewModel.ToPagedListAsync(pageNumber, pageSize));
        }

        private IQueryable<ToDoApp.DB.Model.DbTask> GetSortedTasks(string sortOrder, string searchString)
        {
            var tasks = from s in DbContext.Tasks
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                if (DateTime.TryParse(searchString, out DateTime check_date))
                    tasks = tasks.Where(s => s.Objective.Contains(searchString) || s.ClosingDate.Value.Date.Equals(check_date));
                else
                    tasks = tasks.Where(s => s.Objective.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "finish":
                    tasks = tasks.OrderBy(s => s.Finished).ThenBy(s => s.ClosingDate);
                    break;
                case "objective":
                    tasks = tasks.OrderBy(s => s.Objective);
                    break;
                case "objective_desc":
                    tasks = tasks.OrderByDescending(s => s.Objective);
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
            return tasks;
        }

        /*
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
        */
    }
}
