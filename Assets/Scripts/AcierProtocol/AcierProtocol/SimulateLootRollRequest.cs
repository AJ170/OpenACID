using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SimulateLootRollRequest")]
	public class SimulateLootRollRequest : IExtensible
	{
		private string _rollCode = "";

		private bool _giveLoot;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "rollCode", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string rollCode
		{
			get
			{
				return _rollCode;
			}
			set
			{
				_rollCode = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "giveLoot", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool giveLoot
		{
			get
			{
				return _giveLoot;
			}
			set
			{
				_giveLoot = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
