using System.Collections.Generic;
using SISTEMA_RRHH.DATOSSQL;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.DATOS
{
    public class TractosImpuestoRentaDatos
    {
        private readonly TractosImpuestoRentaDatosSQL sql = new TractosImpuestoRentaDatosSQL();

        public List<TractoImpuestoRenta> Listar() => sql.Listar();

        public TractoImpuestoRenta ObtenerPorID(int id) => sql.ObtenerPorID(id);

        public void Insertar(TractoImpuestoRenta tracto) => sql.Insertar(tracto);

        public void Actualizar(TractoImpuestoRenta tracto) => sql.Actualizar(tracto);

        public void Eliminar(int id) => sql.Eliminar(id);
    }
}
