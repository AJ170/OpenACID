using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionSpawnPointContent")]
	public class MissionSpawnPointContent : IExtensible
	{
		private MissionSpawnEntityContent _MissionSpawnEntityContent;

		private int _SpawnCount;

		private int _SpawnDelaySec;

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionSpawnEntityContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionSpawnEntityContent MissionSpawnEntityContent
		{
			get
			{
				return _MissionSpawnEntityContent;
			}
			set
			{
				_MissionSpawnEntityContent = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "SpawnCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int SpawnCount
		{
			get
			{
				return _SpawnCount;
			}
			set
			{
				_SpawnCount = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "SpawnDelaySec", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int SpawnDelaySec
		{
			get
			{
				return _SpawnDelaySec;
			}
			set
			{
				_SpawnDelaySec = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
