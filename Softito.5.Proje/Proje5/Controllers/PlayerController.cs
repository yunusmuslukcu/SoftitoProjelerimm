using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proje5.Data;
using Proje5.Models;

namespace Proje5.Controllers
{
    public class PlayerController : Controller
    {
        private readonly ScoutContext context;
        public PlayerController(ScoutContext Context)
        {
            this.context = Context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult PlayerList(string? search)
        {
            var query = context.Players.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => 
                    p.FirstName.Contains(search) || 
                    p.LastName.Contains(search) || 
                    p.MarketValue.Contains(search) ||
                    (p.Team != null && p.Team.TeamName.Contains(search)) ||
                    (p.Position != null && p.Position.PositionName.Contains(search))
                );
            }

            var data = query
                .Select(p => new
                {
                    p.PlayerId,
                    p.FirstName,
                    p.LastName,
                    p.Age,
                    p.MarketValue,
                    p.TeamId,
                    TeamName = p.Team != null ? p.Team.TeamName : "",
                    p.PositionId,
                    PositionName = p.Position != null ? p.Position.PositionName : ""
                }).ToList();

            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult AddPlayer(Player player)
        {
            var ply = new Player()
            {
                FirstName = player.FirstName,
                LastName = player.LastName,
                Age = player.Age,
                MarketValue = player.MarketValue,
                TeamId = player.TeamId,
                PositionId = player.PositionId
            };

            context.Players.Add(ply);
            context.SaveChanges();
            return new JsonResult("data Saved");
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            var data = context.Players.Where(m => m.PlayerId == id).SingleOrDefault();
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult Update(Player player)
        {
            context.Update(player);
            context.SaveChanges();
            return new JsonResult("Record updated");
        }

        public JsonResult Delete(int id)
        {
            var data = context.Players.Where(m => m.PlayerId == id).SingleOrDefault();
            if (data != null)
            {
                context.Players.Remove(data);
                context.SaveChanges();
            }
            return new JsonResult("data deleted");
        }
    }
}
