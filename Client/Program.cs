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
			.WithUrl($"http://localhost:5261/Chat?username={client.Name}")
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
				string option2 = "y";
				while (option2 == "y")
				{
					Console.WriteLine("Send message to everyone: ");
					var message = Console.ReadLine();

					await SendMessage(connection, client.Name, message);

					Console.WriteLine("Do you want to send more messages(y/n): ");
					option2 = Console.ReadLine();
				}
			}

			else if (option == "3")
			{
				Console.WriteLine("Which group?");
				string groupName = Console.ReadLine();

				connection.InvokeAsync("AddToGroup", groupName);

				Console.WriteLine($"Send messages to {groupName}: ");
				string message = Console.ReadLine();

				string option2 = "y";
				while (option2 == "y")
				{
					connection.InvokeAsync("SendMessageToGroup", groupName, client.Name, message);

					Console.WriteLine("Do you want to send more messages to this group(y/n): ");
					option2 = Console.ReadLine();

					if (option2 == "n")
					{
						connection.InvokeAsync("RemoveFromGroup", groupName);
					}

				}
			}

			else if (option == "4") 
			{
				finished = true;
			}

			else if(option == "2")
			{
				Console.WriteLine("Which person?");
				string person = Console.ReadLine();

				string option2 = "y";
				while (option2 == "y")
				{

					Console.WriteLine($"Send Message to {person}:");
					var message = Console.ReadLine();

					connection.InvokeAsync("SendPrivateMessage", client.Name, person, message);

					Console.WriteLine("Do you want to send more messages(y/n): ");
					option2 = Console.ReadLine();

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