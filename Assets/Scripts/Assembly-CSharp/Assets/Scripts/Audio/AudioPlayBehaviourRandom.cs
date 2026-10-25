using System.Collections.Generic;
using Database;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioPlayBehaviourRandom : AudioPlayBehaviourBase
	{
		public const string Description = "Random: This behaviour will get a random sound every time it is played.";

		private readonly float m_randomWeightSum;

		public override EPlayBehaviour EPlayBehaviour
		{
			get
			{
				return EPlayBehaviour.Random;
			}
		}

		public AudioPlayBehaviourRandom(List<AudioControllerDef.SoundProperty> _list)
			: base(_list)
		{
			m_index = -1;
			for (int i = 0; i < _list.Count; i++)
			{
				m_randomWeightSum += _list[i].RandomWeight;
			}
		}

		protected override Future PushFuture()
		{
			Future future;
			if (m_rawSoundPropertiesConfig.Count == 0)
			{
				future = new Future();
			}
			else if (m_rawSoundPropertiesConfig.Count == 1)
			{
				m_index = 0;
				Future future2 = new Future();
				future2.SoundLoadDef = new SRawSoundLoadDef(m_rawSoundPropertiesConfig[m_index].GameID);
				future2.Index = m_index;
				future = future2;
			}
			else
			{
				float num = Random.Range(0f, m_randomWeightSum);
				float num2 = 0f;
				for (int i = 0; i < m_rawSoundPropertiesConfig.Count; i++)
				{
					num2 += m_rawSoundPropertiesConfig[i].RandomWeight;
					if (num <= num2)
					{
						m_index = i;
						break;
					}
				}
				Future future2 = new Future();
				future2.SoundLoadDef = new SRawSoundLoadDef(m_rawSoundPropertiesConfig[m_index].GameID);
				future2.Index = m_index;
				future = future2;
			}
			m_futures.Add(future);
			return future;
		}
	}
}
