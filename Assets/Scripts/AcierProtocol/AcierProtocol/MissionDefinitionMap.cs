using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionDefinitionMap")]
	public class MissionDefinitionMap : IExtensible
	{
		private MissionDefinition[] _mapping = new MissionDefinition[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "mapping", DataFormat = DataFormat.Default)]
		public MissionDefinition[] mapping
		{
			get
			{
				return _mapping;
			}
			set
			{
				Array.Resize(ref _mapping, value.Length);
				value.CopyTo(_mapping, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
