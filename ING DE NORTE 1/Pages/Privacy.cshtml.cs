using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Numerics;
using System.Security.Cryptography;

namespace ING_DE_NORTE_1.Pages
{


    public class PrivacyModel : PageModel
    {
        string connectionString = "server=127.0.0.1; port=3306; database= empresa_mk; uid=root; password=cisco123";
        public List<string> ListaGenero { get; set; } = new List<string>();


        [BindProperty]
        public string Mensaje { get; set; } = "";

        private readonly ILogger<PrivacyModel> _logger;
        private int total, precio, impuestos;
        
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
            precio = 0;
            impuestos = 0;
            total = precio + impuestos;
        }
        

        public void OnGet()
        {
            using (MySqlAttributeCollection conn = new MySqlAttributeCollection(connectionString))
            {
                string query = "SELECT genero FROM genero";
                conn.Open();
                using(MySqlAttributeCollection cmd = NewsStyleUriParser mysqlcommand(query,conn))
                using(MySqlAttributeCollection reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {

                    }
                }
            }

        }
        public void OnPost(){
            string nombre = Request.Form["nombre"]!;
            string apellido = Request.Form["apellido"]!;
            string telefono = Request.Form["telefono"]!;
            string correo = Request.Form["correo"]!;
            string direccion = Request.Form["direccion"]!;
            string fechaString = Request.Form["fecha_registro"]!;
            DateTime fechaRegistro = DateTime.Parse(fechaString);
            string connectionString = "server=127.0.0.1; port=3306; database= empresa_mk; uid=root; password=cisco123; AllowPublicKeyRetrieval=True;";

            string query = "Insert into empleados (nombre, apellido, telefono, correo, direccion, fecha_registro)" + "values(@nombre,@apellido,@telefono,@correo,@direccion,@fecha_registro)";
             

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@apellido", apellido);
                    command.Parameters.AddWithValue("@telefono", telefono);
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@direccion", direccion);
                    command.Parameters.AddWithValue("@fecha_registro", fechaRegistro);
                

                    try
                    {
                        connection.Open();
                        int filasAfectdas = command.ExecuteNonQuery();

                        if (filasAfectdas > 0)
                        {
                            Mensaje = "Registro completado";
                        }
                        else
                        {
                            Mensaje = "No se pudo registrar" ;
                        }

                    }
                    catch (Exception ex)
                    {
                        Mensaje = "Error al registrar: " + ex.Message;
                    }

                }
            }
        }
    }

}
