using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.DATOSSQL
{
    public class UsuarioDatosSQL : SQLServer
    {
        public async Task<Usuarios> ValidarCredencialesAsync(string correo, string contrasena)
        {
            Usuarios usuario = null;
            using (var cmd = new SqlCommand("SP_ValidarCredenciales", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@usuario", correo);
                cmd.Parameters.AddWithValue("@pass", contrasena);

                await _conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usuario = new Usuarios
                        {
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                            EstadoFuncionarioID = Convert.ToInt32(reader["EstadoFuncionarioID"]),
                            DebeCambiarContrasena = Convert.ToBoolean(reader["DebeCambiarContrasena"]),
                            DepartamentoID = Convert.ToInt32(reader["DepartamentoID"]),
                            NombreDepartamento = reader["NombreDepartamento"].ToString(),
                            NombreCompleto = reader["NombreCompleto"].ToString()    // <-- ahora existe
                        };
                    }
                }
                await _conn.CloseAsync();
            }
            return usuario;
        }
        public async Task<Usuarios> ObtenerPorCorreoAsync(string correo)
        {
            Usuarios usuario = null;

            using (var cmd = new SqlCommand("SP_ObtenerUsuarioPorCorreo", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Correo", correo);

                await _conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usuario = new Usuarios
                        {
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                            DepartamentoID = Convert.ToInt32(reader["DepartamentoID"]),
                            EstadoFuncionarioID = Convert.ToInt32(reader["EstadoFuncionarioID"]),
                            DebeCambiarContrasena = Convert.ToBoolean(reader["DebeCambiarContrasena"])
                            // Agregá más campos si necesitás mostrar NombreCompleto, etc.
                        };
                    }
                }
                await _conn.CloseAsync();
            }

            return usuario;
        }



        public async Task<int?> ObtenerUsuarioIDPorCorreoYContrasena(string correo, string contrasena)
        {
            int? id = null;
            using (var cmd = new SqlCommand("SP_ValidarCorreoYContrasena", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@correo", correo);
                cmd.Parameters.AddWithValue("@contrasena", contrasena);

                await _conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();
                await _conn.CloseAsync();

                if (result != null && result != DBNull.Value)
                    id = Convert.ToInt32(result);
            }
            return id;
        }

        // ListarUsuarios -->
        public List<Usuarios> ListarUsuarios()
        {
            var lista = new List<Usuarios>();

            using (var cmd = new SqlCommand("SP_ListarUsuarios", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                _conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Usuarios
                        {
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                            Contrasena = reader["Contrasena"].ToString(),
                            DepartamentoID = Convert.ToInt32(reader["DepartamentoID"]), // 🔥 Faltaba
                            NombreDepartamento = reader["NombreDepartamento"].ToString(),
                            EstadoFuncionarioID = Convert.ToInt32(reader["EstadoFuncionarioID"]), // 🔥 Faltaba
                            Estado = reader["Estado"].ToString(),
                            DebeCambiarContrasena = Convert.ToBoolean(reader["DebeCambiarContrasena"])
                        });

                    }
                }

                _conn.Close();
            }

            return lista;
        }


        /// <summary>
        /// Recupera un usuario completo (incluye NombreCompleto, Correo, Departamento, Estado, DebeCambiarContrasena)
        /// </summary>
        public async Task<Usuarios> ObtenerUsuarioPorIDAsync(int usuarioID)
        {
            Usuarios usuario = null;

            using (var cmd = new SqlCommand("SP_ObtenerUsuarioPorID", _conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);

                await _conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        usuario = new Usuarios
                        {
                            UsuarioID = Convert.ToInt32(reader["UsuarioID"]),
                            FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            CorreoElectronico = reader["CorreoElectronico"].ToString(),
                            DepartamentoID = Convert.ToInt32(reader["DepartamentoID"]),
                            NombreDepartamento = reader["NombreDepartamento"].ToString(),
                            EstadoFuncionarioID = Convert.ToInt32(reader["EstadoFuncionarioID"]),
                            DebeCambiarContrasena = Convert.ToBoolean(reader["DebeCambiarContrasena"])
                        };

                    }
                }
                await _conn.CloseAsync();
            }

            return usuario;
        }

        public async Task<string> ActualizarUsuarioYFuncionarioAsync(Usuarios usuario)
        {
            try
            {
                using (var cmd = new SqlCommand("SP_ActualizarUsuarioYFuncionario", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UsuarioID", usuario.UsuarioID);
                    cmd.Parameters.AddWithValue("@Correo", usuario.CorreoElectronico);
                    cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DepartamentoID", usuario.DepartamentoID);
                    cmd.Parameters.AddWithValue("@EstadoFuncionarioID", usuario.EstadoFuncionarioID);
                    cmd.Parameters.AddWithValue("@DebeCambiarContrasena", usuario.DebeCambiarContrasena);

                    await _conn.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    await _conn.CloseAsync();

                    return result?.ToString() ?? "Actualización exitosa.";
                }
            }
            catch (Exception ex)
            {
                _conn.Close();
                return "Error al actualizar: " + ex.Message;
            }
        }


        public async Task<string> CambiarContrasena(int usuarioID, string nuevaContrasena)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_CambiarContrasena", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    cmd.Parameters.AddWithValue("@NuevaContrasena", nuevaContrasena);

                    await _conn.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    await _conn.CloseAsync();

                    return result?.ToString() ?? "Contraseña actualizada.";
                }
            }
            catch (Exception ex)
            {
                _conn.Close();
                return "Error al cambiar la contraseña: " + ex.Message;
            }
        }

        public async Task<string> ActualizarUsuario(int usuarioID, string correo, int departamentoID,  int estadoID, bool debeCambiar, string contrasena)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_ActualizarUsuario", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", correo);
                    cmd.Parameters.AddWithValue("@DepartamentoID", departamentoID);
                    cmd.Parameters.AddWithValue("@EstadoFuncionarioID", estadoID);
                    cmd.Parameters.AddWithValue("@DebeCambiarContrasena", debeCambiar);
                    cmd.Parameters.AddWithValue("@Contrasena", contrasena);
                    await _conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await _conn.CloseAsync();

                    return "Usuario actualizado correctamente.";
                }
            }
            catch (Exception ex)
            {
                _conn.Close();
                return "Error al actualizar el usuario: " + ex.Message;
            }
        }

        // ✅ NUEVO MÉTODO: Eliminar usuario y funcionario relacionado
        public async Task<string> EliminarUsuarioYFuncionario(int usuarioID)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SP_EliminarUsuarioYFuncionario", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UsuarioID", usuarioID);

                    await _conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                    await _conn.CloseAsync();

                    return "Usuario y funcionario eliminados correctamente.";
                }
            }
            catch (Exception ex)
            {
                _conn.Close();
                return "Error al eliminar: " + ex.Message;
            }
        }

    }
}
