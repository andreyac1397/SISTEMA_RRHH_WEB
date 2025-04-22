using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEMA_RRHH.ENTIDADES
{
    // ==================== Clase: Planilla ====================
    public class Planilla
    {
        public int PlanillaID { get; set; }

        public int FuncionarioID { get; set; }
        public decimal SalarioBaseMensual { get; set; }
        public decimal SalarioBruto { get; set; }
        public decimal AportePensionComplementaria { get; set; }
        public decimal TotalCargasSociales { get; set; }
        public decimal TotalCreditosFiscales { get; set; }
        public decimal ImpuestoRenta { get; set; }
        public decimal SalarioNeto { get; set; }
        public int UsuarioCalculoID { get; set; }
        public int PeriodoMes { get; set; } // Mes del periodo
        public int PeriodoAnno { get; set; } // Año del periodo
        public string NombreCompleto { get; set; }
    }
}
    