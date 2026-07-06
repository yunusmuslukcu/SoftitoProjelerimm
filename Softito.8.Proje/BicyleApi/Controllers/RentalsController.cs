using BicyleApi.Models;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BicyleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        // LİSTELEME
        [HttpGet]
        public IActionResult GetAllRentals()
        {
            var rentals = Context.Listeleme<Rental>("RentalsViewAll");
            return Ok(rentals);
        }

        // 2.ID'YE GÖRE KİRALAMA KAYDI GETİR
        [HttpGet("{id}")]
        public IActionResult GetRentalById(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var rental = Context.Listeleme<Rental>("RentalsViewById", parameters).FirstOrDefault();

            if (rental == null)
                return NotFound("Kiralama kaydı bulunamadı.");

            return Ok(rental);
        }

        // 3.KİRALAMA EKLE VEYA GÜNCELLE 

        [HttpPost]
        public IActionResult SaveOrUpdateRental(Rental rental)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", rental.Id); 
            parameters.Add("@UserId", rental.UserId);
            parameters.Add("@StationId", rental.StationId);
            parameters.Add("@RentalStartTime", rental.RentalStartTime);
            parameters.Add("@RentalEndTime", rental.RentalEndTime);
            parameters.Add("@TotalPrice", rental.TotalPrice);

            Context.ExecuteReturn("RentalsEY", parameters);

            string mesaj = rental.Id == 0 ? "Kiralama işlemi başarıyla başlatıldı." : "Kiralama kaydı başarıyla güncellendi (Bisiklet teslim alındı).";
            return Ok(new { Message = mesaj });
        }

        // 4.KİRALAMA KAYDINI SİL
        [HttpDelete("{id}")]
        public IActionResult DeleteRental(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            Context.ExecuteReturn("RentalsSil", parameters);
            return Ok(new { Message = "Kiralama kaydı başarıyla silindi." });
        }
    }
}
