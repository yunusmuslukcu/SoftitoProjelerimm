using BicyleApi.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BicyleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationsController : ControllerBase
    {
        //İSTASYONLARI LİSTELE
        [HttpGet]
        public IActionResult GetAllStations()
        {

            var stations = Context.Listeleme<Station>("StationsViewAll");
            return Ok(stations);
        }

        // 2. ID'YE GÖRE İSTASYON GETİR
        [HttpGet("{id}")]
        public IActionResult GetStationById(int id)
        {
            
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            
            var station = Context.Listeleme<Station>("StationsViewById", parameters).FirstOrDefault();

            if (station == null)
                return NotFound("İstasyon bulunamadı.");

            return Ok(station);
        }

        // 3. İSTASYON EKLE VEYA GÜNCELLE
        [HttpPost]
        public IActionResult SaveOrUpdateStation(Station station)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", station.Id); 
            parameters.Add("@StationName", station.StationName);
            parameters.Add("@Location", station.Location);
            parameters.Add("@Capacity", station.Capacity);
            parameters.Add("@ActiveBicyclesCount", station.ActiveBicyclesCount);
            parameters.Add("@IsActive", station.IsActive);

            Context.ExecuteReturn("StationsEY", parameters);

            string mesaj = station.Id == 0 ? "İstasyon başarıyla eklendi." : "İstasyon başarıyla güncellendi.";
            return Ok(new { Message = mesaj });
        }

        // 4. İSTASYON SİL
        [HttpDelete("{id}")]
        public IActionResult DeleteStation(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            Context.ExecuteReturn("StationsSil", parameters);
            return Ok(new { Message = "İstasyon başarıyla silindi." });
        }
    }
}
    

