using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.DATOSSQL
{
    public class HorasLaboradasDatosSQL : SQLServer
    {
        public List<HorasLaboradas> Listar(int mes, int anno)
        {
            var lista = new List<HorasLaboradas>();
            using (SqlCommand cmd = new SqlCommand("ListarHorasLaboradas", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PeriodoMes", mes);
                cmd.Parameters.AddWithValue("@PeriodoAnno", anno);

                _conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new HorasLaboradas
                        {
                            HorasLaboradasID = Convert.ToInt32(reader["HorasLaboradasID"]),
                            FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            HorasNormales = Convert.ToDecimal(reader["HorasNormales"]),
                            HorasExtras = Convert.ToDecimal(reader["HorasExtras"]),
                            HorasDobles = Convert.ToDecimal(reader["HorasDobles"]),
                            SalarioBase = Convert.ToDecimal(reader["SalarioBase"]),
                            SalarioBruto = Convert.ToDecimal(reader["SalarioBruto"]),
                            PeriodoMes = Convert.ToInt32(reader["PeriodoMes"]),
                            PeriodoAnno = Convert.ToInt32(reader["PeriodoAnno"])
                        });
                    }
                }
                _conn.Close();
            }
            return lista;
        }

        public List<HorasLaboradas> ListarPorPeriodo(int mes, int anno)
        {
            var lista = new List<HorasLaboradas>();
            using (SqlCommand cmd = new SqlCommand("ListarHorasLaboradasPorPeriodo", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PeriodoMes", mes);
                cmd.Parameters.AddWithValue("@PeriodoAnno", anno);

                _conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new HorasLaboradas
                        {
                            HorasLaboradasID = Convert.ToInt32(reader["HorasLaboradasID"]),
                            FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            HorasNormales = Convert.ToInt32(reader["HorasNormales"]),
                            HorasExtras = Convert.ToInt32(reader["HorasExtras"]),
                            HorasDobles = Convert.ToInt32(reader["HorasDobles"]),
                            SalarioBase = Convert.ToDecimal(reader["SalarioBase"]),
                            SalarioBruto = Convert.ToDecimal(reader["SalarioBruto"]),
                            PeriodoMes = Convert.ToInt32(reader["PeriodoMes"]),
                            PeriodoAnno = Convert.ToInt32(reader["PeriodoAnno"])
                        });
                    }
                }
                _conn.Close();
            }
            return lista;
        }

        public void RecalcularSalariosBrutos()
        {
            using (SqlCommand cmd = new SqlCommand("RecalcularSalariosBrutos", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

        public void Insertar(HorasLaboradas horas)
        {
            using (SqlCommand cmd = new SqlCommand("InsertarHorasLaboradas", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FuncionarioID", horas.FuncionarioID);
                cmd.Parameters.AddWithValue("@HorasNormales", horas.HorasNormales);
                cmd.Parameters.AddWithValue("@HorasExtras", horas.HorasExtras);
                cmd.Parameters.AddWithValue("@HorasDobles", horas.HorasDobles);
                cmd.Parameters.AddWithValue("@SalarioBase", horas.SalarioBase);
                cmd.Parameters.AddWithValue("@SalarioBruto", horas.SalarioBruto);
                cmd.Parameters.AddWithValue("@PeriodoMes", horas.PeriodoMes);
                cmd.Parameters.AddWithValue("@PeriodoAnno", horas.PeriodoAnno);

                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

        public void Actualizar(HorasLaboradas horas)
        {
            using (SqlCommand cmd = new SqlCommand("ActualizarHorasLaboradas", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HorasLaboradasID", horas.HorasLaboradasID);
                cmd.Parameters.AddWithValue("@FuncionarioID", horas.FuncionarioID);
                cmd.Parameters.AddWithValue("@HorasNormales", horas.HorasNormales);
                cmd.Parameters.AddWithValue("@HorasExtras", horas.HorasExtras);
                cmd.Parameters.AddWithValue("@HorasDobles", horas.HorasDobles);
                cmd.Parameters.AddWithValue("@SalarioBase", horas.SalarioBase);
                cmd.Parameters.AddWithValue("@SalarioBruto", horas.SalarioBruto);
                cmd.Parameters.AddWithValue("@PeriodoMes", horas.PeriodoMes);
                cmd.Parameters.AddWithValue("@PeriodoAnno", horas.PeriodoAnno);

                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlCommand cmd = new SqlCommand("EliminarHorasLaboradas", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@HorasLaboradasID", id);
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }
    }
}
