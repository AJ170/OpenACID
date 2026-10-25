using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionAreaContent")]
	public class MissionAreaContent : IExtensible
	{
		private MissionAreaBaseContent _MissionAreaBaseContent;

		private EAreaSize _AreaSize;

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

		[ProtoMember(9, IsRequired = false, Name = "AreaSize", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EAreaSize.Small)]
		public EAreaSize AreaSize
		{
			get
			{
				return _AreaSize;
			}
			set
			{
				_AreaSize = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
