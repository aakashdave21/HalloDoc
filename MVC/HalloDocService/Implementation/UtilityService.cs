using HalloDocRepository.Interfaces;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;
using HalloDocRepository.DataModels;
using System.Net.Mail;
using System.Net;

namespace HalloDocService.Implementation;
public class UtilityService : IUtilityService
{
    public async Task EmailSend(string callbackUrl,string rcvrMail,string? MessageBody=null,string? Subject=null){
                string senderEmail = "tatva.dotnet.aakashdave@outlook.com";
                string senderPassword = "Aakash21##";

                SmtpClient client = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(senderEmail, senderPassword),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, "HalloDoc"),
                    Subject = Subject ?? "Set up your Account",
                    IsBodyHtml = true,
                    Body = MessageBody ?? $"Please click the following link to reset your password: <a href='{callbackUrl}'>{callbackUrl}</a>"
                };

                mailMessage.To.Add(rcvrMail);

                client.Send(mailMessage);
    }
}
