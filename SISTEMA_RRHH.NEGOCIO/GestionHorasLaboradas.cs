using System;
using System.Collections.Generic;
using SISTEMA_RRHH.DATOS;
using SISTEMA_RRHH.DATOSSQL;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.NEGOCIO
{
    public class GestionHorasLaboradas
    {
        private readonly HorasLaboradasDatos datos = new HorasLaboradasDatos();
        private readonly HorasLaboradasDatosSQL datosSQL = new HorasLaboradasDatosSQL();

        // Lista completa sin filtro
        public List<HorasLaboradas> ListarHorasLaboradas(int mes, int anno)
        {
            return datos.Listar(mes, anno);
        }

        // ✅ NUEVO: Lista con filtro por mes y año
        public List<HorasLaboradas> ListarPorPeriodo(int mes, int anno)
        {
            return datosSQL.ListarPorPeriodo(mes, anno);
        }

        public void InsertarHorasLaboradas(HorasLaboradas horas)
        {
            if (horas.HorasNormales < 0 || horas.HorasExtras < 0 || horas.HorasDobles < 0)
                throw new Exception("Las horas no pueden ser negativas.");

            var funcionario = new FuncionarioDatosSQL().ObtenerFuncionarioPorID(horas.FuncionarioID);
            horas.SalarioBase = funcionario.SalarioBaseMensual;

            decimal salarioPorHora = horas.SalarioBase / 240m;
            horas.SalarioBruto =
                (horas.HorasNormales * salarioPorHora) +
                (horas.HorasExtras * salarioPorHora * 1.5m) +
                (horas.HorasDobles * salarioPorHora * 2.0m);

            datos.Insertar(horas);
        }
        public List<HorasLaboradas> ListarHorasPorPeriodo(int mes, int anno)
        {
            return new HorasLaboradasDatosSQL().Listar(mes, anno);
        }

        public void RecalcularSalariosBrutos()
        {
            datosSQL.RecalcularSalariosBrutos();
        }

        // Actualiza un registro existente de horas laboradas
        public void ActualizarHorasLaboradas(HorasLaboradas horas)
        {
            if (horas.HorasLaboradasID <= 0)
                throw new Exception("ID inválido.");

            // ❌ Validación: No se permiten horas negativas
            if (horas.HorasNormales < 0 || horas.HorasExtras < 0 || horas.HorasDobles < 0)
                throw new Exception("Las horas no pueden ser negativas.");

            // Validaciones de límites máximos
            if (horas.HorasNormales > 173)
                throw new Exception("Las horas normales no pueden ser mayores a 173 horas mensuales.");

            if (horas.HorasExtras > 86)
                throw new Exception("Las horas extras no pueden ser mayores a 86 horas mensuales.");

            if (horas.HorasDobles > 40)
                throw new Exception("Las horas dobles no pueden ser mayores a 40 horas mensuales.");

            if ((horas.HorasNormales + horas.HorasExtras + horas.HorasDobles) > 270)
                throw new Exception("El total de horas no puede ser mayor a 270 horas mensuales.");

            // Cargar salario base y calcular salario bruto
            var funcionario = new FuncionarioDatosSQL().ObtenerFuncionarioPorID(horas.FuncionarioID);
            horas.SalarioBase = funcionario.SalarioBaseMensual;

            decimal salarioPorHora = horas.SalarioBase / 240m;
            horas.SalarioBruto =
                (horas.HorasNormales * salarioPorHora) +
                (horas.HorasExtras * salarioPorHora * 1.5m) +
                (horas.HorasDobles * salarioPorHora * 2.0m);

            datos.Actualizar(horas);
        }


        public void EliminarHorasLaboradas(int id)
        {
            if (id <= 0)
                throw new Exception("ID inválido.");

            datos.Eliminar(id);
        }
    }
}
