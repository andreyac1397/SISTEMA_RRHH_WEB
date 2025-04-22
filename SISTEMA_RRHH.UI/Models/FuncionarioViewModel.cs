using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.UI.Models
{
    public class FuncionarioViewModel
    {
        // Para el formulario, se utiliza la entidad Funcionarios
        public Funcionarios Funcionario { get; set; }

        // Opcional: para mostrar la lista en la vista
        public List<Funcionarios> ListaFuncionarios { get; set; }

        // Listas para los dropdowns de catálogo
        public IEnumerable<SelectListItem> ListaEstadosCiviles { get; set; }
        public IEnumerable<SelectListItem> ListaTiposEmpleados { get; set; }
        public IEnumerable<SelectListItem> ListaEstadosFuncionario { get; set; }
        public IEnumerable<SelectListItem> ListaDepartamentos { get; set; }
    }
}
