using Microsoft.Data.SqlClient;
using SISTEMA_RRHH.ENTIDADES;
using SISTEMA_RRHH.ENTIDADES.DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace SISTEMA_RRHH.DATOSSQL
{
    public class FuncionarioDatosSQL : SQLServer
    {
        // ─────────────────────────────────────────────
        // Inserta un funcionario con datos completos
        // SP: InsertarFuncionario
        // ─────────────────────────────────────────────
        public string InsertarFuncionario(Funcionarios f)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("InsertarFuncionario", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NumeroIdentificacion", f.NumeroIdentificacion);
                    cmd.Parameters.AddWithValue("@Nombre", f.Nombre);
                    cmd.Parameters.AddWithValue("@PrimerApellido", f.PrimerApellido);
                    cmd.Parameters.AddWithValue("@SegundoApellido", f.SegundoApellido ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", f.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@FechaIngreso", f.FechaIngreso);
                    cmd.Parameters.AddWithValue("@CantidadHijos", f.CantidadHijosMayores);
                    cmd.Parameters.AddWithValue("@CantidadHijosCredito", f.CantidadHijosAplicables);
                    cmd.Parameters.AddWithValue("@EstadoCivilID", f.EstadoCivilID);
                    cmd.Parameters.AddWithValue("@SalarioBaseMensual", f.SalarioBaseMensual);
                    cmd.Parameters.AddWithValue("@TipoEmpleadoID", f.TipoEmpleado);
                    cmd.Parameters.AddWithValue("@EstadoFuncionarioID", f.EstadoFuncionario);
                    cmd.Parameters.AddWithValue("@DepartamentoID", f.Departamento);
                    cmd.Parameters.AddWithValue("@AportePensionComplementaria", f.AportePensionComplementaria);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", f.CorreoElectronico ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nacionalidad", f.Nacionalidad);
                    cmd.Parameters.AddWithValue("@Sexo", f.Sexo);


                    _conn.Open();
                    cmd.ExecuteNonQuery();
                    _conn.Close();

                    return "Funcionario insertado correctamente";
                }
            }
            catch (Exception ex)
            {
                return "Error al insertar funcionario: " + ex.Message;
            }
        }
        // ─────────────────────────────────────────────
        // Actualiza todos los datos de un funcionario
        // SP: ActualizarFuncionario
        // ─────────────────────────────────────────────
        public string ActualizarFuncionario(Funcionarios f)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("ActualizarFuncionario", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FuncionarioID", f.FuncionarioID);
                    cmd.Parameters.AddWithValue("@NumeroIdentificacion", f.NumeroIdentificacion);
                    cmd.Parameters.AddWithValue("@Nombre", f.Nombre);
                    cmd.Parameters.AddWithValue("@PrimerApellido", f.PrimerApellido);
                    cmd.Parameters.AddWithValue("@SegundoApellido", f.SegundoApellido ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", f.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@FechaIngreso", f.FechaIngreso);
                    cmd.Parameters.AddWithValue("@CantidadHijos", f.CantidadHijosMayores);
                    cmd.Parameters.AddWithValue("@CantidadHijosCredito", f.CantidadHijosAplicables);
                    cmd.Parameters.AddWithValue("@EstadoCivilID", f.EstadoCivilID);
                    cmd.Parameters.AddWithValue("@SalarioBaseMensual", f.SalarioBaseMensual);
                    cmd.Parameters.AddWithValue("@TipoEmpleadoID", f.TipoEmpleado);
                    cmd.Parameters.AddWithValue("@EstadoFuncionarioID", f.EstadoFuncionario);
                    cmd.Parameters.AddWithValue("@DepartamentoID", f.Departamento);
                    cmd.Parameters.AddWithValue("@AportePensionComplementaria", f.AportePensionComplementaria);
                    cmd.Parameters.AddWithValue("@CorreoElectronico", f.CorreoElectronico ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nacionalidad", f.Nacionalidad);
                    cmd.Parameters.AddWithValue("@Sexo", f.Sexo);


                    _conn.Open();
                    cmd.ExecuteNonQuery();
                    _conn.Close();

                    return "Funcionario actualizado correctamente";
                }
            }
            catch (Exception ex)
            {
                return "Error al actualizar funcionario: " + ex.Message;
            }
        }
        // ─────────────────────────────────────────────
        // Elimina un funcionario por su ID
        // SP: EliminarFuncionario
        // ─────────────────────────────────────────────
        public string EliminarFuncionario(int funcionarioID)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("EliminarFuncionario", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FuncionarioID", funcionarioID);

                    _conn.Open();
                    cmd.ExecuteNonQuery();
                    _conn.Close();

                    return "Funcionario eliminado correctamente";
                }
            }
            catch (Exception ex)
            {
                return "Error al eliminar funcionario: " + ex.Message;
            }
        }

        // ─────────────────────────────────────────────
        // Lista todos los funcionarios con datos crudos
        // SP: ListarFuncionarios
        // ─────────────────────────────────────────────
        public List<Funcionarios> ListarFuncionarios()
        {
            var lista = new List<Funcionarios>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("ListarFuncionarios", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    _conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Funcionarios
                            {
                                FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                                NumeroIdentificacion = reader["NumeroIdentificacion"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                PrimerApellido = reader["PrimerApellido"].ToString(),
                                SegundoApellido = reader["SegundoApellido"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                                FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                                CantidadHijosMayores = Convert.ToInt32(reader["CantidadHijos"]),
                                CantidadHijosAplicables = Convert.ToInt32(reader["CantidadHijosCredito"]),
                                EstadoCivilID = Convert.ToInt32(reader["EstadoCivilID"]),
                                SalarioBaseMensual = Convert.ToDecimal(reader["SalarioBaseMensual"]),
                                TipoEmpleado = reader["TipoEmpleadoID"].ToString(),
                                EstadoFuncionario = reader["EstadoFuncionarioID"].ToString(),
                                Departamento = reader["DepartamentoID"].ToString(),
                                AportePensionComplementaria = Convert.ToDecimal(reader["AportePensionComplementaria"]),
                                CorreoElectronico = reader["CorreoElectronico"].ToString(),
                                Nacionalidad = reader["Nacionalidad"].ToString(),
                                Sexo = reader["Sexo"].ToString()
                            });
                        }
                    }

                    _conn.Close();
                }
            }
            catch (Exception)
            {
                _conn.Close();
                throw;
            }

            return lista;
        }

        // ─────────────────────────────────────────────
        // Lista funcionarios con descripciones legibles
        // SP: ObtenerFuncionariosConDescripciones
        // ─────────────────────────────────────────────
        public List<FuncionarioVista> ObtenerFuncionariosConDescripciones()
        {
            var lista = new List<FuncionarioVista>();

            try
            {
                using (SqlCommand cmd = new SqlCommand("ObtenerFuncionariosConDescripciones", _conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    _conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new FuncionarioVista
                            {
                                FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                                NumeroIdentificacion = reader["NumeroIdentificacion"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                PrimerApellido = reader["PrimerApellido"].ToString(),
                                SegundoApellido = reader["SegundoApellido"].ToString(),
                                FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                                FechaIngreso = Convert.ToDateTime(reader["FechaIngreso"]),
                                CantidadHijosMayores = Convert.ToInt32(reader["CantidadHijosMayores"]),
                                CantidadHijosAplicables = Convert.ToInt32(reader["CantidadHijosAplicables"]),
                                SalarioBaseMensual = Convert.ToDecimal(reader["SalarioBaseMensual"]),
                                AportePensionComplementaria = Convert.ToDecimal(reader["AportePensionComplementaria"]),
                                CorreoElectronico = reader["CorreoElectronico"].ToString(),
                                Nacionalidad = reader["Nacionalidad"].ToString(),
                                Sexo = reader["Sexo"].ToString(),
                                EstadoCivilID = Convert.ToInt32(reader["EstadoCivilID"]),
                                TipoEmpleadoID = Convert.ToInt32(reader["TipoEmpleadoID"]),
                                EstadoFuncionarioID = Convert.ToInt32(reader["EstadoFuncionarioID"]),
                                DepartamentoID = Convert.ToInt32(reader["DepartamentoID"]),
                                EstadoCivilDescripcion = reader["EstadoCivilDescripcion"].ToString(),
                                TipoEmpleadoDescripcion = reader["TipoEmpleadoDescripcion"].ToString(),
                                EstadoFuncionarioDescripcion = reader["EstadoFuncionarioDescripcion"].ToString(),
                                DepartamentoDescripcion = reader["DepartamentoDescripcion"].ToString()

                            });
                        }
                    }

                    _conn.Close();
                }
            }
            catch (Exception)
            {
                _conn.Close();
                throw;
            }

            return lista;
        }

        // ─────────────────────────────────────────────
        // Devuelve solo el salario base por ID
        // No usa SP, es una consulta directa
        // ─────────────────────────────────────────────
        public Funcionarios ObtenerFuncionarioPorID(int funcionarioID)
        {
            Funcionarios f = null;

            using (SqlCommand cmd = new SqlCommand("SELECT FuncionarioID, SalarioBaseMensual FROM Funcionarios WHERE FuncionarioID = @id", _conn))
            {
                cmd.Parameters.AddWithValue("@id", funcionarioID);

                _conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        f = new Funcionarios
                        {
                            FuncionarioID = Convert.ToInt32(reader["FuncionarioID"]),
                            SalarioBaseMensual = Convert.ToDecimal(reader["SalarioBaseMensual"])
                        };
                    }
                }

                _conn.Close();
            }

            return f;
        }

    }
}