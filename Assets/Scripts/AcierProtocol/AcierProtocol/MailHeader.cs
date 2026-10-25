using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MailHeader")]
	public class MailHeader : IExtensible
	{
		private string _headerId = "";

		private string _senderName = "";

		private string _senderId = "";

		private string _receiverName = "";

		private string _receiverId = "";

		private EMailFolder _folder;

		private EMailType _type;

		private bool _isRead;

		private string _subject = "";

		private long _creationDate;

		private long _expirationDate;

		private bool _hasAttachment;

		private EMailAttachmentType _attachmentType = EMailAttachmentType.MailAttachmentTypeNone;

		private LangKey _subjectLangKey;

		private EModuleOrigin _requestOrigin;

		private bool _fullInventory;

		private EMailPriority _priority;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "headerId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string headerId
		{
			get
			{
				return _headerId;
			}
			set
			{
				_headerId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "senderName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string senderName
		{
			get
			{
				return _senderName;
			}
			set
			{
				_senderName = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "senderId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string senderId
		{
			get
			{
				return _senderId;
			}
			set
			{
				_senderId = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "receiverName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string receiverName
		{
			get
			{
				return _receiverName;
			}
			set
			{
				_receiverName = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "receiverId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string receiverId
		{
			get
			{
				return _receiverId;
			}
			set
			{
				_receiverId = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "folder", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMailFolder.MailFolderNews)]
		public EMailFolder folder
		{
			get
			{
				return _folder;
			}
			set
			{
				_folder = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMailType.MailTypePlayer2Player)]
		public EMailType type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "isRead", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool isRead
		{
			get
			{
				return _isRead;
			}
			set
			{
				_isRead = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "subject", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string subject
		{
			get
			{
				return _subject;
			}
			set
			{
				_subject = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "creationDate", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long creationDate
		{
			get
			{
				return _creationDate;
			}
			set
			{
				_creationDate = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "expirationDate", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long expirationDate
		{
			get
			{
				return _expirationDate;
			}
			set
			{
				_expirationDate = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "hasAttachment", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool hasAttachment
		{
			get
			{
				return _hasAttachment;
			}
			set
			{
				_hasAttachment = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "attachmentType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMailAttachmentType.MailAttachmentTypeNone)]
		public EMailAttachmentType attachmentType
		{
			get
			{
				return _attachmentType;
			}
			set
			{
				_attachmentType = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "subjectLangKey", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LangKey subjectLangKey
		{
			get
			{
				return _subjectLangKey;
			}
			set
			{
				_subjectLangKey = value;
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "requestOrigin", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EModuleOrigin.ModuleOrigin_DailyReward)]
		public EModuleOrigin requestOrigin
		{
			get
			{
				return _requestOrigin;
			}
			set
			{
				_requestOrigin = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "fullInventory", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool fullInventory
		{
			get
			{
				return _fullInventory;
			}
			set
			{
				_fullInventory = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "priority", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMailPriority.MailPriorityLow)]
		public EMailPriority priority
		{
			get
			{
				return _priority;
			}
			set
			{
				_priority = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
