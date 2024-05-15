
using HalloDocService.ViewModels;

namespace HalloDocService.Interfaces;
public interface IChatService
{
    ChatHistoryViewModel GetUserList(int SenderId);
    void CreateChatUser(int senderId, int ReceiverId);
    ChatViewModel LoadPreviousMessage(int Sender,int Receiver);
    
}