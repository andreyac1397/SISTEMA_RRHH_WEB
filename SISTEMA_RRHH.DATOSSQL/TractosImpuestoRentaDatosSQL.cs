using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.DATOSSQL
{
    public class TractosImpuestoRentaDatosSQL : SQLServer
    {
        public List<TractoImpuestoRenta> Listar()
        {
            var lista = new List<TractoImpuestoRenta>();
            using (SqlCommand cmd = new SqlCommand("ListarTractosImpuestoRenta", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                _conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new TractoImpuestoRenta
                        {
                            TractoID = Convert.ToInt32(reader["TractoID"]),
                            SalarioDesde = Convert.ToDecimal(reader["SalarioDesde"]),
                            SalarioHasta = Convert.ToDecimal(reader["SalarioHasta"]),
                            PorcentajeAplicable = Convert.ToDecimal(reader["PorcentajeAplicable"]),
                            
                        });
                    }
                }
                _conn.Close();
            }
            return lista;
        }
        public TractoImpuestoRenta ObtenerPorID(int id)
        {
            TractoImpuestoRenta obj = null;
            using (SqlCommand cmd = new SqlCommand("BuscarTractoImpuestoRentaPorID", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TractoID", id);

                _conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        obj = new TractoImpuestoRenta()
                        {
                            TractoID = Convert.ToInt32(reader["TractoID"]),
                            SalarioDesde = Convert.ToDecimal(reader["SalarioDesde"]),
                            SalarioHasta = Convert.ToDecimal(reader["SalarioHasta"]),
                            PorcentajeAplicable = Convert.ToDecimal(reader["PorcentajeAplicable"]),
                           
                        };
                    }
                }
                _conn.Close();
            }
            return obj;
        }

        public void Insertar(TractoImpuestoRenta tracto)
        {
            using (SqlCommand cmd = new SqlCommand("InsertarTractoImpuestoRenta", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SalarioDesde", tracto.SalarioDesde);
                cmd.Parameters.AddWithValue("@SalarioHasta", tracto.SalarioHasta);
                cmd.Parameters.AddWithValue("@PorcentajeAplicable", tracto.PorcentajeAplicable);
              
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

        public void Actualizar(TractoImpuestoRenta tracto)
        {
            using (SqlCommand cmd = new SqlCommand("ActualizarTractoImpuestoRenta", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TractoID", tracto.TractoID);
                cmd.Parameters.AddWithValue("@SalarioDesde", tracto.SalarioDesde);
                cmd.Parameters.AddWithValue("@SalarioHasta", tracto.SalarioHasta);
                cmd.Parameters.AddWithValue("@PorcentajeAplicable", tracto.PorcentajeAplicable);
                
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlCommand cmd = new SqlCommand("EliminarTractoImpuestoRenta", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TractoID", id);
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }
    }
}
