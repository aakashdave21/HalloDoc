using HalloDocRepository.DataModels;
using HalloDocRepository.Interfaces;
using HalloDocService.Interfaces;
using HalloDocService.ViewModels;

namespace HalloDocService.Implementation;
public class ChatService : IChatService
{
    public readonly IChatRepo _chatRepo;
    public ChatService(IChatRepo chatRepo)
    {
        _chatRepo = chatRepo;
    }


    public ChatHistoryViewModel GetUserList(int SenderId)
    {
        IEnumerable<Chathistory> UserList = _chatRepo.GetUserList(SenderId);
        ChatHistoryViewModel chatHistoryView = new();
        if (UserList.Any())
        {
            chatHistoryView.ChatUserLists = UserList.Select(x => new ChatUserList()
            {
                Sender = x.Sender,
                Receiver = x.Receiver,
                ReceiverName = x.ReceiverNavigation.Username,
                ReceiverEmail = x.ReceiverNavigation.Email,
                ReceiverPhone = x.ReceiverNavigation.Phonenumber,
                UserRoles = _chatRepo.GetUserRoleFromAspId(x.Receiver)
            }).ToList();
        }

        return chatHistoryView;
    }

    public void CreateChatUser(int senderId, int ReceiverId){
        _chatRepo.CreateChatUser(senderId,ReceiverId);
    }

    public ChatViewModel LoadPreviousMessage(int Sender,int Receiver){
        IEnumerable<Chat>? chatList = _chatRepo.LoadPreviousMessage(Sender,Receiver);
        Aspnetuser? SenderData = _chatRepo.GetUsersUserName(Sender);
        Aspnetuser? ReceiverData = _chatRepo.GetUsersUserName(Receiver);
        ChatViewModel chatViewModel = new(){
            Sender = Sender,
            Receiver = Receiver,
            SenderName = SenderData?.Username ?? "",
            ReceiverName = ReceiverData?.Username ?? "",
            ReceiverEmail = ReceiverData?.Email ?? "",
            ReceiverPhone = ReceiverData?.Phonenumber ?? ""
        };
        if(chatList != null){
            chatViewModel.MessagesList = chatList.Select(ch => new ChatMessage(){
                Message = ch.Textcontent,
                CreatedDate = ch.Createddate
            }).ToList();
        }
        return chatViewModel;
    }
}