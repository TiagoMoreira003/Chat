﻿using Client1;
using Microsoft.AspNetCore.SignalR.Client;

public class Program
{
	private readonly HubConnection connection;

	public static async Task Main(string[] agrs)
	{

		Console.WriteLine("Coloque o seu nome!");
		Client client = new Client(Console.ReadLine());

		HubConnection connection = new HubConnectionBuilder()
			.WithUrl($"http://localhost:5261/Chat")
			.Build();

		connection.On<string, string>("ReceiveMessage", (username, message) =>
		{
			Console.WriteLine($"{username}: {message} ");
		});

		await connection.StartAsync();
		Console.WriteLine("Connected to the hub!");
		
		// Enviar para o server para ele perceber para que user enviar as mensagens posteriormente.
		bool finished = false;

		while (finished == false)
		{

			Console.WriteLine("Chose the option:\n" +
				"1 - Send message to everyone\n" +
				"2 - Send message to specific person\n" +
				"3 - Send message to group\n" +
				"4 - Close app ");
			string option = Console.ReadLine();

			if (option == "1")
			{
				while (option == "1")
				{

					var message = Console.ReadLine();

					await SendMessage(connection, client.Name, message);

					Console.WriteLine("Message sent!");

					Console.WriteLine("Do you want to send more messages(1/0): ");
					option = Console.ReadLine();

				}
			}

			else if (option == "3")
			{
				Console.WriteLine("Wich group?");
				string groupName = Console.ReadLine();

				connection.InvokeAsync("AddToGroup", groupName);

				Console.WriteLine("Send messages to the group");
				string message = Console.ReadLine();

				while (option == "3")
				{
					connection.InvokeAsync("SendMessageToGroup", groupName, client.Name, message);

					Console.WriteLine("Do you want to send more messages to this group(3/0): ");
					option = Console.ReadLine();

					if (option == "0")
					{
						connection.InvokeAsync("RemoveFromGroup", groupName);
					}

				}
			}

			else if (option == "4") 
			{
				finished = true;
			}

			else
			{
				Console.WriteLine("Wich person?");
				string connectionId = Console.ReadLine();

				while (option == "2")
				{

					Console.WriteLine("Send Message!");
					var message = Console.ReadLine();

					connection.InvokeAsync("SendPrivateMessage", client.Name, connectionId, message);

					Console.WriteLine("Message sent!");

					Console.WriteLine("Do you want to send more messages(2/0): ");
					option = Console.ReadLine();

				}
			}
		}

	}

	public static async Task SendMessage(HubConnection connection, string name, string text)
	{
		try
		{
			await connection.InvokeAsync("SendMessage", name, text);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error sending message: {ex.Message}");
		}
	}
}