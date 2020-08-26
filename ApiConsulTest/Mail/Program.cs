using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Globalization;
using System.Net;
//using System.Net.Mail;
using System.Text;

namespace Mail
{

    class Program
    {
        public static string ConvertToDecimal(string value, int decimalPlaces)
        {
            string result = "-";
            if (double.TryParse(value, out var parseValue))
            {
                if (Math.Abs(parseValue) > 0)
                {
                    result = string.Format(new NumberFormatInfo() { NumberDecimalDigits = decimalPlaces }, "{0:n}",
                        new decimal(parseValue));
                }
                else
                {
                    result = "0";
                }
            }

            return result;
        }

        static void Main(string[] args)
        {
            Console.WriteLine( ("1").PadLeft(5, '0') );
           
            Console.WriteLine(ConvertToDecimal("123" , 4));
            Console.ReadKey();
            return;
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("lapadoljp@gmail.com", "lapadoljp@gmail.com"));
                message.To.Add(new MailboxAddress("uopeydel@gmail.com", "uopeydel@gmail.com"));
                message.Subject = "Test Email from IBank To dev";

                message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = "<b>Test Message</b>"
                };

                using (var cliente = new SmtpClient())
                {
                    cliente.Connect("smtp-relay.gmail.com", 25, false);

                    // Note: only needed if the SMTP server requires authentication
                    cliente.Authenticate("smtp@ibank.co.th", "puamkxgrrfgcllbq");

                    cliente.Send(message);
                    cliente.Disconnect(true);
                }


                ////SMTP : smtp-relay.gmail.com  port 25
                //SmtpClient client = new SmtpClient("smtp-relay.gmail.com", 25)
                //{
                //    Credentials = new NetworkCredential("smtp@ibank.co.th", "puamkxgrrfgcllbq"),
                //    EnableSsl = true,
                //    UseDefaultCredentials = true,
                //};

                //MailMessage mailMessage = new MailMessage();
                //mailMessage.From = new MailAddress("lapadoljp@gmail.com", "lapadolmsn");
                //mailMessage.BodyEncoding = Encoding.UTF8;
                //mailMessage.To.Add("lapadoljp@gmail.com");
                //mailMessage.Body = "<h1>1 test</h1> <br> <span> app span </span>";
                //mailMessage.Subject = "subj";
                //mailMessage.IsBodyHtml = true;
                //client.Send(mailMessage);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
