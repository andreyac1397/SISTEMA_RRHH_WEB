using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SISTEMA_RRHH.NEGOCIO;
using System.Linq;

namespace SISTEMA_RRHH.UI.Controllers
{
    [Authorize] // Solo autenticados pueden acceder
    public class ControladorComprobante : Controller
    {
        private readonly GestionPlanilla _gestionPlanilla;

        public ControladorComprobante()
        {
            _gestionPlanilla = new GestionPlanilla();
        }

        public IActionResult Index(int mes = 0, int anno = 0)
        {
            // Verificar sesión
            if (!SesionActiva())
            {
                return RedirectToAction("Ingresar", "ControladorLogin");
            }

            mes = (mes == 0) ? System.DateTime.Now.Month : mes;
            anno = (anno == 0) ? System.DateTime.Now.Year : anno;

            ViewBag.MesActual = mes;
            ViewBag.AnnoActual = anno;

            // Obtener FuncionarioID desde el claim
            int funcionarioID = int.Parse(User.FindFirst("FuncionarioID").Value);
            int usuarioCalculoID = 1; // interno

            var planillas = _gestionPlanilla.CalcularPlanilla(mes, anno, usuarioCalculoID);
            var comprobante = planillas.FirstOrDefault(p => p.FuncionarioID == funcionarioID);

            if (comprobante == null)
            {
                ViewBag.Mensaje = "No hay datos disponibles para este periodo.";
                return View();
            }

            return View(comprobante);
        }

        // Validación de sesión
        private bool SesionActiva()
        {
            return User.Identity.IsAuthenticated &&
                   !string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogueado"));
        }
    }
}
