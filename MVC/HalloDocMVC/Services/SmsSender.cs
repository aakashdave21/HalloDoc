using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace HalloDocMVC.Services;
public class SmsSender {
    public static void SendSMS(){
        try
        {
            var accountSid = "AC334567f054d67859401b21946524d191";
            var authToken = "eba678aed3844cc23e01fd7394ebc31d";
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
            new PhoneNumber("+917600766250"))
            {
                From = new PhoneNumber("+19723626467"),
                Body = "hello"
            };


            var message = MessageResource.Create(messageOptions);
            Console.WriteLine(message.Body);
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
        }
            
    }
}