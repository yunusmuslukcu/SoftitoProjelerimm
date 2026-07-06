using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proje5.Data;
using Proje5.Models;
using System.Linq;

namespace Proje5.Controllers
{
    public class ReportController : Controller
    {
        private readonly ScoutContext context;

        public ReportController(ScoutContext Context)
        {
            this.context = Context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult InnerJoinReport()
        {
            // Inner Join: Displays players who have BOTH team and position assigned
            var data = (from p in context.Players
                        join t in context.Teams on p.TeamId equals t.TeamId
                        join pos in context.Positions on p.PositionId equals pos.PositionId
                        select new
                        {
                            p.PlayerId,
                            p.FirstName,
                            p.LastName,
                            p.Age,
                            p.MarketValue,
                            TeamName = t.TeamName,
                            PositionName = pos.PositionName
                        }).ToList();

            return new JsonResult(data);
        }

        [HttpGet]
        public JsonResult LeftJoinReport()
        {
            // Left Join: Displays ALL players, displaying their team/position or showing "Takımsız" / "Pozisyonsuz"
            var data = (from p in context.Players
                        join t in context.Teams on p.TeamId equals t.TeamId into pts
                        from pt in pts.DefaultIfEmpty()
                        join pos in context.Positions on p.PositionId equals pos.PositionId into pps
                        from pp in pps.DefaultIfEmpty()
                        select new
                        {
                            p.PlayerId,
                            p.FirstName,
                            p.LastName,
                            p.Age,
                            p.MarketValue,
                            TeamName = pt != null ? pt.TeamName : "Takımsız",
                            PositionName = pp != null ? pp.PositionName : "Pozisyonsuz"
                        }).ToList();

            return new JsonResult(data);
        }

        [HttpGet]
        public JsonResult GroupByReport()
        {
            // Group By Count: Players grouped by Team
            var teamCounts = (from p in context.Players
                              join t in context.Teams on p.TeamId equals t.TeamId into pts
                              from pt in pts.DefaultIfEmpty()
                              group p by pt != null ? pt.TeamName : "Takımsız" into g
                              select new
                              {
                                  GroupName = g.Key,
                                  Count = g.Count()
                              }).ToList();

            // Group By Count: Players grouped by Position
            var positionCounts = (from p in context.Players
                                  join pos in context.Positions on p.PositionId equals pos.PositionId into pps
                                  from pp in pps.DefaultIfEmpty()
                                  group p by pp != null ? pp.PositionName : "Pozisyonsuz" into g
                                  select new
                                  {
                                      GroupName = g.Key,
                                      Count = g.Count()
                                  }).ToList();

            return new JsonResult(new { TeamCounts = teamCounts, PositionCounts = positionCounts });
        }
    }
}
