using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SISTEMA_RRHH.NEGOCIO;
using SISTEMA_RRHH.ENTIDADES;
using SISTEMA_RRHH.ENTIDADES.DTO;
using SISTEMA_RRHH.UI.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace SISTEMA_RRHH.UI.Controllers
{
    // ─────────────────────────────────────────────
    // Controlador protegido: solo accesible por RRHH o Sistemas
    // ─────────────────────────────────────────────
    [Authorize(Policy = "RRHHoSistemas")]
    public class ControladorFuncionarios : Controller
    {
        // Instancia de la lógica de negocio de funcionarios
        private readonly GestionFuncionarios gestionFuncionarios;

        // Instancia de la lógica para obtener catálogos (Estados civiles, departamentos, etc.)
        private readonly GestionTablasAuxiliares gestionAuxiliares;

        // ─────────────────────────────────────────────
        // Constructor: inicializa la lógica de negocio y catálogos
        // ─────────────────────────────────────────────
        public ControladorFuncionarios()
        {
            gestionFuncionarios = new GestionFuncionarios();
            gestionAuxiliares = new GestionTablasAuxiliares();
        }

        // ─────────────────────────────────────────────
        // Acción principal que muestra el formulario y la tabla de funcionarios
        // ─────────────────────────────────────────────
        public IActionResult Index()
        {
            // Verifica que el usuario esté autenticado
            if (!User.Identity.IsAuthenticated || string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogueado")))
            {
                return RedirectToAction("Ingresar", "ControladorLogin");
            }

            // Construye el ViewModel con los catálogos
            var viewModel = ConstruirViewModel();

            // Carga los funcionarios con descripciones para mostrar en tabla
            ViewBag.FuncionariosConDescripcion = gestionFuncionarios.ObtenerFuncionariosConDescripciones();

            return View(viewModel);
        }

        // ─────────────────────────────────────────────
        // Acción POST que agrega un nuevo funcionario
        // ─────────────────────────────────────────────
        [HttpPost]
        public IActionResult Agregar(Funcionarios funcionario)
        {
            string mensajeError;

            // Si pasa la validación de negocio, lo guarda
            if (gestionFuncionarios.Validar(funcionario, out mensajeError))
            {
                gestionFuncionarios.AgregarFuncionario(funcionario);
                TempData["Mensaje"] = "Funcionario agregado correctamente.";
                return RedirectToAction("Index");
            }

            // Si falla la validación, vuelve a la vista con el formulario y el error
            var viewModel = ConstruirViewModel();
            viewModel.Funcionario = funcionario; // Mantiene los datos ingresados
            ViewBag.FuncionariosConDescripcion = gestionFuncionarios.ObtenerFuncionariosConDescripciones();
            TempData["Error"] = mensajeError;
            TempData["Origen"] = "Agregar"; // Activa el modal Agregar desde la vista
            return View("Index", viewModel);
        }

        // ─────────────────────────────────────────────
        // Acción POST que modifica un funcionario existente
        // ─────────────────────────────────────────────
        [HttpPost]
        public IActionResult Modificar(Funcionarios funcionario)
        {
            string mensajeError;

            if (gestionFuncionarios.Validar(funcionario, out mensajeError))
            {
                gestionFuncionarios.ModificarFuncionario(funcionario);
                TempData["Mensaje"] = "Funcionario modificado correctamente.";
                return RedirectToAction("Index");
            }

            // Si falla, se reconstruye el ViewModel con los datos que fallaron
            var viewModel = ConstruirViewModel();
            viewModel.Funcionario = funcionario;
            ViewBag.FuncionariosConDescripcion = gestionFuncionarios.ObtenerFuncionariosConDescripciones();
            TempData["Error"] = mensajeError;
            TempData["Origen"] = "Modificar"; // Activa el modal de modificar
            return View("Index", viewModel);
        }

        // ─────────────────────────────────────────────
        // Acción POST que elimina un funcionario
        // ─────────────────────────────────────────────
        [HttpPost]
        public IActionResult Eliminar(int funcionarioID)
        {
            gestionFuncionarios.EliminarFuncionario(funcionarioID);
            TempData["Mensaje"] = "Funcionario eliminado correctamente.";
            return RedirectToAction("Index");
        }

        // ─────────────────────────────────────────────
        // Método privado que construye el ViewModel con los catálogos
        // Se usa tanto al cargar como en errores de validación
        // ─────────────────────────────────────────────
        private FuncionarioViewModel ConstruirViewModel()
        {
            var estadosCiviles = gestionAuxiliares.ObtenerEstadosCiviles();
            var tiposEmpleado = gestionAuxiliares.ObtenerTiposEmpleado();
            var estadosFuncionario = gestionAuxiliares.ObtenerEstadosFuncionario();
            var departamentos = gestionAuxiliares.ObtenerDepartamentos();

            return new FuncionarioViewModel
            {
                ListaFuncionarios = new List<Funcionarios>(),
                Funcionario = new Funcionarios(),

                ListaEstadosCiviles = estadosCiviles.Select(ec => new SelectListItem
                {
                    Value = ec.EstadoCivilID.ToString(),
                    Text = ec.Descripcion
                }),

                ListaTiposEmpleados = tiposEmpleado.Select(te => new SelectListItem
                {
                    Value = te.TipoEmpleadoID.ToString(),
                    Text = te.Descripcion
                }),

                ListaEstadosFuncionario = estadosFuncionario.Select(ef => new SelectListItem
                {
                    Value = ef.EstadoFuncionarioID.ToString(),
                    Text = ef.Descripcion
                }),

                ListaDepartamentos = departamentos.Select(d => new SelectListItem
                {
                    Value = d.DepartamentoID.ToString(),
                    Text = d.Nombre
                })
            };
        }
    }
}

