using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveEffectSpawnerContent")]
	public class ObjectiveEffectSpawnerContent : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "ObjectDelay")]
		public class ObjectDelay : IExtensible
		{
			private int _ID;

			private int _DelayMs;

			private EEffectSpawnerSoundType _SoundType;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "ID", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int ID
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

			[ProtoMember(2, IsRequired = false, Name = "DelayMs", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int DelayMs
			{
				get
				{
					return _DelayMs;
				}
				set
				{
					_DelayMs = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "SoundType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EEffectSpawnerSoundType.None_EEffectSpawnerSoundType)]
			public EEffectSpawnerSoundType SoundType
			{
				get
				{
					return _SoundType;
				}
				set
				{
					_SoundType = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private ObjectiveBaseContent _ObjectiveBaseContent;

		private ObjectDelay[] _UniqueIds = new ObjectDelay[0];

		private int[] _HideObjectIds = new int[0];

		private IExtension extensionObject;

		[ProtoMember(10, IsRequired = false, Name = "ObjectiveBaseContent", DataFormat = DataFormat.Default)]
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

		[ProtoMember(4, Name = "UniqueIds", DataFormat = DataFormat.Default)]
		public ObjectDelay[] UniqueIds
		{
			get
			{
				return _UniqueIds;
			}
			set
			{
				Array.Resize(ref _UniqueIds, value.Length);
				value.CopyTo(_UniqueIds, 0);
			}
		}

		[ProtoMember(5, Name = "HideObjectIds", DataFormat = DataFormat.TwosComplement)]
		public int[] HideObjectIds
		{
			get
			{
				return _HideObjectIds;
			}
			set
			{
				Array.Resize(ref _HideObjectIds, value.Length);
				value.CopyTo(_HideObjectIds, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
