using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "TemplateSpawnPointContent")]
	public class TemplateSpawnPointContent : IExtensible
	{
		private MissionSpawnEntityContent _MissionSpawnEntityContent;

		private ESpawnPointKind[] _SpawnPointKind = new ESpawnPointKind[0];

		private int _MaxEntityCount;

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

		[ProtoMember(9, Name = "SpawnPointKind", DataFormat = DataFormat.TwosComplement)]
		public ESpawnPointKind[] SpawnPointKind
		{
			get
			{
				return _SpawnPointKind;
			}
			set
			{
				Array.Resize(ref _SpawnPointKind, value.Length);
				value.CopyTo(_SpawnPointKind, 0);
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "MaxEntityCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int MaxEntityCount
		{
			get
			{
				return _MaxEntityCount;
			}
			set
			{
				_MaxEntityCount = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
