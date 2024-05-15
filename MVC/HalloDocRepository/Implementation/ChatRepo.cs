using HalloDocRepository.DataModels;
using HalloDocRepository.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace HalloDocRepository.Implementation;
public class ChatRepo : IChatRepo
{

    private readonly HalloDocContext _dbContext;

    public ChatRepo(HalloDocContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<Chathistory> GetUserList(int SenderId){
        if(SenderId != 0){
            return _dbContext.Chathistories.Include(user=> user.SenderNavigation).Include(user => user.ReceiverNavigation).Where(chatUser => chatUser.Sender == SenderId);
        }
        throw new Exception();
    }

    public void CreateChatUser(int senderId, int ReceiverId){
        Chathistory? chathistory = _dbContext.Chathistories.FirstOrDefault(user => user.Sender == senderId && user.Receiver == ReceiverId);
        if(chathistory == null){
            Chathistory newConn1 = new(){
                Sender = senderId,
                Receiver = ReceiverId
            };
            Chathistory newConn2 = new(){
                Sender = ReceiverId,
                Receiver = senderId
            };
            _dbContext.Chathistories.Add(newConn1);
            _dbContext.Chathistories.Add(newConn2);
            _dbContext.SaveChanges();
        }
    }

    public IEnumerable<string?>? GetUserRoleFromAspId(int AspId){
        return _dbContext.Aspnetuserroles
        .Where(userRole => userRole.Userid == AspId)
        .Join(_dbContext.Aspnetroles,
              userRole => userRole.Roleid,
              role => role.Id,
              (userRole, role) => role.Name);
    }

    public IEnumerable<Chat>? LoadPreviousMessage(int Sender,int Receiver){
        return _dbContext.Chats.Include(chat => chat.Senderaspnetuser).Include(chat => chat.Receiveraspnetuser)
                .Where(chat => (chat.Senderaspnetuserid == Sender && chat.Receiveraspnetuserid == Receiver) || (chat.Senderaspnetuserid == Receiver && chat.Receiveraspnetuserid == Sender));
    }

    public Aspnetuser? GetUsersUserName(int AspId){
        return _dbContext.Aspnetusers.FirstOrDefault(user => user.Id == AspId);
    }
}