using HalloDocRepository.DataModels;

namespace HalloDocRepository.Interfaces;
public interface IChatRepo
{
    IEnumerable<Chathistory> GetUserList(int SenderId);
    void CreateChatUser(int senderId, int ReceiverId);
    IEnumerable<string?>? GetUserRoleFromAspId(int AspId);
    IEnumerable<Chat>? LoadPreviousMessage(int Sender,int Receiver);
    Aspnetuser? GetUsersUserName(int AspId);
}