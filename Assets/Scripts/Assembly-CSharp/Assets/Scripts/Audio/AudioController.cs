using System.Collections.Generic;
using Database;
using SharedBaseLib.Core;

namespace Assets.Scripts.Audio
{
	public class AudioController
	{
		private List<RawSoundProperty> m_rawSoundProperties = new List<RawSoundProperty>();

		private IAudioPlayBehaviour m_audioPlayBehaviour;

		public BBSimpleList<SRawSoundLoadDef> RawSounds = new BBSimpleList<SRawSoundLoadDef>();

		public GameID Id { get; private set; }

		public float Volume { get; set; }

		public ELayer Layer { get; private set; }

		public GameID MixingGroups { get; private set; }

		public int Priority { get; private set; }

		public bool IgnoreSlowMotion { get; private set; }

		public bool Is2DSound { get; private set; }

		public IAudioPlayBehaviour AudioPlayBehaviour
		{
			get
			{
				return m_audioPlayBehaviour;
			}
			set
			{
				if (value != null)
				{
					m_audioPlayBehaviour = value;
				}
			}
		}

		public AudioController(AudioControllerDef _def)
		{
			Id = _def.GameID;
			Volume = 1f;
			Layer = _def.Channel;
			MixingGroups = _def.MixingGroupGameId;
			Priority = _def.Priority;
			IgnoreSlowMotion = _def.IgnoreSlowMotion;
			Is2DSound = _def.Is2DSound;
			AudioControllerDef.SoundTheme activeTheme = _def.GetActiveTheme();
			if (activeTheme == null)
			{
				return;
			}
			foreach (AudioControllerDef.SoundProperty soundProperty in activeTheme.SoundProperties)
			{
				m_rawSoundProperties.Add(new RawSoundProperty(soundProperty));
				RawSounds.Add(new SRawSoundLoadDef(soundProperty.GameID));
			}
			switch (_def.PlayBehaviour)
			{
			case EPlayBehaviour.Single:
				AudioPlayBehaviour = new AudioPlayBehaviourSingle(activeTheme.SoundProperties);
				break;
			case EPlayBehaviour.Random:
				AudioPlayBehaviour = new AudioPlayBehaviourRandom(activeTheme.SoundProperties);
				break;
			case EPlayBehaviour.RandomNotSameTwiceInRow:
				AudioPlayBehaviour = new AudioPlayBehaviourRandomNotSameTwice(activeTheme.SoundProperties);
				break;
			}
		}

		public RawSoundProperty GetPropertyFromSound(SRawSoundLoadDef _soundLoadDef)
		{
			for (int i = 0; i < m_rawSoundProperties.Count; i++)
			{
				if (m_rawSoundProperties[i].SoundLoadDef == _soundLoadDef)
				{
					return m_rawSoundProperties[i];
				}
			}
			return null;
		}
	}
}
