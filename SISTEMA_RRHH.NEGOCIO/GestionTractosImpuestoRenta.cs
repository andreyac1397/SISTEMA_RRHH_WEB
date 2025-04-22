using System;
using System.Collections.Generic;
using SISTEMA_RRHH.DATOS;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.NEGOCIO
{
    public class GestionTractosImpuestoRenta
    {
        private readonly TractosImpuestoRentaDatos datos = new TractosImpuestoRentaDatos();

        public List<TractoImpuestoRenta> ListarTractosImpuestoRenta() => datos.Listar();

        public TractoImpuestoRenta ObtenerTractoPorID(int id)
        {
            if (id <= 0)
                throw new Exception("ID inválido para la búsqueda.");
            return datos.ObtenerPorID(id);
        }

        public void InsertarTracto(TractoImpuestoRenta tracto)
        {
            if (tracto.SalarioDesde < 0 || tracto.SalarioHasta < 0)
                throw new Exception("El salario no puede ser negativo.");
            datos.Insertar(tracto);
        }

        public void ActualizarTracto(TractoImpuestoRenta tracto)
        {
            if (tracto.TractoID <= 0)
                throw new Exception("ID inválido para actualizar.");

            if (tracto.SalarioDesde < 0 || tracto.SalarioHasta < 0)
                throw new Exception("El salario no puede ser negativo.");

            datos.Actualizar(tracto);
        }

        public void EliminarTractoImpuestoRenta(int id)
        {
            if (id <= 0)
                throw new Exception("ID inválido para eliminar.");
            datos.Eliminar(id);
        }
    }
}

