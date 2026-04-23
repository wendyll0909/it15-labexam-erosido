using Microsoft.AspNetCore.Mvc;

namespace IT15LabExamErosido.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
    }
}