using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using project1.BLL.Interfaces;
using project1.BLL.Model;

namespace project1.App_Start
{
    public class EmailService 
    {
        static private IUserService uservice;

        public EmailService(IUserService service) {
            uservice = service;
        }

        private const string _alg = "HmacSHA256";
        private const string _salt = "rz8LuOtFBXphj9WQfvFh"; // Generated at https://www.random.org/strings
        private const int _expirationMinutes = 720;

        public Task SendMail(string subject, string body, string dest)
        {
            
            // настройка логина, пароля отправителя
            var from = "translateproject@hotmail.com";
            var pass = "123456trans";

            // адрес и порт smtp-сервера, с которого мы и будем отправлять письмо
            SmtpClient client = new SmtpClient("smtp.live.com", 587);
            
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(from, pass);
            client.EnableSsl = true;

            // создаем письмо: message.Destination - адрес получателя
            var mail = new MailMessage(from, dest);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            return client.SendMailAsync(mail);
         
        }

        public string GetHashedPassword(string password)
        {
            string key = string.Join(":", new string[] { password, _salt });
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                // Hash the key.
                hmac.Key = Encoding.UTF8.GetBytes(_salt);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(key));
                return Convert.ToBase64String(hmac.Hash);
            }
        }

        public  string generateEmailToken(string id,string password,string email, long ticks)
        {
            string hash = string.Join(":", new string[] { id, password, email, ticks.ToString() });
            string hashLeft = "";
            string hashRight = "";

            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(GetHashedPassword(password));
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));

                hashLeft = Convert.ToBase64String(hmac.Hash);
                hashRight = string.Join(":", new string[] { email, ticks.ToString() });
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));  
        }

        public bool IsEmailTokenValid(string token)
        {
            bool result = false;
            try
            {
                // Base64 decode the string, obtaining the token:username:timeStamp.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                // Split the parts.
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 3)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    string username = parts[1];
                    long ticks = long.Parse(parts[2]);
                    DateTime timeStamp = new DateTime(ticks);
                    // Ensure the timestamp is valid.
                    bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _expirationMinutes;
                    if (!expired)
                    {
                        //
                        // Lookup the user's account from the db.
                        //   
                        User user = uservice.GetUserByEmail(username);
                        if (user!=null)
                        {
                            // Hash the message with the key to generate a token.
                           string computedToken = generateEmailToken(user.id.ToString(), user.password, user.email, ticks);
                            // Compare the computed token with the one supplied and ensure they match.
                           result = (token == computedToken);
                        }
                    }
                }
            }
            catch
            {

            }

            return result;
        }
    }
}