using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient("smtp.gmail.com", 587);

			Client.EnableSsl = true;

			Client.Credentials = new NetworkCredential("dondon2799@gmail.com", "tefu ouhy bcsk zrar");

			Client.Send("dondon2799@gmail.com" , email.Recipents , email.Subject , email.Body);
		}
	}
}
