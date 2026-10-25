using System.Collections.Generic;
using Database;

namespace Assets.Scripts.Audio
{
	public class AudioPlayBehaviourSingle : AudioPlayBehaviourBase
	{
		public const string Description = "Single: This behaviour should be used when only one sound is connected.";

		public override EPlayBehaviour EPlayBehaviour
		{
			get
			{
				return EPlayBehaviour.Single;
			}
		}

		public AudioPlayBehaviourSingle(List<AudioControllerDef.SoundProperty> _list)
			: base(_list)
		{
			m_index = 0;
		}

		protected override Future PushFuture()
		{
			Future future2;
			if (m_rawSoundPropertiesConfig.Count > 0)
			{
				Future future = new Future();
				future.SoundLoadDef = new SRawSoundLoadDef(m_rawSoundPropertiesConfig[m_index].GameID);
				future.Index = m_index;
				future2 = future;
			}
			else
			{
				future2 = new Future();
			}
			m_futures.Add(future2);
			return future2;
		}
	}
}
