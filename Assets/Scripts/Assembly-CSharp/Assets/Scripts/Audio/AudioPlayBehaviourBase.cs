using System.Collections.Generic;
using Database;

namespace Assets.Scripts.Audio
{
	public abstract class AudioPlayBehaviourBase : IAudioPlayBehaviourFuture, IAudioPlayBehaviour
	{
		protected int m_index;

		protected SRawSoundLoadDef RawSoundLoadDef = SRawSoundLoadDef.Invalid;

		protected readonly List<Future> m_unloadables = new List<Future>();

		protected List<Future> m_futures = new List<Future>();

		protected List<AudioControllerDef.SoundProperty> m_rawSoundPropertiesConfig;

		public abstract EPlayBehaviour EPlayBehaviour { get; }

		public IConstFuture Future
		{
			get
			{
				return PushFuture();
			}
		}

		public SRawSoundLoadDef Current { get; protected set; }

		public IConstFuture Take
		{
			get
			{
				if (m_futures.Count <= 0)
				{
					PushFuture();
				}
				Future future = PopFrontFuture();
				Current = future.SoundLoadDef;
				AddUnloadable(future);
				return future;
			}
		}

		public IConstFuture GiveBack
		{
			set
			{
				RemoveUnloadables((Future)value);
				m_futures.Insert(0, (Future)value);
			}
		}

		protected AudioPlayBehaviourBase(List<AudioControllerDef.SoundProperty> _list)
		{
			Current = SRawSoundLoadDef.Invalid;
			m_rawSoundPropertiesConfig = _list;
		}

		protected abstract Future PushFuture();

		protected Future PopFrontFuture()
		{
			if (m_futures.Count > 0)
			{
				Future result = m_futures[0];
				m_futures.RemoveAt(0);
				return result;
			}
			return null;
		}

		protected void AddUnloadable(Future _future)
		{
			m_unloadables.Add(_future);
		}

		protected bool RemoveUnloadables(Future _future)
		{
			return m_unloadables.Remove(_future);
		}

		public virtual void Unload(SRawSoundLoadDef _def)
		{
			for (int num = m_unloadables.Count - 1; num >= 0; num--)
			{
				if (m_unloadables[num].SoundLoadDef == _def)
				{
					RemoveUnloadables(m_unloadables[num]);
					return;
				}
			}
			for (int num2 = m_futures.Count - 1; num2 >= 0; num2--)
			{
				if (m_futures[num2].SoundLoadDef == _def)
				{
					m_futures.RemoveAt(num2);
					break;
				}
			}
		}

		public static string GetDescription(EPlayBehaviour _behaviour)
		{
			switch (_behaviour)
			{
			case EPlayBehaviour.Single:
				return "Single: This behaviour should be used when only one sound is connected.";
			case EPlayBehaviour.Random:
				return "Random: This behaviour will get a random sound every time it is played.";
			case EPlayBehaviour.RandomNotSameTwiceInRow:
				return "Random Not Same Twice: This behaviour will get a random sound every time it is played, but never the same sound twice in a row.";
			default:
				return "No description available.";
			}
		}
	}
}
