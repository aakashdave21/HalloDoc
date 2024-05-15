using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace HalloDocMVC.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatHub(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static Dictionary<int, string> ConnectionStringList = new();

        public async override Task OnConnectedAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("AspUserId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int senderAspNetUserId))
            {
                if (!ConnectionStringList.ContainsKey(senderAspNetUserId))
                {
                    ConnectionStringList.Add(senderAspNetUserId, Context.ConnectionId);
                }
                else
                {
                    ConnectionStringList[senderAspNetUserId] = Context.ConnectionId;
                }
            }
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {

            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("AspUserId");

            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int senderAspNetUserId))
            {
                if (senderAspNetUserId != 0)
                {
                    ConnectionStringList.Remove(senderAspNetUserId);
                }
            }

        }

        public async Task SendMessage(string receiverId, string messageInput)
        {
            Console.WriteLine(receiverId);
            Console.WriteLine(messageInput);
            Console.WriteLine("------------------------------------------------");
            int aspNetUserIdReceiver = int.Parse(receiverId);
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("AspUserId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int senderAspNetUserId))
            {
                if (ConnectionStringList.ContainsKey(aspNetUserIdReceiver))
                {
                    await Clients.Client(ConnectionStringList[aspNetUserIdReceiver]).SendAsync("ReceiveMessage", senderAspNetUserId, receiverId, messageInput);
                }
                await Clients.Client(ConnectionStringList[senderAspNetUserId]).SendAsync("ReceiveMessage", senderAspNetUserId,senderAspNetUserId, messageInput);
            }
        }

    }
}