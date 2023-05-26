
using Delicious.DataContext;
using Delicious.Models;
using Delicious.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Delicious.Controllers
{
    public class HomeController : Controller
    {
        private readonly DeliciousDbContext _deliciousDbContext;

        public HomeController(DeliciousDbContext deliciousDbContext)
        {
            _deliciousDbContext = deliciousDbContext;
        }

        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _deliciousDbContext.Teams.Include(x=>x.Work).ToListAsync();
            HomeVM homeVM = new HomeVM()
            {
                Teams = teams
            };
            return View(homeVM);
        }

      
    }
}