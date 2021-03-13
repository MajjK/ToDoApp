using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using X.PagedList;
using AutoMapper;
using ToDoApp.DB;
using ToDoApp.DB.Model;
using ToDoApp.ViewModel.Tasks;
//Error method w auth controller

namespace ToDoApp.Controllers
{
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
            var tasksViewModel = this.GetMappedViewModel(tasks);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(await tasksViewModel.ToPagedListAsync(pageNumber, pageSize));
        }

        private List<DbTask> GetMappedModel(List<TaskViewModel> tasksViewModel)
        {
            List<DbTask> tasks = new List<DbTask>();
            foreach (var item in tasksViewModel)
            {
                DbTask task = _mapper.Map<DbTask>(item);
                tasks.Add(task);
            }
            return tasks;
        }

        private List<TaskViewModel> GetMappedViewModel (IQueryable<DbTask> tasks)
        {
            List<TaskViewModel> tasksViewModel = new List<TaskViewModel>();
            foreach (var item in tasks)
            {
                TaskViewModel taskViewModel = _mapper.Map<TaskViewModel>(item);
                tasksViewModel.Add(taskViewModel);
            }
            return tasksViewModel;
        }

        private IQueryable<DbTask> GetSortedTasks(string sortOrder, string searchString)
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Objective, Description, ClosingDate")] TaskViewModel taskViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DbTask taskModel = _mapper.Map<DbTask>(taskViewModel);
                    DbContext.Add(taskModel);
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
            return View(taskViewModel);
        }      
        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await DbContext.Tasks
                .Include(task => task.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(task => task.TaskId == id);

            if (task == null)
            {
                return NotFound();
            }

            TaskViewModel taskViewModel = _mapper.Map<TaskViewModel>(task);
            return View(taskViewModel);
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

            TaskViewModel taskViewModel = _mapper.Map<TaskViewModel>(task);
            return View(taskViewModel);
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
            if (await TryUpdateModelAsync<DbTask>(taskToUpdate, "",
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

            TaskViewModel taskViewModel = _mapper.Map<TaskViewModel>(taskToUpdate);
            return View(taskViewModel);
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

            TaskViewModel taskViewModel = _mapper.Map<TaskViewModel>(task);
            return View(taskViewModel);
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
