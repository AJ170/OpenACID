using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionGoalLoot")]
	public class MissionGoalLoot : IExtensible
	{
		private int _missionGoalId;

		private InventoryElements _elements;

		private ResourceFromTemplate[] _resources = new ResourceFromTemplate[0];

		private string[] _entitlements = new string[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "missionGoalId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int missionGoalId
		{
			get
			{
				return _missionGoalId;
			}
			set
			{
				_missionGoalId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "elements", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public InventoryElements elements
		{
			get
			{
				return _elements;
			}
			set
			{
				_elements = value;
			}
		}

		[ProtoMember(4, Name = "resources", DataFormat = DataFormat.Default)]
		public ResourceFromTemplate[] resources
		{
			get
			{
				return _resources;
			}
			set
			{
				Array.Resize(ref _resources, value.Length);
				value.CopyTo(_resources, 0);
			}
		}

		[ProtoMember(5, Name = "entitlements", DataFormat = DataFormat.Default)]
		public string[] entitlements
		{
			get
			{
				return _entitlements;
			}
			set
			{
				Array.Resize(ref _entitlements, value.Length);
				value.CopyTo(_entitlements, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
