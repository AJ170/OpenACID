public class SendMessageBase
{
	public virtual bool IsWhatsappInstalled()
	{
		return false;
	}

	public virtual void SendEmail(string _email, string _subject, string _body, SendMessageManager.MessageType _type = SendMessageManager.MessageType.Undefined)
	{
	}

	public virtual void SendSMS(string _recipients, string _message, SendMessageManager.MessageType _type = SendMessageManager.MessageType.Undefined)
	{
	}

	public virtual void SendWhatsapp(string _message, SendMessageManager.MessageType _type = SendMessageManager.MessageType.Undefined)
	{
	}

	public virtual void SendMessageToAll(string _message, SendMessageManager.MessageType _type = SendMessageManager.MessageType.Undefined)
	{
	}

	public virtual void SendMessageThroughClipboard(string _message, SendMessageManager.MessageType _type = SendMessageManager.MessageType.Undefined)
	{
	}
}
