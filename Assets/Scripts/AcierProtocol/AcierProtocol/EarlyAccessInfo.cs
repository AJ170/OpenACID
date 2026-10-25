using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EarlyAccessInfo")]
	public class EarlyAccessInfo : IExtensible
	{
		private long _comingSoonStartDate;

		private long _comingSoonEndDate;

		private Resources _earlyAccessCosts;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "comingSoonStartDate", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long comingSoonStartDate
		{
			get
			{
				return _comingSoonStartDate;
			}
			set
			{
				_comingSoonStartDate = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "comingSoonEndDate", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long comingSoonEndDate
		{
			get
			{
				return _comingSoonEndDate;
			}
			set
			{
				_comingSoonEndDate = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "earlyAccessCosts", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources earlyAccessCosts
		{
			get
			{
				return _earlyAccessCosts;
			}
			set
			{
				_earlyAccessCosts = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
