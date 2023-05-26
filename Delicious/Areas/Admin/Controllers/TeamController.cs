using Delicious.DataContext;
using Delicious.Models;
using Delicious.ViewModels.TeamViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Delicious.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TeamController : Controller
    {
        private readonly DeliciousDbContext _deliciousDbContext;
        private readonly IWebHostEnvironment _environment;

        public TeamController(DeliciousDbContext deliciousDbContext, IWebHostEnvironment environment)
        {
            _deliciousDbContext = deliciousDbContext;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            List<Team> teams = await _deliciousDbContext.Teams.Include(x=>x.Work).ToListAsync();
            TeamIndexVM teamindexVM = new TeamIndexVM()
            {
                Teams = teams
            };
            return View(teamindexVM);
        }
        public async Task<IActionResult> Create()
        {
            TeamCreateVM teamcreateVM = new TeamCreateVM()
            {
                Works = await _deliciousDbContext.Works.ToListAsync(),
            };
            return View(teamcreateVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TeamCreateVM newTeam)
        {
            if (!ModelState.IsValid)
            {
                newTeam.Works = await _deliciousDbContext.Works.ToListAsync();
                return View(newTeam);
            }
            Team team = new Team()
            {
                Name = newTeam.Name,
                Surname = newTeam.Surname,
                WorkId = newTeam.WorkId,
            };
            if(!newTeam.Image.ContentType.Contains("image") && newTeam.Image.Length / 1024 > 2048)
            {
                newTeam.Works = await _deliciousDbContext.Works.ToListAsync();
                ModelState.AddModelError("image", "type or length incorrect");
                return View(newTeam);
            }
            string newFileName=Guid.NewGuid().ToString() + newTeam.Image.FileName;
            string path = Path.Combine(_environment.WebRootPath, "assets", "img", "chefs", newFileName);
            using(FileStream stream=new FileStream(path, FileMode.CreateNew))
            {
                await newTeam.Image.CopyToAsync(stream);
            }
            team.ImageName = newFileName;
            await _deliciousDbContext.Teams.AddAsync(team);
            await _deliciousDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Detail(int id)
        {
            Team? team=await _deliciousDbContext.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }
        public async Task<IActionResult> Edit(int id)
        {
            Team? team=await _deliciousDbContext.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            TeamEditVM editVM= new TeamEditVM()
            {
                Name = team.Name,
                Surname = team.Surname,
                ImageName = team.ImageName,
                WorkId = team.WorkId,
                Works=await _deliciousDbContext.Works.ToListAsync(),
            };
            return View(editVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id,TeamEditVM newTeam)
        {
            if (!ModelState.IsValid)
            {
                newTeam.Works = await _deliciousDbContext.Works.ToListAsync();
                return View(newTeam);
            }
            Team? team = await _deliciousDbContext.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            if (newTeam.Image != null)
            {
              
                string path = Path.Combine(_environment.WebRootPath, "assets", "img", "chefs", team.ImageName);
                using(FileStream stream=new FileStream(path, FileMode.Create))
                {
                    await newTeam.Image.CopyToAsync(stream);
                }
            }
            
            team.Surname = newTeam.Surname;
            team.Name = newTeam.Name;
            team.WorkId = newTeam.WorkId;
            
            
            await _deliciousDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
           
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            Team? team = await _deliciousDbContext.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            string path = Path.Combine(_environment.WebRootPath, "assets", "img", "chefs", team.ImageName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _deliciousDbContext.Teams.Remove(team);
            await _deliciousDbContext.SaveChangesAsync();   
            return RedirectToAction(nameof(Index));

        }

    }
}
