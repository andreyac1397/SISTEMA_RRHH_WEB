using System.Collections.Generic;
using SISTEMA_RRHH.DATOSSQL;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.DATOS
{
    public class HorasLaboradasDatos
    {
        private readonly HorasLaboradasDatosSQL sql = new HorasLaboradasDatosSQL();

        public List<HorasLaboradas> Listar(int mes, int anno)
        {
            return sql.Listar(mes, anno); // ✅ ahora sí le pasás los parámetros requeridos
        }


        public void Insertar(HorasLaboradas horas) => sql.Insertar(horas);
        public void Actualizar(HorasLaboradas horas) => sql.Actualizar(horas);
        public void Eliminar(int id) => sql.Eliminar(id);
    }
}
