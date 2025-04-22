using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using SISTEMA_RRHH.DTO;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.DATOSSQL
{
    public class PlanillaDatosSQL : SQLServer
    {
        public List<FuncionarioPlanilla> ObtenerFuncionariosActivos()
        {
            var lista = new List<FuncionarioPlanilla>();

            using (var cmd = new SqlCommand("ListarFuncionariosActivosConDatos", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                _conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new FuncionarioPlanilla
                    {
                        FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                        NombreCompleto = reader["NombreCompleto"].ToString(),
                        SalarioBase = Convert.ToDecimal(reader["SalarioBaseMensual"]),
                        CantHijosAplicables = Convert.ToInt32(reader["CantHijosAplicables"]),
                        EstadoCivilID = Convert.ToInt32(reader["EstadoCivilID"]),
                        AportePensionComplementaria = Convert.ToDecimal(reader["AportePensionComplementaria"])
                    });
                }
                _conn.Close();
            }

            return lista;
        }

        public List<HorasLaboradas> ObtenerHorasPorPeriodo(int mes, int anno)
        {
            var lista = new List<HorasLaboradas>();

            using (var cmd = new SqlCommand("ListarHorasLaboradasPorPeriodo", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@mes", mes);
                cmd.Parameters.AddWithValue("@anno", anno);

                _conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new HorasLaboradas
                    {
                        FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                        HorasNormales = Convert.ToInt32(reader["HorasNormales"]),
                        HorasExtras = Convert.ToInt32(reader["HorasExtras"]),
                        HorasDobles = Convert.ToInt32(reader["HorasDobles"]),
                        SalarioBruto = Convert.ToDecimal(reader["SalarioBruto"]) // ← ESTA LÍNEA es clave
                    });
                }
                _conn.Close();
            }

            return lista;
        }


        public List<CargaSocial> ObtenerCargasSociales()
        {
            var lista = new List<CargaSocial>();

            using (var cmd = new SqlCommand("ListarCargasSociales", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                _conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new CargaSocial
                    {
                        PorcentajeTrabajador = Convert.ToDecimal(reader["PorcentajeTrabajador"])
                    });
                }
                _conn.Close();
            }

            return lista;
        }

        public List<CreditoFiscal> ObtenerCreditosFiscales()
        {
            var lista = new List<CreditoFiscal>();

            using (var cmd = new SqlCommand("ListarCreditosFiscales", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                _conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new CreditoFiscal
                    {
                        Tipo = reader["Tipo"].ToString(),
                        Monto = Convert.ToDecimal(reader["Monto"])
                    });
                }
                _conn.Close();
            }

            return lista;
        }

        public List<TractoImpuestoRenta> ObtenerTractosImpuestoRenta()
        {
            var lista = new List<TractoImpuestoRenta>();

            using (var cmd = new SqlCommand("ListarTractosImpuestoRenta", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                _conn.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new TractoImpuestoRenta
                    {
                        SalarioDesde = Convert.ToDecimal(reader["SalarioDesde"]),
                        SalarioHasta = Convert.ToDecimal(reader["SalarioHasta"]),
                        PorcentajeAplicable = Convert.ToDecimal(reader["PorcentajeAplicable"])
                    });
                }
                _conn.Close();
            }

            return lista;
        }
        public Planilla ObtenerComprobanteFuncionario(int funcionarioID, int mes, int anno)
        {
            Planilla planilla = null;

            using (SqlCommand cmd = new SqlCommand("ObtenerComprobanteFuncionario", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FuncionarioID", funcionarioID);
                cmd.Parameters.AddWithValue("@Mes", mes);
                cmd.Parameters.AddWithValue("@Anno", anno);

                _conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        planilla = new Planilla
                        {
                            PlanillaID = Convert.ToInt32(reader["PlanillaID"]),
                            FuncionarioID = funcionarioID,
                            SalarioBaseMensual = Convert.ToDecimal(reader["SalarioBaseMensual"]),
                            SalarioBruto = Convert.ToDecimal(reader["SalarioBruto"]),
                            AportePensionComplementaria = Convert.ToDecimal(reader["AportePensionComplementaria"]),
                            TotalCargasSociales = Convert.ToDecimal(reader["TotalCargasSociales"]),
                            TotalCreditosFiscales = Convert.ToDecimal(reader["TotalCreditosFiscales"]),
                            ImpuestoRenta = Convert.ToDecimal(reader["ImpuestoRenta"]),
                            SalarioNeto = Convert.ToDecimal(reader["SalarioNeto"])
                        };
                    }
                }

                _conn.Close();
            }

            return planilla;
        }

        public void InsertarResultado(Planilla resultado, int mes, int anno)
        {
            using (var cmd = new SqlCommand("InsertarPlanillaResultado", _conn))
            {

                int periodoID = int.Parse($"{anno}{mes:D2}");
                // Esto crea un int. Por ejemplo, si anno=2025 y mes=4 => periodoID = 202504

                cmd.Parameters.AddWithValue("@PeriodoID", periodoID);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FuncionarioID", resultado.FuncionarioID);
                cmd.Parameters.AddWithValue("@SalarioBaseMensual", resultado.SalarioBaseMensual);
                cmd.Parameters.AddWithValue("@SalarioBruto", resultado.SalarioBruto);
                cmd.Parameters.AddWithValue("@AportePensionComplementaria", resultado.AportePensionComplementaria);
                cmd.Parameters.AddWithValue("@TotalCargasSociales", resultado.TotalCargasSociales);
                cmd.Parameters.AddWithValue("@TotalCreditosFiscales", resultado.TotalCreditosFiscales);
                cmd.Parameters.AddWithValue("@ImpuestoRenta", resultado.ImpuestoRenta);
                cmd.Parameters.AddWithValue("@SalarioNeto", resultado.SalarioNeto);
                cmd.Parameters.AddWithValue("@UsuarioCalculoID", resultado.UsuarioCalculoID);

                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

    }
}