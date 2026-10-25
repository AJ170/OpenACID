using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionSectorsContent")]
	public class MissionSectorsContent : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "MissionSector")]
		public class MissionSector : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "Vertex")]
			public class Vertex : IExtensible
			{
				private float _PositionX;

				private float _PositionY;

				private IExtension extensionObject;

				[ProtoMember(9, IsRequired = false, Name = "PositionX", DataFormat = DataFormat.FixedSize)]
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

				[ProtoMember(1, IsRequired = false, Name = "PositionY", DataFormat = DataFormat.FixedSize)]
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

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private int _Index;

			private Vertex[] _Vertices = new Vertex[0];

			private IExtension extensionObject;

			[ProtoMember(9, IsRequired = false, Name = "Index", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int Index
			{
				get
				{
					return _Index;
				}
				set
				{
					_Index = value;
				}
			}

			[ProtoMember(1, Name = "Vertices", DataFormat = DataFormat.Default)]
			public Vertex[] Vertices
			{
				get
				{
					return _Vertices;
				}
				set
				{
					Array.Resize(ref _Vertices, value.Length);
					value.CopyTo(_Vertices, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private MissionBaseContent _MissionBaseContent;

		private MissionSector[] _Sectors = new MissionSector[0];

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "MissionBaseContent", DataFormat = DataFormat.Default)]
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

		[ProtoMember(9, Name = "Sectors", DataFormat = DataFormat.Default)]
		public MissionSector[] Sectors
		{
			get
			{
				return _Sectors;
			}
			set
			{
				Array.Resize(ref _Sectors, value.Length);
				value.CopyTo(_Sectors, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
