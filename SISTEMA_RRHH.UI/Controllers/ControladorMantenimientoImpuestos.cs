using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SISTEMA_RRHH.NEGOCIO;
using SISTEMA_RRHH.ENTIDADES;
using SISTEMA_RRHH.UI.Models;
using SISTEMA_RRHH.DATOSSQL;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;
using SISTEMA_RRHH.DATOS;

namespace SISTEMA_RRHH.UI.Controllers
{
    [Authorize(Policy = "RRHHoSistemas")]
    public class ControladorMantenimientoImpuestos : Controller
    {
        private readonly GestionCargasSociales _gestionDeCargasSociales = new GestionCargasSociales();
        private readonly UsuarioNegocio _gestionUsuarios = new UsuarioNegocio();
        private readonly GestionCreditosFiscales _gestionDeCreditosFiscales = new GestionCreditosFiscales();
        private readonly GestionHorasLaboradas _gestionDeHorasLaboradas = new GestionHorasLaboradas();
        private readonly GestionTractosImpuestoRenta _gestionDeTractosImpuestoRenta = new GestionTractosImpuestoRenta();

        public IActionResult Indice(int? mes, int? anno)
        {
            // Verificar que el usuario siga autenticado
            if (!User.Identity.IsAuthenticated || string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogueado")))
                return RedirectToAction("Ingresar", "ControladorLogin");

            // Recalcular salarios brutos de horas laboradas
            _gestionDeHorasLaboradas.RecalcularSalariosBrutos();

            int mesActual = mes ?? DateTime.Now.Month;
            int annoActual = anno ?? DateTime.Now.Year;

            ViewBag.MesActual = mesActual;
            ViewBag.AnnoActual = annoActual;
            ViewBag.MensajeEdicion = $"Estamos editando el mes de {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mesActual)} de {annoActual}";

            // Construir listas para filtros de mes y año
            ViewBag.ListaMeses = Enumerable.Range(1, 12)
                .Select(m => new SelectListItem { Value = m.ToString(), Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m), Selected = m == mesActual })
                .ToList();

            ViewBag.ListaAnios = Enumerable.Range(DateTime.Now.Year - 5, 7)
                .Select(a => new SelectListItem { Value = a.ToString(), Text = a.ToString(), Selected = a == annoActual })
                .ToList();

            // Montar el ViewModel con todos los datos de mantenimiento
            var modelo = new MantenimientoViewModel
            {
                CargasSociales = _gestionDeCargasSociales.Listar(),
                CreditosFiscales = _gestionDeCreditosFiscales.ListarCreditosFiscales(),
                HorasLaboradas = _gestionDeHorasLaboradas.ListarHorasLaboradas(mesActual, annoActual),
                TractosImpuestoRenta = _gestionDeTractosImpuestoRenta.ListarTractosImpuestoRenta(),
                ListaUsuarios = _gestionUsuarios.ListarUsuarios() 
            };

            return View("Index", modelo);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarUsuario(Usuarios usuario)
        {
            // Delegar la validación y persistencia al negocio
            string mensaje = await _gestionUsuarios.ActualizarUsuarioYFuncionarioAsync(usuario);
            TempData["MensajeUsuarios"] = mensaje;
            return RedirectToAction(nameof(Indice));
        }

        [HttpPost]
        public IActionResult GuardarCargaSocial(CargaSocial carga)
        {
            try
            {
                // Insertar o actualizar con validaciones en la capa de negocio
                string resultado = (carga.CargaSocialID == 0)
                    ? _gestionDeCargasSociales.Insertar(carga)
                    : _gestionDeCargasSociales.ActualizarCargaSocial(carga);

                if (resultado != "OK")
                {
                    TempData["ErrorCargas"] = resultado;
                    TempData["OrigenCargas"] = carga.CargaSocialID == 0 ? "Agregar" : "Modificar";
                    return RedirectToAction(nameof(Indice));
                }

                TempData["MensajeCargas"] = carga.CargaSocialID == 0
                    ? "Carga social insertada correctamente."
                    : "Carga social actualizada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorCargas"] = ex.Message;
            }

            return RedirectToAction(nameof(Indice));
        }
        //ELinar el usuario
        [HttpPost]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            string mensaje = await _gestionUsuarios.EliminarUsuarioYFuncionarioAsync(id);
            TempData["MensajeUsuarios"] = mensaje;
            return RedirectToAction(nameof(Indice));
        }

        [HttpPost]
        public IActionResult EliminarCargaSocial(int id)
        {
            try
            {
                _gestionDeCargasSociales.Eliminar(id);
                TempData["MensajeCargas"] = "Carga social eliminada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorCargas"] = ex.Message;
            }
            return RedirectToAction(nameof(Indice));
        }

        [HttpPost]
        public IActionResult GuardarCreditoFiscal(CreditoFiscal credito)
        {
            try
            {
                if (credito.CreditoFiscalID == 0)
                {
                    _gestionDeCreditosFiscales.InsertarCreditoFiscal(credito);
                    TempData["MensajeCreditos"] = "Crédito fiscal insertado correctamente.";
                }
                else
                {
                    _gestionDeCreditosFiscales.ActualizarCreditoFiscal(credito);
                    TempData["MensajeCreditos"] = "Crédito fiscal actualizado correctamente.";
                }

                // Volver a calcular la planilla tras los cambios
                int mes = int.Parse(Request.Form["PeriodoMes"]);
                int anno = int.Parse(Request.Form["PeriodoAnno"]);
                new GestionPlanilla().CalcularPlanilla(mes, anno, 1);
            }
            catch (Exception ex)
            {
                TempData["ErrorCreditos"] = ex.Message;
            }
            return RedirectToAction(nameof(Indice));
        }

        [HttpPost]
        public IActionResult EliminarCreditoFiscal(int id)
        {
            try
            {
                _gestionDeCreditosFiscales.EliminarCreditoFiscal(id);
                TempData["MensajeCreditos"] = "Crédito fiscal eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorCreditos"] = ex.Message;
            }
            return RedirectToAction(nameof(Indice));
        }

        [HttpPost]
        public IActionResult GuardarHorasLaboradas(HorasLaboradas horas)
        {
            try
            {
                if (horas.HorasLaboradasID == 0)
                {
                    _gestionDeHorasLaboradas.InsertarHorasLaboradas(horas);
                    TempData["MensajeHoras"] = "Horas laboradas insertadas correctamente.";
                }
                else
                {
                    _gestionDeHorasLaboradas.ActualizarHorasLaboradas(horas);
                    TempData["MensajeHoras"] = "Horas laboradas actualizadas correctamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorHoras"] = ex.Message;
                TempData["AbrirModalEdicion"] = "true";

                var funcionario = new FuncionarioDatosSQL().ObtenerFuncionarioPorID(horas.FuncionarioID);
                string nombreCompleto = $"{funcionario.Nombre} {funcionario.PrimerApellido} {funcionario.SegundoApellido}";

                TempData["HorasFallidas_ID"] = horas.HorasLaboradasID;
                TempData["HorasFallidas_FuncionarioID"] = horas.FuncionarioID;
                TempData["HorasFallidas_Nombre"] = nombreCompleto;
                TempData["HorasFallidas_Normales"] = horas.HorasNormales.ToString("N2");
                TempData["HorasFallidas_Extras"] = horas.HorasExtras.ToString("N2");
                TempData["HorasFallidas_Dobles"] = horas.HorasDobles.ToString("N2");
            }
            return RedirectToAction(nameof(Indice));
        }

        [HttpPost]
        public IActionResult EliminarHorasLaboradas(int id)
        {
            try
            {
                _gestionDeHorasLaboradas.EliminarHorasLaboradas(id);
                TempData["MensajeHoras"] = "Horas laboradas eliminadas correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorHoras"] = ex.Message;
            }
            return RedirectToAction(nameof(Indice));
        }

        [HttpPost]
        public IActionResult GuardarTractoImpuestoRenta(TractoImpuestoRenta tracto)
        {
            try
            {
                // Normalizar decimales
                if (Request.Form.ContainsKey("SalarioDesde"))
                {
                    var valor = Request.Form["SalarioDesde"].ToString().Replace(",", ".");
                    if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out var desde))
                        tracto.SalarioDesde = desde;
                }

                if (Request.Form.ContainsKey("SalarioHasta"))
                {
                    var valor = Request.Form["SalarioHasta"].ToString().Replace(",", ".");
                    if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out var hasta))
                        tracto.SalarioHasta = hasta;
                }

                if (Request.Form.ContainsKey("PorcentajeAplicable"))
                {
                    var valor = Request.Form["PorcentajeAplicable"].ToString().Replace(",", ".");
                    if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out var porcentaje))
                        tracto.PorcentajeAplicable = porcentaje;
                }

                // Insertar o actualizar tracto
                if (tracto.TractoID == 0)
                {
                    _gestionDeTractosImpuestoRenta.InsertarTracto(tracto);
                    TempData["MensajeTractos"] = "Tracto de impuesto insertado correctamente.";
                }
                else
                {
                    _gestionDeTractosImpuestoRenta.ActualizarTracto(tracto);
                    TempData["MensajeTractos"] = "Tracto de impuesto actualizado correctamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorTractos"] = ex.Message;
            }
            return RedirectToAction(nameof(Indice));
        }

        [HttpPost]
        public IActionResult EliminarTractoImpuestoRenta(int id)
        {
            try
            {
                _gestionDeTractosImpuestoRenta.EliminarTractoImpuestoRenta(id);
                TempData["MensajeTractos"] = "Tracto eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorTractos"] = ex.Message;
            }
            return RedirectToAction(nameof(Indice));
        }
    }
}
