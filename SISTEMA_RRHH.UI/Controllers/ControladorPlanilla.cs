using SISTEMA_RRHH.NEGOCIO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace SISTEMA_RRHH.UI.Controllers
{
    [Authorize(Policy = "RRHHoSistemas")]
    public class ControladorPlanilla : Controller
    {
        private readonly GestionPlanilla _gestionPlanilla;

        public ControladorPlanilla()
        {
            _gestionPlanilla = new GestionPlanilla();
        }

        public IActionResult Index(int mes = 0, int anno = 0)
        {
            // 🔐 Validación extra para evitar acceso tras cierre de sesión
            if (!SesionActiva())
            {
                return RedirectToAction("Ingresar", "ControladorLogin");
            }

            mes = mes == 0 ? DateTime.Now.Month : mes;
            anno = anno == 0 ? DateTime.Now.Year : anno;

            ViewBag.MesActual = mes;
            ViewBag.AnnoActual = anno;

            int usuarioCalculoID = 1; // ← Este valor podría ajustarse más adelante con claims

            var planillas = _gestionPlanilla.CalcularPlanilla(mes, anno, usuarioCalculoID);

            return View(planillas);
        }

        // 🔒 Función reutilizable para validar autenticación y sesión
        private bool SesionActiva()
        {
            return User.Identity.IsAuthenticated &&
                   !string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogueado"));
        }
    }
}
