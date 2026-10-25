using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ModifierObjective")]
	public class ModifierObjective : IExtensible
	{
		private RankSetting[] _supportedRanks = new RankSetting[0];

		private ObjectiveBase _objectiveBase;

		private IExtension extensionObject;

		[ProtoMember(21, Name = "supportedRanks", DataFormat = DataFormat.Default)]
		public RankSetting[] supportedRanks
		{
			get
			{
				return _supportedRanks;
			}
			set
			{
				Array.Resize(ref _supportedRanks, value.Length);
				value.CopyTo(_supportedRanks, 0);
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "objectiveBase", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveBase objectiveBase
		{
			get
			{
				return _objectiveBase;
			}
			set
			{
				_objectiveBase = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
