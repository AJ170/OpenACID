using SharedBaseLib.Core;
using UbiCore.Debug;

namespace Assets.Scripts.Audio
{
	public class LoadStrategyAll : LoadStrategyBase
	{
		private int m_isReadyToPlayCallbackCount;

		private int m_loadedCallbackCount;

		public override ELoadStrategy ELoadStrategy
		{
			get
			{
				return ELoadStrategy.LoadAll;
			}
		}

		public LoadStrategyAll(BBSimpleList<SRawSoundLoadDef> _list)
			: base(_list)
		{
		}

		protected override void OnIsReadyToPlayCallback(params object[] _objects)
		{
			m_isReadyToPlayCallbackCount++;
			if (m_isReadyToPlayCallbackCount >= m_loadedSounds.Count)
			{
				AudioResourceManager.Instance.OnIsReadyToPlay -= new SimpleDelegate.CallbackFunction(OnIsReadyToPlayCallback);
				base.OnIsReadyToPlayCallback(_objects);
			}
		}

		protected override void OnLoadedCallback(object[] _objects)
		{
			m_loadedCallbackCount++;
			if (m_loadedCallbackCount >= m_loadedSounds.Count)
			{
				AudioResourceManager.Instance.OnLoaded -= new SimpleDelegate.CallbackFunction(OnLoadedCallback);
				base.OnLoadedCallback(_objects);
			}
		}

		public override void Load()
		{
			Profiling.Start("LoadStrategy::Load()", float.MaxValue);
			if (m_loadedSounds.Count <= 0)
			{
				AudioResourceManager.Instance.OnIsReadyToPlay += new SimpleDelegate.CallbackFunction(OnIsReadyToPlayCallback);
				AudioResourceManager.Instance.OnLoaded += new SimpleDelegate.CallbackFunction(OnLoadedCallback);
				m_loadedSounds.Capacity = m_sounds.Count;
				for (int num = m_sounds.Count - 1; num >= 0; num--)
				{
					SRawSoundLoadDef sRawSoundLoadDef = m_sounds[num];
					m_loadedSounds.Add(sRawSoundLoadDef);
					if (AudioManager.Instance.CoroutineHelper != null)
					{
						AudioManager.Instance.CoroutineHelper.StartCoroutine(AudioResourceManager.Instance.Load(sRawSoundLoadDef));
					}
				}
			}
			Profiling.Stop();
		}

		public override void Unload()
		{
			Profiling.Start("LoadStrategy::Unload()", float.MaxValue);
			if (m_loadedSounds.Count > 0)
			{
				m_isReadyToPlayCallbackCount = (m_loadedCallbackCount = 0);
				AudioResourceManager.Instance.OnIsReadyToPlay -= new SimpleDelegate.CallbackFunction(OnIsReadyToPlayCallback);
				AudioResourceManager.Instance.OnLoaded -= new SimpleDelegate.CallbackFunction(OnLoadedCallback);
				for (int num = m_loadedSounds.Count - 1; num >= 0; num--)
				{
					SRawSoundLoadDef soundLoadDef = m_loadedSounds[num];
					m_loadedSounds.RemoveAt(num);
					AudioResourceManager.Instance.Unload(soundLoadDef);
				}
			}
			Profiling.Stop();
		}
	}
}
