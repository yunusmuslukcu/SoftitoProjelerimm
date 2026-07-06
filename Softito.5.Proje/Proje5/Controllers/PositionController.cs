using Microsoft.AspNetCore.Mvc;
using Proje5.Data;
using Proje5.Models;

namespace Proje5.Controllers
{
    public class PositionController : Controller
    {
        private readonly ScoutContext context;

        public PositionController(ScoutContext Context)
        {
            this.context = Context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult PositionList(string? search)
        {
            var query = context.Positions.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.PositionName.Contains(search) || p.ShortName.Contains(search));
            }

            var data = query
                .Select(p => new
                {
                    p.PositionId,
                    p.PositionName,
                    p.ShortName
                }).ToList();

            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult AddPosition(Position position)
        {
            var pos = new Position()
            {
                PositionName = position.PositionName,
                ShortName = position.ShortName
            };

            context.Positions.Add(pos);
            context.SaveChanges();
            return new JsonResult("data Saved");
        }

        [HttpGet]
        public JsonResult Edit(int id)
        {
            var data = context.Positions.Where(m => m.PositionId == id).SingleOrDefault();
            return new JsonResult(data);
        }

        [HttpPost]
        public JsonResult Update(Position position)
        {
            context.Update(position);
            context.SaveChanges();
            return new JsonResult("Record updated");
        }

        public JsonResult Delete(int id)
        {
            var data = context.Positions.Where(m => m.PositionId == id).SingleOrDefault();
            if (data != null)
            {
                context.Positions.Remove(data);
                context.SaveChanges();
            }
            return new JsonResult("data deleted");


        }
    }
}
