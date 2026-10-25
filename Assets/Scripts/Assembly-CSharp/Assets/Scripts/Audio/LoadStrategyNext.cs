using System.Collections.Generic;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class LoadStrategyNext : LoadStrategyBase
	{
		private abstract class CallbackData
		{
			protected readonly LoadStrategyNext m_strategy;

			public SRawSoundLoadDef RawSoundLoadDef { get; private set; }

			public AudioClip AudioClip { get; private set; }

			public bool IsDone { get; private set; }

			protected CallbackData(LoadStrategyNext _strategy, SRawSoundLoadDef _loadDef)
			{
				m_strategy = _strategy;
				RawSoundLoadDef = _loadDef;
			}

			protected bool ValidateAndHandleData(SRawSoundLoadDef _loadDef, AudioClip _audioClip)
			{
				if (RawSoundLoadDef == _loadDef)
				{
					AudioClip = _audioClip;
					IsDone = true;
					return true;
				}
				return false;
			}

			public abstract void DisconnectEvent();
		}

		private class IsReadyToPlayCallbackData : CallbackData
		{
			public IsReadyToPlayCallbackData(LoadStrategyNext _strategy, SRawSoundLoadDef _loadDef)
				: base(_strategy, _loadDef)
			{
				AudioResourceManager.Instance.OnIsReadyToPlay += new SimpleDelegate.CallbackFunction(OnIsReadyToPlayCallback);
			}

			public override void DisconnectEvent()
			{
				AudioResourceManager.Instance.OnIsReadyToPlay -= new SimpleDelegate.CallbackFunction(OnIsReadyToPlayCallback);
			}

			public void OnIsReadyToPlayCallback(object[] _objects)
			{
				SRawSoundLoadDef loadDef = (SRawSoundLoadDef)_objects[0];
				AudioClip audioClip = (AudioClip)_objects[1];
				if (ValidateAndHandleData(loadDef, audioClip))
				{
					DisconnectEvent();
					m_strategy.HandleOnReadyToPlayQueue();
				}
			}
		}

		private class LoadDoneCallbackData : CallbackData
		{
			public LoadDoneCallbackData(LoadStrategyNext _strategy, SRawSoundLoadDef _loadDef)
				: base(_strategy, _loadDef)
			{
				AudioResourceManager.Instance.OnLoaded += new SimpleDelegate.CallbackFunction(OnLoadedCallback);
			}

			public override void DisconnectEvent()
			{
				AudioResourceManager.Instance.OnLoaded -= new SimpleDelegate.CallbackFunction(OnLoadedCallback);
			}

			public void OnLoadedCallback(object[] _objects)
			{
				SRawSoundLoadDef loadDef = (SRawSoundLoadDef)_objects[0];
				AudioClip audioClip = (AudioClip)_objects[1];
				if (ValidateAndHandleData(loadDef, audioClip))
				{
					DisconnectEvent();
					m_strategy.HandleLoadedQueue();
				}
			}
		}

		private readonly IAudioPlayBehaviourFuture m_audioPlayBehaviourFuture;

		private readonly List<CallbackData> m_readyToPlayQueue = new List<CallbackData>();

		private readonly List<CallbackData> m_loadedQueue = new List<CallbackData>();

		public override ELoadStrategy ELoadStrategy
		{
			get
			{
				return ELoadStrategy.LoadNext;
			}
		}

		public LoadStrategyNext(BBSimpleList<SRawSoundLoadDef> _list, IAudioPlayBehaviourFuture _audioPlayBehaviourFuture)
			: base(_list)
		{
			m_audioPlayBehaviourFuture = _audioPlayBehaviourFuture;
		}

		public void HandleOnReadyToPlayQueue()
		{
			int num = m_readyToPlayQueue.Count - 1;
			while (num >= 0)
			{
				CallbackData callbackData = m_readyToPlayQueue[num];
				if (callbackData.IsDone)
				{
					base.OnIsReadyToPlayCallback(callbackData.RawSoundLoadDef, callbackData.AudioClip);
					if (m_readyToPlayQueue.Count > num)
					{
						m_readyToPlayQueue.RemoveAt(num);
					}
					num--;
					continue;
				}
				break;
			}
		}

		public void HandleLoadedQueue()
		{
			int num = m_loadedQueue.Count - 1;
			while (num >= 0)
			{
				CallbackData callbackData = m_loadedQueue[num];
				if (callbackData.IsDone)
				{
					base.OnLoadedCallback(callbackData.RawSoundLoadDef, callbackData.AudioClip);
					if (m_loadedQueue.Count > num)
					{
						m_loadedQueue.RemoveAt(num);
					}
					num--;
					continue;
				}
				break;
			}
		}

		public override void Load()
		{
			if (m_audioPlayBehaviourFuture != null)
			{
				IConstFuture future = m_audioPlayBehaviourFuture.Future;
				m_loadedSounds.Add(future.SoundLoadDef);
				m_readyToPlayQueue.Insert(0, new IsReadyToPlayCallbackData(this, future.SoundLoadDef));
				m_loadedQueue.Insert(0, new LoadDoneCallbackData(this, future.SoundLoadDef));
				if (AudioManager.Instance.CoroutineHelper != null)
				{
					AudioManager.Instance.CoroutineHelper.StartCoroutine(AudioResourceManager.Instance.Load(future.SoundLoadDef));
				}
			}
		}

		public override void Unload()
		{
			if (m_loadedSounds.Count > 0)
			{
				SRawSoundLoadDef sRawSoundLoadDef = m_loadedSounds[0];
				m_loadedSounds.RemoveAt(0);
				m_audioPlayBehaviourFuture.Unload(sRawSoundLoadDef);
				if (m_readyToPlayQueue.Count > 0)
				{
					CallbackData callbackData = m_readyToPlayQueue[m_readyToPlayQueue.Count - 1];
					callbackData.DisconnectEvent();
					m_readyToPlayQueue.Remove(callbackData);
				}
				if (m_loadedQueue.Count > 0)
				{
					CallbackData callbackData2 = m_loadedQueue[m_loadedQueue.Count - 1];
					callbackData2.DisconnectEvent();
					m_loadedQueue.Remove(callbackData2);
				}
				AudioResourceManager.Instance.Unload(sRawSoundLoadDef);
			}
		}
	}
}
