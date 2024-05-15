
namespace HalloDocService.ViewModels;
public class ChatViewModel
{
    public int Sender { get; set; }
    public string? SenderName {get; set;}
    public string? ReceiverName {get; set;}
    public string? ReceiverEmail {get; set;} 
    public string? ReceiverPhone {get; set;} 
    public int? Receiver { get; set; }
    public List<ChatMessage> MessagesList { get; set; } = new();
}


public class ChatMessage {
    public string? Message {get; set;} 
    public DateTime? CreatedDate {get; set;} 
}