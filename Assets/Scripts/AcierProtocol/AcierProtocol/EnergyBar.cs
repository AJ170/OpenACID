using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EnergyBar")]
	public class EnergyBar : IExtensible
	{
		private uint _currentEnergy;

		private ulong _updatedSinceTimeStamp;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "currentEnergy", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public uint currentEnergy
		{
			get
			{
				return _currentEnergy;
			}
			set
			{
				_currentEnergy = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "updatedSinceTimeStamp", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0f)]
		public ulong updatedSinceTimeStamp
		{
			get
			{
				return _updatedSinceTimeStamp;
			}
			set
			{
				_updatedSinceTimeStamp = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
