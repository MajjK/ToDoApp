using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Controllers
{
    public class UsersController : Controller
    {
        private ToDoDatabaseContext DbContext;

        public UsersController(ToDoDatabaseContext context)
        {
            DbContext = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await DbContext.Users.Include(s => s.Tasks).ToListAsync());
        }

    }
}
