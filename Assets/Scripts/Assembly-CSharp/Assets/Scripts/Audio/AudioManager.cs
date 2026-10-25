using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Scripts.Audio.Managers.EventManager;
using Database;
using SharedBaseLib.Core;
using UbiCore;
using UbiCore.Debug;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioManager
	{
		private class AudioHandle : IAudioLoadHandle, IAudioPlayHandle, IAudioHandle
		{
			private static readonly Type[] OnFinishedPlayingTypes = new Type[1] { typeof(SRawSoundLoadDef) };

			private static readonly Type[] OnSheduledPlayTypes = new Type[1] { typeof(PlayResult) };

			private VolumeFader m_volumeFader;

			private AudioSource m_audioSource;

			public AudioController AudioController { get; private set; }

			public ILoadStrategy LoadStrategy { get; private set; }

			public ILoadStrategyEvent LoadStrategyEvent
			{
				get
				{
					return LoadStrategy;
				}
			}

			public IStoppedFinishedStrategy StoppedFinishedStrategy { get; private set; }

			public bool ShouldPlayOnLoadReady { get; set; }

			public bool UnloadOnPlayFailed { get; set; }

			public bool Loop { get; private set; }

			public PlayResult LastPlayResult { get; private set; }

			public float Pitch { get; set; }

			public bool IsPlaying { get; private set; }

			public IVolumeFader VolumeFader
			{
				get
				{
					VolumeFader obj = m_volumeFader ?? new VolumeFader(this);
					VolumeFader result = obj;
					m_volumeFader = obj;
					return result;
				}
			}

			public AudioManagerCoroutineHelper CoroutineHelper
			{
				get
				{
					return Instance.CoroutineHelper;
				}
			}

			public GameID AudioControllerId
			{
				get
				{
					return AudioController.Id;
				}
			}

			public float PlaybackTime
			{
				get
				{
					return Instance.PlaybackTime(this);
				}
			}

			public float PlaybackLength
			{
				get
				{
					return Instance.PlaybackLength(this);
				}
			}

			public IEnumerator FadeOutHandler { get; set; }

			public float Volume
			{
				get
				{
					return AudioController.Volume;
				}
				set
				{
					AudioController.Volume = Mathf.Clamp01(value);
					Instance.UpdateVolume(this);
				}
			}

			public SimpleDelegate OnFinishedEvent { get; set; }

			public SimpleDelegate OnScheduledPlay { get; set; }

			public AudioHandle(AudioController _controller, ILoadStrategy _loadStrategy, IStoppedFinishedStrategy _stoppedFinishedStrategy)
			{
				OnFinishedEvent = new SimpleDelegate(this, OnFinishedPlayingTypes);
				OnScheduledPlay = new SimpleDelegate(this, OnSheduledPlayTypes);
				AudioController = _controller;
				LoadStrategy = _loadStrategy;
				StoppedFinishedStrategy = _stoppedFinishedStrategy;
				ShouldPlayOnLoadReady = true;
				UnloadOnPlayFailed = true;
			}

			public bool DetachChannelFromParent()
			{
				return Instance.DetachChannelFromParent(this);
			}

			public void ChangeStoppedFinishedStrategy(EStoppedFinishedStrategy _eStrategy)
			{
				if (StoppedFinishedStrategy.EStoppedFinishedStrategy != _eStrategy)
				{
					switch (_eStrategy)
					{
					case EStoppedFinishedStrategy.UnloadAll:
						StoppedFinishedStrategy = new StoppedFinishedStrategyAll((ILoadStrategyUnloadAll)LoadStrategy);
						break;
					case EStoppedFinishedStrategy.UnloadCurrent:
						StoppedFinishedStrategy = new StoppedFinishedStrategyCurrent(LoadStrategy);
						break;
					case EStoppedFinishedStrategy.UnloadNone:
						StoppedFinishedStrategy = new StoppedFinishedStrategyNone();
						break;
					}
				}
			}

			public void Load()
			{
				LoadStrategy.Load();
			}

			public void Unload()
			{
				LoadStrategy.Unload();
			}

			public void SchedulePlayOnLoadReady()
			{
				if (ShouldPlayOnLoadReady)
				{
					LoadStrategy.OnIsReadyToPlay.Add(OnIsReadyToPlay);
				}
			}

			public void OnIsReadyToPlay(params object[] _objects)
			{
				LoadStrategy.OnIsReadyToPlay.Remove(OnIsReadyToPlay);
				if (ShouldPlayOnLoadReady)
				{
					PlayResult playResult = Play(Loop, m_audioSource);
					OnScheduledPlay.Trigger(playResult);
				}
			}

			public PlayResult Play(bool _loop = false, AudioSource _audioSource = null, float _delay = 0f)
			{
				if (!MathHelper.Approximately(_delay, 0f) && Instance.CoroutineHelper != null)
				{
					Instance.CoroutineHelper.StartCoroutine(PlayDelayed(_delay, _loop, _audioSource));
					return PlayResult.SuccessPlayDelayed;
				}
				PlayResult playResult = false;
				if (!IsPlaying)
				{
					Loop = _loop;
					m_audioSource = _audioSource;
					PlayResult playResult3 = (LastPlayResult = Instance.Play(this, _loop, _audioSource));
					playResult = playResult3;
					IsPlaying = playResult == PlayResult.Success;
					if (UnloadOnPlayFailed && !playResult)
					{
						Unload();
					}
				}
				return playResult;
			}

			public void Stop()
			{
				LoadStrategy.OnIsReadyToPlay.Remove(OnIsReadyToPlay);
				Instance.Stop(this);
			}

			public void OnStopped(SRawSoundLoadDef _soundLoadDef)
			{
				IsPlaying = false;
				StoppedFinishedStrategy.OnStoppedFinished(_soundLoadDef);
			}

			public void OnFinished(SRawSoundLoadDef _soundLoadDef)
			{
				IsPlaying = false;
				StoppedFinishedStrategy.OnStoppedFinished(_soundLoadDef);
				OnFinishedEvent.Trigger(_soundLoadDef);
			}

			private IEnumerator PlayDelayed(float _delay, bool _loop = false, AudioSource _audioSource = null)
			{
				yield return new WaitForSeconds(_delay);
				Play(_loop, _audioSource);
			}
		}

		private class VolumeFader : IVolumeFader
		{
			private bool m_shouldStop;

			private readonly AudioHandle m_handle;

			public IEnumerator Coroutine { get; private set; }

			public bool IsRunning { get; private set; }

			public VolumeFader(AudioHandle _handle)
			{
				m_handle = _handle;
			}

			public void Start(float _volumeTo, float _time)
			{
				Stop();
				if (m_handle.CoroutineHelper != null)
				{
					m_shouldStop = false;
					Coroutine = FadeVolume(_volumeTo, _time);
					m_handle.CoroutineHelper.StartCoroutine(Coroutine);
				}
			}

			public void Stop()
			{
				if (IsRunning)
				{
					m_shouldStop = true;
					Coroutine.MoveNext();
				}
			}

			private IEnumerator FadeVolume(float _to, float _time)
			{
				IsRunning = true;
				float startVolume = m_handle.Volume;
				float lerp = 0f;
				while (m_handle.LastPlayResult == PlayResult.SuccessPlayOnReadyScheduled)
				{
					yield return null;
				}
				while (lerp < 1f)
				{
					lerp = ((!m_handle.AudioController.IgnoreSlowMotion) ? (lerp + UbiCore.Utils.Singleton<Timer>.Instance.deltaTime / _time) : (lerp + UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime / _time));
					float volume = Mathf.Lerp(startVolume, _to, lerp);
					m_handle.Volume = volume;
					yield return null;
					if (m_shouldStop || (m_handle != null && !m_handle.IsPlaying))
					{
						break;
					}
				}
				if (m_handle.Volume <= 0f)
				{
					Instance.Stop(m_handle);
				}
				IsRunning = false;
			}
		}

		public interface IPlayingInfo : IMixGroupCarrier
		{
			IAudioHandle IAudioHandle { get; }
		}

		private class PlayingInfo : IPlayingInfo, IMixGroupCarrier
		{
			public AudioHandle AudioHandle;

			public AudioSource AudioSource;

			public SRawSoundLoadDef RawSoundLoadDef;

			public IAudioHandle IAudioHandle
			{
				get
				{
					return AudioHandle;
				}
			}

			public GameID MixGroup
			{
				get
				{
					return AudioHandle.AudioController.MixingGroups;
				}
			}

			public long StartTimeStamp { get; set; }

			public override string ToString()
			{
				return string.Format("MixGroup: {0}, AudioSource: {1}", MixGroup, AudioSource);
			}
		}

		private class ChannelHandler
		{
			private readonly BBDictionary<int, PoolEnumeratorConstruct<AudioSource>> m_channels = new BBDictionary<int, PoolEnumeratorConstruct<AudioSource>>();

			private readonly PoolEnumeratorConstruct<AudioSource>.SwapCondition m_swapCondition = new PoolEnumeratorConstruct<AudioSource>.SwapCondition();

			private readonly ReadOnlyCollection<PlayingInfo> m_playing;

			private readonly AudioManagerInspector m_audioManagerInspector;

			public ChannelHandler(ReadOnlyCollection<PlayingInfo> _playing, AudioManagerInspector _audioManagerInspector)
			{
				m_playing = _playing;
				m_audioManagerInspector = _audioManagerInspector;
				m_channels[1] = new PoolEnumeratorConstruct<AudioSource>(12, () => CreateChannel("Effect"));
				m_channels[2] = new PoolEnumeratorConstruct<AudioSource>(5, () => CreateChannel("Music"));
				m_channels[3] = new PoolEnumeratorConstruct<AudioSource>(6, () => CreateChannel("UI"));
				m_channels[4] = new PoolEnumeratorConstruct<AudioSource>(8, () => CreateChannel("Environment"));
				m_swapCondition.Condition = ChannelSwapConditionAndHandleing;
			}

			public AudioSource GetChannel(ELayer _eChannel, int _priority, AudioSource _audioSource)
			{
				if (_audioSource == null)
				{
					m_swapCondition.CompareObj = _priority;
					PoolEnumeratorConstruct<AudioSource> value;
					if (m_channels.TryGetValue((int)_eChannel, out value))
					{
						value.TryGet(out _audioSource, m_swapCondition);
					}
				}
				if (_audioSource == null)
				{
					return null;
				}
				if (IsChannelInUse(_audioSource))
				{
					StopAudioHandleOnChannel(_audioSource);
					return _audioSource;
				}
				List<AudioSource> audioSourcesInUse = GetAudioSourcesInUse(_eChannel);
				if (audioSourcesInUse.Count < GetCapForChannel(_eChannel))
				{
					return _audioSource;
				}
				AudioSource audioSourceWithLowestPriority = GetAudioSourceWithLowestPriority(audioSourcesInUse);
				if (audioSourceWithLowestPriority.priority <= _priority)
				{
					return null;
				}
				StopAudioHandleOnChannel(audioSourceWithLowestPriority);
				return _audioSource;
			}

			public bool IsChannelAvailable(ELayer _eChannel, int _priority)
			{
				List<AudioSource> audioSourcesInUse = GetAudioSourcesInUse(_eChannel);
				if (audioSourcesInUse.Count < GetCapForChannel(_eChannel))
				{
					return true;
				}
				AudioSource audioSourceWithLowestPriority = GetAudioSourceWithLowestPriority(audioSourcesInUse);
				return audioSourceWithLowestPriority.priority > _priority;
			}

			public void Free(ELayer _channel, AudioSource _audioSource)
			{
				PoolEnumeratorConstruct<AudioSource> value;
				if (m_channels.TryGetValue((int)_channel, out value))
				{
					value.Free(_audioSource);
				}
			}

			private IEnumerator<AudioSource> CreateChannel(string _channelName)
			{
				while (true)
				{
					AudioSource audioSource = UnityEngine.Object.Instantiate(Resources.Load<AudioSource>("Audio/Channel"));
					audioSource.spread = 40f;
					audioSource.spatialBlend = 1f;
					audioSource.transform.name = _channelName;
					SetDefaultChannelSettings(audioSource);
					yield return audioSource;
				}
			}

			private void SetDefaultChannelSettings(AudioSource _channel)
			{
				if (m_audioManagerInspector != null)
				{
					_channel.transform.parent = m_audioManagerInspector.Channels.transform;
					_channel.transform.localPosition = Vector3.zero;
					_channel.transform.localRotation = Quaternion.identity;
				}
			}

			private bool IsChannelInUse(AudioSource _audioSource)
			{
				for (int i = 0; i < m_playing.Count; i++)
				{
					if (m_playing[i].AudioSource == _audioSource)
					{
						return true;
					}
				}
				return false;
			}

			private List<AudioSource> GetAudioSourcesInUse(ELayer _eChannel)
			{
				List<AudioSource> list = new List<AudioSource>();
				for (int i = 0; i < m_playing.Count; i++)
				{
					if (m_playing[i].AudioHandle.AudioController.Layer == _eChannel)
					{
						list.Add(m_playing[i].AudioSource);
					}
				}
				return list;
			}

			private void StopAudioHandleOnChannel(AudioSource _audioSource)
			{
				if (!(_audioSource != null))
				{
					return;
				}
				for (int num = m_playing.Count - 1; num >= 0; num--)
				{
					if (_audioSource == m_playing[num].AudioSource)
					{
						m_playing[num].AudioHandle.Stop();
						break;
					}
				}
			}

			private int GetCapForChannel(ELayer _eChannel)
			{
				PoolEnumeratorConstruct<AudioSource> value;
				if (m_channels.TryGetValue((int)_eChannel, out value))
				{
					return value.Capacity;
				}
				return 0;
			}

			private AudioSource GetAudioSourceWithLowestPriority(List<AudioSource> _list)
			{
				AudioSource result = null;
				int num = -1;
				for (int i = 0; i < _list.Count; i++)
				{
					if (num == -1)
					{
						num = i;
					}
					else if (_list[num].priority < _list[i].priority)
					{
						num = i;
					}
					result = _list[num];
				}
				return result;
			}

			private bool ChannelSwapConditionAndHandleing(List<AudioSource> _list, out AudioSource _audioSource)
			{
				_audioSource = null;
				int num = -1;
				for (int i = 0; i < _list.Count; i++)
				{
					if (_list[i].priority > (int)m_swapCondition.CompareObj)
					{
						if (num == -1)
						{
							num = i;
						}
						else if (_list[num].priority < _list[i].priority)
						{
							num = i;
						}
						_audioSource = _list[num];
					}
				}
				if (_audioSource != null)
				{
					StopAudioHandleOnChannel(_audioSource);
				}
				return _audioSource != null;
			}
		}

		private class SlowMotionHandler
		{
			private const float Interval = 0.02f;

			private readonly AudioManagerInspector m_audioManagerInspector;

			private readonly ReadOnlyCollection<PlayingInfo> m_playing;

			private bool m_isSlowingDown = true;

			private float m_tick;

			private float m_smoothTimeScale;

			private float m_factorIncrease;

			public SlowMotionHandler(ReadOnlyCollection<PlayingInfo> _playing, AudioManagerInspector _audioManagerInspector)
			{
				m_playing = _playing;
				m_audioManagerInspector = _audioManagerInspector;
			}

			public void Update()
			{
				if (!MathHelper.Approximately(m_smoothTimeScale, UbiCore.Utils.Singleton<Timer>.Instance.timeScale))
				{
					UpdateSmoothSlowMotionScale(UbiCore.Utils.Singleton<Timer>.Instance.timeScale);
					UpdatePitchAccordingToTimeScale();
				}
			}

			public float GetPitch(AudioHandle _handle)
			{
				return (!_handle.AudioController.IgnoreSlowMotion) ? (_handle.Pitch - (1f - TranslateFactorIntoRange(m_smoothTimeScale, m_audioManagerInspector.SlowMotionTimeScaleMin, 1f))) : _handle.Pitch;
			}

			private void UpdateSmoothSlowMotionScale(float _timeScale)
			{
				if (m_audioManagerInspector == null)
				{
					return;
				}
				if (m_smoothTimeScale < _timeScale)
				{
					if (m_isSlowingDown)
					{
						m_isSlowingDown = false;
						float num = m_audioManagerInspector.SlowMotionFromZeroToOne - m_audioManagerInspector.SlowMotionFromZeroToOne * _timeScale;
						m_factorIncrease = 0.02f / num;
					}
					m_tick += Time.deltaTime;
					if (m_tick >= 0.02f)
					{
						m_tick = 0f;
						float num2 = _timeScale - m_smoothTimeScale;
						if (num2 <= m_factorIncrease)
						{
							m_smoothTimeScale = _timeScale;
						}
						else
						{
							m_smoothTimeScale += m_factorIncrease;
						}
					}
				}
				else
				{
					m_isSlowingDown = true;
					m_tick = 0f;
					m_smoothTimeScale = _timeScale;
				}
				m_smoothTimeScale = Mathf.Clamp01(m_smoothTimeScale);
			}

			private void UpdatePitchAccordingToTimeScale()
			{
				for (int num = m_playing.Count - 1; num >= 0; num--)
				{
					PlayingInfo playingInfo = m_playing[num];
					if (playingInfo.AudioSource != null)
					{
						playingInfo.AudioSource.pitch = GetPitch(playingInfo.AudioHandle);
					}
				}
			}

			private float TranslateFactorIntoRange(float _factor, float _min, float _max)
			{
				float num = _max - _min;
				return _min + num * _factor;
			}
		}

		public class DuckingSystemManager : MixGroupDuckingManager
		{
			private const int QSamples = 2048;

			private const float RefValue = 0.1f;

			private const float UpdateInterval = 0.04f;

			private readonly ReadOnlyCollection<PlayingInfo> m_playing;

			private readonly float[] m_floats = new float[2048];

			private float m_updateTick;

			public bool UseAudioOutputData { get; set; }

			public DuckingSystemManager(ref BBDictionary<GameID, float> _volumes, GameDB.SubDB _subDb)
				: base(ref _volumes, _subDb)
			{
				m_playing = Instance.m_playing.AsReadOnly();
				UseAudioOutputData = false;
			}

			public override void Start(IMixGroupCarrier _carrier)
			{
				if (!UseAudioOutputData)
				{
					base.Start(_carrier);
				}
			}

			public override void Update()
			{
				Profiling.Start("DuckingSystemManager::Update", float.MaxValue);
				if (m_updateTick < 0.04f)
				{
					m_updateTick += Time.deltaTime;
				}
				else
				{
					m_updateTick = 0f;
					if (UseAudioOutputData)
					{
						UpdateAudioOutputData();
					}
				}
				Profiling.Stop();
			}

			private void UpdateAudioOutputData()
			{
				for (int i = 0; i < m_playing.Count; i++)
				{
					PlayingInfo playingInfo = m_playing[i];
					if (!(playingInfo.AudioHandle.AudioController.MixingGroups == UbiCore.Utils.Singleton<MixGroupGameIds>.Instance.Default) && playingInfo.AudioSource != null)
					{
						playingInfo.AudioSource.GetOutputData(m_floats, 0);
						float num = 0f;
						for (int j = 0; j < 2048; j++)
						{
							num += m_floats[j] * m_floats[j];
						}
						float value = Mathf.Sqrt(num / 2048f);
						HandleCheckOnGroup(value, playingInfo);
					}
				}
			}

			private void HandleCheckOnGroup(float _value, IPlayingInfo _playing)
			{
				GameID mixingGroups = _playing.IAudioHandle.AudioController.MixingGroups;
				AudioMixingGroupDef audioMixingGroupDef = (AudioMixingGroupDef)m_subDb.Data[mixingGroups];
				for (int i = 0; i < audioMixingGroupDef.DuckingInfos.Count; i++)
				{
					AudioMixingGroupDef.DuckingInfo duckingInfo = audioMixingGroupDef.DuckingInfos[i];
					if (duckingInfo.Threshold < _value)
					{
						DuckingData duckingData = Find(duckingInfo.MixingGroup, _playing, m_running);
						if ((duckingData == null || !duckingData.DirectionReduce) && Find(duckingInfo.MixingGroup, _playing, m_ducked) == null)
						{
							Start(duckingInfo.MixingGroup, duckingInfo.VolumeReduceTo, duckingInfo.FadeInTime, _playing);
						}
						continue;
					}
					DuckingData duckingData2 = Find(duckingInfo.MixingGroup, _playing, m_running);
					if (duckingData2 != null && duckingData2.DirectionReduce)
					{
						Start(duckingInfo.MixingGroup, 1f, duckingInfo.FadeOutTime, _playing);
						continue;
					}
					DuckingData duckingData3 = Find(duckingInfo.MixingGroup, _playing, m_ducked);
					if (duckingData3 != null)
					{
						Start(duckingInfo.MixingGroup, 1f, duckingInfo.FadeOutTime, _playing);
					}
				}
			}
		}

		public interface IMixGroupCarrier
		{
			GameID MixGroup { get; }
		}

		public class MixGroupCarrier : IMixGroupCarrier
		{
			public GameID MixGroup { get; private set; }

			public MixGroupCarrier(GameID _mixGroup)
			{
				MixGroup = _mixGroup;
			}

			public override string ToString()
			{
				return string.Format("MixGroup: {0}", MixGroup);
			}
		}

		public class MixGroupDuckingManager
		{
			public class DuckingData
			{
				public IMixGroupCarrier StartedBy;

				public GameID MixingGroups;

				public bool ShouldStop;

				public IEnumerator Generator;

				public bool DirectionReduce;

				public float Volume;

				public float FadeStayTime;

				public override string ToString()
				{
					return string.Format("StartedBy: {0}, MixingGroups: {1}", StartedBy, MixingGroups);
				}
			}

			private readonly BBList<IMixGroupCarrier> m_initiatorMixGroupCarriers = new BBList<IMixGroupCarrier>();

			protected readonly List<DuckingData> m_running = new List<DuckingData>();

			protected readonly List<DuckingData> m_ducked = new List<DuckingData>();

			protected readonly GameDB.SubDB m_subDb;

			protected readonly Dictionary<GameID, float> m_volumes;

			private BBDictionary<int, float> m_state = new BBDictionary<int, float>();

			public BBDictionary<int, float> State
			{
				get
				{
					return m_state;
				}
			}

			public MixGroupDuckingManager(ref BBDictionary<GameID, float> _volumes, GameDB.SubDB _subDb)
			{
				m_volumes = _volumes;
				m_subDb = _subDb;
			}

			public float GetVolume(GameID _eMixingGroups)
			{
				return m_volumes[_eMixingGroups];
			}

			public void SetVolume(GameID _eMixingGroups, float _volume)
			{
				m_volumes[_eMixingGroups] = _volume;
				Instance.UpdateVolumes();
			}

			public virtual void Start(IMixGroupCarrier _carrier)
			{
				if (_carrier == null)
				{
					return;
				}
				GameID mixGroup = _carrier.MixGroup;
				IDuckingInfoListCarrier duckingInfoListCarrier = (IDuckingInfoListCarrier)m_subDb.Data[mixGroup];
				for (int i = 0; i < duckingInfoListCarrier.DuckingInfos.Count; i++)
				{
					AudioMixingGroupDef.DuckingInfo duckingInfo = duckingInfoListCarrier.DuckingInfos[i];
					DuckingData duckingData = Find(duckingInfo.MixingGroup, _carrier, m_running);
					if (duckingData == null)
					{
						Start(duckingInfo.MixingGroup, duckingInfo.VolumeReduceTo, duckingInfo.FadeInTime, _carrier);
					}
				}
				m_initiatorMixGroupCarriers.Add(_carrier);
			}

			public virtual void Stop(IMixGroupCarrier _carrier)
			{
				if (_carrier == null)
				{
					return;
				}
				GameID mixGroup = _carrier.MixGroup;
				IDuckingInfoListCarrier duckingInfoListCarrier = (IDuckingInfoListCarrier)m_subDb.Data[mixGroup];
				for (int i = 0; i < duckingInfoListCarrier.DuckingInfos.Count; i++)
				{
					AudioMixingGroupDef.DuckingInfo duckingInfo = duckingInfoListCarrier.DuckingInfos[i];
					DuckingData duckingData = Find(duckingInfo.MixingGroup, _carrier, m_ducked);
					if (duckingData != null)
					{
						Start(duckingInfo.MixingGroup, 1f, duckingInfo.FadeOutTime, _carrier);
					}
				}
				m_initiatorMixGroupCarriers.Remove(_carrier);
			}

			public void StopAll()
			{
				for (int num = m_initiatorMixGroupCarriers.Count - 1; num >= 0; num--)
				{
					Stop(m_initiatorMixGroupCarriers[num]);
				}
			}

			public virtual void Update()
			{
			}

			protected void Start(GameID _eMixingGroups, float _toVolume, float _time, IMixGroupCarrier _startedBy)
			{
				Stop(_eMixingGroups, _startedBy);
				if (Instance.CoroutineHelper == null)
				{
					return;
				}
				float fadeStayTime = 0f;
				IDuckingInfoListCarrier duckingInfoListCarrier = (IDuckingInfoListCarrier)m_subDb.Data[_startedBy.MixGroup];
				for (int i = 0; i < duckingInfoListCarrier.DuckingInfos.Count; i++)
				{
					if (duckingInfoListCarrier.DuckingInfos[i].MixingGroup == _eMixingGroups)
					{
						fadeStayTime = duckingInfoListCarrier.DuckingInfos[i].FadeStayTime;
					}
				}
				DuckingData duckingData = new DuckingData();
				duckingData.MixingGroups = _eMixingGroups;
				duckingData.ShouldStop = false;
				duckingData.StartedBy = _startedBy;
				duckingData.DirectionReduce = _toVolume < 1f;
				duckingData.FadeStayTime = fadeStayTime;
				DuckingData duckingData2 = duckingData;
				if (duckingData2.DirectionReduce)
				{
					m_ducked.Add(duckingData2);
				}
				else
				{
					m_ducked.Remove(Find(_eMixingGroups, _startedBy, m_ducked));
				}
				duckingData2.Generator = Duck(duckingData2, _toVolume, _time);
				Instance.CoroutineHelper.StartCoroutine(duckingData2.Generator);
			}

			protected void Stop(GameID _eMixingGroups, IMixGroupCarrier _startedBy)
			{
				DuckingData duckingData = Find(_eMixingGroups, _startedBy, m_running);
				if (duckingData != null)
				{
					duckingData.ShouldStop = true;
					duckingData.Generator.MoveNext();
				}
			}

			protected IEnumerator SimulateStoppedSound(DuckingData _data, float _time)
			{
				if (_time >= 0f)
				{
					yield return new WaitForSeconds(_time);
					Stop(_data.StartedBy);
				}
			}

			protected DuckingData Find(GameID _ducking, IMixGroupCarrier _starter, List<DuckingData> _list)
			{
				for (int i = 0; i < _list.Count; i++)
				{
					DuckingData duckingData = _list[i];
					if (duckingData.MixingGroups == _ducking && duckingData.StartedBy == _starter)
					{
						return duckingData;
					}
				}
				return null;
			}

			private IEnumerator Duck(DuckingData _data, float _toVolume, float _time)
			{
				if (!m_volumes.ContainsKey(_data.MixingGroups))
				{
					yield break;
				}
				m_running.Add(_data);
				m_state[_data.MixingGroups.Guid] = (_data.DirectionReduce ? 0f : 1f);
				bool directionReduce = _toVolume < 1f;
				float startVolume = m_volumes[_data.MixingGroups];
				float lerp = 0f;
				while (lerp < 1f && !_data.ShouldStop)
				{
					Profiling.Start("DuckingSystemManager::Duck() loop", float.MaxValue);
					lerp += Time.fixedDeltaTime / _time;
					_data.Volume = Mathf.Lerp(startVolume, _toVolume, lerp);
					m_state[_data.MixingGroups.Guid] = ((!directionReduce) ? (1f - lerp) : lerp);
					if ((!directionReduce || !(m_volumes[_data.MixingGroups] <= _data.Volume)) && (directionReduce || RunningCount(_data.MixingGroups, true, _data.StartedBy) <= 0) && (directionReduce || IsLowestVolume(_data.MixingGroups, false, _data.StartedBy, _data.Volume, m_running)) && (directionReduce || IsLowestVolume(_data.MixingGroups, true, _data.StartedBy, _data.Volume, m_ducked)))
					{
						m_volumes[_data.MixingGroups] = _data.Volume;
						Instance.UpdateVolumes();
					}
					Profiling.Stop();
					yield return new WaitForEndOfFrame();
				}
				m_running.Remove(_data);
				m_state[_data.MixingGroups.Guid] = ((!_data.DirectionReduce) ? 0f : 1f);
				if (directionReduce && Instance.CoroutineHelper != null)
				{
					Instance.CoroutineHelper.StartCoroutine(SimulateStoppedSound(_data, _data.FadeStayTime));
				}
			}

			private int RunningCount(GameID _ducking, bool _directionReduce, IMixGroupCarrier _excludeStarter)
			{
				int num = 0;
				for (int i = 0; i < m_running.Count; i++)
				{
					DuckingData duckingData = m_running[i];
					if (duckingData.MixingGroups == _ducking && duckingData.DirectionReduce == _directionReduce && duckingData.StartedBy != _excludeStarter)
					{
						num++;
					}
				}
				return num;
			}

			private bool IsLowestVolume(GameID _ducking, bool _directionReduce, IMixGroupCarrier _excludeStarter, float _volume, List<DuckingData> _list)
			{
				for (int i = 0; i < _list.Count; i++)
				{
					DuckingData duckingData = _list[i];
					if (duckingData.MixingGroups == _ducking && duckingData.DirectionReduce == _directionReduce && duckingData.StartedBy != _excludeStarter && duckingData.Volume < _volume)
					{
						return false;
					}
				}
				return true;
			}
		}

		private static AudioGameDbInitializer m_dbInitializer;

		private AudioManagerInspector m_audioManagerInspector;

		private AudioManagerCoroutineHelper m_coroutineHelper;

		private MixGroupDuckingManager m_mixStateManager;

		private DuckingSystemManager m_duckingSystem;

		private AudioEventManager m_audioEventManager;

		private AudioSourceFactory m_audioSourceFactory;

		private List<float> m_channelVolumes = new List<float>();

		private BBDictionary<GameID, float> m_mixingGroupDuckingVolumes = new BBDictionary<GameID, float>();

		private BBDictionary<GameID, float> m_mixingGroupVolumes = new BBDictionary<GameID, float>();

		private BBDictionary<GameID, float> m_mixingGroupDefaultVolumes = new BBDictionary<GameID, float>();

		private List<AudioController> m_sharedAudioControllers = new List<AudioController>();

		private List<PlayingInfo> m_playing = new List<PlayingInfo>();

		private float m_volume;

		private bool m_isDisabled;

		private Timer.EPauseState m_pauseReason;

		private SlowMotionHandler m_slowMotionHandler;

		private ChannelHandler m_channelHandler;

		private readonly Type[] OnVolumeChangedDelegate = new Type[1] { typeof(float) };

		private readonly Type[] OnChannelVolumeChangedDelegate = new Type[2]
		{
			typeof(ELayer),
			typeof(float)
		};

		public SimpleDelegate OnVolumeChanged;

		public SimpleDelegate OnChannelVolumeChanged;

		private long m_minDelayForSameRawSound = 300L;

		private bool m_themeSongOn;

		public static AudioManager Instance { get; private set; }

		public static bool IsAvailable
		{
			get
			{
				return Instance != null;
			}
		}

		public static GameDB GameDb
		{
			get
			{
				return DbInitializer.GameDb;
			}
		}

		public static GameDB.SubDB AudioControllerSubDb
		{
			get
			{
				return DbInitializer.AudioControllerDb;
			}
		}

		public static ESoundTheme ActiveSoundTheme { get; set; }

		public bool Disabled
		{
			get
			{
				return m_isDisabled;
			}
			set
			{
				m_isDisabled = value;
				for (int num = m_playing.Count - 1; num >= 0; num--)
				{
					PlayingInfo playingInfo = m_playing[num];
					Stop(playingInfo.AudioHandle);
				}
				m_sharedAudioControllers.Clear();
				AudioResourceManager.Instance.Disabled = m_isDisabled;
			}
		}

		public AudioManagerCoroutineHelper CoroutineHelper
		{
			get
			{
				if (m_coroutineHelper == null)
				{
				}
				return m_coroutineHelper;
			}
		}

		public DuckingSystemManager DuckingSystem
		{
			get
			{
				return m_duckingSystem;
			}
		}

		public MixGroupDuckingManager MixStateManager
		{
			get
			{
				return m_mixStateManager;
			}
		}

		public AudioEventManager EventManager
		{
			get
			{
				return m_audioEventManager;
			}
		}

		public float Volume
		{
			get
			{
				return m_volume;
			}
			set
			{
				if (m_volume != value)
				{
					m_volume = value;
					VolumeChanged();
					OnVolumeChanged.Trigger(m_volume);
				}
			}
		}

		public bool ThemeSongOn
		{
			get
			{
				return m_themeSongOn;
			}
			set
			{
				m_themeSongOn = value;
				EventManager.ThemeSongOn = m_themeSongOn && ThemeSongOnFinal;
			}
		}

		private bool ThemeSongOnFinal
		{
			get
			{
				return GetVolume(ELayer.Music) > 0.5f;
			}
		}

		private static AudioGameDbInitializer DbInitializer
		{
			get
			{
				AudioGameDbInitializer obj = m_dbInitializer ?? new AudioGameDbInitializer(Main.SafeGameDb);
				m_dbInitializer = obj;
				return obj;
			}
		}

		public bool IsIngame { get; private set; }

		private AudioManager()
		{
			OnVolumeChanged = new SimpleDelegate(this, OnVolumeChangedDelegate);
			OnChannelVolumeChanged = new SimpleDelegate(this, OnChannelVolumeChangedDelegate);
			m_volume = 1f;
			m_themeSongOn = true;
			IEnumerator enumerator = Enum.GetValues(typeof(ELayer)).GetEnumerator();
			while (enumerator.MoveNext())
			{
				m_channelVolumes.Add(1f);
			}
			foreach (KeyValuePair<GameID, DBEntry> datum in DbInitializer.MixGroupDb.Data)
			{
				AudioMixingGroupDef audioMixingGroupDef = (AudioMixingGroupDef)datum.Value;
				m_mixingGroupVolumes.Add(audioMixingGroupDef.GameID, 1f);
				m_mixingGroupDuckingVolumes.Add(audioMixingGroupDef.GameID, 1f);
				m_mixingGroupDefaultVolumes.Add(audioMixingGroupDef.GameID, audioMixingGroupDef.Volume);
			}
			Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
			instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Combine(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnPause));
		}

		public static void Init()
		{
			if (Instance == null)
			{
				Instance = new AudioManager();
				Instance.PrivateInit();
			}
		}

		public void SetAudioListenerParent(Transform _parent)
		{
			if (m_audioManagerInspector == null)
			{
				return;
			}
			if (_parent == null)
			{
				if (m_audioManagerInspector.transform != null)
				{
					SetAudioListenerParent(m_audioManagerInspector.transform);
				}
				return;
			}
			m_audioManagerInspector.AudioListener.transform.parent = _parent;
			m_audioManagerInspector.AudioListener.transform.localPosition = Vector3.zero;
			m_audioManagerInspector.AudioListener.transform.localRotation = Quaternion.identity;
			m_audioManagerInspector.Channels.transform.parent = _parent;
			m_audioManagerInspector.Channels.transform.localPosition = Vector3.zero;
			m_audioManagerInspector.Channels.transform.localRotation = Quaternion.identity;
			AudioSource[] componentsInChildren = m_audioManagerInspector.Channels.GetComponentsInChildren<AudioSource>();
			foreach (AudioSource audioSource in componentsInChildren)
			{
				audioSource.transform.localPosition = Vector3.zero;
				audioSource.transform.localRotation = Quaternion.identity;
			}
		}

		public void ResetAudioListenerParent()
		{
			if (m_audioManagerInspector != null)
			{
				SetAudioListenerParent(m_audioManagerInspector.transform);
			}
		}

		public void Load(GameID _eController)
		{
			if (CoroutineHelper != null)
			{
				AudioControllerDef audioControllerDef = (AudioControllerDef)AudioControllerSubDb.Data[_eController];
				AudioControllerDef.SoundTheme activeTheme = audioControllerDef.GetActiveTheme();
				for (int i = 0; i < activeTheme.SoundProperties.Count; i++)
				{
					CoroutineHelper.StartCoroutine(AudioResourceManager.Instance.Load(new SRawSoundLoadDef(activeTheme.SoundProperties[i].GameID)));
				}
			}
		}

		public void Unload(GameID _eController)
		{
			AudioControllerDef audioControllerDef = (AudioControllerDef)AudioControllerSubDb.Data[_eController];
			AudioControllerDef.SoundTheme activeTheme = audioControllerDef.GetActiveTheme();
			for (int i = 0; i < activeTheme.SoundProperties.Count; i++)
			{
				AudioResourceManager.Instance.Unload(new SRawSoundLoadDef(activeTheme.SoundProperties[i].GameID));
			}
		}

		public void Play(GameID _eController)
		{
			IAudioHandle audioHandle = CreateHandle(_eController, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
			if (audioHandle != null)
			{
				audioHandle.Load();
				audioHandle.Play();
			}
		}

		private IAudioPlayBehaviour CreateOrGetSharedPlayBehaviour(AudioController _controller)
		{
			for (int i = 0; i < m_sharedAudioControllers.Count; i++)
			{
				AudioController audioController = m_sharedAudioControllers[i];
				if (audioController.Id == _controller.Id)
				{
					return audioController.AudioPlayBehaviour;
				}
			}
			m_sharedAudioControllers.Add(_controller);
			return _controller.AudioPlayBehaviour;
		}

		public IAudioLoadHandle CreateLoadHandle(GameID _eController)
		{
			return CreateHandle(_eController, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
		}

		public IAudioPlayHandle CreatePlayHandle(GameID _eController)
		{
			return CreateHandle(_eController, ELoadStrategy.LoadNone, EStoppedFinishedStrategy.UnloadNone);
		}

		public IAudioHandle CreateHandle(GameID _eController, ELoadStrategy _loadStrategy, EStoppedFinishedStrategy _stoppedFinishedStrategy)
		{
			if (!AudioControllerSubDb.Data.ContainsKey(_eController))
			{
				return null;
			}
			AudioController controller = new AudioController((AudioControllerDef)AudioControllerSubDb.Data[_eController]);
			return CreateHandle(controller, _loadStrategy, _stoppedFinishedStrategy);
		}

		public IAudioHandle CreateHandle(AudioController _controller, ELoadStrategy _loadStrategy, EStoppedFinishedStrategy _stoppedFinishedStrategy)
		{
			ILoadStrategy loadStrategy = null;
			switch (_loadStrategy)
			{
			case ELoadStrategy.LoadAll:
				loadStrategy = new LoadStrategyAll(_controller.RawSounds);
				break;
			case ELoadStrategy.LoadNext:
				loadStrategy = new LoadStrategyNext(_controller.RawSounds, _controller.AudioPlayBehaviour);
				break;
			case ELoadStrategy.LoadNone:
				loadStrategy = new LoadStrategyNone(_controller.RawSounds);
				break;
			}
			IStoppedFinishedStrategy stoppedFinishedStrategy = null;
			switch (_stoppedFinishedStrategy)
			{
			case EStoppedFinishedStrategy.UnloadAll:
				stoppedFinishedStrategy = new StoppedFinishedStrategyAll((ILoadStrategyUnloadAll)loadStrategy);
				break;
			case EStoppedFinishedStrategy.UnloadCurrent:
				stoppedFinishedStrategy = new StoppedFinishedStrategyCurrent(loadStrategy);
				break;
			case EStoppedFinishedStrategy.UnloadNone:
				stoppedFinishedStrategy = new StoppedFinishedStrategyNone();
				break;
			}
			return new AudioHandle(_controller, loadStrategy, stoppedFinishedStrategy);
		}

		public void SetVolume(ELayer _eChannel, float _volume)
		{
			m_channelVolumes[(int)_eChannel] = _volume;
			VolumeChanged();
			OnChannelVolumeChanged.Trigger(_eChannel, _volume);
		}

		public float GetVolume(ELayer _eChannel)
		{
			return m_channelVolumes[(int)_eChannel];
		}

		public void Update()
		{
			if (m_pauseReason == Timer.EPauseState.Running)
			{
				m_slowMotionHandler.Update();
			}
		}

		public void OnResourceUnloaded(SRawSoundLoadDef _soundLoadDef)
		{
			for (int num = m_playing.Count - 1; num >= 0; num--)
			{
				if (m_playing[num].RawSoundLoadDef == _soundLoadDef)
				{
					Stop(m_playing[num].AudioHandle);
				}
			}
		}

		private void PrivateInit()
		{
			m_audioManagerInspector = UnityEngine.Object.FindObjectOfType<AudioManagerInspector>();
			if (m_audioManagerInspector == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("Audio/AudioManager"));
				if (!(gameObject != null))
				{
					return;
				}
				gameObject.transform.name = "AudioManager";
				m_audioManagerInspector = gameObject.GetComponent<AudioManagerInspector>();
			}
			UnityEngine.Object.DontDestroyOnLoad(m_audioManagerInspector.gameObject);
			m_coroutineHelper = m_audioManagerInspector.GetComponent<AudioManagerCoroutineHelper>();
			m_coroutineHelper.StartCoroutine(UpdatePlaying());
			m_audioEventManager = new AudioEventManager();
			m_audioEventManager.Ingame += OnIngame;
			m_mixStateManager = new MixGroupDuckingManager(ref m_mixingGroupVolumes, DbInitializer.MixStateDb);
			m_duckingSystem = new DuckingSystemManager(ref m_mixingGroupDuckingVolumes, DbInitializer.MixGroupDb);
			m_audioSourceFactory = new AudioSourceFactory(m_audioManagerInspector.AudioSourceFactory.transform);
			m_slowMotionHandler = new SlowMotionHandler(m_playing.AsReadOnly(), m_audioManagerInspector);
			m_channelHandler = new ChannelHandler(m_playing.AsReadOnly(), m_audioManagerInspector);
		}

		private void OnIngame(bool _active)
		{
			IsIngame = _active;
			UpdateVolumes();
		}

		private void OnPause(Timer.EPauseState _pauseReason)
		{
			m_pauseReason = _pauseReason;
		}

		private PlayResult Play(AudioHandle _handle, bool _loop = false, AudioSource _audioSource = null)
		{
			if (Disabled)
			{
				return false;
			}
			if (_handle.LoadStrategy.ELoadStrategy != ELoadStrategy.LoadNone && _handle.LoadStrategy.LoadedSoundsCount <= 0)
			{
				return false;
			}
			IConstFuture future = _handle.AudioController.AudioPlayBehaviour.Take;
			if (future.SoundLoadDef.IsInvalid)
			{
				_handle.OnFinished(future.SoundLoadDef);
				return false;
			}
			if (!AudioResourceManager.Instance.IsReadyToPlay(future.SoundLoadDef))
			{
				if (!AudioResourceManager.Instance.IsLoading(future.SoundLoadDef))
				{
					return false;
				}
				if (_handle.ShouldPlayOnLoadReady)
				{
					if (m_channelHandler.IsChannelAvailable(_handle.AudioController.Layer, _handle.AudioController.Priority))
					{
						_handle.SchedulePlayOnLoadReady();
						_handle.AudioController.AudioPlayBehaviour.GiveBack = future;
						return PlayResult.SuccessPlayOnReadyScheduled;
					}
					return PlayResult.FailedNoAvailableChannels;
				}
				return PlayResult.FailedNotReadyToPlayYet;
			}
			if (m_playing.FindIndex((PlayingInfo _x) => IsPlaying(_x, future)) >= 0)
			{
				return PlayResult.FailedOmittedAlreadyPlaying;
			}
			AudioSource audioSource = m_channelHandler.GetChannel(_handle.AudioController.Layer, _handle.AudioController.Priority, _audioSource);
			if (audioSource != null && audioSource == _audioSource)
			{
				audioSource = m_audioSourceFactory.GetFromTemplate(_audioSource);
			}
			RawSoundProperty propertyFromSound = _handle.AudioController.GetPropertyFromSound(future.SoundLoadDef);
			if (audioSource != null)
			{
				audioSource.loop = _loop;
				audioSource.clip = AudioResourceManager.Instance.GetClip(future.SoundLoadDef);
				_handle.Pitch = propertyFromSound.Pitch;
				audioSource.pitch = m_slowMotionHandler.GetPitch(_handle);
				audioSource.priority = _handle.AudioController.Priority;
				audioSource.spatialBlend = ((!_handle.AudioController.Is2DSound) ? 1f : 0f);
				PlayingInfo playingInfo = new PlayingInfo();
				playingInfo.AudioHandle = _handle;
				playingInfo.AudioSource = audioSource;
				playingInfo.RawSoundLoadDef = future.SoundLoadDef;
				playingInfo.StartTimeStamp = DateTime.Now.Ticks / 10000;
				PlayingInfo playingInfo2 = playingInfo;
				m_playing.Add(playingInfo2);
				float fadeInSeconds = propertyFromSound.FadeInSeconds;
				float fadeOutSeconds = propertyFromSound.FadeOutSeconds;
				UpdateVolume(_handle);
				audioSource.Play();
				DuckingSystem.Start(playingInfo2);
				if (fadeInSeconds > 0f)
				{
					_handle.VolumeFader.Start(1f, fadeInSeconds);
				}
				if (fadeOutSeconds > 0f)
				{
					IEnumerator routine = (_handle.FadeOutHandler = DelayedFadeOut(_handle.VolumeFader, audioSource.clip.length - fadeOutSeconds, fadeOutSeconds));
					CoroutineHelper.StartCoroutine(routine);
				}
				return true;
			}
			return PlayResult.FailedNoAvailableChannels;
		}

		private IEnumerator DelayedFadeOut(IVolumeFader _volumeFader, float _delay, float _fadeOutSeconds)
		{
			yield return new WaitForSeconds(_delay);
			_volumeFader.Start(0f, _fadeOutSeconds);
		}

		private bool IsPlaying(PlayingInfo _playingInfo, IConstFuture _future)
		{
			bool flag = _playingInfo.RawSoundLoadDef == _future.SoundLoadDef;
			long num = DateTime.Now.Ticks / 10000 - _playingInfo.StartTimeStamp;
			bool flag2 = num < m_minDelayForSameRawSound;
			return flag && flag2;
		}

		private IEnumerator UpdatePlaying()
		{
			while (true)
			{
				for (int i = m_playing.Count - 1; i >= 0; i--)
				{
					PlayingInfo playing = m_playing[i];
					if (playing.AudioSource != null)
					{
						if (!playing.AudioSource.isPlaying && !playing.AudioSource.loop)
						{
							RemovePlaying(playing);
							playing.AudioHandle.OnFinished(playing.RawSoundLoadDef);
							Instance.DuckingSystem.Stop(playing);
						}
					}
					else
					{
						RemovePlaying(playing);
					}
				}
				yield return new WaitForEndOfFrame();
			}
		}

		private void Stop(AudioHandle _handle)
		{
			if (_handle == null || !_handle.IsPlaying)
			{
				return;
			}
			PlayingInfo playingInfo = null;
			for (int i = 0; i < m_playing.Count; i++)
			{
				if (m_playing[i].AudioHandle == _handle)
				{
					playingInfo = m_playing[i];
					break;
				}
			}
			if (playingInfo != null)
			{
				if (playingInfo.AudioSource != null)
				{
					playingInfo.AudioSource.Stop();
				}
				RemovePlaying(playingInfo);
				playingInfo.AudioHandle.OnStopped(playingInfo.RawSoundLoadDef);
				Instance.DuckingSystem.Stop(playingInfo);
			}
		}

		private float PlaybackTime(IAudioHandle _handle)
		{
			for (int i = 0; i < m_playing.Count; i++)
			{
				if (m_playing[i].AudioHandle == _handle)
				{
					return m_playing[i].AudioSource.time;
				}
			}
			return -1f;
		}

		private float PlaybackLength(IAudioHandle _handle)
		{
			for (int i = 0; i < m_playing.Count; i++)
			{
				if (m_playing[i].AudioHandle == _handle && m_playing[i].AudioSource.clip != null)
				{
					return m_playing[i].AudioSource.clip.length;
				}
			}
			return -1f;
		}

		private bool DetachChannelFromParent(IAudioHandle _handle)
		{
			for (int i = 0; i < m_playing.Count; i++)
			{
				if (m_playing[i].AudioHandle == _handle)
				{
					PlayingInfo playingInfo = m_playing[i];
					m_audioSourceFactory.DetachFromParent(playingInfo.AudioSource);
				}
			}
			return false;
		}

		private void VolumeChanged()
		{
			for (int i = 0; i < m_playing.Count; i++)
			{
				UpdateVolume(m_playing[i].AudioHandle);
			}
		}

		private void UpdateVolumes()
		{
			for (int i = 0; i < m_playing.Count; i++)
			{
				UpdateVolume(m_playing[i].AudioHandle);
			}
		}

		private void UpdateVolume(AudioHandle _handle)
		{
			if (_handle == null)
			{
				return;
			}
			PlayingInfo playingInfo = null;
			for (int i = 0; i < m_playing.Count; i++)
			{
				if (m_playing[i].AudioHandle == _handle)
				{
					playingInfo = m_playing[i];
					break;
				}
			}
			if (playingInfo != null)
			{
				if (playingInfo.AudioSource == null)
				{
					RemovePlaying(playingInfo);
					return;
				}
				RawSoundProperty propertyFromSound = _handle.AudioController.GetPropertyFromSound(playingInfo.RawSoundLoadDef);
				float volume = propertyFromSound.Volume;
				float channelVolume = GetChannelVolume(_handle.AudioController.Layer);
				float num = m_mixingGroupDefaultVolumes[_handle.AudioController.MixingGroups];
				float num2 = m_mixingGroupVolumes[_handle.AudioController.MixingGroups];
				float num3 = m_mixingGroupDuckingVolumes[_handle.AudioController.MixingGroups];
				float volume2 = volume * _handle.Volume * channelVolume * num * num2 * num3 * m_volume;
				playingInfo.AudioSource.volume = volume2;
			}
		}

		private float GetChannelVolume(ELayer _layer)
		{
			return (!IsIngame || _layer != ELayer.UI) ? m_channelVolumes[(int)_layer] : 1f;
		}

		private void RemovePlaying(PlayingInfo _playing)
		{
			if (_playing != null)
			{
				if (_playing.AudioHandle.FadeOutHandler != null)
				{
					CoroutineHelper.StopCoroutine(_playing.AudioHandle.FadeOutHandler);
					_playing.AudioHandle.FadeOutHandler = null;
				}
				m_channelHandler.Free(_playing.AudioHandle.AudioController.Layer, _playing.AudioSource);
				m_audioSourceFactory.GiveBack(_playing.AudioSource);
				if (_playing.AudioSource != null)
				{
					_playing.AudioSource.clip = null;
				}
				m_playing.Remove(_playing);
			}
		}
	}
}
