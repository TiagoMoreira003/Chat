using Microsoft.AspNetCore.SignalR.Client;

public class Program
{
	private readonly HubConnection connection;

	public static async Task Main(string[] agrs)
	{

		HubConnection connection = new HubConnectionBuilder()
			.WithUrl($"http://localhost:5261/Chat?type=interface")
			.Build();

		connection.On<int, int>("Data", (messagesSent, connectionCounter) =>
		{
			Console.WriteLine($"Connections Counter: {messagesSent}\n" +
				$"Messages Sent: {messagesSent}");
		});

		await connection.StartAsync();
		Console.WriteLine("Connected to the hub!");

		bool finished = false;

		while(finished == false) 
		{
			Console.WriteLine("Do you wanna close the app?(y/n)");
			string option = Console.ReadLine();

			if (option == "y") 
			{
				finished = true;
			}
		}
	}
}