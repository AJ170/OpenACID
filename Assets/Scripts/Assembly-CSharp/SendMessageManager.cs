using UnityEngine;

public class SendMessageManager
{
	public enum MessageType
	{
		FriendInvite = 0,
		Undefined = 1
	}

	public enum MessageMedium
	{
		Email = 0,
		SMS = 1,
		Whatsapp = 2,
		Clipboard = 3
	}

	private static SendMessageBase m_sendMessageMgr;

	public static SendMessageBase SendMessage
	{
		get
		{
			if (m_sendMessageMgr == null)
			{
				if (Application.platform == RuntimePlatform.IPhonePlayer)
				{
					m_sendMessageMgr = new SendMessageIOS();
				}
				else if (Application.platform == RuntimePlatform.Android)
				{
					m_sendMessageMgr = new SendMessageAndroid();
				}
				else
				{
					m_sendMessageMgr = new SendMessageBase();
				}
			}
			return m_sendMessageMgr;
		}
	}

	public static string GetMessageMedium(MessageMedium _type)
	{
		switch (_type)
		{
		case MessageMedium.Email:
			return "Email";
		case MessageMedium.SMS:
			return "SMS";
		case MessageMedium.Whatsapp:
			return "Whatsapp";
		case MessageMedium.Clipboard:
			return "Clipboard";
		default:
			return string.Empty;
		}
	}
}
