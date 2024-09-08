using Microsoft.AspNetCore.Mvc;
using Registro.Models;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Registro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsuarioController : ControllerBase
    {

        private readonly IConfiguration _connection;
        public ApiUsuarioController(IConfiguration conection)
        {

            _connection = conection;
        }

        // GET: api/<ApiUsuarioController>
        [HttpGet]
        public List<UsuarioModel> Get()
        {
            using (SqlConnection con = new(_connection["ConnectionStrings:Conexion"]))
            {
                using (SqlCommand cmd = new("sp_usuarios", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    con.Open();
                    SqlDataAdapter da = new(cmd);
                    DataTable dt = new();
                    da.Fill(dt);
                    da.Dispose();
                    List<UsuarioModel> usuarios = new();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        usuarios.Add(new UsuarioModel
                        {
                            IDUsuario = Convert.ToInt32(dt.Rows[i][0]),
                            Nombre = (dt.Rows[i][1]).ToString(),
                            Edad = Convert.ToInt32(dt.Rows[i][2]),
                            Email = (dt.Rows[i][3]).ToString()
                        });

                    }
                    con.Close();
                    return usuarios;
                }
            }
        }

        // GET api/<ApiUsuarioController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            using (SqlConnection con = new(_connection["ConnectionStrings:Conexion"]))
            {
                using (SqlCommand cmd = new("sp_usuario", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    con.Open();
                    SqlDataAdapter da = new(cmd);
                    DataTable dt = new();
                    da.Fill(dt);
                    da.Dispose();
                    var usuario = new UsuarioModel
                    {
                        IDUsuario = Convert.ToInt32(dt.Rows[0][0]),
                        Nombre = (dt.Rows[0][1]).ToString(),
                        Edad = Convert.ToInt32(dt.Rows[0][2]),
                        Email = Convert.ToString(dt.Rows[0][3])
                    };
                    con.Close();
                    return Ok(usuario);
                }
            };
        }
        // POST api/<ApiUsuarioController>
        [HttpPost]
        public IActionResult Post([FromBody] UsuarioModel usuario)
        {
            try
            {
                using (SqlConnection con = new(_connection["ConnectionStrings:Conexion"]))
                {
                    using (SqlCommand cmd = new("sp_registrar", con)) {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Nombre", SqlDbType.Text).Value = usuario.Nombre;
                        cmd.Parameters.Add("@Edad", SqlDbType.Int).Value = usuario.Edad;
                        cmd.Parameters.Add("@Correo", SqlDbType.Text).Value = usuario.Email;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return Ok(usuario);
                    }
                }
            }
            catch {
                return BadRequest();
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(UsuarioModel usuario, int id) {


                using (SqlConnection con = new(_connection["ConnectionStrings:Conexion"]))
                {
                    using (SqlCommand cmd = new("sp_actualizar", con)) {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value=usuario.IDUsuario;
                        cmd.Parameters.Add("@Nombre", SqlDbType.Text).Value=usuario.Nombre;
                        cmd.Parameters.Add("@Edad", SqlDbType.Int).Value=usuario.Edad;
                        cmd.Parameters.Add("@Correo", SqlDbType.Text).Value=usuario.Email;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return Ok(usuario);
                    }
                }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {

            try
            {
                var usuario = new UsuarioModel();
                using (SqlConnection con = new(_connection["ConnectionStrings:Conexion"])) {
                    using (SqlCommand cmd = new("sp_usuario", con)) {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        con.Open();
                        SqlDataAdapter da = new(cmd);
                        DataTable dt = new();
                        da.Fill(dt);
                        da.Dispose();
                        usuario.IDUsuario = id;
                        usuario.Nombre = Convert.ToString(dt.Rows[0][1]);
                        usuario.Edad = Convert.ToInt32(dt.Rows[0][2]);
                        usuario.Email = Convert.ToString(dt.Rows[0][3]);
                        con.Close();
                    }
                    using (SqlCommand cmd = new("sp_delete", con))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    return Ok(usuario);
                }
            }
            catch(Exception ex){
                return BadRequest(new { message = ex.Message});
            }
        }
    }
}
