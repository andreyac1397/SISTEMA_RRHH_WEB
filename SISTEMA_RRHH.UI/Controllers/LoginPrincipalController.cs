using Microsoft.AspNetCore.Mvc;

namespace SISTEMA_RRHH.UI.Controllers
{
    public class LoginPrincipalController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult AccesDenid()
        {
            return View();
        }
    }
}
