using System;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace SISTEMA_RRHH.DATOSSQL
{
    public class SQLServer
    {
        protected SqlConnection _conn;

        public SQLServer()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.DatosSql.json", optional: false, reloadOnChange: true)
                .Build();

            string connectionString = builder.GetConnectionString("strConnDesarrollo");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("❌ No se pudo leer la cadena de conexión 'strConnDesarrollo'. Verifica el nombre o si el archivo fue copiado correctamente.");
            }

            _conn = new SqlConnection(connectionString);
        }
    }
}
