using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "HandleDailyRewardRequest")]
	public class HandleDailyRewardRequest : IExtensible
	{
		private bool _claim;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "claim", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool claim
		{
			get
			{
				return _claim;
			}
			set
			{
				_claim = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
