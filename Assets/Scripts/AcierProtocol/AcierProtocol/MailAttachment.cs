using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MailAttachment")]
	public class MailAttachment : IExtensible
	{
		private MailGift _gift;

		private MailFriendRequest _friendRequest;

		private ShareMissionAttachement _shareMission;

		private MailProgressionEventAttachment _progressionEvent;

		private HirelingUsageStatisticsAttachment _hirelingAttachement;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "gift", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MailGift gift
		{
			get
			{
				return _gift;
			}
			set
			{
				_gift = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "friendRequest", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MailFriendRequest friendRequest
		{
			get
			{
				return _friendRequest;
			}
			set
			{
				_friendRequest = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "shareMission", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ShareMissionAttachement shareMission
		{
			get
			{
				return _shareMission;
			}
			set
			{
				_shareMission = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "progressionEvent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MailProgressionEventAttachment progressionEvent
		{
			get
			{
				return _progressionEvent;
			}
			set
			{
				_progressionEvent = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "hirelingAttachement", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public HirelingUsageStatisticsAttachment hirelingAttachement
		{
			get
			{
				return _hirelingAttachement;
			}
			set
			{
				_hirelingAttachement = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
