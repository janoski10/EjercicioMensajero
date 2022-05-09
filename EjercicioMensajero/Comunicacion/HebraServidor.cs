using MensajeroModel.DAL;
using MensajeroModel.DTO;
using ServidorSocketUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EjercicioMensajero.Comunicacion
{
    public class HebraServidor
    {
        private IMensajesDAL mensajesDAL = MensajesDALArchivos.GetInstancia();
        public void Ejecutar()
        {
            int puerto = Convert.ToInt32(ConfigurationManager.AppSettings["puerto"]);
            ServerSocket serverSocket = new ServerSocket(puerto);
            Console.WriteLine("S: Iniciando servidor en puerto {0}", puerto);
            if (serverSocket.Iniciar())
            {
                while (true)
                {
                    Console.WriteLine("S: Esperando Cliente......");
                    Socket cliente = serverSocket.ObtenerCliente();
                    Console.WriteLine("S: Cliente Recibido");
                    ClienteCom clienteCom = new ClienteCom(cliente);
                    clienteCom.Escribir("Ingrese Nombre: ");
                    string nombre = clienteCom.Leer();
                    clienteCom.Escribir("Ingrese Texto: ");
                    string texto = clienteCom.Leer();
                    Mensaje mensaje = new Mensaje()
                    {
                        Nombre = nombre,
                        Texto = texto,
                        Tipo = "TCP"
                    };
                    mensajesDAL.AgregarMensaje(mensaje);
                    clienteCom.Desconectar();
                }
            }
            else
            {
                Console.WriteLine("Fail, no se puede levantar server en {0}", puerto);
            }
        }
    }
}
