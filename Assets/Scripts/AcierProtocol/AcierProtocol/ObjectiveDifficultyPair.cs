using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveDifficultyPair")]
	public class ObjectiveDifficultyPair : IExtensible
	{
		private EObjectiveType _objectiveBaseType;

		private int _difficulty;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "objectiveBaseType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectiveType.Deliver_EObjectiveType)]
		public EObjectiveType objectiveBaseType
		{
			get
			{
				return _objectiveBaseType;
			}
			set
			{
				_objectiveBaseType = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "difficulty", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int difficulty
		{
			get
			{
				return _difficulty;
			}
			set
			{
				_difficulty = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
