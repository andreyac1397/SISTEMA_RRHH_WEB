using System.Collections.Generic;
using SISTEMA_RRHH.ENTIDADES;
using SISTEMA_RRHH.DATOSSQL;
using SISTEMA_RRHH.DTO; // ✅ Agregado

namespace SISTEMA_RRHH.DATOS
{
    public class PlanillaDatos
    {
        private readonly PlanillaDatosSQL sql = new PlanillaDatosSQL();

        public List<FuncionarioPlanilla> ListarFuncionarios()
        {
            return sql.ObtenerFuncionariosActivos();
        }

        public List<HorasLaboradas> ListarHorasPorPeriodo(int mes, int anno)
        {
            return sql.ObtenerHorasPorPeriodo(mes, anno);
        }

        public List<CargaSocial> ListarCargas()
        {
            return sql.ObtenerCargasSociales();
        }

        public List<CreditoFiscal> ListarCreditos()
        {
            return sql.ObtenerCreditosFiscales();
        }

        public List<TractoImpuestoRenta> ListarTractos()
        {
            return sql.ObtenerTractosImpuestoRenta();
        }

        public void InsertarResultado(Planilla resultado, int mes, int anno)
        {
            sql.InsertarResultado(resultado, mes, anno);
        }
    }
}
