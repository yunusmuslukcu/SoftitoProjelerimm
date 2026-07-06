using System.Net.Http.Json;
using BicyleMvc.Models;

namespace BicyleMvc.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BicyleApi");
        }

        // --- ACCOUNT API CALLS ---

        public async Task<(bool IsSuccess, string Message, UserViewModel? User)> LoginAsync(string email, string password)
        {
            try
            {
                // Parametreler query string olarak gönderiliyor çünkü API'de [FromBody] tanımlı değil
                var response = await _httpClient.PostAsync($"api/Account/login?email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}", null);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    if (result != null)
                    {
                        return (true, result.Message, result.User);
                    }
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, !string.IsNullOrEmpty(errorMessage) ? errorMessage : "Giriş başarısız.", null);
                }
            }
            catch (Exception ex)
            {
                return (false, $"API bağlantı hatası: {ex.Message}", null);
            }

            return (false, "Beklenmeyen bir hata oluştu.", null);
        }

        public async Task<(bool IsSuccess, string Message)> RegisterAsync(string firstName, string lastName, string email, string password)
        {
            try
            {
                var url = $"api/Account/register?firstName={Uri.EscapeDataString(firstName)}&lastName={Uri.EscapeDataString(lastName)}&email={Uri.EscapeDataString(email)}&password={Uri.EscapeDataString(password)}";
                var response = await _httpClient.PostAsync(url, null);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return (true, result?.Message ?? "Kayıt başarılı.");
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return (false, !string.IsNullOrEmpty(errorMessage) ? errorMessage : "Kayıt başarısız.");
                }
            }
            catch (Exception ex)
            {
                return (false, $"API bağlantı hatası: {ex.Message}");
            }
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<UserViewModel>>("api/Account/users");
                return response ?? new List<UserViewModel>();
            }
            catch
            {
                return new List<UserViewModel>();
            }
        }

        // --- STATIONS API CALLS ---

        public async Task<List<StationViewModel>> GetAllStationsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<StationViewModel>>("api/Stations");
                return response ?? new List<StationViewModel>();
            }
            catch
            {
                return new List<StationViewModel>();
            }
        }

        public async Task<StationViewModel?> GetStationByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<StationViewModel>($"api/Stations/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool IsSuccess, string Message)> SaveOrUpdateStationAsync(StationViewModel station)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Stations", station);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return (true, result?.Message ?? "İşlem başarılı.");
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return (false, error);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string Message)> DeleteStationAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Stations/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return (true, result?.Message ?? "İstasyon silindi.");
                }
                return (false, "İstasyon silinemedi.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // --- RENTALS API CALLS ---

        public async Task<List<RentalViewModel>> GetAllRentalsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<RentalViewModel>>("api/Rentals");
                return response ?? new List<RentalViewModel>();
            }
            catch
            {
                return new List<RentalViewModel>();
            }
        }

        public async Task<RentalViewModel?> GetRentalByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<RentalViewModel>($"api/Rentals/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<(bool IsSuccess, string Message)> SaveOrUpdateRentalAsync(RentalViewModel rental)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Rentals", rental);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return (true, result?.Message ?? "İşlem başarılı.");
                }
                return (false, "Kiralama işlemi başarısız.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string Message)> DeleteRentalAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Rentals/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ApiResponse>();
                    return (true, result?.Message ?? "Kayıt silindi.");
                }
                return (false, "Kiralama kaydı silinemedi.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }

    // Helper classes for deserialization
    public class ApiResponse
    {
        public string Message { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Message { get; set; } = string.Empty;
        public UserViewModel? User { get; set; }
    }
}
