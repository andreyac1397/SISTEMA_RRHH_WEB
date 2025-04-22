using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEMA_RRHH.ENTIDADES
{
    public class TractoImpuestoRenta
    {
        public int TractoID { get; set; }
        public decimal SalarioDesde { get; set; }
        public decimal SalarioHasta { get; set; }
        public decimal PorcentajeAplicable { get; set; }
       
    }
}
