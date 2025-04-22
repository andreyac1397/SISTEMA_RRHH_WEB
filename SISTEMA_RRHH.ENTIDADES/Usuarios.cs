using System.ComponentModel.DataAnnotations;

namespace SISTEMA_RRHH.ENTIDADES
{
    public class Usuarios
    {
        public int UsuarioID { get; set; }
        public int FuncionarioID { get; set; }

        #region Login
        [Required]
        [DataType(DataType.EmailAddress)]
        public string CorreoElectronico { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
        #endregion
        public string NombreCompleto { get; set; }
        public int EstadoFuncionarioID { get; set; }
        public int DepartamentoID { get; set; }
        public string NombreDepartamento { get; set; }
        public bool DebeCambiarContrasena { get; set; }
        public string Estado { get; set; }

    }
}
