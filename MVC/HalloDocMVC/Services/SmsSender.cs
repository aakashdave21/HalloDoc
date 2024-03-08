using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace HalloDocMVC.Services;
public class SmsSender {
    public static void SendSMS(){
        try
        {
            var accountSid = "AC334567f054d67859401b21946524d191";
            var authToken = "73173d70191f3a9494ba9d4aa67ac15d";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
            new PhoneNumber("+917600766250"));
            // new PhoneNumber("+917990146412"));
            // new PhoneNumber("+918511331279"));
            messageOptions.From = new PhoneNumber("+19723626467");
            messageOptions.Body = "Hello, This Message From HalloDoc";


            var message = MessageResource.Create(messageOptions);
            Console.WriteLine(message.Body);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
        }
            
    }
}