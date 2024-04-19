using HalloDocService.Interfaces;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace HalloDocService.Implementation;
public class UtilityService : IUtilityService
{
    private readonly IConfiguration _configuration;

    public UtilityService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void EmailSend(string rcvrMail, string? MessageBody = null, string? Subject = null,string[]? fileAttachments=null,int? role=null,int? req=null,int? phy=null,int? admin=null)
    {
        string? senderEmail = _configuration["EmailSettings:SenderEmail"];
        string? senderPassword = _configuration["EmailSettings:SenderPassword"];
        int PORT = int.Parse(_configuration["EmailSettings:PORT"]);
        string? smtpServer = _configuration["EmailSettings:Server"];
        bool isMailSent = false;
        int retryCount = 0;

        while (!isMailSent && retryCount < 3)
            {
                try
                {
                    SmtpClient client = new(smtpServer)
                    {
                        Port = PORT,
                        Credentials = new NetworkCredential(senderEmail, senderPassword),
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false
                    };

                    MailMessage mailMessage = new()
                    {
                        From = new MailAddress(senderEmail, "HalloDoc"),
                        Subject = Subject ?? "",
                        IsBodyHtml = true,
                        Body = MessageBody ?? ""
                    };

                    if (fileAttachments != null)
                    {
                        foreach (var file in fileAttachments)
                        {
                            if (File.Exists(file))
                            {
                                mailMessage.Attachments.Add(new Attachment(file));
                            }
                            else
                            {
                                Console.WriteLine($"File '{file}' does not exist.");
                            }
                        }
                    }

                    mailMessage.To.Add(rcvrMail);
                    client.Send(mailMessage);

                    // Email sent successfully
                    isMailSent = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    retryCount++;
                    Thread.Sleep(1000); // Wait for 1 second before retrying
                }
            }
            if (isMailSent){
                string confirmationNumber = Guid.NewGuid().ToString();
            }
            else
            {
                throw new Exception("Email sending failed after maximum retries.");
            }
    }
}
