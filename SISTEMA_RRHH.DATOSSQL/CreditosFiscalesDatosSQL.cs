using SISTEMA_RRHH.ENTIDADES;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace SISTEMA_RRHH.DATOSSQL
{
    public class CreditosFiscalesDatosSQL : SQLServer
    {
        public List<CreditoFiscal> Listar()
        {
            List<CreditoFiscal> lista = new List<CreditoFiscal>();
            using (SqlCommand cmd = new SqlCommand("ListarCreditosFiscales", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                _conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new CreditoFiscal
                        {
                            CreditoFiscalID = Convert.ToInt32(reader["CreditoFiscalID"]),
                            Tipo = reader["Tipo"].ToString(),
                            Monto = Convert.ToDecimal(reader["Monto"])
                        });
                    }
                }
                _conn.Close();
            }
            return lista;
        }

        public void Insertar(CreditoFiscal credito)
        {
            using (SqlCommand cmd = new SqlCommand("InsertarCreditoFiscal", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tipo", credito.Tipo);
                cmd.Parameters.AddWithValue("@Monto", credito.Monto);
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

        public void Actualizar(CreditoFiscal credito)
        {
            using (SqlCommand cmd = new SqlCommand("ActualizarCreditoFiscal", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CreditoFiscalID", credito.CreditoFiscalID);
                cmd.Parameters.AddWithValue("@Tipo", credito.Tipo);
                cmd.Parameters.AddWithValue("@Monto", credito.Monto);
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlCommand cmd = new SqlCommand("EliminarCreditoFiscal", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CreditoFiscalID", id);
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }
    }
}
