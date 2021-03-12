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
using ToDoApp.ViewModel.Users;

namespace ToDoApp.Controllers
{
    public class UsersController : Controller
    {
        private ToDoDatabaseContext DbContext;
        private readonly IMapper _mapper;

        public UsersController(ToDoDatabaseContext context, IMapper mapper)
        {
            DbContext = context;
            _mapper = mapper;
        }

        
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["LoginSortParm"] = String.IsNullOrEmpty(sortOrder) ? "login_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["TasksSortParm"] = sortOrder == "tasks" ? "tasks_desc" : "tasks";
            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;
            ViewData["CurrentFilter"] = searchString;

            var users = this.GetSortedUsers(sortOrder, searchString);
            var usersViewModel = this.GetMappedViewModel(users);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(await usersViewModel.ToPagedListAsync(pageNumber, pageSize));
        }

        private List<DbUser> GetMappedModel(List<UserViewModel> usersViewModel)
        {
            List<DbUser> users = new List<DbUser>();
            foreach (var item in usersViewModel)
            {
                DbUser user = _mapper.Map<DbUser>(item);
                users.Add(user);
            }
            return users;
        }

        private List<UserViewModel> GetMappedViewModel(IQueryable<DbUser> users)
        {
            List<UserViewModel> usersViewModel = new List<UserViewModel>();
            foreach (var item in users)
            {
                UserViewModel userViewModel = _mapper.Map<UserViewModel>(item);
                usersViewModel.Add(userViewModel);
            }
            return usersViewModel;
        }

        private IQueryable<DbUser> GetSortedUsers(string sortOrder, string searchString)
        {
            var users = from s in DbContext.Users
                        select s;
            if (!String.IsNullOrEmpty(searchString))
                if (DateTime.TryParse(searchString, out DateTime check_date))
                    users = users.Where(s => s.Login.Contains(searchString) || s.AdditionDate.Value.Date.Equals(check_date));
                else
                    users = users.Where(s => s.Login.Contains(searchString));
            switch (sortOrder)
            {
                case "login_desc":
                    users = users.OrderByDescending(s => s.Login);
                    break;
                case "date":
                    users = users.OrderBy(s => s.AdditionDate);
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.AdditionDate);
                    break;
                case "tasks":
                    users = users.OrderBy(s => s.Tasks.Count);
                    break;
                case "tasks_desc":
                    users = users.OrderByDescending(s => s.Tasks.Count);
                    break;
                default:
                    users = users.OrderBy(s => s.Login);
                    break;
            }
            return users;
        }

        /*
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Login, Password")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DbContext.Add(user);
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
            return View(user);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await DbContext.Users
                .Include(s => s.Tasks)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await DbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userToUpdate = await DbContext.Users.FirstOrDefaultAsync(s => s.UserId == id);
            if (await TryUpdateModelAsync<User>(userToUpdate, "",
                s => s.Login, s => s.Password, s => s.AdditionDate))
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
            return View(userToUpdate);
        }

        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await DbContext.Users.Include(s => s.Tasks).AsNoTracking().FirstOrDefaultAsync(s => s.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await DbContext.Users.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                DbContext.Users.Remove(user);
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
