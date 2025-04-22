using System;
using System.Collections.Generic;
using SISTEMA_RRHH.DATOS;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.NEGOCIO
{
    public class GestionCargasSociales
    {
        private readonly CargasSocialesDatos datos = new CargasSocialesDatos();

        public List<CargaSocial> Listar() => datos.Listar();

        public string Insertar(CargaSocial carga)
        {
            if (string.IsNullOrWhiteSpace(carga.Concepto))
                return "El concepto es obligatorio.";

            if (carga.PorcentajePatrono < 0 || carga.PorcentajeTrabajador < 0)
                return "Los porcentajes no pueden ser negativos.";

            if (carga.PorcentajePatrono > 100 || carga.PorcentajeTrabajador > 100)
                return "Los porcentajes no pueden ser mayores a 100.";

            if (carga.PorcentajePatrono == 0 && carga.PorcentajeTrabajador == 0)
                return "Ambos porcentajes no pueden ser cero.";
            datos.Insertar(carga);
            return "OK";
        }

        public string ActualizarCargaSocial(CargaSocial carga)
        {
            if (string.IsNullOrWhiteSpace(carga.Concepto))
                return "El concepto es obligatorio.";

            if (carga.PorcentajePatrono < 0 || carga.PorcentajeTrabajador < 0)
                return "Los porcentajes no pueden ser negativos.";

            if (carga.PorcentajePatrono > 100 || carga.PorcentajeTrabajador > 100)
                return "Los porcentajes no pueden ser mayores a 100.";

            if (carga.PorcentajePatrono == 0 && carga.PorcentajeTrabajador == 0)
                return "Ambos porcentajes no pueden ser cero.";

            datos.Actualizar(carga);
            return "OK";
        }


        public void Eliminar(int id) => datos.Eliminar(id);
    }
}



