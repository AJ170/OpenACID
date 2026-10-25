using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionNpcFormationContent")]
	public class MissionNpcFormationContent : IExtensible
	{
		private MissionFormationBaseContent _MissionFormationBaseContent;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionFormationBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionFormationBaseContent MissionFormationBaseContent
		{
			get
			{
				return _MissionFormationBaseContent;
			}
			set
			{
				_MissionFormationBaseContent = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
