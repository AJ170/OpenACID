using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionDesignTimeContent")]
	public class MissionDesignTimeContent : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "NodeLayout")]
		public class NodeLayout : IExtensible
		{
			private long _ID;

			private Rectangle _Rectangle;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long ID
			{
				get
				{
					return _ID;
				}
				set
				{
					_ID = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "Rectangle", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Rectangle Rectangle
			{
				get
				{
					return _Rectangle;
				}
				set
				{
					_Rectangle = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "Rectangle")]
		public class Rectangle : IExtensible
		{
			private float _XMin;

			private float _YMin;

			private float _Width;

			private float _Height;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "XMin", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float XMin
			{
				get
				{
					return _XMin;
				}
				set
				{
					_XMin = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "YMin", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float YMin
			{
				get
				{
					return _YMin;
				}
				set
				{
					_YMin = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "Width", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float Width
			{
				get
				{
					return _Width;
				}
				set
				{
					_Width = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "Height", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float Height
			{
				get
				{
					return _Height;
				}
				set
				{
					_Height = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private MissionBaseContent _MissionBaseContent;

		private NodeLayout[] _GraphLayout = new NodeLayout[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBaseContent MissionBaseContent
		{
			get
			{
				return _MissionBaseContent;
			}
			set
			{
				_MissionBaseContent = value;
			}
		}

		[ProtoMember(2, Name = "GraphLayout", DataFormat = DataFormat.Default)]
		public NodeLayout[] GraphLayout
		{
			get
			{
				return _GraphLayout;
			}
			set
			{
				Array.Resize(ref _GraphLayout, value.Length);
				value.CopyTo(_GraphLayout, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
