using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;

namespace BusinessLogic
{
    public class InicioSesionLogic
    {

        public short detectarUsuario(string Usuario, string password)
        {

            LogicaDatos logicaDatos = new LogicaDatos();


            // Verifica si el usuario o la contraseña están vacíos
            if (string.IsNullOrEmpty(Usuario) || string.IsNullOrEmpty(password))
                return 0;

            // Comprueba si el usuario existe
            if (!logicaDatos.existeUser(Usuario))
                return -1; // Usuario no encontrado

            // Verifica si la contraseña es correcta
            if (!logicaDatos.PasswordIsCorrecta(Usuario, password))
                return 1; // Contraseña incorrecta

            // Determina el rol del usuario
            string rol = logicaDatos.DeterminarRol(Usuario, password);
            return rol switch
            {
                "Administrativo" => 2, // Usuario administrativo
                "Secretario" => 3,     // Usuario secretario
                _ => -1
            };
        }

        public string NombreEmpl(string usuario)
        {

            LogicaDatos logicaDatos = new LogicaDatos();

            return logicaDatos.NombreUser(usuario);

        }
    }
}
