using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using SISTEMA_RRHH.NEGOCIO;
using SISTEMA_RRHH.ENTIDADES;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SISTEMA_RRHH.UI.Controllers
{
    public class ControladorLogin : Controller
    {
        private readonly UsuarioNegocio _usuarioNegocio;

        public ControladorLogin()
        {
            _usuarioNegocio = new UsuarioNegocio();
        }

        [HttpGet]
        public IActionResult Ingresar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Ingresar(string correo, string contrasena)
        {
            Usuarios usuario = await _usuarioNegocio.IniciarSesionAsync(correo, contrasena);

            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña inválidos, o el usuario no está activo.";
                return View();
            }

            /* ─────────────────────────────
               ❶  Si el usuario DEBE cambiar
            ────────────────────────────── */
            if (usuario.DebeCambiarContrasena)
            {
                ViewBag.Error = "Debés cambiar tu contraseña antes de ingresar. " +
                                "Haz clic en «¿Olvidaste tu contraseña?» para actualizarla.";
                /*  ⬅️  NO redirigimos, simplemente mostramos el mensaje        */
                return View();
            }

            /* ❷  Si NO debe cambiarla → login normal */
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.CorreoElectronico),
        new Claim("FuncionarioID",  usuario.FuncionarioID.ToString()),
        new Claim("DepartamentoID", usuario.DepartamentoID.ToString()),
        new Claim("DepartamentoNombre", usuario.NombreDepartamento)
    };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)));

            HttpContext.Session.SetString("UsuarioLogueado", usuario.CorreoElectronico);
            HttpContext.Session.SetInt32("FuncionarioID", usuario.FuncionarioID);

            return usuario.DepartamentoID is 1 or 3
                ? RedirectToAction("Index", "ControladorMenu")
                : RedirectToAction("Index", "ControladorComprobante");
        }


        // =============================
        // GET: Mostrar vista de cambiar contraseña
        // =============================
        [HttpGet]
        public IActionResult CambiarContrasena()
        {
            return View();   // sin lógica de TempData
        }





        // =============================
        // POST: Procesar el formulario de cambio de contraseña
        // =============================
        [HttpPost]
        public async Task<IActionResult> CambiarContrasena(
        string correo,
        string contrasenaActual,
        string nuevaContrasena,
        string confirmarContrasena)
        {
            if (string.IsNullOrWhiteSpace(correo) || string.IsNullOrWhiteSpace(contrasenaActual))
            {
                ViewBag.Error = "Correo y contraseña actual son requeridos.";
                return View();
            }

            /* ❶  Validar correo + contraseña actual */
            var usuarioID = await _usuarioNegocio.ObtenerUsuarioIDPorCorreoYContrasenaAsync(correo, contrasenaActual);
            if (usuarioID == null)
            {
                ViewBag.Error = "Correo o contraseña actual incorrectos.";
                return View();
            }

            /* ❷  Validar nuevas contraseñas */
            if (nuevaContrasena != confirmarContrasena)
            {
                ViewBag.Error = "Las contraseñas nuevas no coinciden.";
                return View();
            }

            var regex = new System.Text.RegularExpressions.Regex(
                @"^(?=.*\d)(?=.*[\u0021-\u002b\u003c-\u0040])(?=.*[A-Z])(?=.*[a-z])\S{8,16}$");
            if (!regex.IsMatch(nuevaContrasena))
            {
                ViewBag.Error = "La contraseña no cumple los requisitos de seguridad (8‑16 car., mayúscula, minúscula, número y símbolo).";
                return View();
            }

            /* ❸  Cambiar en BD */
            var resultado = await _usuarioNegocio.CambiarContrasenaAsync(usuarioID.Value, nuevaContrasena);

            if (resultado.Contains("correctamente"))
            {
                TempData["Mensaje"] = "Contraseña cambiada correctamente. Iniciá sesión con tu nueva contraseña.";
                return RedirectToAction("Ingresar");
            }

            ViewBag.Error = resultado;
            return View();
        }







        [HttpGet]
        public async Task<IActionResult> CerrarSesion()
        {
            // Cierra la cookie de autenticación
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Borra la cookie manualmente
            if (Request.Cookies[".AspNetCore.Cookies"] != null)
            {
                Response.Cookies.Append(".AspNetCore.Cookies", "", new CookieOptions
                {
                    Expires = System.DateTimeOffset.UtcNow.AddDays(-1),
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

        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            ViewBag.Mensaje = "No tenés permisos para acceder a esta sección.";
            return View();
        }
    }
}
