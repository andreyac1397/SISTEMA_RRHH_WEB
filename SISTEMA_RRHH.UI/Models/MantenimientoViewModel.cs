using System.Collections.Generic;
using SISTEMA_RRHH.ENTIDADES;



namespace SISTEMA_RRHH.UI.Models
{
    public class MantenimientoViewModel
    {
        public List<CargaSocial> CargasSociales { get; set; }
        public List<CreditoFiscal> CreditosFiscales { get; set; }
        public List<HorasLaboradas> HorasLaboradas { get; set; }
        public List<TractoImpuestoRenta> TractosImpuestoRenta { get; set; }
        public List<Usuarios> ListaUsuarios { get; set; }
    }
}
