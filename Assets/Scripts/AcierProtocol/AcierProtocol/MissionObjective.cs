using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionObjective")]
	public class MissionObjective : IExtensible
	{
		private bool _primaryObjective;

		private int[] _phaseCompletedMessageGroup = new int[0];

		private long _activationOverride;

		private long _removalOverride;

		private ObjectiveBase _objectiveBase;

		private IExtension extensionObject;

		[ProtoMember(2, IsRequired = false, Name = "primaryObjective", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool primaryObjective
		{
			get
			{
				return _primaryObjective;
			}
			set
			{
				_primaryObjective = value;
			}
		}

		[ProtoMember(3, Name = "phaseCompletedMessageGroup", DataFormat = DataFormat.TwosComplement)]
		public int[] phaseCompletedMessageGroup
		{
			get
			{
				return _phaseCompletedMessageGroup;
			}
			set
			{
				Array.Resize(ref _phaseCompletedMessageGroup, value.Length);
				value.CopyTo(_phaseCompletedMessageGroup, 0);
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "activationOverride", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long activationOverride
		{
			get
			{
				return _activationOverride;
			}
			set
			{
				_activationOverride = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "removalOverride", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long removalOverride
		{
			get
			{
				return _removalOverride;
			}
			set
			{
				_removalOverride = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "objectiveBase", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveBase objectiveBase
		{
			get
			{
				return _objectiveBase;
			}
			set
			{
				_objectiveBase = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
