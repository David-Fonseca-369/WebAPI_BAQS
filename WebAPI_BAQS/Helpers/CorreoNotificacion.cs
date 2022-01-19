using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebAPI_BAQS.Helpers
{
    public class CorreoNotificacion
    {

        public static void SendMail(string subject, string mensaje)
        {
            string time = DateTime.Now.ToString("HH:mm:ss");

            string body = "<html><head><Font Face = \"Calibri \" Color = \"Black\" Size=\"2\"><body><p> " + "Hora: " + time + " " + subject + " Mensaje: " + mensaje
                + "<p>" + Dns.GetHostName() + " <P></Body></html>";

            MailMessage correo = new MailMessage();
            correo.From = new MailAddress("Reporte_BAQS@elkay.com", "Reporte de BAQS");

            correo.To.Add("fonsecasoundcruz@gmail.com");
            correo.To.Add("mariela.jimenez@elkay.com");
            correo.To.Add("marielajimenez296@gmail.com");


            correo.Subject = subject;
            correo.Body = body;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.High;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "10.1.13.154";
            try
            {
                smtp.Send(correo);
            }
            catch (Exception e)
            {
                //SaveFile("Correo", e);
            }
        }
    }
}
