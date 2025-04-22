using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISTEMA_RRHH.ENTIDADES
{
    public class CargaSocial
    {
        public int CargaSocialID { get; set; }
        public string Concepto { get; set; }
        public decimal PorcentajePatrono { get; set; }
        public decimal PorcentajeTrabajador { get; set; }
       
    }
}

