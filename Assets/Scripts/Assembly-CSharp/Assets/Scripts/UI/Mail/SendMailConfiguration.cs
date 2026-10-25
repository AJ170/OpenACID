namespace Assets.Scripts.UI.Mail
{
	public sealed class SendMailConfiguration
	{
		public delegate void HandleMailSent(string _recieverId);

		public string RecieverId { get; set; }

		public string RecieverName { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public HandleMailSent OnMailSent { get; set; }
	}
}
