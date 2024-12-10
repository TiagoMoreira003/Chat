using Microsoft.AspNetCore.SignalR;

namespace Server
{
	public class MyCustomProvider : IUserIdProvider
	{
		public string GetUserId(HubConnectionContext connection)
		{
			var httpContext = connection.GetHttpContext();
			var username = httpContext.Request.Query["username"].ToString();

			return username;
		}
	}
}
