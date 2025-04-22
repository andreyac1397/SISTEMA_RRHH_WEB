using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.DATOSSQL
{
    public class CargasSocialesDatosSQL : SQLServer
    {
        public List<CargaSocial> Listar()
        {
            var lista = new List<CargaSocial>();
            using (SqlCommand cmd = new SqlCommand("ListarCargasSociales", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                _conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new CargaSocial
                        {
                            CargaSocialID = Convert.ToInt32(reader["CargaSocialID"]),
                            Concepto = reader["Concepto"].ToString(),
                            PorcentajePatrono = Convert.ToDecimal(reader["PorcentajePatrono"]),
                            PorcentajeTrabajador = Convert.ToDecimal(reader["PorcentajeTrabajador"])
                        });
                    }
                }
                _conn.Close();
            }
            return lista;
        }

        public void Insertar(CargaSocial carga)
        {
            using (SqlCommand cmd = new SqlCommand("InsertarCargaSocial", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Concepto", carga.Concepto);
                cmd.Parameters.AddWithValue("@PorcentajePatrono", carga.PorcentajePatrono);
                cmd.Parameters.AddWithValue("@PorcentajeTrabajador", carga.PorcentajeTrabajador);
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

        public void Actualizar(CargaSocial carga)
        {
            using (SqlCommand cmd = new SqlCommand("ActualizarCargaSocial", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CargaSocialID", carga.CargaSocialID);
                cmd.Parameters.AddWithValue("@Concepto", carga.Concepto);
                cmd.Parameters.AddWithValue("@PorcentajePatrono", carga.PorcentajePatrono);
                cmd.Parameters.AddWithValue("@PorcentajeTrabajador", carga.PorcentajeTrabajador);
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlCommand cmd = new SqlCommand("EliminarCargaSocial", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CargaSocialID", id);
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }
    }
}

