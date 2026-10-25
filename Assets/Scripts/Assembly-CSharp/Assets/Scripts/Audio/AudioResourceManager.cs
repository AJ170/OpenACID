using System;
using System.Collections;
using System.Collections.Generic;
using Database;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioResourceManager
	{
		private class LoadedInfo
		{
			public readonly GameID Id;

			public AudioClip Clip;

			public bool IsLoading;

			public int ReferenceCount { get; set; }

			internal LoadedInfo(GameID _id)
			{
				Id = _id;
			}

			public void IncreaseReferenceCount()
			{
				ReferenceCount++;
			}

			public void DecreaseReferenceCount()
			{
				if (ReferenceCount > 0)
				{
					ReferenceCount--;
				}
			}
		}

		private bool m_isDisabled;

		private readonly BBDictionary<GameID, LoadedInfo> m_loadedInfos = new BBDictionary<GameID, LoadedInfo>();

		private readonly BBSimpleList<SRawSoundLoadDef> m_scheduledRawSoundsToUnload = new BBSimpleList<SRawSoundLoadDef>();

		private readonly GameDB.SubDB m_soundDefSubDB;

		private static AudioResourceManager m_instance;

		public SimpleDelegate OnIsReadyToPlay;

		public SimpleDelegate OnLoaded;

		public SimpleDelegate OnLoadError;

		public static AudioResourceManager Instance
		{
			get
			{
				if (m_instance != null)
				{
					return m_instance;
				}
				return m_instance = new AudioResourceManager();
			}
		}

		public bool Disabled
		{
			get
			{
				return m_isDisabled;
			}
			set
			{
				m_isDisabled = value;
				foreach (KeyValuePair<GameID, LoadedInfo> loadedInfo in m_loadedInfos)
				{
					if (loadedInfo.Value.Clip != null)
					{
						Resources.UnloadAsset(loadedInfo.Value.Clip);
						loadedInfo.Value.Clip = null;
						loadedInfo.Value.ReferenceCount = 0;
					}
				}
			}
		}

		private AudioResourceManager()
		{
			Type[] paramTypes = new Type[2]
			{
				typeof(SRawSoundLoadDef),
				typeof(AudioClip)
			};
			OnIsReadyToPlay = new SimpleDelegate(this, paramTypes);
			OnLoaded = new SimpleDelegate(this, paramTypes);
			OnLoadError = new SimpleDelegate(this, new Type[2]
			{
				typeof(string),
				typeof(SRawSoundLoadDef)
			});
			m_soundDefSubDB = Main.SafeGameDb.GetSubDB(typeof(SoundDef));
			foreach (KeyValuePair<GameID, DBEntry> datum in m_soundDefSubDB.Data)
			{
				m_loadedInfos.Add(datum.Key, new LoadedInfo(datum.Key));
			}
		}

		public string GetAudioName(SRawSoundLoadDef _loadDef)
		{
			if (_loadDef.IsInvalid)
			{
				return "Invalid";
			}
			return m_soundDefSubDB.Data[_loadDef.Id].Name;
		}

		public AudioClip GetClip(SRawSoundLoadDef _soundLoadDef)
		{
			if (_soundLoadDef.IsInvalid)
			{
				return null;
			}
			return m_loadedInfos[_soundLoadDef.Id].Clip;
		}

		public bool IsReadyToPlay(SRawSoundLoadDef _soundLoadDef)
		{
			if (_soundLoadDef.IsInvalid)
			{
				return false;
			}
			LoadedInfo loadedInfo = m_loadedInfos[_soundLoadDef.Id];
			return loadedInfo.Clip != null && loadedInfo.Clip.loadState == AudioDataLoadState.Loaded;
		}

		public bool IsLoading(SRawSoundLoadDef _soundLoadDef)
		{
			if (_soundLoadDef.IsInvalid)
			{
				return false;
			}
			LoadedInfo loadedInfo = m_loadedInfos[_soundLoadDef.Id];
			return loadedInfo.IsLoading;
		}

		public IEnumerator Load(SRawSoundLoadDef _loadDef)
		{
			if (Disabled || _loadDef.IsInvalid)
			{
				yield break;
			}
			LoadedInfo info = m_loadedInfos[_loadDef.Id];
			info.IncreaseReferenceCount();
			if (info.Clip == null)
			{
				SoundDef soundDef = (SoundDef)m_soundDefSubDB.Data[info.Id];
				info.IsLoading = true;
				IEnumerator ite = soundDef.SoundResource.PrepareAsync();
				while (ite.MoveNext())
				{
					yield return 0;
				}
				info.IsLoading = false;
				info.Clip = soundDef.SoundResource.GetObject<AudioClip>();
				if (info.Clip == null)
				{
					HandleLoadError(info, _loadDef, "Unable to request soundresource");
					yield break;
				}
				if (OnIsReadyToPlay != null)
				{
					OnIsReadyToPlay.Trigger(_loadDef, info.Clip);
				}
				if (OnLoaded != null)
				{
					OnLoaded.Trigger(_loadDef, info.Clip);
				}
			}
			else if (info.Clip != null && info.Clip.loadState == AudioDataLoadState.Loaded && OnIsReadyToPlay != null)
			{
				OnIsReadyToPlay.Trigger(_loadDef, info.Clip);
			}
		}

		public void Unload(SRawSoundLoadDef _soundLoadDef)
		{
			if (!_soundLoadDef.IsInvalid)
			{
				LoadedInfo loadedInfo = m_loadedInfos[_soundLoadDef.Id];
				loadedInfo.DecreaseReferenceCount();
				if (loadedInfo.ReferenceCount <= 0 && loadedInfo.Clip != null)
				{
					Resources.UnloadAsset(loadedInfo.Clip);
					loadedInfo.Clip = null;
					loadedInfo.IsLoading = false;
					AudioManager.Instance.OnResourceUnloaded(_soundLoadDef);
				}
			}
		}

		public void HandleScheduledUnloads()
		{
			lock (m_scheduledRawSoundsToUnload)
			{
				for (int num = m_scheduledRawSoundsToUnload.Count - 1; num >= 0; num--)
				{
					Unload(m_scheduledRawSoundsToUnload[num]);
				}
				m_scheduledRawSoundsToUnload.Clear();
			}
		}

		public void ScheduleUnloadInMainThread(SRawSoundLoadDef _soundLoadDef)
		{
			lock (m_scheduledRawSoundsToUnload)
			{
				m_scheduledRawSoundsToUnload.Add(_soundLoadDef);
			}
		}

		private void HandleLoadError(LoadedInfo _info, SRawSoundLoadDef _loadDef, string _errorMsg)
		{
			if (OnLoadError != null)
			{
				OnLoadError.Trigger(_errorMsg, _loadDef);
			}
			_info.Clip = null;
			_info.DecreaseReferenceCount();
		}
	}
}
