using Microsoft.AspNetCore.SignalR;

namespace Server1.Controllers
{
	public class ChatHub : Hub
	{

		public async Task SendMessage(string name, string message)
		{
			await Clients.All.SendAsync("ReceiveMessage", name, message);
		}

		public Task SendPrivateMessage(string name, string connectionId, string message)
		{
			return Clients.Client(connectionId).SendAsync("ReceiveMessage", name, message);
		}

		public override Task OnConnectedAsync()
		{
			Console.WriteLine(Context.ConnectionId);
			return base.OnConnectedAsync();	
		}
	}
}