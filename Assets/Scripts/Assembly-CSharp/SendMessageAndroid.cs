using System.Text;
using Assets.Scripts.Native;
using Assets.Scripts.Tracking;
using UnityEngine;

public class SendMessageAndroid : SendMessageBase
{
	public override bool IsWhatsappInstalled()
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.acid.plugins.messages.SendMessage");
		return androidJavaObject.Call<bool>("IsWhatsappInstalled", new object[0]);
	}

	public override void SendEmail(string _email, string _subject, string _body, SendMessageManager.MessageType _type = SendMessageManager.MessageType.Undefined)
	{
		StringBuilder stringBuilder = new StringBuilder("mailto:");
		stringBuilder.Append(_email);
		stringBuilder.Append("?subject=" + _subject);
		stringBuilder.Append("&body=" + _body);
		Application.OpenURL(stringBuilder.ToString().Replace(" ", "%20"));
		if (_type == SendMessageManager.MessageType.FriendInvite)
		{
			TrackingManager.InviteFriendTrackPoint(SendMessageManager.GetMessageMedium(SendMessageManager.MessageMedium.Email));
		}
	}

	public override void SendSMS(string _recipients, string _message, SendMessageManager.MessageType _type = SendMessageManager.MessageType.Undefined)
	{
		Application.OpenURL(string.Format("sms:{0}?body={1}", _recipients, _message));
		if (_type == SendMessageManager.MessageType.FriendInvite)
		{
			TrackingManager.InviteFriendTrackPoint(SendMessageManager.GetMessageMedium(SendMessageManager.MessageMedium.SMS));
		}
	}

	public override void SendWhatsapp(string _message, SendMessageManager.MessageType _type = SendMessageManager.MessageType.Undefined)
	{
		if (IsWhatsappInstalled())
		{
			string text = string.Format("whatsapp://send?text={0}", _message);
			Application.OpenURL(text.Replace(" ", "%20"));
		}
	}

	public override void SendMessageToAll(string _message, SendMessageManager.MessageType _type = SendMessageManager.MessageType.Undefined)
	{
		AndroidJavaObject androidJavaObject = new AndroidJavaObject("com.acid.plugins.messages.SendMessage");
		androidJavaObject.Call("SendText", _message);
	}

	public override void SendMessageThroughClipboard(string _message, SendMessageManager.MessageType _type = SendMessageManager.MessageType.Undefined)
	{
		NativeTools.CopyToClipboard(_message);
	}
}
