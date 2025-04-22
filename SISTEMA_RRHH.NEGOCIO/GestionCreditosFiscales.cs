using System;
using System.Collections.Generic;
using SISTEMA_RRHH.DATOS;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.NEGOCIO
{
    public class GestionCreditosFiscales
    {
        // Instancia de la clase de acceso a datos
        private readonly CreditosFiscalesDatos datos = new CreditosFiscalesDatos();

        // Método para obtener todos los créditos fiscales registrados
        public List<CreditoFiscal> ListarCreditosFiscales()
        {
            return datos.Listar();
        }

        // Método para insertar un nuevo crédito fiscal
        public void InsertarCreditoFiscal(CreditoFiscal credito)
        {
            // Validación: el tipo de crédito no puede estar vacío ni ser solo espacios
            if (string.IsNullOrWhiteSpace(credito.Tipo))
                throw new Exception("El tipo de crédito fiscal es obligatorio.");

            // Validación: el monto no puede ser negativo
            if (credito.Monto < 0)
                throw new Exception("El monto del crédito fiscal no puede ser negativo.");

            // Inserta el crédito fiscal en la base de datos
            datos.Insertar(credito);
        }

        // Método para actualizar un crédito fiscal existente
        public void ActualizarCreditoFiscal(CreditoFiscal credito)
        {
            // Validación: el ID debe ser mayor que cero
            if (credito.CreditoFiscalID <= 0)
                throw new Exception("El ID del crédito es inválido.");

            // Validación: el tipo no puede estar vacío
            if (string.IsNullOrWhiteSpace(credito.Tipo))
                throw new Exception("El tipo de crédito fiscal es obligatorio.");

            // Validación: el monto no puede ser negativo
            if (credito.Monto < 0)
                throw new Exception("El monto del crédito fiscal no puede ser negativo.");

            // Actualiza el crédito fiscal en la base de datos
            datos.Actualizar(credito);
        }

        // Método para eliminar un crédito fiscal por su ID
        public void EliminarCreditoFiscal(int id)
        {
            // Validación: el ID debe ser mayor que cero
            if (id <= 0)
                throw new Exception("ID inválido para eliminación.");

            // Elimina el crédito fiscal de la base de datos
            datos.Eliminar(id);
        }
    }
}


