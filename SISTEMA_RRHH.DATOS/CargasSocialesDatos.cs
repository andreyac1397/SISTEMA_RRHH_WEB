using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using SISTEMA_RRHH.DATOSSQL;
using SISTEMA_RRHH.ENTIDADES;  // Asegúrate de que la entidad CargaSocial esté definida aquí

namespace SISTEMA_RRHH.DATOS
{
    public class CargasSocialesDatos
    {
        private readonly CargasSocialesDatosSQL sql = new CargasSocialesDatosSQL();

        public List<CargaSocial> Listar() => sql.Listar();
        public void Insertar(CargaSocial carga) => sql.Insertar(carga);
        public void Actualizar(CargaSocial carga) => sql.Actualizar(carga);
        public void Eliminar(int id) => sql.Eliminar(id);
    }
}
