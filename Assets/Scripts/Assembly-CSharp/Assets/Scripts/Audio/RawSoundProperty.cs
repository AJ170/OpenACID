using Database;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class RawSoundProperty
	{
		private float m_volume;

		private readonly float m_randomVolumeMin;

		private readonly float m_randomVolumeMax;

		private float m_pitch;

		private readonly float m_randomPitchMin;

		private readonly float m_randomPitchMax;

		public float Volume
		{
			get
			{
				return (!UseRandomVolume) ? m_volume : (m_volume = Random.Range(m_randomVolumeMin, m_randomVolumeMax));
			}
		}

		public float CurrentVolume
		{
			get
			{
				return m_volume;
			}
		}

		public bool UseRandomVolume { get; private set; }

		public float Pitch
		{
			get
			{
				return (!UseRandomPitch) ? m_pitch : (m_pitch = Random.Range(m_randomPitchMin, m_randomPitchMax));
			}
		}

		public float CurrentPitch
		{
			get
			{
				return m_pitch;
			}
		}

		public bool UseRandomPitch { get; private set; }

		public SRawSoundLoadDef SoundLoadDef { get; private set; }

		public float FadeInSeconds { get; private set; }

		public float FadeOutSeconds { get; private set; }

		public RawSoundProperty(AudioControllerDef.SoundProperty _config)
		{
			SoundLoadDef = new SRawSoundLoadDef(_config.GameID);
			m_volume = _config.Volume;
			UseRandomVolume = _config.UseRandomVolume;
			m_randomVolumeMin = _config.RandomVolumeMin;
			m_randomVolumeMax = _config.RandomVolumeMax;
			m_pitch = _config.Pitch;
			UseRandomPitch = _config.UseRandomPitch;
			m_randomPitchMin = _config.RandomPitchMin;
			m_randomPitchMax = _config.RandomPitchMax;
			FadeInSeconds = _config.FadeInSeconds;
			FadeOutSeconds = _config.FadeOutSeconds;
		}
	}
}
