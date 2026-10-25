using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionHotspotsContent")]
	public class MissionHotspotsContent : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Hotspot")]
		public class Hotspot : IExtensible
		{
			private int _UniqueId;

			private bool _Active;

			private float _PositionX;

			private float _PositionY;

			private float _PositionZ;

			private IExtension extensionObject;

			[ProtoMember(9, IsRequired = false, Name = "UniqueId", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int UniqueId
			{
				get
				{
					return _UniqueId;
				}
				set
				{
					_UniqueId = value;
				}
			}

			[ProtoMember(1, IsRequired = false, Name = "Active", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool Active
			{
				get
				{
					return _Active;
				}
				set
				{
					_Active = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "PositionX", DataFormat = DataFormat.FixedSize)]
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

			[ProtoMember(3, IsRequired = false, Name = "PositionY", DataFormat = DataFormat.FixedSize)]
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

			[ProtoMember(4, IsRequired = false, Name = "PositionZ", DataFormat = DataFormat.FixedSize)]
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

		private MissionBaseContent _MissionBaseContent;

		private Hotspot[] _Benches = new Hotspot[0];

		private Hotspot[] _Haystacks = new Hotspot[0];

		private Hotspot[] _RoofGardens = new Hotspot[0];

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

		[ProtoMember(1, Name = "Benches", DataFormat = DataFormat.Default)]
		public Hotspot[] Benches
		{
			get
			{
				return _Benches;
			}
			set
			{
				Array.Resize(ref _Benches, value.Length);
				value.CopyTo(_Benches, 0);
			}
		}

		[ProtoMember(2, Name = "Haystacks", DataFormat = DataFormat.Default)]
		public Hotspot[] Haystacks
		{
			get
			{
				return _Haystacks;
			}
			set
			{
				Array.Resize(ref _Haystacks, value.Length);
				value.CopyTo(_Haystacks, 0);
			}
		}

		[ProtoMember(3, Name = "RoofGardens", DataFormat = DataFormat.Default)]
		public Hotspot[] RoofGardens
		{
			get
			{
				return _RoofGardens;
			}
			set
			{
				Array.Resize(ref _RoofGardens, value.Length);
				value.CopyTo(_RoofGardens, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
