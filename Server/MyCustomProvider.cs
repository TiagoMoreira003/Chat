using Microsoft.AspNetCore.SignalR;

namespace Server
{
	public class MyCustomProvider : IUserIdProvider
	{
		public string GetUserId(HubConnectionContext connection)
		{
			return Guid.NewGuid().ToString();
		}
	}
}
