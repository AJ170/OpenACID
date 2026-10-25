using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionAreaBaseContent")]
	public class MissionAreaBaseContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private bool _DoNotRenderOnMiniMap;

		private EMissionAreaType _Type;

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

		[ProtoMember(9, IsRequired = false, Name = "DoNotRenderOnMiniMap", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool DoNotRenderOnMiniMap
		{
			get
			{
				return _DoNotRenderOnMiniMap;
			}
			set
			{
				_DoNotRenderOnMiniMap = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "Type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMissionAreaType.Restricted)]
		public EMissionAreaType Type
		{
			get
			{
				return _Type;
			}
			set
			{
				_Type = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
