using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionAreaMovingContent")]
	public class MissionAreaMovingContent : IExtensible
	{
		private MissionAreaBaseContent _MissionAreaBaseContent;

		private float _Radius;

		private long _Target;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionAreaBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionAreaBaseContent MissionAreaBaseContent
		{
			get
			{
				return _MissionAreaBaseContent;
			}
			set
			{
				_MissionAreaBaseContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "Radius", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float Radius
		{
			get
			{
				return _Radius;
			}
			set
			{
				_Radius = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "Target", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long Target
		{
			get
			{
				return _Target;
			}
			set
			{
				_Target = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
