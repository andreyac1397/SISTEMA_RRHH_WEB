using SISTEMA_RRHH.ENTIDADES;
using SISTEMA_RRHH.ENTIDADES.DTO;
using SISTEMA_RRHH.DATOSSQL;
using System.Collections.Generic;

namespace SISTEMA_RRHH.DATOS
{
    public class FuncionarioDatos
    {
        private readonly FuncionarioDatosSQL sql = new FuncionarioDatosSQL();

        public void AgregarFuncionario(Funcionarios funcionario)
        {
            sql.InsertarFuncionario(funcionario);
        }

        public void ModificarFuncionario(Funcionarios funcionario)
        {
            sql.ActualizarFuncionario(funcionario);
        }

        //elimina por FuncionarioID
        public void EliminarFuncionarioPorID(int funcionarioID)
        {
            sql.EliminarFuncionario(funcionarioID);
        }

        

        public List<Funcionarios> ObtenerFuncionarios()
        {
            return sql.ListarFuncionarios();
        }

        //mostrar con descripcioneas
        public List<FuncionarioVista> ObtenerFuncionariosConDescripciones()
        {
            return sql.ObtenerFuncionariosConDescripciones();
        }
    }
}
