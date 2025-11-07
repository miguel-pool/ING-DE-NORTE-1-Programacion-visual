using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace ING_DE_NORTE_1.Pages
{
    
    public class PrivacyModel : PageModel
    {
        [BindProperty]
        public string Mensaje { get; set; } = "";

        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
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
