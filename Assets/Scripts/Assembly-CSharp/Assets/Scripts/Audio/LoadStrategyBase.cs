using SharedBaseLib.Core;

namespace Assets.Scripts.Audio
{
	public abstract class LoadStrategyBase : ILoadStrategyUnloadAll
	{
		protected BBSimpleList<SRawSoundLoadDef> m_loadedSounds = new BBSimpleList<SRawSoundLoadDef>();

		protected readonly BBSimpleList<SRawSoundLoadDef> m_sounds;

		public override int LoadedSoundsCount
		{
			get
			{
				return m_loadedSounds.Count;
			}
		}

		protected LoadStrategyBase(BBSimpleList<SRawSoundLoadDef> _list)
		{
			m_sounds = _list;
		}

		~LoadStrategyBase()
		{
			ScheduleUnloadAll();
		}

		private void ScheduleUnloadAll()
		{
			for (int num = m_loadedSounds.Count - 1; num >= 0; num--)
			{
				AudioResourceManager.Instance.ScheduleUnloadInMainThread(m_loadedSounds[num]);
			}
			m_loadedSounds.Clear();
		}

		protected virtual void OnIsReadyToPlayCallback(params object[] _objects)
		{
			SRawSoundLoadDef sRawSoundLoadDef = (SRawSoundLoadDef)_objects[0];
			if (m_loadedSounds.Contains(sRawSoundLoadDef))
			{
				OnIsReadyToPlay.Trigger(sRawSoundLoadDef);
			}
		}

		protected virtual void OnLoadedCallback(params object[] _objects)
		{
			SRawSoundLoadDef sRawSoundLoadDef = (SRawSoundLoadDef)_objects[0];
			if (m_loadedSounds.Contains(sRawSoundLoadDef))
			{
				OnLoaded.Trigger(sRawSoundLoadDef);
			}
		}

		public override void UnloadAll()
		{
			for (int num = m_loadedSounds.Count - 1; num >= 0; num--)
			{
				AudioResourceManager.Instance.Unload(m_loadedSounds[num]);
			}
			m_loadedSounds.Clear();
		}
	}
}
