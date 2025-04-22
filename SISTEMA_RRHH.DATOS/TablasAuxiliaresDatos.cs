using System.Collections.Generic;
using System.Data.SqlClient;
using SISTEMA_RRHH.ENTIDADES.TablasAuxiliares;

namespace SISTEMA_RRHH.DATOS
{
    public class TablasAuxiliaresDatos
    {
        private readonly string connectionString = "Server=localhost;Database=Proyecto_Final;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;Connect Timeout=60;";

        public List<EstadoCivil> ObtenerEstadosCiviles()
        {
            var lista = new List<EstadoCivil>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT EstadoCivilID, Descripcion FROM EstadosCiviles", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new EstadoCivil
                        {
                            EstadoCivilID = Convert.ToInt32(reader["EstadoCivilID"]),
                            Descripcion = reader["Descripcion"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public List<TipoEmpleado> ObtenerTiposEmpleado()
        {
            var lista = new List<TipoEmpleado>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT TipoEmpleadoID, Descripcion FROM TiposEmpleado", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new TipoEmpleado
                        {
                            TipoEmpleadoID = Convert.ToInt32(reader["TipoEmpleadoID"]),
                            Descripcion = reader["Descripcion"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public List<EstadoFuncionario> ObtenerEstadosFuncionario()
        {
            var lista = new List<EstadoFuncionario>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT EstadoFuncionarioID, Descripcion FROM EstadosFuncionario", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new EstadoFuncionario
                        {
                            EstadoFuncionarioID = Convert.ToInt32(reader["EstadoFuncionarioID"]),
                            Descripcion = reader["Descripcion"].ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public List<Departamento> ObtenerDepartamentos()
        {
            var lista = new List<Departamento>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT DepartamentoID, Nombre FROM Departamentos", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Departamento
                        {
                            DepartamentoID = Convert.ToInt32(reader["DepartamentoID"]),
                            Nombre = reader["Nombre"].ToString()
                        });
                    }
                }
            }
            return lista;
        }
    }
}
