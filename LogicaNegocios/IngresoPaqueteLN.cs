using Servicio_Correo_Recepcion_Tracking.AccesoDatos;
using Servicio_Correo_Recepcion_Tracking.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicio_Correo_Recepcion_Tracking.LogicaNegocios
{
    public class IngresoPaqueteLN
    {
        private readonly IngresoPaqueteAD _IngresoPaqueteAD = new IngresoPaqueteAD();

        public int AgregarPaqueteNuevo(RecepcionTracking ElPaquete)
        {
            int Resultado = 0;
            try
            {
                return Resultado = _IngresoPaqueteAD.AgregarPaqueteNuevo(ElPaquete);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
