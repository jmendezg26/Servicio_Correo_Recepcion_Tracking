using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using Servicio_Correo_Recepcion_Tracking.Entidades;
using Servicio_Correo_Recepcion_Tracking.LogicaNegocios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace Servicio_Correo_Recepcion_Tracking
{
    public partial class Service1 : ServiceBase
    {
        private readonly IngresoPaqueteLN _IngresoPaqueteLN = new IngresoPaqueteLN();

        private string imapServer = "imap.gmail.com";
        private int imapPort = 993;
        private string email = "jmendezg26@gmail.com";
        private string password = "nqdi taep szht uvwz";

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1.Start();
        }

        protected override void OnStop()
        {
            timer1.Stop();
        }

        private void CheckEmails()
        {
            int Resultado = 0;

            using (var client = new ImapClient())
            {
                try
                {
                    client.Connect(imapServer, imapPort, true);
                    client.Authenticate(email, password);
                    var inbox = client.Inbox;
                    inbox.Open(MailKit.FolderAccess.ReadWrite);
                    var uids = inbox.Search(SearchQuery.NotSeen);

                    foreach (var uid in uids)
                    {
                        var message = inbox.GetMessage(uid);
                        if (message.Subject.Contains("TRACKING"))
                        {
                            RecepcionTracking ElPaquete = ExtractData(message.Subject);
                            Resultado = SaveToDatabase(ElPaquete);
                        }
                        inbox.AddFlags(uid, MailKit.MessageFlags.Seen, true);
                    }
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        private RecepcionTracking ExtractData(string body)
        {
            //var match = Regex.Match(body, "DATO: (.*?)\n");
            //return match.Success ? match.Groups[1].Value : string.Empty;

            RecepcionTracking ElPaquete = new RecepcionTracking();
            ElPaquete.NumTracking = Convert.ToString(Regex.Match(body, "TRACKING: (.*?)\n"));
            ElPaquete.Cliente = Convert.ToString(Regex.Match(body, "CLIENTE: (.*?)\n"));
            ElPaquete.Estado = Convert.ToString(Regex.Match(body, "ESTADO: (.*?)\n"));

            return ElPaquete;
        }

        private int SaveToDatabase(RecepcionTracking ElPaquete)
        {
            int Resultado = 0;

            try
            {
                Resultado = _IngresoPaqueteLN.AgregarPaqueteNuevo(ElPaquete);

                return Resultado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckEmails();
        }
    }
}
