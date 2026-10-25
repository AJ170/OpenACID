using System.Collections.Generic;
using Database;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioPlayBehaviourRandomNotSameTwice : AudioPlayBehaviourBase
	{
		public const string Description = "Random Not Same Twice: This behaviour will get a random sound every time it is played, but never the same sound twice in a row.";

		private static int m_sharedIndex = -1;

		private readonly float m_randomWeightSum;

		public override EPlayBehaviour EPlayBehaviour
		{
			get
			{
				return EPlayBehaviour.RandomNotSameTwiceInRow;
			}
		}

		public AudioPlayBehaviourRandomNotSameTwice(List<AudioControllerDef.SoundProperty> _list)
			: base(_list)
		{
			m_index = m_sharedIndex;
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
				m_index = (m_sharedIndex = 0);
				Future future2 = new Future();
				future2.SoundLoadDef = new SRawSoundLoadDef(m_rawSoundPropertiesConfig[m_index].GameID);
				future2.Index = m_index;
				future = future2;
			}
			else if (m_rawSoundPropertiesConfig.Count == 2)
			{
				m_index = (m_sharedIndex = ((m_sharedIndex == 0) ? 1 : 0));
				Future future2 = new Future();
				future2.SoundLoadDef = new SRawSoundLoadDef(m_rawSoundPropertiesConfig[m_index].GameID);
				future2.Index = m_index;
				future = future2;
			}
			else
			{
				int num = -1;
				do
				{
					float num2 = Random.Range(0f, m_randomWeightSum);
					float num3 = 0f;
					for (int i = 0; i < m_rawSoundPropertiesConfig.Count; i++)
					{
						num3 += m_rawSoundPropertiesConfig[i].RandomWeight;
						if (num2 <= num3)
						{
							num = i;
							break;
						}
					}
				}
				while (m_sharedIndex == num);
				m_index = (m_sharedIndex = num);
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
