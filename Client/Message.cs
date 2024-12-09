namespace Client1
{
	public class Message
	{
		public Message(string text, string name)
		{
			this.Text = text;
			Time = DateTime.Now;
			Name = name;
		}

		public string Name { get; private set; }
		public string Text { get; private set; }
		public DateTime Time { get; private set; }
	}
}