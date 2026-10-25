using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AchievementRank")]
	public class AchievementRank : IExtensible
	{
		private int _idx;

		private EAchievementState _state;

		private long _unlockDate;

		private bool _isRead;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "idx", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int idx
		{
			get
			{
				return _idx;
			}
			set
			{
				_idx = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "state", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EAchievementState.AchievementNotReached)]
		public EAchievementState state
		{
			get
			{
				return _state;
			}
			set
			{
				_state = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "unlockDate", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long unlockDate
		{
			get
			{
				return _unlockDate;
			}
			set
			{
				_unlockDate = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "isRead", DataFormat = DataFormat.Default)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
