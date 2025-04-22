using SISTEMA_RRHH.ENTIDADES;
using SISTEMA_RRHH.ENTIDADES.DTO;
using SISTEMA_RRHH.DATOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SISTEMA_RRHH.NEGOCIO
{
    // ─────────────────────────────────────────────
    // Clase de lógica de negocio para los funcionarios.
    // Se encarga de validar datos y comunicarse con la capa de datos.
    // ─────────────────────────────────────────────
    public class GestionFuncionarios
    {
        // Instancia de la capa de datos intermedia (FuncionarioDatos)
        private readonly FuncionarioDatos datos = new FuncionarioDatos();

        // ─────────────────────────────────────────────
        // Valida todos los campos obligatorios del funcionario.
        // Devuelve false si hay errores y asigna el mensaje a "mensajeError".
        // ─────────────────────────────────────────────
        public bool Validar(Funcionarios funcionario, out string mensajeError)
        {
            mensajeError = "";

            // Validación de número de identificación (solo números)
            if (string.IsNullOrWhiteSpace(funcionario.NumeroIdentificacion)
                || !Regex.IsMatch(funcionario.NumeroIdentificacion, @"^\d+$"))
                return Error("La identificación es inválida.", ref mensajeError);

            // Validación de nombre (solo letras)
            if (string.IsNullOrWhiteSpace(funcionario.Nombre)
                || !Regex.IsMatch(funcionario.Nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                return Error("El nombre es inválido.", ref mensajeError);

            // Validación del primer apellido
            if (string.IsNullOrWhiteSpace(funcionario.PrimerApellido)
                || !Regex.IsMatch(funcionario.PrimerApellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                return Error("El primer apellido es inválido.", ref mensajeError);

            // Validación opcional del segundo apellido
            if (!string.IsNullOrWhiteSpace(funcionario.SegundoApellido)
                && !Regex.IsMatch(funcionario.SegundoApellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                return Error("El segundo apellido es inválido.", ref mensajeError);

            // Validación de nacionalidad
            if (string.IsNullOrWhiteSpace(funcionario.Nacionalidad)
                || !Regex.IsMatch(funcionario.Nacionalidad, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                return Error("La nacionalidad es inválida.", ref mensajeError);

            // Salario base debe ser positivo
            if (funcionario.SalarioBaseMensual < 0)
                return Error("El salario debe ser positivo.", ref mensajeError);

            // Cantidad de hijos no puede ser negativa
            if (funcionario.CantidadHijosMayores < 0 || funcionario.CantidadHijosAplicables < 0)
                return Error("La cantidad de hijos debe ser positiva.", ref mensajeError);

            // Validación del sexo (letras solamente)
            if (string.IsNullOrWhiteSpace(funcionario.Sexo)
                || !Regex.IsMatch(funcionario.Sexo, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                return Error("El sexo es inválido.", ref mensajeError);

            // Tipo de empleado: letras, números y guiones permitidos
            if (string.IsNullOrWhiteSpace(funcionario.TipoEmpleado)
                || !Regex.IsMatch(funcionario.TipoEmpleado, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-]+$"))
                return Error("El tipo de empleado es inválido.", ref mensajeError);

            // Validación de fechas lógicas
            if (funcionario.FechaNacimiento > DateTime.Now)
                return Error("La fecha de nacimiento no puede estar en el futuro.", ref mensajeError);

            if ((DateTime.Now.Year - funcionario.FechaNacimiento.Year) > 100)
                return Error("La persona no puede tener más de 100 años.", ref mensajeError);

            if (funcionario.FechaIngreso > DateTime.Now)
                return Error("La fecha de ingreso no puede estar en el futuro.", ref mensajeError);

            if (funcionario.FechaIngreso < funcionario.FechaNacimiento)
                return Error("La fecha de ingreso no puede ser anterior a la fecha de nacimiento.", ref mensajeError);

            if ((DateTime.Now.Year - funcionario.FechaIngreso.Year) > 60)
                return Error("La fecha de ingreso indica más de 60 años de trabajo. Verifique el dato.", ref mensajeError);

            // Verifica si ya existe un funcionario con la misma identificación (evita duplicados)
            var funcionariosExistentes = datos.ObtenerFuncionarios();
            var yaExiste = funcionariosExistentes.Any(f =>
                f.NumeroIdentificacion == funcionario.NumeroIdentificacion &&
                f.FuncionarioID != funcionario.FuncionarioID);

            if (yaExiste)
                return Error("Ya existe un funcionario con esta identificación.", ref mensajeError);
            // Validación de correo duplicado
            var correoDuplicado = funcionariosExistentes.Any(f =>
                f.CorreoElectronico.Equals(funcionario.CorreoElectronico, StringComparison.OrdinalIgnoreCase) &&
                f.FuncionarioID != funcionario.FuncionarioID);

            if (correoDuplicado)
                return Error("Ya existe un funcionario con este correo electrónico.", ref mensajeError);

            // Validación de aporte a pensión complementaria (solo números positivos)
            if (funcionario.AportePensionComplementaria < 0)
                return Error("El aporte a pensión complementaria no puede ser negativo.", ref mensajeError);

            return true;
        }

        // ─────────────────────────────────────────────
        // Método auxiliar que asigna un mensaje de error
        // ─────────────────────────────────────────────
        private bool Error(string mensaje, ref string acumulado)
        {
            acumulado = mensaje;
            return false;
        }

        // ─────────────────────────────────────────────
        // Llama a la capa de datos para insertar un nuevo funcionario
        // ─────────────────────────────────────────────
        public void AgregarFuncionario(Funcionarios funcionario)
        {
            datos.AgregarFuncionario(funcionario);
        }

        // ─────────────────────────────────────────────
        // Llama a la capa de datos para actualizar un funcionario
        // ─────────────────────────────────────────────
        public void ModificarFuncionario(Funcionarios funcionario)
        {
            datos.ModificarFuncionario(funcionario);
        }

        // ─────────────────────────────────────────────
        // Llama a la capa de datos para eliminar un funcionario
        // ─────────────────────────────────────────────
        public void EliminarFuncionario(int funcionarioID)
        {
            datos.EliminarFuncionarioPorID(funcionarioID);
        }

        // ─────────────────────────────────────────────
        // Devuelve lista básica de funcionarios
        // ─────────────────────────────────────────────
        public List<Funcionarios> ObtenerFuncionarios()
        {
            return datos.ObtenerFuncionarios();
        }

        // ─────────────────────────────────────────────
        // Devuelve funcionarios con catálogos legibles para la vista
        // ─────────────────────────────────────────────
        public List<FuncionarioVista> ObtenerFuncionariosConDescripciones()
        {
            return datos.ObtenerFuncionariosConDescripciones();
        }
    }
}
