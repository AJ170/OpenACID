using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "UnitData")]
	public class UnitData : IExtensible
	{
		private string _guid = "";

		private int _cost;

		private int _unlockLevel;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "guid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string guid
		{
			get
			{
				return _guid;
			}
			set
			{
				_guid = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "cost", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int cost
		{
			get
			{
				return _cost;
			}
			set
			{
				_cost = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "unlockLevel", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int unlockLevel
		{
			get
			{
				return _unlockLevel;
			}
			set
			{
				_unlockLevel = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
