using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionBaseContent")]
	public class MissionBaseContent : IExtensible
	{
		private long _ID;

		private long _ActivateAtId;

		private long _RemoveAtId;

		private EMapIconType _MapIcon;

		private bool _ClampMapIconOnBorder;

		private IExtension extensionObject;

		[ProtoMember(9, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long ID
		{
			get
			{
				return _ID;
			}
			set
			{
				_ID = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "ActivateAtId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long ActivateAtId
		{
			get
			{
				return _ActivateAtId;
			}
			set
			{
				_ActivateAtId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "RemoveAtId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long RemoveAtId
		{
			get
			{
				return _RemoveAtId;
			}
			set
			{
				_RemoveAtId = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "MapIcon", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMapIconType.Invalid_EMapIconType)]
		public EMapIconType MapIcon
		{
			get
			{
				return _MapIcon;
			}
			set
			{
				_MapIcon = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "ClampMapIconOnBorder", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool ClampMapIconOnBorder
		{
			get
			{
				return _ClampMapIconOnBorder;
			}
			set
			{
				_ClampMapIconOnBorder = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
