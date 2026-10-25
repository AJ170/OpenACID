using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SerializedNode")]
	public class SerializedNode : IExtensible
	{
		private SerializedNode[] _Children = new SerializedNode[0];

		private SerializedNodeComponent[] _Components = new SerializedNodeComponent[0];

		private float _EulerAnglesX;

		private float _EulerAnglesY;

		private float _EulerAnglesZ;

		private string _Name = "";

		private float _PositionX;

		private float _PositionY;

		private float _PositionZ;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "Children", DataFormat = DataFormat.Default)]
		public SerializedNode[] Children
		{
			get
			{
				return _Children;
			}
			set
			{
				Array.Resize(ref _Children, value.Length);
				value.CopyTo(_Children, 0);
			}
		}

		[ProtoMember(2, Name = "Components", DataFormat = DataFormat.Default)]
		public SerializedNodeComponent[] Components
		{
			get
			{
				return _Components;
			}
			set
			{
				Array.Resize(ref _Components, value.Length);
				value.CopyTo(_Components, 0);
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "EulerAnglesX", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float EulerAnglesX
		{
			get
			{
				return _EulerAnglesX;
			}
			set
			{
				_EulerAnglesX = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "EulerAnglesY", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float EulerAnglesY
		{
			get
			{
				return _EulerAnglesY;
			}
			set
			{
				_EulerAnglesY = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "EulerAnglesZ", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float EulerAnglesZ
		{
			get
			{
				return _EulerAnglesZ;
			}
			set
			{
				_EulerAnglesZ = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "Name", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				_Name = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "PositionX", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float PositionX
		{
			get
			{
				return _PositionX;
			}
			set
			{
				_PositionX = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "PositionY", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float PositionY
		{
			get
			{
				return _PositionY;
			}
			set
			{
				_PositionY = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "PositionZ", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float PositionZ
		{
			get
			{
				return _PositionZ;
			}
			set
			{
				_PositionZ = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
