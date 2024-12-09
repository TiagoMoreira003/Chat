namespace Client1
{
	public class Client
	{

		public string Name { get; private set; }

		public Client() { }

		public Client(string name) 
		{
			Name = name;
		}
	}
}