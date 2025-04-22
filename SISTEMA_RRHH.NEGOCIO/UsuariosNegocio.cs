using System.Collections.Generic;
using System.Threading.Tasks;
using SISTEMA_RRHH.DATOS;
using SISTEMA_RRHH.ENTIDADES;

namespace SISTEMA_RRHH.NEGOCIO
{
    public class UsuarioNegocio
    {
        private readonly UsuariosDatos _usuarioDatos;

        public UsuarioNegocio()
        {
            _usuarioDatos = new UsuariosDatos();
        }

        // 🔐 LOGIN normal (correo + contraseña)
        public async Task<Usuarios> IniciarSesionAsync(string correo, string contrasena)
        {
            return await _usuarioDatos.ValidarLoginAsync(correo, contrasena);
        }

        // 🔄 Validación de correo y contraseña actual para cambio manual
        public async Task<int?> ObtenerUsuarioIDPorCorreoYContrasenaAsync(string correo, string contrasena)
        {
            return await _usuarioDatos.ObtenerUsuarioIDPorCorreoYContrasenaAsync(correo, contrasena);
        }

        // 🔍 Obtener datos por ID (para editar en mantenimiento)
        public async Task<Usuarios> ObtenerUsuarioPorIDAsync(int usuarioID)
        {
            if (usuarioID <= 0)
                return null;

            return await _usuarioDatos.ObtenerUsuarioPorIDAsync(usuarioID);
        }

        // 🔒 Cambiar contraseña (mantiene lógica actual)
        public async Task<string> CambiarContrasenaAsync(int usuarioID, string nuevaContrasena)
        {
            if (usuarioID <= 0)
                return "ID de usuario inválido.";

            if (string.IsNullOrWhiteSpace(nuevaContrasena) || nuevaContrasena.Length < 8)
                return "La nueva contraseña debe tener al menos 8 caracteres.";

            return await _usuarioDatos.CambiarContrasenaAsync(usuarioID, nuevaContrasena);
        }

        // ✏️ Actualizar correo, departamento, estado y flag de cambio de contraseña
        public async Task<string> ActualizarUsuarioYFuncionarioAsync(Usuarios usuario)
        {
            if (usuario == null)
                return "Usuario inválido.";

            if (string.IsNullOrWhiteSpace(usuario.CorreoElectronico))
                return "El correo no puede estar vacío.";

            if (!usuario.CorreoElectronico.Contains("@"))
                return "El correo debe tener un formato válido.";

            if (usuario.DepartamentoID <= 0)
                return "Debe seleccionar un departamento válido.";

            if (usuario.EstadoFuncionarioID <= 0)
                return "Debe seleccionar un estado válido.";

            if (string.IsNullOrWhiteSpace(usuario.Contrasena) || usuario.Contrasena.Length < 8)
                return "La contraseña debe tener al menos 8 caracteres.";

            var existente = await _usuarioDatos.ObtenerPorCorreoAsync(usuario.CorreoElectronico);
            if (existente != null && existente.UsuarioID != usuario.UsuarioID)
                return "Ya existe otro usuario con ese correo electrónico.";

            return await _usuarioDatos.ActualizarUsuarioAsync(
                usuario.UsuarioID,
                usuario.CorreoElectronico,
                usuario.DepartamentoID,
                usuario.EstadoFuncionarioID,
                usuario.DebeCambiarContrasena,
                usuario.Contrasena

            );
        }

        // 🗑️ Eliminar usuario y funcionario asociado
        public async Task<string> EliminarUsuarioYFuncionarioAsync(int usuarioID)
        {
            if (usuarioID <= 0)
                return "ID de usuario inválido.";

            return await _usuarioDatos.EliminarUsuarioYFuncionarioAsync(usuarioID);
        }

        // 📋 Listar todos los usuarios para la vista de mantenimiento
        public List<Usuarios> ListarUsuarios()
        {
            return _usuarioDatos.ListarUsuarios();
        }
    }
}
