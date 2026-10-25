using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ModuleData")]
	public class ModuleData : IExtensible
	{
		private EModuleType _moduleType;

		private EDurationType _durationType = EDurationType.Time;

		private uint _duration;

		private long _expireTimeStamp;

		private Modification[] _modifications = new Modification[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "moduleType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EModuleType.DamageAlgorithm)]
		public EModuleType moduleType
		{
			get
			{
				return _moduleType;
			}
			set
			{
				_moduleType = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "durationType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EDurationType.Time)]
		public EDurationType durationType
		{
			get
			{
				return _durationType;
			}
			set
			{
				_durationType = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "duration", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public uint duration
		{
			get
			{
				return _duration;
			}
			set
			{
				_duration = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "expireTimeStamp", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long expireTimeStamp
		{
			get
			{
				return _expireTimeStamp;
			}
			set
			{
				_expireTimeStamp = value;
			}
		}

		[ProtoMember(5, Name = "modifications", DataFormat = DataFormat.Default)]
		public Modification[] modifications
		{
			get
			{
				return _modifications;
			}
			set
			{
				Array.Resize(ref _modifications, value.Length);
				value.CopyTo(_modifications, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
