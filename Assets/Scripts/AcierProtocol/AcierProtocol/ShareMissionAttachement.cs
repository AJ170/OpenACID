using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ShareMissionAttachement")]
	public class ShareMissionAttachement : IExtensible
	{
		private LocationSet _location;

		private string _missionFullName = "";

		private string _senderId = "";

		private int _senderDuration;

		private bool _wasRewardGiven;

		private string _createdMissionId = "";

		private Mission _originalMission;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "location", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LocationSet location
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "missionFullName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string missionFullName
		{
			get
			{
				return _missionFullName;
			}
			set
			{
				_missionFullName = value;
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

		[ProtoMember(4, IsRequired = false, Name = "senderDuration", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int senderDuration
		{
			get
			{
				return _senderDuration;
			}
			set
			{
				_senderDuration = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "wasRewardGiven", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool wasRewardGiven
		{
			get
			{
				return _wasRewardGiven;
			}
			set
			{
				_wasRewardGiven = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "createdMissionId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string createdMissionId
		{
			get
			{
				return _createdMissionId;
			}
			set
			{
				_createdMissionId = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "originalMission", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Mission originalMission
		{
			get
			{
				return _originalMission;
			}
			set
			{
				_originalMission = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
