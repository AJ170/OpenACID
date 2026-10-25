using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionTargetContent")]
	public class MissionTargetContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private float _Range;

		private ETemplateTargetKind[] _TargetKind = new ETemplateTargetKind[0];

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBaseContent MissionBaseContent
		{
			get
			{
				return _MissionBaseContent;
			}
			set
			{
				_MissionBaseContent = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "Range", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float Range
		{
			get
			{
				return _Range;
			}
			set
			{
				_Range = value;
			}
		}

		[ProtoMember(2, Name = "TargetKind", DataFormat = DataFormat.TwosComplement)]
		public ETemplateTargetKind[] TargetKind
		{
			get
			{
				return _TargetKind;
			}
			set
			{
				Array.Resize(ref _TargetKind, value.Length);
				value.CopyTo(_TargetKind, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
