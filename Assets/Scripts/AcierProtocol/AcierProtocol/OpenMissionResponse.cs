using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "OpenMissionResponse")]
	public class OpenMissionResponse : IExtensible
	{
		private MissionToken _missionToken;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "missionToken", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionToken missionToken
		{
			get
			{
				return _missionToken;
			}
			set
			{
				_missionToken = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
