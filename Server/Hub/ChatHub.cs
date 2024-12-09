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
			Console.WriteLine($"SendPrivateMessage chamado com: name={name}, connectionId={connectionId}, message={message}");

			if (string.IsNullOrEmpty(connectionId))
			{
				throw new ArgumentException("ConnectionId não pode ser nulo ou vazio.");
			}

			return Clients.Client(connectionId).SendAsync("ReceiveMessage", name, message);
		}

		public override Task OnConnectedAsync()
		{
			Console.WriteLine(Context.ConnectionId);
			return base.OnConnectedAsync();	
		}


	}
}