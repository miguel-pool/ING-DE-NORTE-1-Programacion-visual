using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Numerics;
using System.Security.Cryptography;

namespace ING_DE_NORTE_1.Pages
{
    public class GeneroOpcion
    {
        //Aqui agarra el ID de las opciones
        public int Id { get; set; }
        public string Nombre { get; set; } = "";

    }

    public class PrivacyModel : PageModel
    {
        [BindProperty]
        public string Mensaje { get; set; } = "";

        //Accede a la base de datos y genera la lista
        public List<GeneroOpcion> ListaDeGeneros { get; set; } = new List<GeneroOpcion>();
        private readonly string connectionString = "server=127.0.0.1; port=3306; database=empresa_mk; uid=root; password=cisco123; AllowPublicKeyRetrieval=True;";
        
        private readonly ILogger<PrivacyModel> _logger;
        private int total, precio, impuestos;
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
            precio = 0;
            impuestos = 0;
            total = precio + impuestos;
        }

        //Puente que hace la conexion
        public void OnGet()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    //Abre la base de datos
                    connection.Open();
                    string query = "SELECT id_genero, genero FROM genero";

                    
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListaDeGeneros.Add(new GeneroOpcion
                                {
                                    Id = reader.GetInt32("id_genero"),
                                    
                                    Nombre = reader.GetString("genero")
                                });
                            }
                        }
                    }
                } 
            }
            //Mensaje de error
            catch (Exception ex)
            {
                Mensaje = "Error al cargar generos: " + ex.Message;
            }
        }

        //Codigo para el formulario del P1
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
             
            //Conexion a la base de datos
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
                

                    //Mensajes de exito o fracaso
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
