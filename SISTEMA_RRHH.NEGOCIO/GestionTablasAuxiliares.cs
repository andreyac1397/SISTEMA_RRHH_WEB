using SISTEMA_RRHH.DATOS;
using SISTEMA_RRHH.ENTIDADES.TablasAuxiliares;
using System.Collections.Generic;

namespace SISTEMA_RRHH.NEGOCIO
{
    public class GestionTablasAuxiliares
    {
        private readonly TablasAuxiliaresDatos datos = new TablasAuxiliaresDatos();

        public List<EstadoCivil> ObtenerEstadosCiviles()
        {
            return datos.ObtenerEstadosCiviles();
        }

        public List<TipoEmpleado> ObtenerTiposEmpleado()
        {
            return datos.ObtenerTiposEmpleado();
        }

        public List<EstadoFuncionario> ObtenerEstadosFuncionario()
        {
            return datos.ObtenerEstadosFuncionario();
        }

        public List<Departamento> ObtenerDepartamentos()
        {
            return datos.ObtenerDepartamentos();
        }
    }
}
