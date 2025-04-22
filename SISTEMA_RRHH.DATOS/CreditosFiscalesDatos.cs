using SISTEMA_RRHH.ENTIDADES;
using SISTEMA_RRHH.DATOSSQL;
using System.Collections.Generic;

namespace SISTEMA_RRHH.DATOS
{
    public class CreditosFiscalesDatos
    {
        private readonly CreditosFiscalesDatosSQL sql = new CreditosFiscalesDatosSQL();

        public List<CreditoFiscal> Listar() => sql.Listar();
        public void Insertar(CreditoFiscal credito) => sql.Insertar(credito);
        public void Actualizar(CreditoFiscal credito) => sql.Actualizar(credito);
        public void Eliminar(int id) => sql.Eliminar(id);
    }
}
