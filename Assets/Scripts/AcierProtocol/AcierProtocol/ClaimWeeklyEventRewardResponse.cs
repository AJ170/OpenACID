using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ClaimWeeklyEventRewardResponse")]
	public class ClaimWeeklyEventRewardResponse : IExtensible
	{
		private bool _challengeRewardInInventory;

		private bool _mainEventRewardInInventory;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "challengeRewardInInventory", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool challengeRewardInInventory
		{
			get
			{
				return _challengeRewardInInventory;
			}
			set
			{
				_challengeRewardInInventory = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "mainEventRewardInInventory", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool mainEventRewardInInventory
		{
			get
			{
				return _mainEventRewardInInventory;
			}
			set
			{
				_mainEventRewardInInventory = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
