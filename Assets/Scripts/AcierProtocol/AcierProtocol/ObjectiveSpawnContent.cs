using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveSpawnContent")]
	public class ObjectiveSpawnContent : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "SpawnContent")]
		public class SpawnContent : IExtensible
		{
			private bool _IsCounted = true;

			private bool _IsKillingMandatory;

			private float _Delay;

			private string _Guid = "";

			private long _DestinationSpot;

			private long[] _SpawnPoints = new long[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "IsCounted", DataFormat = DataFormat.Default)]
			[DefaultValue(true)]
			public bool IsCounted
			{
				get
				{
					return _IsCounted;
				}
				set
				{
					_IsCounted = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "IsKillingMandatory", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool IsKillingMandatory
			{
				get
				{
					return _IsKillingMandatory;
				}
				set
				{
					_IsKillingMandatory = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "Delay", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float Delay
			{
				get
				{
					return _Delay;
				}
				set
				{
					_Delay = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "Guid", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string Guid
			{
				get
				{
					return _Guid;
				}
				set
				{
					_Guid = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "DestinationSpot", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long DestinationSpot
			{
				get
				{
					return _DestinationSpot;
				}
				set
				{
					_DestinationSpot = value;
				}
			}

			[ProtoMember(6, Name = "SpawnPoints", DataFormat = DataFormat.TwosComplement)]
			public long[] SpawnPoints
			{
				get
				{
					return _SpawnPoints;
				}
				set
				{
					Array.Resize(ref _SpawnPoints, value.Length);
					value.CopyTo(_SpawnPoints, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private ObjectiveBaseContent _ObjectiveBaseContent;

		private SpawnContent[] _SpawnFrames = new SpawnContent[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "ObjectiveBaseContent", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ObjectiveBaseContent ObjectiveBaseContent
		{
			get
			{
				return _ObjectiveBaseContent;
			}
			set
			{
				_ObjectiveBaseContent = value;
			}
		}

		[ProtoMember(2, Name = "SpawnFrames", DataFormat = DataFormat.Default)]
		public SpawnContent[] SpawnFrames
		{
			get
			{
				return _SpawnFrames;
			}
			set
			{
				Array.Resize(ref _SpawnFrames, value.Length);
				value.CopyTo(_SpawnFrames, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
