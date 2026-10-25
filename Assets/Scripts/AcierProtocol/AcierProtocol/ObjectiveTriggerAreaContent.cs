using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ObjectiveTriggerAreaContent")]
	public class ObjectiveTriggerAreaContent : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "TriggerAreaSetting")]
		public class TriggerAreaSetting : IExtensible
		{
			private int _ID;

			private EEffectSpawnerSoundType _SoundTypeIn;

			private EEffectSpawnerSoundType _SoundTypeOut;

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

			[ProtoMember(3, IsRequired = false, Name = "SoundTypeIn", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EEffectSpawnerSoundType.None_EEffectSpawnerSoundType)]
			public EEffectSpawnerSoundType SoundTypeIn
			{
				get
				{
					return _SoundTypeIn;
				}
				set
				{
					_SoundTypeIn = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "SoundTypeOut", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EEffectSpawnerSoundType.None_EEffectSpawnerSoundType)]
			public EEffectSpawnerSoundType SoundTypeOut
			{
				get
				{
					return _SoundTypeOut;
				}
				set
				{
					_SoundTypeOut = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private ObjectiveBaseContent _ObjectiveBaseContent;

		private TriggerAreaSetting[] _Targets = new TriggerAreaSetting[0];

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

		[ProtoMember(4, Name = "Targets", DataFormat = DataFormat.Default)]
		public TriggerAreaSetting[] Targets
		{
			get
			{
				return _Targets;
			}
			set
			{
				Array.Resize(ref _Targets, value.Length);
				value.CopyTo(_Targets, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
