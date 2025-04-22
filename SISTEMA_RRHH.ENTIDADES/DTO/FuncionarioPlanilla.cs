using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEMA_RRHH.DTO
{
    public class FuncionarioPlanilla
    {
        public int FuncionarioID { get; set; }
        public string NombreCompleto { get; set; }
        public decimal SalarioBase { get; set; }
        public int CantHijosAplicables { get; set; }
        public int EstadoCivilID { get; set; }
        public decimal AportePensionComplementaria { get; set; }
    }
}

