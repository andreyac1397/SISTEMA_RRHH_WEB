using System;

namespace SISTEMA_RRHH.ENTIDADES
{
    // ─────────────────────────────────────────────
    // Representa un funcionario en la base de datos.
    // Esta clase se usa para insertar, actualizar
    // y eliminar registros de la BD. 
    // ─────────────────────────────────────────────
    public class Funcionarios
    {
        public int FuncionarioID { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime FechaIngreso { get; set; }
        public int CantidadHijosMayores { get; set; }
        public int CantidadHijosAplicables { get; set; }
        public int EstadoCivilID { get; set; }
        public decimal SalarioBaseMensual { get; set; }
        public string TipoEmpleado { get; set; }
        public string EstadoFuncionario { get; set; }
        public string Departamento { get; set; }
        public decimal AportePensionComplementaria { get; set; }
        public string CorreoElectronico { get; set; }
        public string Nacionalidad { get; set; }
        public string Sexo { get; set; } 
    }
}
