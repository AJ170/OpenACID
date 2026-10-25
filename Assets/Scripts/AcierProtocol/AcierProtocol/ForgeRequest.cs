using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ForgeRequest")]
	public class ForgeRequest : IExtensible
	{
		private string[] _elementIds = new string[0];

		private Resource _expectedCost;

		private int _expectedItemLevel;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "elementIds", DataFormat = DataFormat.Default)]
		public string[] elementIds
		{
			get
			{
				return _elementIds;
			}
			set
			{
				Array.Resize(ref _elementIds, value.Length);
				value.CopyTo(_elementIds, 0);
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "expectedCost", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resource expectedCost
		{
			get
			{
				return _expectedCost;
			}
			set
			{
				_expectedCost = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "expectedItemLevel", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int expectedItemLevel
		{
			get
			{
				return _expectedItemLevel;
			}
			set
			{
				_expectedItemLevel = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
