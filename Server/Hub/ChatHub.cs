namespace Server.Hub
{
	using Microsoft.AspNetCore.SignalR;

	public class ChatHub : Hub
	{

		public static int messagesSent = 0;
		public static int connectionsCounter  = 0;
		public static string connectionInterface = "";

		public async Task SendMessage(string name, string message)
		{
			IncrementMessages();
			await Clients.All.SendAsync("ReceiveMessage", name, message);
			SendToInterface();
		}

		public Task SendPrivateMessage(string name, string connectionId, string message)
		{
			IncrementMessages();
			Console.WriteLine($"SendPrivateMessage chamado com: name={name}, connectionId={connectionId}, message={message}");

			if (string.IsNullOrEmpty(connectionId))
			{
				throw new ArgumentException("ConnectionId não pode ser nulo ou vazio.");
			}

			SendToInterface();

			return Clients.Client(connectionId).SendAsync("ReceiveMessage", name, message);
		}

		public async Task SendMessageToGroup(string groupName, string name, string message)
		{
			IncrementMessages();
			await SendToInterface();
			await Clients.Group(groupName).SendAsync("ReceiveMessage", name, message);
		}

		public async Task AddToGroup(string groupName)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
		}

		public async Task RemoveFromGroup(string groupName)
		{
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
		}


		public override Task OnConnectedAsync()
		{
			var httpContext = Context.GetHttpContext();
			var type = httpContext.Request.Query["type"].ToString();

			if (type == "interface")
			{
				connectionInterface = Context.ConnectionId;
				return base.OnConnectedAsync();
			}

			Console.WriteLine(Context.ConnectionId);
			IncrementConnection();
			Console.WriteLine(connectionsCounter);


			SendToInterface();

			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			DecrementConnection();
			SendToInterface();
			return base.OnDisconnectedAsync(exception);
		}

		public async Task SendToInterface()
		{
			await Clients.Client(connectionInterface).SendAsync("Data", messagesSent, connectionsCounter);
		}

		public void IncrementMessages() 
		{
			messagesSent++;
		}

		public void IncrementConnection() 
		{
			connectionsCounter++;
		}

		public void DecrementConnection()
		{
			connectionsCounter--;
		}

	}
}
