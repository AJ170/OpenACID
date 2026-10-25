using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SimulateLootRollResponse")]
	public class SimulateLootRollResponse : IExtensible
	{
		private string _rollInformation = "";

		private InventoryElement[] _elements = new InventoryElement[0];

		private ResourceFromTemplate[] _resources = new ResourceFromTemplate[0];

		private long _currentVersionHash;

		private long _rollCodeHash;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "rollInformation", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string rollInformation
		{
			get
			{
				return _rollInformation;
			}
			set
			{
				_rollInformation = value;
			}
		}

		[ProtoMember(2, Name = "elements", DataFormat = DataFormat.Default)]
		public InventoryElement[] elements
		{
			get
			{
				return _elements;
			}
			set
			{
				Array.Resize(ref _elements, value.Length);
				value.CopyTo(_elements, 0);
			}
		}

		[ProtoMember(3, Name = "resources", DataFormat = DataFormat.Default)]
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

		[ProtoMember(5, IsRequired = false, Name = "currentVersionHash", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long currentVersionHash
		{
			get
			{
				return _currentVersionHash;
			}
			set
			{
				_currentVersionHash = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "rollCodeHash", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long rollCodeHash
		{
			get
			{
				return _rollCodeHash;
			}
			set
			{
				_rollCodeHash = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
