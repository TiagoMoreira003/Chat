namespace Server.Hub
{
	using Microsoft.AspNetCore.SignalR;

	public class ChatHub : Hub
	{

		public static int messagesSent = 0;
		public static int connectionsCounter  = 0;
		public static List<string> usernames = new List<string>();

		public async Task SendMessage(string name, string message)
		{
			IncrementMessages();
			await Clients.All.SendAsync("ReceiveMessage", name, message);
			SendToInterface();
		}

		public Task SendPrivateMessage(string name, string person, string message)
		{
			var userId = usernames.FirstOrDefault(username => username == person);

			IncrementMessages();
			SendToInterface();

			return Clients.User(userId).SendAsync("ReceiveMessage", name, message);
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
			var username = Context.UserIdentifier;
			usernames.Add(username);

			Console.WriteLine(username);

			if (username == "interface")
			{
				return base.OnConnectedAsync();
			}

			IncrementConnection();
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
			var interfaceId = usernames.FirstOrDefault(username => username == "interface");

			await Clients.User(interfaceId).SendAsync("Data", messagesSent, connectionsCounter);
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
