using System.Collections.Generic;
using AssetBundleData;
using Assets.Scripts.Audio;
using SharedBaseLib.Serialization;

namespace Database
{
	[Database("AudioControllerDefinition")]
	public class AudioControllerDef : DBEntry
	{
		public class SoundProperty : DataSerializableBase
		{
			public GameID GameID;

			public string SoundGuid { get; set; }

			public float RandomWeight { get; set; }

			public float Volume { get; set; }

			public bool UseRandomVolume { get; set; }

			public float RandomVolumeMin { get; set; }

			public float RandomVolumeMax { get; set; }

			public float Pitch { get; set; }

			public bool UseRandomPitch { get; set; }

			public float RandomPitchMin { get; set; }

			public float RandomPitchMax { get; set; }

			public float FadeInSeconds { get; set; }

			public float FadeOutSeconds { get; set; }

			public SoundProperty()
			{
				RandomWeight = 1f;
				Volume = 1f;
				UseRandomVolume = false;
				RandomVolumeMin = 1f;
				RandomVolumeMax = 1f;
				Pitch = 1f;
				UseRandomPitch = false;
				RandomPitchMin = 1f;
				RandomPitchMax = 1f;
				FadeInSeconds = 0f;
				FadeOutSeconds = 0f;
			}

			public SoundProperty(SoundProperty _soundProperty)
			{
				RandomWeight = _soundProperty.RandomWeight;
				Volume = _soundProperty.Volume;
				UseRandomVolume = _soundProperty.UseRandomVolume;
				RandomVolumeMin = _soundProperty.RandomVolumeMin;
				RandomVolumeMax = _soundProperty.RandomVolumeMax;
				Pitch = _soundProperty.Pitch;
				UseRandomPitch = _soundProperty.UseRandomPitch;
				RandomPitchMin = _soundProperty.RandomPitchMin;
				RandomPitchMax = _soundProperty.RandomPitchMax;
				FadeInSeconds = _soundProperty.FadeInSeconds;
				FadeOutSeconds = _soundProperty.FadeOutSeconds;
				GameID = _soundProperty.GameID;
				SoundGuid = _soundProperty.SoundGuid;
			}

			protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
			{
				SoundGuid = _s.SerializeString("SoundGuid", SoundGuid);
				RandomWeight = _s.SerializeFloat("RandomWeight", RandomWeight);
				Volume = _s.SerializeFloat("Volume", Volume);
				UseRandomVolume = _s.SerializeBool("UseRandomVolume", UseRandomVolume);
				RandomVolumeMin = _s.SerializeFloat("RandomVolumeMin", RandomVolumeMin);
				RandomVolumeMax = _s.SerializeFloat("RandomVolumeMax", RandomVolumeMax);
				Pitch = _s.SerializeFloat("Pitch", Pitch);
				UseRandomPitch = _s.SerializeBool("UseRandomPitch", UseRandomPitch);
				RandomPitchMin = _s.SerializeFloat("RandomPitchMin", RandomPitchMin);
				RandomPitchMax = _s.SerializeFloat("RandomPitchMax", RandomPitchMax);
				FadeInSeconds = _s.SerializeFloat("FadeInSeconds", FadeInSeconds);
				FadeOutSeconds = _s.SerializeFloat("FadeOutSeconds", FadeOutSeconds);
			}
		}

		public class SoundTheme : DataSerializableBase
		{
			public List<SoundProperty> SoundProperties { get; set; }

			public ESoundTheme Theme { get; set; }

			public SoundTheme()
			{
				SoundProperties = new List<SoundProperty>();
				Theme = ESoundTheme.Standard;
			}

			public SoundTheme(SoundTheme _soundTheme)
			{
				SoundProperties = _soundTheme.SoundProperties;
				Theme = _soundTheme.Theme;
			}

			protected override void SerializeInternal(DataSerializerBase _s, object _customData = null)
			{
				Theme = (ESoundTheme)_s.SerializeEnum("SoundTheme", Theme.GetType(), (int)Theme);
				SoundProperties = _s.SerializeList("SoundProperties", SoundProperties);
			}
		}

		public int Priority { get; set; }

		public bool IgnoreSlowMotion { get; set; }

		public ELayer Channel { get; set; }

		public EPlayBehaviour PlayBehaviour { get; set; }

		public bool Is2DSound { get; set; }

		public GameID MixingGroupGameId { get; set; }

		public string MixingGroupGuidString { get; set; }

		public List<SoundTheme> SoundThemes { get; private set; }

		public string AssetPath { get; private set; }

		public string AssetPathWithoutFirstAssetFolder { get; private set; }

		public string AudioControllerAssetPathGuid { get; private set; }

		public AudioControllerDef(AssetBundleInfo _assetBundleInfo)
			: base(_assetBundleInfo)
		{
		}

		public override void Serialize(DataSerializerBase _s)
		{
			base.Serialize(_s);
			Priority = _s.SerializeInt("Priority", Priority);
			IgnoreSlowMotion = _s.SerializeBool("IgnoreSlowMotion", IgnoreSlowMotion);
			Channel = (ELayer)_s.SerializeEnum("Channel", Channel.GetType(), (int)Channel);
			PlayBehaviour = (EPlayBehaviour)_s.SerializeEnum("PlayBehaviour", PlayBehaviour.GetType(), (int)PlayBehaviour);
			MixingGroupGuidString = _s.SerializeString("MixingGroupGuidString", MixingGroupGuidString);
			SoundThemes = _s.SerializeList("SoundThemes", SoundThemes);
			Is2DSound = _s.SerializeBool("Is2DSound", Is2DSound);
			AudioControllerAssetPathGuid = _s.SerializeString("Guid", AudioControllerAssetPathGuid);
		}

		public SoundTheme GetActiveTheme()
		{
			return GetThemeByType(AudioManager.ActiveSoundTheme);
		}

		private SoundTheme GetThemeByType(ESoundTheme _type)
		{
			SoundTheme result = null;
			for (int i = 0; i < SoundThemes.Count; i++)
			{
				SoundTheme soundTheme = SoundThemes[i];
				if (soundTheme.Theme == _type && soundTheme.SoundProperties.Count > 0)
				{
					result = soundTheme;
					break;
				}
				if (soundTheme.Theme == ESoundTheme.Standard)
				{
					result = soundTheme;
				}
			}
			return result;
		}
	}
}
