using Microsoft.AspNetCore.Mvc;
using Proje5.Data;
using Proje5.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace Proje5.Controllers
{
    public class HomeController : Controller
    {
        private readonly ScoutContext context;

        public HomeController(ScoutContext Context)
        {
            this.context = Context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetDashboardData()
        {
            var totalPlayers = context.Players.Count();
            var totalTeams = context.Teams.Count();
            var totalPositions = context.Positions.Count();
            var averageAge = totalPlayers > 0 ? context.Players.Average(p => p.Age) : 0;

            // Group by team counts
            var teamCounts = (from p in context.Players
                              join t in context.Teams on p.TeamId equals t.TeamId into pts
                              from pt in pts.DefaultIfEmpty()
                              group p by pt != null ? pt.TeamName : "Takımsız" into g
                              select new
                              {
                                  Label = g.Key,
                                  Value = g.Count()
                              }).ToList();

            // Group by position counts
            var positionCounts = (from p in context.Players
                                  join pos in context.Positions on p.PositionId equals pos.PositionId into pps
                                  from pp in pps.DefaultIfEmpty()
                                  group p by pp != null ? pp.PositionName : "Pozisyonsuz" into g
                                  select new
                                  {
                                      Label = g.Key,
                                      Value = g.Count()
                                  }).ToList();

            return new JsonResult(new
            {
                TotalPlayers = totalPlayers,
                TotalTeams = totalTeams,
                TotalPositions = totalPositions,
                AverageAge = Math.Round(averageAge, 1),
                TeamCounts = teamCounts,
                PositionCounts = positionCounts
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
