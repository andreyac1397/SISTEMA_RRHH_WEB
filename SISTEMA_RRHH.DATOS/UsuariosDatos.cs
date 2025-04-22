using SISTEMA_RRHH.DATOSSQL;
using SISTEMA_RRHH.ENTIDADES;
using System.Threading.Tasks;

namespace SISTEMA_RRHH.DATOS
{
    public class UsuariosDatos
    {
        private readonly UsuarioDatosSQL _sql;

        public UsuariosDatos()
        {
            _sql = new UsuarioDatosSQL();
        }

        // Validación de inicio de sesión
        public async Task<Usuarios> ValidarLoginAsync(string correo, string contrasena)
        {
            return await _sql.ValidarCredencialesAsync(correo, contrasena);
        }

        // Validación segura para cambio de contraseña manual
        public async Task<int?> ObtenerUsuarioIDPorCorreoYContrasenaAsync(string correo, string contrasena)
        {
            return await _sql.ObtenerUsuarioIDPorCorreoYContrasena(correo, contrasena);
        }

        //Obtener un usuario por ID para mantenimiento
        public async Task<Usuarios> ObtenerUsuarioPorIDAsync(int usuarioID)
        {
            return await _sql.ObtenerUsuarioPorIDAsync(usuarioID);
        }

        //Cambio de contraseña desde mantenimiento o lógica de primer ingreso
        public async Task<string> CambiarContrasenaAsync(int usuarioID, string nuevaContrasena)
        {
            return await _sql.CambiarContrasena(usuarioID, nuevaContrasena);
        }
        public List<Usuarios> ListarUsuarios()
        {
            return _sql.ListarUsuarios();
        }
        public async Task<Usuarios> ObtenerPorCorreoAsync(string correo)
        {
            return await _sql.ObtenerPorCorreoAsync(correo);
        }

        // Actualizar usuario y funcionario (correo, estado, departamento, etc.)
        public async Task<string> ActualizarUsuarioAsync(int usuarioID, string correo, int departamentoID, int estadoID, bool debeCambiar, string contrasena)
        {
            return await _sql.ActualizarUsuario(usuarioID, correo, departamentoID, estadoID, debeCambiar, contrasena);
        }

        //Eliminar usuario y su funcionario asociado
        public async Task<string> EliminarUsuarioYFuncionarioAsync(int usuarioID)
        {
            return await _sql.EliminarUsuarioYFuncionario(usuarioID);
        }
    }
}

