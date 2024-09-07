using Microsoft.AspNetCore.Mvc;
using Registro.Models;
using System.Data.SqlClient;

namespace Registro.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IConfiguration _configuration;

        public UsuariosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registrar(UsuarioModel model)
        {
            //La instrucción using en C# es una construcción que
            //asegura que los recursos que se usan dentro de un bloque
            //se liberen correctamente una vez que el bloque termina.
            using (SqlConnection con = new(_configuration["ConnectionStrings:Conexion"]))
            {
                // sqlcomand sirveapra crear instrucciones sql
                using (SqlCommand cmd = new("sp_registrar",con ))
                {
                    //DECLARAMOS QUE EL COMANDO ES DEL TIPO DE PROCEDIMIENTO ALMACENADO
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //INSTRUCCION COMPLEJA
                    //1.AGREGAMOS PARAMETROS A NUESTRO COMANDO Y DEFINIMOS SU TIPO
                    //2.LES ASIGNAMOS UN VALOR A ESTOS PARAMETROS, EN ESTE CASO LOS DEL MODELO
                    cmd.Parameters.Add("@Nombre", System.Data.SqlDbType.VarChar).Value = model.Nombre;
                    cmd.Parameters.Add("@Edad", System.Data.SqlDbType.Int).Value = model.Edad;
                    cmd.Parameters.Add("@Correo", System.Data.SqlDbType.VarChar).Value = model.Email;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                };
                return Redirect("Index");
            }
        }

        public IActionResult Index() {
            return View();
        }
    }
}
