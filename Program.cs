using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.Net;
using System.Net.Mail;

namespace emailSenderApp
{
    class Program
    {
        public static IConfiguration Configuration { get; set; }
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json");

            Configuration = builder.Build();

            var smtpServer = Configuration["smtpServer"];
            var smtpUser = Configuration["smtpUser"];
            var smtpPassword = Configuration["smtpPassword"];
            var sendTo = Configuration["sendTo"];
            var sendFrom = Configuration["sendFrom"];
            var subject = Configuration["subject"];

            var html = File.ReadAllText("your-html.htm");

            var client = new SmtpClient(smtpServer);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(smtpUser, smtpPassword);

            var email = new MailMessage();
            email.IsBodyHtml = true;
            email.From = new MailAddress(sendFrom);
            email.To.Add(sendTo);
            email.Body = html;
            email.Subject = subject;
            try
            {
                client.Send(email);
                Console.WriteLine("Email sent.");
            }
            catch (SmtpFailedRecipientException ex)
            {
                Console.WriteLine($"Error: { ex.GetBaseException() }");
            }
            catch (Exception ex) {
                Console.WriteLine($"Error: { ex.GetBaseException() }");
            }
        }
    }
}
