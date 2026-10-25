using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionSpawnEntityContent")]
	public class MissionSpawnEntityContent : IExtensible
	{
		private MissionBaseContent _MissionBaseContent;

		private bool _IsMimic;

		private EFadeMode _FadeMode;

		private float _FadeDuration = 1.5f;

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

		[ProtoMember(9, IsRequired = false, Name = "IsMimic", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool IsMimic
		{
			get
			{
				return _IsMimic;
			}
			set
			{
				_IsMimic = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "FadeMode", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EFadeMode.Simple_EFadeMode)]
		public EFadeMode FadeMode
		{
			get
			{
				return _FadeMode;
			}
			set
			{
				_FadeMode = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "FadeDuration", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(1.5f)]
		public float FadeDuration
		{
			get
			{
				return _FadeDuration;
			}
			set
			{
				_FadeDuration = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
