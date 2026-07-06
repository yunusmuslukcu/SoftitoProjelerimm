using Microsoft.AspNetCore.Mvc;
using Proje5.Data;
using Proje5.Models;

namespace Proje5.Controllers
{
    public class TeamController : Controller
    {
        private readonly ScoutContext context;

        public TeamController(ScoutContext Context)
        {
            this.context = Context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult TeamList(string? search)
        {
            var query = context.Teams.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.TeamName.Contains(search) || t.City.Contains(search));
            }

            var data = query
                .Select(t => new
                {
                    t.TeamId,
                    t.TeamName,
                    t.City
                }).ToList();

            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult AddTeam(Team team)
        {
            var tm = new Team()
            {
                TeamName = team.TeamName,
                City = team.City
            };

            context.Teams.Add(tm);
            context.SaveChanges();
            return new JsonResult("data Saved");
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            var data = context.Teams.Where(m => m.TeamId == id).SingleOrDefault();
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult Update(Team team)
        {
            context.Update(team);
            context.SaveChanges();
            return new JsonResult("Record updated");
        }
        public JsonResult Delete(int id)
        {
            var data = context.Teams.Where(m => m.TeamId == id).SingleOrDefault();
            if (data != null)
            {
                context.Teams.Remove(data);
                context.SaveChanges();
            }
            return new JsonResult("data deleted");
        }
    }
}
