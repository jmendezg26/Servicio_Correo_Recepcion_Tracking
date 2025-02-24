using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Servicio_Correo_Recepcion_Tracking.Entidades;

namespace Servicio_Correo_Recepcion_Tracking.AccesoDatos
{
    public class IngresoPaqueteAD
    {
        string connectionString = ConfigurationManager.ConnectionStrings["BDPruebas"].ConnectionString;


        public int AgregarPaqueteNuevo(RecepcionTracking ElPaquete)
        {
            int Resultado = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "PA_InsertarNuevoPaquete";
                    cmd.Parameters.AddWithValue("@NombreCliente", ElPaquete.Cliente);
                    cmd.Parameters.AddWithValue("@Tracking", ElPaquete.NumTracking);
                    cmd.Parameters.AddWithValue("@Estado", ElPaquete.Estado);


                    cmd.Parameters.Add("@ID", SqlDbType.BigInt);
                    cmd.Parameters["@ID"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    Resultado = Convert.ToInt32(cmd.Parameters["@ID"].Value);

                    connection.Close();

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error en la conexión: " + ex.Message);
                }
            }

            return Resultado;
        }
    }
}
