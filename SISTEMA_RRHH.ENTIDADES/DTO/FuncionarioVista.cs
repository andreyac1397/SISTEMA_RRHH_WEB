using System;

namespace SISTEMA_RRHH.ENTIDADES.DTO
{
    public class FuncionarioVista
    {

        // ─────────────────────────────────────────────
        // Lo que se va a mostrar en la UI.
        // ─────────────────────────────────────────────
        public int FuncionarioID { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int CantidadHijosMayores { get; set; }
        public int CantidadHijosAplicables { get; set; }
        public decimal SalarioBaseMensual { get; set; }
        public decimal AportePensionComplementaria { get; set; }
        public string CorreoElectronico { get; set; }
        public string Nacionalidad { get; set; }
        public string Sexo { get; set; }

        // Descripciones en vez de IDs
        public string EstadoCivilDescripcion { get; set; }
        public string TipoEmpleadoDescripcion { get; set; }
        public string EstadoFuncionarioDescripcion { get; set; }
        public string DepartamentoDescripcion { get; set; }
        public int EstadoCivilID { get; set; }
        public int TipoEmpleadoID { get; set; }
        public int EstadoFuncionarioID { get; set; }
        public int DepartamentoID { get; set; }

    }
}
