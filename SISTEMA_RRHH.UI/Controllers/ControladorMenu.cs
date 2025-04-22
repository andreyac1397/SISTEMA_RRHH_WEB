using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Threading.Tasks;

namespace SISTEMA_RRHH.UI.Controllers
{
    [Authorize(Policy = "RRHHoSistemas")] // Solo RRHH y Sistemas pueden acceder
    public class ControladorMenu : Controller
    {
        public IActionResult Index()
        {
            // Validación extra: autenticado + sesión activa
            if (!SesionActiva())
            {
                return RedirectToAction("Ingresar", "ControladorLogin");
            }

            ViewBag.FuncionariosUrl = Url.Action("Index", "ControladorFuncionarios");
            ViewBag.Usuario = HttpContext.Session.GetString("UsuarioLogueado");

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CerrarSesion()
        {
            // Cerrar la cookie de autenticación
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Borrar manualmente la cookie por seguridad extra
            if (Request.Cookies[".AspNetCore.Cookies"] != null)
            {
                Response.Cookies.Append(".AspNetCore.Cookies", "", new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(-1),
                    Path = "/",
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict
                });
            }

            // Limpiar la sesión
            HttpContext.Session.Clear();

            // Redirigir al login
            return RedirectToAction("Ingresar", "ControladorLogin");
        }

        // ✅ Método privado reutilizable para validar sesión + autenticación
        private bool SesionActiva()
        {
            return User.Identity.IsAuthenticated &&
                   !string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogueado"));
        }
    }
}
