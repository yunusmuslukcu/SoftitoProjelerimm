using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace Marketing.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class LogController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public LogController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            var logPath = Path.Combine(_env.ContentRootPath, "Logs");
            
            var logs = new List<string>();

            if (Directory.Exists(logPath))
            {
                var logFiles = Directory.GetFiles(logPath, "app-log-*.txt").OrderByDescending(f => f).ToList();
                
                // En son dosyayı oku
                if (logFiles.Any())
                {
                    try
                    {
                        var latestLogFile = logFiles.First();
                        
                        using (var fileStream = new FileStream(latestLogFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        using (var streamReader = new StreamReader(fileStream))
                        {
                            var content = streamReader.ReadToEnd();
                            
                            logs = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                                          .Reverse()
                                          .Take(100) // Son 100 satırı al
                                          .ToList();
                        }
                    }
                    catch
                    {
                        logs.Add("Log dosyası şu anda başka bir işlem tarafından kullanılıyor veya okunamıyor.");
                    }
                }
                else
                {
                    logs.Add("Henüz log dosyası oluşturulmamış.");
                }
            }
            else
            {
                logs.Add("Logs klasörü bulunamadı.");
            }

            return View(logs);
        }
    }
}
