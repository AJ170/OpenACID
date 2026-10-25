using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "QualitySettings")]
	public class QualitySettings : IExtensible
	{
		private EQualityAssetBundle _assetBundleQuality;

		private EQualityShaderLOD _renderingLOD;

		private int _qualityLevel;

		private float _crowdQuality;

		private float _virtualStickScale;

		private bool _downScaling;

		private EQualityTextureType _textureType;

		private int _anTuTuScore;

		private bool _everyPlayEnabled;

		private bool _lowEndDeviceWarning;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "assetBundleQuality", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EQualityAssetBundle.High)]
		public EQualityAssetBundle assetBundleQuality
		{
			get
			{
				return _assetBundleQuality;
			}
			set
			{
				_assetBundleQuality = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "renderingLOD", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EQualityShaderLOD.None_0)]
		public EQualityShaderLOD renderingLOD
		{
			get
			{
				return _renderingLOD;
			}
			set
			{
				_renderingLOD = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "qualityLevel", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int qualityLevel
		{
			get
			{
				return _qualityLevel;
			}
			set
			{
				_qualityLevel = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "crowdQuality", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float crowdQuality
		{
			get
			{
				return _crowdQuality;
			}
			set
			{
				_crowdQuality = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "virtualStickScale", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float virtualStickScale
		{
			get
			{
				return _virtualStickScale;
			}
			set
			{
				_virtualStickScale = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "downScaling", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool downScaling
		{
			get
			{
				return _downScaling;
			}
			set
			{
				_downScaling = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "textureType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EQualityTextureType.Generic)]
		public EQualityTextureType textureType
		{
			get
			{
				return _textureType;
			}
			set
			{
				_textureType = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "anTuTuScore", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int anTuTuScore
		{
			get
			{
				return _anTuTuScore;
			}
			set
			{
				_anTuTuScore = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "everyPlayEnabled", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool everyPlayEnabled
		{
			get
			{
				return _everyPlayEnabled;
			}
			set
			{
				_everyPlayEnabled = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "lowEndDeviceWarning", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool lowEndDeviceWarning
		{
			get
			{
				return _lowEndDeviceWarning;
			}
			set
			{
				_lowEndDeviceWarning = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
