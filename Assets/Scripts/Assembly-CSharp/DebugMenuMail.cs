using System;
using AcierProtocol;
using Assets.Scripts.Network;
using Assets.Scripts.UI.Mail;
using Assets.Scripts.UI.State;
using UbiCore.Debug.UI;
using UnityEngine;

public class DebugMenuMail : DebugMenuItem
{
	private enum ESystemType
	{
		None = 0,
		Resource = 1,
		Item = 2
	}

	private static readonly int[] s_CheatTypes = new int[4] { 19, 18, 17, 60 };

	private int m_typeIndex;

	private string m_subject;

	private string m_body;

	private int m_selectedFriendIndex;

	private FriendProfile[] m_friends;

	private bool m_loaded;

	private ESystemType m_systemType;

	private string[] m_systemNames;

	private string[] m_friendNames;

	private string[] m_mailNames;

	private string m_phoneNumber = string.Empty;

	private string m_smsMsg = string.Empty;

	private string m_whatsappMsg = string.Empty;

	private string m_email = string.Empty;

	private string m_subjectMsg = string.Empty;

	private string m_bodyMsg = string.Empty;

	private string m_text = string.Empty;

	private CheatRequest.CheatType MailType
	{
		get
		{
			return (CheatRequest.CheatType)s_CheatTypes[m_typeIndex];
		}
	}

	private bool IsDataStoreInitialized
	{
		get
		{
			if (Main.Instance != null && Main.Instance.IsNetworkEnabled)
			{
				return Singleton<StateManager>.Instance.ProfileManager != null;
			}
			return false;
		}
	}

	private void Awake()
	{
	}

	public override string GetName()
	{
		return "Mail";
	}

	public override void OnApply()
	{
	}

	public override void OnOpen()
	{
		m_loaded = false;
		if (IsDataStoreInitialized)
		{
			Singleton<StateManager>.Instance.ProfileManager.GetMyFriends(delegate
			{
				Reset();
				m_loaded = true;
			});
		}
	}

	public override void OnMenu()
	{
		if (m_loaded)
		{
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			DrawMail();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.BeginVertical();
			GUILayout.BeginHorizontal();
			DrawSendEmailOrSMS();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
		}
	}

	private void DrawMail()
	{
		GUILayout.BeginVertical();
		DrawMailType();
		DrawSystemType();
		DrawSender();
		DrawHirelingOptions();
		DrawSubject();
		DrawBody();
		DrawButtons();
		GUILayout.Space(10f);
		DrawCache();
		GUILayout.EndVertical();
	}

	private void DrawSystemType()
	{
		if (MailType == CheatRequest.CheatType.SEND_SYSTEM_MAILS)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Attachment Type: ");
			m_systemType = (ESystemType)GUILayout.SelectionGrid((int)m_systemType, m_systemNames, 3, "toggle");
			GUILayout.EndHorizontal();
		}
	}

	private void DrawMailType()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Mail Type: ");
		m_typeIndex = GUILayout.SelectionGrid(m_typeIndex, m_mailNames, 3, "toggle");
		GUILayout.EndHorizontal();
	}

	private void DrawButtons()
	{
		GUILayout.BeginHorizontal();
		bool flag = GUI.enabled;
		GUI.enabled = MailType != CheatRequest.CheatType.SEND_FRIEND_MAILS || (m_friends != null && m_friends.Length > 0);
		if (GUILayout.Button("Send mail"))
		{
			SendMail(MailType);
		}
		GUI.enabled = flag;
		GUILayout.EndHorizontal();
	}

	private void DrawBody()
	{
		if (MailType == CheatRequest.CheatType.SEND_FRIEND_MAILS)
		{
			GUILayout.BeginHorizontal();
			m_body = GUILayout.TextArea(m_body, GUILayout.MinHeight(400f));
			GUILayout.EndHorizontal();
		}
	}

	private void DrawSubject()
	{
		if (MailType == CheatRequest.CheatType.SEND_FRIEND_MAILS)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Subject: ");
			m_subject = GUILayout.TextField(m_subject);
			GUILayout.EndHorizontal();
		}
	}

	private void DrawSender()
	{
		if (MailType == CheatRequest.CheatType.SEND_FRIEND_MAILS)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Sender: ");
			m_selectedFriendIndex = GUILayout.SelectionGrid(m_selectedFriendIndex, m_friendNames, 4, "toggle");
			GUILayout.EndHorizontal();
		}
	}

	private void DrawHirelingOptions()
	{
		if (MailType == CheatRequest.CheatType.SEND_HIRELING_MAILS)
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Flush Mails"))
			{
				Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(CheatRequest.CheatType.SEND_HIRELING_MAILS, string.Empty, new CheatParameters
				{
					Param4 = "true"
				});
			}
			GUILayout.EndHorizontal();
		}
	}

	private void DrawCache()
	{
		GUILayout.BeginVertical("box");
		GUILayout.Label("Mail Cache:");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Usage:");
		GUILayout.Label(string.Format("Headers: {0}", MailCache.NumberOfHeaders));
		GUILayout.Label(string.Format("Bodies: {0}", MailCache.NumberOfBodies));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Clear Directory"))
		{
			MailCache.Clear();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private void SendMail(CheatRequest.CheatType _type)
	{
		switch (_type)
		{
		case CheatRequest.CheatType.SEND_SYSTEM_MAILS:
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_type, string.Empty, new CheatParameters
			{
				Param1 = 1,
				Param2 = (int)m_systemType
			});
			break;
		case CheatRequest.CheatType.SEND_MISSION_MAILS:
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_type, string.Empty, new CheatParameters
			{
				Param1 = 1
			});
			break;
		case CheatRequest.CheatType.SEND_FRIEND_MAILS:
		{
			FriendProfile friendProfile = m_friends[m_selectedFriendIndex];
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_type, string.Empty, new CheatParameters
			{
				Param1 = 1,
				Param3 = friendProfile.id,
				Param4 = m_body,
				Param5 = m_subject
			});
			break;
		}
		case CheatRequest.CheatType.SEND_HIRELING_MAILS:
			Singleton<StateManager>.Instance.ProfileManager.UserProfileCheat(_type, string.Empty, new CheatParameters
			{
				Param1 = 1,
				Param2 = 1,
				Param3 = null
			});
			break;
		default:
			throw new ArgumentOutOfRangeException("_type");
		}
	}

	private void Reset()
	{
		if (Singleton<StateManager>.Instance.ProfileManager.Profile.friends != null && Singleton<StateManager>.Instance.ProfileManager.Profile.friends.friendProfiles != null)
		{
			m_friends = Singleton<StateManager>.Instance.ProfileManager.Profile.friends.friendProfiles.Where((FriendProfile _x) => _x.friendType == EFriendType.FriendTypeBidirectional);
		}
		else
		{
			m_friends = new FriendProfile[0];
		}
		m_subject = string.Empty;
		m_body = string.Empty;
		m_typeIndex = 0;
		m_systemNames = Enum.GetNames(typeof(ESystemType));
		m_mailNames = new string[s_CheatTypes.Length];
		for (int i = 0; i < s_CheatTypes.Length; i++)
		{
			m_mailNames[i] = ((CheatRequest.CheatType)s_CheatTypes[i]).ToString();
		}
		m_friendNames = new string[m_friends.Length];
		for (int j = 0; j < m_friends.Length; j++)
		{
			FriendProfile friendProfile = m_friends[j];
			m_friendNames[j] = string.Format("{0}({1})", friendProfile.userName, friendProfile.userIdHash);
		}
	}

	private void DrawSendEmailOrSMS()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.Label("Send SMS");
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("phone number:");
		m_phoneNumber = GUILayout.TextField(m_phoneNumber);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("message: ");
		m_smsMsg = GUILayout.TextField(m_smsMsg);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Send SMS"))
		{
			SendSMS(m_phoneNumber, m_smsMsg);
		}
		GUILayout.EndHorizontal();
		GUILayout.Label("Send Whatsapp Message");
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("message: ");
		m_whatsappMsg = GUILayout.TextField(m_whatsappMsg);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Send Whatsapp message"))
		{
			SendWhatsapp(m_whatsappMsg);
		}
		GUILayout.EndHorizontal();
		GUILayout.Label("Send E-mail");
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("email:");
		m_email = GUILayout.TextField(m_email);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("Subject: ");
		m_subjectMsg = GUILayout.TextField(m_subjectMsg);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("Message: ");
		m_bodyMsg = GUILayout.TextField(m_bodyMsg);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Send E-mail"))
		{
			SendEmail(m_email, m_subjectMsg, m_bodyMsg);
		}
		GUILayout.EndHorizontal();
		GUILayout.Label("Send Text Message To All");
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("message: ");
		m_text = GUILayout.TextField(m_text);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Send message to multiple targets"))
		{
			SendMessageToAll(m_text);
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private void SendSMS(string _phoneNumber, string _message)
	{
		SendMessageManager.SendMessage.SendSMS(_phoneNumber, _message);
	}

	private void SendWhatsapp(string _message)
	{
		SendMessageManager.SendMessage.SendWhatsapp(_message);
	}

	private void SendEmail(string _email, string _subject, string _body)
	{
		SendMessageManager.SendMessage.SendEmail(_email, _subject, _body);
	}

	private void SendMessageToAll(string _message)
	{
		SendMessageManager.SendMessage.SendMessageToAll(_message);
	}
}
