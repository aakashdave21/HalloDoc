
namespace HalloDocService.ViewModels;
public class ChatHistoryViewModel
{
    public List<ChatUserList> ChatUserLists { get; set; } = new();
}


public class ChatUserList {
    public int Sender { get; set; }
    public int Receiver { get; set; }
    public string? ReceiverName {get; set;} 
    public string? ReceiverEmail {get; set;} 
    public string? ReceiverPhone {get; set;} 
    public IEnumerable<string?>? UserRoles {get; set;} = new List<string>();
}