using System.Net;
using System.Text;
using System.Net.Mail;

using HtmlAgilityPack;

namespace Safe_Sign.Util
{
    public class EmailTools
    {
        /// <summary>
        /// Transform CPF in Base64
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static string GenerateCode(string cpf)
        {
            var codeAux = Encoding.UTF8.GetBytes(cpf);
            return Convert.ToBase64String(codeAux);
        }

        /// <summary>
        /// Decode the Base64 in a CPF
        /// </summary>
        /// <param name="stringCode"></param>
        /// <returns></returns>
        public static string DeCode(string stringCode)
        {
            var stringAux = Convert.FromBase64String(stringCode);
            return Encoding.UTF8.GetString(stringAux);
        }


        /// <summary>
        /// Configure the Email Body
        /// </summary>
        /// <param name="cpfEncoded"></param>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="login"></param>
        /// <returns></returns>
        public static MailMessage Configuration(string cpfEncoded, string email, string name, string login)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = true;
            
            string path = Path.GetFullPath("..//Safe-Sign.Util//Email//confirm-email.html");
            string safeSignURL = Environment.GetEnvironmentVariable("SAFE_SIGN_URL");
            string emailAux = Environment.GetEnvironmentVariable("SAFE_SIGN_SYSTEM_EMAIL");
            
            var bodyHtml = new HtmlDocument();      
            bodyHtml.DetectEncodingAndLoad(path);
            
            mail.From = new MailAddress(emailAux);
            mail.To.Add(email); // to
            mail.Subject = "Confirmação de conta do SafeSign"; // Subject
            mail.Body = bodyHtml.DocumentNode.InnerHtml; // message
            mail.Body = mail.Body.Replace("@nome", name); //Put the user name in the body
            mail.Body = mail.Body.Replace("@loginUser", login); //Put login user in the body
            mail.Body = mail.Body.Replace("@cpfEncoded", cpfEncoded); //Put the code to validate in the body
            mail.Body = mail.Body.Replace("@safesignurl", safeSignURL); //Put the safe sign url in the body

            return mail;    
        }

        /// <summary>
        /// Send Verificate Email to User
        /// </summary>
        /// <param name="email"></param>
        /// <param name="codeToValidate"></param>
        /// <param name="name"></param>
        /// <param name="login"></param>
        public static void SendEmail(string email, string codeToValidate, string name, string login)
        {
            MailMessage mail = Configuration(codeToValidate, email, name, login);
            string emailAccount = Environment.GetEnvironmentVariable("SAFE_SIGN_SYSTEM_EMAIL");
            string emailPassword = Environment.GetEnvironmentVariable("SAFE_SIGN_SYSTEM_PASSWORD");

            using (var smtp = new SmtpClient("smtp.gmail.com"))
            {
                smtp.EnableSsl = true; // GMail requer SSL
                smtp.Port = 587;       // port to SSL
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network; // send method
                smtp.UseDefaultCredentials = false;

                // Use User and Password to authenticate
                smtp.Credentials = new NetworkCredential(emailAccount, emailPassword);

                smtp.Send(mail);
            }
        }
    }
}
