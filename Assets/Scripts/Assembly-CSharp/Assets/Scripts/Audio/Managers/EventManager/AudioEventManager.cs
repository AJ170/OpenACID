using System;
using System.Collections;
using AcierProtocol;
using Assets.Scripts.Audio.Behaviours;
using Assets.Scripts.Crowd;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Network.Missions.Mission.Data;
using Assets.Scripts.UI.Tutorial;
using Database;
using Mission;
using SharedBaseLib.Core;
using UbiCore.Messenger;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Audio.Managers.EventManager
{
	public class AudioEventManager
	{
		private class AmbientSfxHandler : IAudioHandler
		{
			private const int c_MaxFires = 2;

			private const int c_MaxWaterSender = 1;

			private IAudioHandle[] m_bonFireSounds = new IAudioHandle[2];

			private IAudioHandle[] m_waterEmitterSounds = new IAudioHandle[1];

			private BonFireSender[] m_bonFires;

			private readonly BonFireSoundEmitter[] m_bonFireCurrent = new BonFireSoundEmitter[2];

			private readonly WaterPlaneSoundEmitter[] m_waterEmitterCurrent = new WaterPlaneSoundEmitter[1];

			private WaterPlaneSoundSender[] m_waterPlaneSender;

			public void OnEnterIngame()
			{
				CollectBonfires();
				CollectWaterEmitters();
				LoadBonfireAudioHandles();
				LoadWaterplaneAudioHandles();
			}

			private void CollectBonfires()
			{
				m_bonFires = UnityEngine.Object.FindObjectsOfType<BonFireSender>();
			}

			private void CollectWaterEmitters()
			{
				m_waterPlaneSender = UnityEngine.Object.FindObjectsOfType<WaterPlaneSoundSender>();
			}

			public void OnExitIngame()
			{
				UnloadBonfireAudioHandles();
				UnloadWaterplaneAudioHandles();
				m_bonFires = null;
			}

			private void LoadBonfireAudioHandles()
			{
				UnloadBonfireAudioHandles();
				if (m_bonFires != null && m_bonFires.Length > 0)
				{
					for (int i = 0; i < m_bonFireCurrent.Length; i++)
					{
						m_bonFireSounds[i] = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_env_fire_01_lp, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
						m_bonFireSounds[i].Load();
					}
					BonFireSender[] bonFires = m_bonFires;
					foreach (BonFireSender bonFireSender in bonFires)
					{
						BonFireSoundEmitter bonFireSoundEmitter = bonFireSender.gameObject.AddMissingComponent<BonFireSoundEmitter>();
						bonFireSoundEmitter.Trigger += BonFireTriggerChanged;
					}
				}
			}

			private void LoadWaterplaneAudioHandles()
			{
				UnloadWaterplaneAudioHandles();
				if (m_bonFires != null && m_waterPlaneSender.Length > 0)
				{
					for (int i = 0; i < m_waterEmitterCurrent.Length; i++)
					{
						m_waterEmitterSounds[i] = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_river_calmlapping_lp, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
						m_waterEmitterSounds[i].Load();
					}
					WaterPlaneSoundSender[] waterPlaneSender = m_waterPlaneSender;
					foreach (WaterPlaneSoundSender waterPlaneSoundSender in waterPlaneSender)
					{
						WaterPlaneSoundEmitter waterPlaneSoundEmitter = waterPlaneSoundSender.gameObject.AddMissingComponent<WaterPlaneSoundEmitter>();
						waterPlaneSoundEmitter.Trigger += WaterPlaneTriggerChanged;
					}
				}
			}

			private int GetWaterPlaneIdx(WaterPlaneSoundEmitter _sender)
			{
				for (int i = 0; i < m_waterEmitterCurrent.Length; i++)
				{
					if (m_waterEmitterCurrent[i] == _sender)
					{
						return i;
					}
				}
				return -1;
			}

			private int GetBonfireIdx(BonFireSoundEmitter _sender)
			{
				for (int i = 0; i < m_bonFireCurrent.Length; i++)
				{
					if (m_bonFireCurrent[i] == _sender)
					{
						return i;
					}
				}
				return -1;
			}

			private int GetLeastAudibleBonfireIdx(BonFireSoundEmitter _sender)
			{
				int result = -1;
				for (int i = 0; i < m_bonFireCurrent.Length; i++)
				{
					if (m_bonFireCurrent[i] == null)
					{
						return i;
					}
					if (m_bonFireCurrent[i].Distance > _sender.Distance)
					{
						result = 0;
					}
				}
				return result;
			}

			private int GetLeastAudibleWaterPlaneIdx(WaterPlaneSoundEmitter _sender)
			{
				int result = -1;
				for (int i = 0; i < m_waterEmitterCurrent.Length; i++)
				{
					if (m_waterEmitterCurrent[i] == null)
					{
						return i;
					}
					if (m_waterEmitterCurrent[i].Distance > _sender.Distance)
					{
						result = 0;
					}
				}
				return result;
			}

			private bool BonFireTriggerChanged(BonFireSoundEmitter _sender, bool _activated)
			{
				if (!_activated)
				{
					int bonfireIdx = GetBonfireIdx(_sender);
					if (bonfireIdx != -1)
					{
						StopCurrentBonfireSound(bonfireIdx);
					}
					return true;
				}
				int leastAudibleBonfireIdx = GetLeastAudibleBonfireIdx(_sender);
				if (leastAudibleBonfireIdx >= 0)
				{
					StopCurrentBonfireSound(leastAudibleBonfireIdx);
					m_bonFireSounds[leastAudibleBonfireIdx].Play(true, _sender.AudioSource);
					m_bonFireCurrent[leastAudibleBonfireIdx] = _sender;
					return true;
				}
				return false;
			}

			private bool WaterPlaneTriggerChanged(WaterPlaneSoundEmitter _sender, bool _activated)
			{
				if (!_activated)
				{
					int waterPlaneIdx = GetWaterPlaneIdx(_sender);
					if (waterPlaneIdx != -1)
					{
						StopCurrentWaterPlaneSound(waterPlaneIdx);
					}
					return true;
				}
				int leastAudibleWaterPlaneIdx = GetLeastAudibleWaterPlaneIdx(_sender);
				if (leastAudibleWaterPlaneIdx >= 0)
				{
					StopCurrentWaterPlaneSound(leastAudibleWaterPlaneIdx);
					m_waterEmitterSounds[leastAudibleWaterPlaneIdx].Play(true, _sender.AudioSource);
					m_waterEmitterCurrent[leastAudibleWaterPlaneIdx] = _sender;
					return true;
				}
				return false;
			}

			private void StopCurrentBonfireSound(int _idx)
			{
				m_bonFireSounds[_idx].Stop();
				m_bonFireCurrent[_idx] = null;
			}

			private void StopCurrentWaterPlaneSound(int _idx)
			{
				m_waterEmitterSounds[_idx].Stop();
				m_waterEmitterCurrent[_idx] = null;
			}

			private void UnloadBonfireAudioHandles()
			{
				if (m_bonFires == null)
				{
					return;
				}
				for (int i = 0; i < m_bonFires.Length; i++)
				{
					BonFireSender bonFireSender = m_bonFires[i];
					BonFireSoundEmitter component = bonFireSender.gameObject.GetComponent<BonFireSoundEmitter>();
					if (component != null)
					{
						component.Trigger -= BonFireTriggerChanged;
					}
				}
				for (int j = 0; j < m_bonFireSounds.Length; j++)
				{
					if (m_bonFireSounds[j] != null)
					{
						m_bonFireSounds[j].Unload();
					}
				}
			}

			private void UnloadWaterplaneAudioHandles()
			{
				if (m_bonFires == null)
				{
					return;
				}
				for (int i = 0; i < m_waterPlaneSender.Length; i++)
				{
					WaterPlaneSoundSender waterPlaneSoundSender = m_waterPlaneSender[i];
					WaterPlaneSoundEmitter component = waterPlaneSoundSender.gameObject.GetComponent<WaterPlaneSoundEmitter>();
					if (component != null)
					{
						component.Trigger -= WaterPlaneTriggerChanged;
					}
				}
				for (int j = 0; j < m_waterEmitterSounds.Length; j++)
				{
					if (m_waterEmitterSounds[j] != null)
					{
						m_waterEmitterSounds[j].Unload();
					}
				}
			}
		}

		private class AnimusHandler
		{
			private abstract class MusicFrame
			{
				private bool m_isActive = true;

				protected bool IsActive
				{
					get
					{
						return m_isActive;
					}
					private set
					{
						m_isActive = value;
					}
				}

				public abstract IEnumerator Update();

				public virtual void Deactivate()
				{
					IsActive = false;
				}
			}

			private class WaitForFadesFrame : MusicFrame
			{
				public override IEnumerator Update()
				{
					while (((m_hMusicTheme != null && m_hMusicTheme.IsPlaying) || (m_hAnimusDrone != null && m_hAnimusDrone.IsPlaying)) && base.IsActive)
					{
						yield return 0;
					}
				}
			}

			private class DroneMusicFrame : MusicFrame
			{
				public override IEnumerator Update()
				{
					StartAnimusDroneSoundNow(true);
					while (base.IsActive)
					{
						yield return 0;
					}
				}

				public override void Deactivate()
				{
					base.Deactivate();
					FadeOutAnimusDrone();
				}
			}

			private class EnterAnimusMusicFrame : MusicFrame
			{
				public override IEnumerator Update()
				{
					yield return new WaitForSeconds(1.3f);
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_animus_transition);
				}
			}

			private class MainThemeFrame : MusicFrame
			{
				public override IEnumerator Update()
				{
					StartAnimusMusicNow();
					while (!m_hMusicTheme.IsPlaying)
					{
						yield return 0;
					}
					float droneStartDelay = Mathf.Max(0f, m_hMusicTheme.PlaybackLength - m_hMusicTheme.PlaybackTime - 11f);
					DateTime startTime = DateTime.Now;
					while ((DateTime.Now - startTime).TotalSeconds < (double)droneStartDelay && base.IsActive)
					{
						yield return 0;
					}
				}

				public override void Deactivate()
				{
					base.Deactivate();
					FadeOutAnimusMusic();
				}
			}

			private enum EScreenState
			{
				None = 0,
				Animus = 1,
				Credits = 2
			}

			private const float c_AnimusDroneStartDelayFromEnd = 11f;

			private BBQueue<MusicFrame> m_musicPlaybackQueue = new BBQueue<MusicFrame>();

			private IAudioHandle m_denySound;

			private static IAudioHandle m_hMusicTheme;

			private static IAudioHandle m_hAnimusDrone;

			private EScreenState m_screenState;

			private bool m_isFirstStart = true;

			private MixStateHandler m_mixStateHandler;

			private bool m_isJustEntered;

			private bool m_isThemeOn;

			private bool m_isFirstContact;

			private IEnumerator m_coroutineMainLoop;

			public bool IsThemeOn
			{
				get
				{
					return m_isThemeOn;
				}
				set
				{
					m_isThemeOn = value;
					if (!IsThemeOn && m_musicPlaybackQueue.Count > 0 && m_musicPlaybackQueue.Peek() is MainThemeFrame)
					{
						ClearMusicQueue();
					}
				}
			}

			public AnimusHandler(MixStateHandler _mixStateHandler)
			{
				m_mixStateHandler = _mixStateHandler;
				IsThemeOn = true;
				LoadAudioHandles();
			}

			public void Begin()
			{
				m_screenState = EScreenState.Animus;
				m_isJustEntered = true;
				m_denySound.Load();
				m_coroutineMainLoop = CoroutineMainLoop();
				AudioManager.Instance.CoroutineHelper.StartCoroutine(m_coroutineMainLoop);
				TutorialFlowBase.BlockerTapped += TutorialBlockerTapped;
				if (AdColonyManager.IsAvailable)
				{
					AdColonyManager.Instance.AddOnVideoStartedMethod(OnAdColonyStart);
					AdColonyManager.Instance.AddOnVideoFinishedMethod(OnAdColonyEnd);
				}
			}

			private IEnumerator CoroutineMainLoop()
			{
				while (true)
				{
					m_musicPlaybackQueue.Clear();
					if (!m_isFirstContact)
					{
						m_musicPlaybackQueue.Enqueue(new WaitForFadesFrame());
					}
					if (m_isJustEntered && !m_isFirstContact)
					{
						m_musicPlaybackQueue.Enqueue(new EnterAnimusMusicFrame());
						m_isJustEntered = false;
					}
					if ((IsThemeOn && m_isFirstStart) || m_screenState == EScreenState.Credits)
					{
						m_musicPlaybackQueue.Enqueue(new MainThemeFrame());
						m_isFirstStart = false;
					}
					m_musicPlaybackQueue.Enqueue(new DroneMusicFrame());
					while (m_musicPlaybackQueue.Count > 0)
					{
						lock (m_musicPlaybackQueue)
						{
							yield return AudioManager.Instance.CoroutineHelper.StartCoroutine(m_musicPlaybackQueue.Peek().Update());
							if (m_musicPlaybackQueue.Count > 0)
							{
								m_musicPlaybackQueue.Dequeue();
							}
						}
						yield return 0;
					}
					yield return 0;
				}
			}

			private static void StartAnimusMusicNow(bool _loop = false, float _fadeDuration = 2f)
			{
				FadeOutAnimusDrone();
				if (!m_hMusicTheme.IsPlaying)
				{
					m_hMusicTheme.Load();
					m_hMusicTheme.Volume = 0f;
					m_hMusicTheme.Play(_loop);
					m_hMusicTheme.VolumeFader.Start(1f, _fadeDuration);
				}
			}

			private void TutorialBlockerTapped()
			{
				m_denySound.Play();
			}

			public void End()
			{
				m_screenState = EScreenState.None;
				m_isFirstContact = false;
				ClearMusicQueue();
				AudioManager.Instance.CoroutineHelper.StopCoroutine(m_coroutineMainLoop);
				UnloadAudioHandles();
				TutorialFlowBase.BlockerTapped -= TutorialBlockerTapped;
				if (AdColonyManager.IsAvailable)
				{
					AdColonyManager.Instance.RemoveOnVideoStartedMethod(OnAdColonyStart);
					AdColonyManager.Instance.RemoveOnVideoFinishedMethod(OnAdColonyEnd);
				}
			}

			private static void FadeOutAnimusMusic(float _fadeDuration = 2f)
			{
				if (m_hMusicTheme != null && m_hMusicTheme.IsPlaying)
				{
					m_hMusicTheme.VolumeFader.Start(0f, _fadeDuration);
				}
			}

			private static void FadeOutAnimusDrone(float _fadeDuration = 2f)
			{
				if (m_hAnimusDrone != null && m_hAnimusDrone.IsPlaying)
				{
					m_hAnimusDrone.VolumeFader.Start(0f, _fadeDuration);
				}
			}

			public void StartAnimusThemeEarly()
			{
				m_isFirstContact = true;
				StartAnimusMusicNow();
			}

			private void LoadAudioHandles()
			{
				m_hMusicTheme = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_title_01, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				m_denySound = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_deny, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
			}

			private void UnloadAudioHandles()
			{
				if (m_denySound != null)
				{
					m_denySound.Unload();
				}
				if (m_hAnimusDrone != null)
				{
					if (m_hAnimusDrone.IsPlaying)
					{
						m_hAnimusDrone.Stop();
					}
					m_hAnimusDrone.Unload();
				}
				if (m_hMusicTheme != null)
				{
					if (m_hMusicTheme.IsPlaying)
					{
						m_hMusicTheme.Stop();
					}
					m_hMusicTheme.Unload();
				}
			}

			private static void StartAnimusDroneSoundNow(bool _fadeIn, float _fadeDuration = 5f)
			{
				if (m_hAnimusDrone == null || !m_hAnimusDrone.IsPlaying)
				{
					m_hAnimusDrone = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_lp, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
					m_hAnimusDrone.Load();
				}
				if (m_hAnimusDrone.IsPlaying)
				{
					m_hAnimusDrone.Stop();
				}
				m_hAnimusDrone.Volume = ((!_fadeIn) ? 1f : 0f);
				m_hAnimusDrone.Play(true);
				if (_fadeIn)
				{
					m_hAnimusDrone.VolumeFader.Start(1f, _fadeDuration);
				}
			}

			public void OnLeaveCredits()
			{
				m_screenState = EScreenState.Animus;
				ClearMusicQueue();
			}

			public void OnEnterCredits()
			{
				m_screenState = EScreenState.Credits;
				ClearMusicQueue();
			}

			private void ClearMusicQueue()
			{
				if (m_musicPlaybackQueue.Count > 0)
				{
					MusicFrame musicFrame = m_musicPlaybackQueue.Peek();
					musicFrame.Deactivate();
				}
				m_musicPlaybackQueue.Clear();
			}

			private void OnAdColonyStart()
			{
				m_mixStateHandler.OnAdColonyStart();
			}

			private void OnAdColonyEnd(bool _adShown)
			{
				m_mixStateHandler.OnAdColonyEnd();
			}
		}

		private interface IAudioHandler
		{
			void OnEnterIngame();

			void OnExitIngame();
		}

		private class CodexHandler : IAudioHandler
		{
			private IAudioHandle m_codexMusic;

			private readonly MixStateHandler m_mixStateHandler;

			public CodexHandler(MixStateHandler _mixStatehandler)
			{
				m_mixStateHandler = _mixStatehandler;
			}

			public void OnEnterIngame()
			{
				LoadAudioHandles();
			}

			public void OnExitIngame()
			{
				UnloadAudioHandles();
			}

			private void LoadAudioHandles()
			{
				UnloadAudioHandles();
				m_codexMusic = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_vistapoint_highlight, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				m_codexMusic.Load();
			}

			private void UnloadAudioHandles()
			{
				if (m_codexMusic != null)
				{
					m_codexMusic.Unload();
				}
			}

			public void Begin()
			{
				m_mixStateHandler.OnRegionIntroStart();
				m_codexMusic.Play();
			}

			public void End()
			{
				m_mixStateHandler.OnRegionIntroEnd();
			}
		}

		private class CombatHandler
		{
			private readonly MixStateHandler m_mixStateHandler;

			private IAudioHandle m_hCombat;

			private IAudioHandle m_screamSound;

			private IAudioHandle m_threatMusic;

			private bool m_isMuted;

			private bool m_isThreateningOn;

			public bool InCombat { get; private set; }

			public bool IsMuted
			{
				get
				{
					return m_isMuted;
				}
				set
				{
					if (m_isMuted != value)
					{
						m_isMuted = value;
						if (IsMuted && InCombat)
						{
							StopCombatState(false);
						}
						if (!IsMuted && InCombat)
						{
							StartCombatState();
						}
					}
				}
			}

			public CombatHandler(MixStateHandler _mixStateHandler)
			{
				m_mixStateHandler = _mixStateHandler;
			}

			public void Begin()
			{
				InCombat = true;
				if (!IsMuted)
				{
					StartCombatState();
				}
			}

			private void StartCombatState(float _fadeTime = 0.5f)
			{
				m_mixStateHandler.OnEnterCombat();
				if (m_hCombat == null)
				{
					m_hCombat = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_combat_01, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				}
				if (!m_hCombat.IsPlaying)
				{
					m_hCombat.Load();
					m_hCombat.Volume = 0f;
					m_hCombat.Play(true);
				}
				m_hCombat.VolumeFader.Start(1f, _fadeTime);
				StopThreateningMusic();
			}

			private void StopThreateningMusic()
			{
				if (m_threatMusic != null && m_threatMusic.IsPlaying)
				{
					m_threatMusic.VolumeFader.Start(0f, 0.5f);
				}
			}

			public void End(bool _instant = false)
			{
				if (!IsMuted)
				{
					StopCombatState(_instant);
				}
			}

			private void StopCombatState(bool _instant)
			{
				if (!InCombat)
				{
					return;
				}
				InCombat = false;
				m_mixStateHandler.OnExitCombat();
				if (!_instant)
				{
					if (m_hCombat != null && m_hCombat.IsPlaying && AudioManager.Instance.CoroutineHelper != null)
					{
						AudioManager.Instance.CoroutineHelper.StartCoroutine(BetweenCombatWonAndAnonymous());
					}
				}
				else if (m_hCombat != null && m_hCombat.IsPlaying)
				{
					m_hCombat.Stop();
					m_mixStateHandler.OnSlowMotionExit();
				}
			}

			private IEnumerator BetweenCombatWonAndAnonymous()
			{
				float timestamp = Time.time;
				bool playSuccessSound = false;
				float alertedTick = 0f;
				if (UbiCore.Utils.Singleton<UiMain>.IsAvailable && UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface != null)
				{
					if (UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerAlertLevel == EAlertLevel.Exposed)
					{
						m_hCombat.VolumeFader.Start(0.5f, 0.5f);
					}
					while (!InCombat && m_hCombat != null && UbiCore.Utils.Singleton<UiMain>.IsAvailable && UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface != null && UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerAlertLevel != 0)
					{
						if (UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerAlertLevel == EAlertLevel.Alerted)
						{
							alertedTick += Time.deltaTime;
							if (alertedTick >= 2f)
							{
								float timePassed = Time.time - timestamp;
								if (timePassed <= 4f)
								{
									playSuccessSound = true;
								}
								break;
							}
						}
						else
						{
							alertedTick = 0f;
						}
						if (!m_hCombat.VolumeFader.IsRunning)
						{
							m_hCombat.Volume = (float)UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerNotoriety * 0.01f * 0.5f;
						}
						yield return new WaitForEndOfFrame();
					}
				}
				else
				{
					m_hCombat.VolumeFader.Start(0.5f, 0.5f);
					m_mixStateHandler.OnSlowMotionExit();
				}
				if (m_hCombat != null && !InCombat)
				{
					m_hCombat.VolumeFader.Start(0f, 3f);
					if (playSuccessSound)
					{
						IAudioHandle handle = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_generic_battle_complete, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
						handle.Load();
						handle.Play(false, null, 2.5f);
					}
					m_mixStateHandler.OnSlowMotionExit();
				}
			}

			public void OnEnterIngame()
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<CombatUiEvent>(OnCombatEvent);
				m_screamSound = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_player_hitground, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
				m_screamSound.Load();
			}

			private void OnCombatEvent(CombatUiEvent _message)
			{
				if (_message.Type == CombatUiEvent.EType.Hit && (_message.Flags & LogicInterface.ECombatEventModifierFlags.FallDamage) > LogicInterface.ECombatEventModifierFlags.None)
				{
					m_screamSound.Play();
				}
			}

			public void OnExitIngame()
			{
				End(true);
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<CombatUiEvent>(OnCombatEvent);
				m_screamSound.Unload();
				StopThreateningMusic();
			}

			public void OnEscorteeThreatened(bool _escorteeThreatened)
			{
				m_isThreateningOn = _escorteeThreatened;
				if (m_isThreateningOn)
				{
					StartThreadeningMusic();
				}
				else
				{
					StopThreateningMusic();
				}
			}

			private void StartThreadeningMusic()
			{
				if (m_threatMusic == null)
				{
					m_threatMusic = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_escort_underattack, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				}
				if (!m_threatMusic.IsPlaying)
				{
					m_threatMusic.Load();
					m_threatMusic.Volume = 0f;
					m_threatMusic.Play(true);
					m_threatMusic.VolumeFader.Start(1f, 0.5f);
				}
			}
		}

		private class CrowdHandler : IAudioHandler
		{
			private const int c_MinPanicUnitsForSoundFeedback = 2;

			private const int c_MinCoughingUnitsForSoundFeedback = 2;

			private IAudioHandle m_crowdPanicSoundHandle;

			private IAudioHandle m_crowdCoughingSoundHandle;

			private readonly MixStateHandler m_mixStateHandler;

			public CrowdHandler(MixStateHandler _mixStatehandler)
			{
				m_mixStateHandler = _mixStatehandler;
			}

			public void OnEnterIngame()
			{
				LoadAudioHandles();
				Assets.Scripts.Crowd.Crowd.Instance.Statistics.Changed += StatisticsOnChanged;
			}

			private void StatisticsOnChanged(Assets.Scripts.Crowd.Crowd.CrowdStatistics _statistics)
			{
				StartFleeing(_statistics);
				if (_statistics.SmokeBombed >= 2)
				{
					StartCaughing();
				}
			}

			private void StartCaughing()
			{
				if (!m_crowdCoughingSoundHandle.IsPlaying)
				{
					m_crowdCoughingSoundHandle.Play();
				}
			}

			private void StartFleeing(Assets.Scripts.Crowd.Crowd.CrowdStatistics _statistics)
			{
				int num = _statistics.UnitsPanicking + _statistics.UnitsPanickingHeightDiff;
				if (num >= 2 && !m_crowdPanicSoundHandle.IsPlaying)
				{
					m_crowdPanicSoundHandle.Play();
				}
				if (m_crowdPanicSoundHandle.IsPlaying)
				{
					if (_statistics.UnitsPanickingHeightDiff > _statistics.UnitsPanicking || num == 0)
					{
						m_mixStateHandler.OnHeightDifferenceStart();
					}
					else
					{
						m_mixStateHandler.OnHeightDifferenceEnd();
					}
				}
			}

			public void OnExitIngame()
			{
				Assets.Scripts.Crowd.Crowd.Instance.Statistics.Changed -= StatisticsOnChanged;
				UnloadAudioHandles();
			}

			private void LoadAudioHandles()
			{
				UnloadAudioHandles();
				m_crowdPanicSoundHandle = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_walla_panic, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
				m_crowdPanicSoundHandle.Load();
				m_crowdCoughingSoundHandle = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_walla_coughing, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
				m_crowdCoughingSoundHandle.Load();
			}

			private void UnloadAudioHandles()
			{
				if (m_crowdPanicSoundHandle != null)
				{
					m_crowdPanicSoundHandle.Unload();
				}
			}
		}

		private class EnvironmentMoodHandler
		{
			private const float c_RestrictedAreaFadeOutSec = 6f;

			private const float c_RestrictedAreaFadeOutToCombatSec = 0.5f;

			private IAudioHandle m_hEnvironmentMood;

			private bool m_isIngame;

			private readonly BBDictionary<int, BBDictionary<int, GameID>> m_environmentMoods = new BBDictionary<int, BBDictionary<int, GameID>>();

			private IAudioHandle m_restrictedAreaSound;

			private IAudioHandle m_parkourSound;

			private bool m_isInArea;

			public UIMapData.ESoundMood Mood { get; set; }

			public bool InCombat { private get; set; }

			public bool InCodex { private get; set; }

			public bool InIntro { private get; set; }

			public bool IsAlive { private get; set; }

			public EnvironmentMoodHandler()
			{
				AddMoods();
			}

			public void OnEnterIngame(ERegion _region)
			{
				InIntro = true;
				IsAlive = true;
				m_isInArea = false;
				InCombat = false;
				InCodex = false;
				Messenger.Instance.AddListener<InfoEvent>(OnInfoEvent);
				m_isInArea = UbiCore.Utils.Singleton<MissionManager>.Instance.AreaManager.IsInArea(LogicMain.Player.transform.position, EMissionAreaType.Restricted, true);
				m_isIngame = true;
				AudioManager.Instance.CoroutineHelper.StartCoroutine(HandleRestrictedAreaSound());
				CreateAudioHandle(_region);
				if (m_hEnvironmentMood != null && !m_hEnvironmentMood.IsPlaying)
				{
					m_hEnvironmentMood.Load();
					m_hEnvironmentMood.Volume = 0f;
					m_hEnvironmentMood.Play(true);
					m_hEnvironmentMood.VolumeFader.Start(1f, 2f);
					if (Mood == UIMapData.ESoundMood.NightRain && AudioManager.Instance.CoroutineHelper != null)
					{
						AudioManager.Instance.CoroutineHelper.StartCoroutine(ThunderInterval(5f, 10f));
					}
				}
			}

			private IEnumerator HandleRestrictedAreaSound()
			{
				WaitForEndOfFrame wait = new WaitForEndOfFrame();
				m_restrictedAreaSound = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_restricted_area, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				bool wasInside = false;
				while (m_isIngame)
				{
					bool isInside = !InCombat && !InCodex && !InIntro && m_isInArea && IsAlive;
					if (isInside != wasInside)
					{
						if (isInside)
						{
							if (m_restrictedAreaSound.IsPlaying)
							{
								m_restrictedAreaSound.Stop();
							}
							m_restrictedAreaSound.Load();
							m_restrictedAreaSound.Volume = 1f;
							m_restrictedAreaSound.Play(true);
						}
						else
						{
							m_restrictedAreaSound.VolumeFader.Start(0f, (!InCombat) ? 6f : 0.5f);
						}
					}
					wasInside = isInside;
					yield return wait;
				}
				if (m_restrictedAreaSound != null)
				{
					m_restrictedAreaSound.Unload();
				}
				m_restrictedAreaSound = null;
			}

			public void OnExitIngame()
			{
				Messenger.Instance.RemoveListener<InfoEvent>(OnInfoEvent);
				m_isIngame = false;
				if (m_hEnvironmentMood != null)
				{
					if (m_hEnvironmentMood.IsPlaying)
					{
						m_hEnvironmentMood.VolumeFader.Start(0f, 1.3f);
					}
					else
					{
						m_hEnvironmentMood.Stop();
						m_hEnvironmentMood.Unload();
						m_hEnvironmentMood = null;
					}
				}
				if (m_parkourSound != null)
				{
					m_parkourSound.Unload();
					m_parkourSound = null;
				}
			}

			private void OnInfoEvent(InfoEvent _message)
			{
				switch (_message.InfoType)
				{
				case EInfoEventType.RestrictedAreaEnter:
					m_isInArea = true;
					break;
				case EInfoEventType.RestrictedAreaLeave:
					m_isInArea = false;
					break;
				}
			}

			private void AddMoods()
			{
				BBDictionary<int, GameID> bBDictionary = new BBDictionary<int, GameID>();
				bBDictionary.Add(0, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_monteriggioni_day);
				bBDictionary.Add(1, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_monteriggioni_night);
				bBDictionary.Add(2, GameID.Invalid);
				bBDictionary.Add(5, GameID.Invalid);
				bBDictionary.Add(6, GameID.Invalid);
				bBDictionary.Add(4, GameID.Invalid);
				bBDictionary.Add(3, GameID.Invalid);
				bBDictionary.Add(7, GameID.Invalid);
				bBDictionary.Add(9, GameID.Invalid);
				bBDictionary.Add(8, GameID.Invalid);
				BBDictionary<int, GameID> value = bBDictionary;
				m_environmentMoods.Add(1, value);
				bBDictionary = new BBDictionary<int, GameID>();
				bBDictionary.Add(0, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_firenze_palazzo_day);
				bBDictionary.Add(1, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_firenze_palazzo_night);
				bBDictionary.Add(2, GameID.Invalid);
				bBDictionary.Add(5, GameID.Invalid);
				bBDictionary.Add(6, GameID.Invalid);
				bBDictionary.Add(4, GameID.Invalid);
				bBDictionary.Add(3, GameID.Invalid);
				bBDictionary.Add(7, GameID.Invalid);
				bBDictionary.Add(9, GameID.Invalid);
				bBDictionary.Add(8, GameID.Invalid);
				BBDictionary<int, GameID> value2 = bBDictionary;
				m_environmentMoods.Add(2, value2);
				bBDictionary = new BBDictionary<int, GameID>();
				bBDictionary.Add(0, GameID.Invalid);
				bBDictionary.Add(1, GameID.Invalid);
				bBDictionary.Add(2, GameID.Invalid);
				bBDictionary.Add(5, GameID.Invalid);
				bBDictionary.Add(6, GameID.Invalid);
				bBDictionary.Add(4, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_firenze_santa_croce_stormy);
				bBDictionary.Add(3, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_firenze_santa_croce_sunny);
				bBDictionary.Add(7, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_rome_sant_angelo_stormy);
				bBDictionary.Add(9, GameID.Invalid);
				bBDictionary.Add(8, GameID.Invalid);
				BBDictionary<int, GameID> value3 = bBDictionary;
				m_environmentMoods.Add(3, value3);
				bBDictionary = new BBDictionary<int, GameID>();
				bBDictionary.Add(0, GameID.Invalid);
				bBDictionary.Add(1, GameID.Invalid);
				bBDictionary.Add(2, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_rome_colosseum_afternoon);
				bBDictionary.Add(5, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_rome_colosseum_foggy);
				bBDictionary.Add(6, GameID.Invalid);
				bBDictionary.Add(4, GameID.Invalid);
				bBDictionary.Add(3, GameID.Invalid);
				bBDictionary.Add(7, GameID.Invalid);
				bBDictionary.Add(9, GameID.Invalid);
				bBDictionary.Add(8, GameID.Invalid);
				BBDictionary<int, GameID> value4 = bBDictionary;
				m_environmentMoods.Add(4, value4);
				bBDictionary = new BBDictionary<int, GameID>();
				bBDictionary.Add(0, GameID.Invalid);
				bBDictionary.Add(1, GameID.Invalid);
				bBDictionary.Add(2, GameID.Invalid);
				bBDictionary.Add(5, GameID.Invalid);
				bBDictionary.Add(6, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_rome_sant_angelo_overcast);
				bBDictionary.Add(4, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_rome_sant_angelo_stormy);
				bBDictionary.Add(3, GameID.Invalid);
				bBDictionary.Add(7, GameID.Invalid);
				bBDictionary.Add(9, GameID.Invalid);
				bBDictionary.Add(8, GameID.Invalid);
				BBDictionary<int, GameID> value5 = bBDictionary;
				m_environmentMoods.Add(5, value5);
				bBDictionary = new BBDictionary<int, GameID>();
				bBDictionary.Add(0, GameID.Invalid);
				bBDictionary.Add(1, GameID.Invalid);
				bBDictionary.Add(2, GameID.Invalid);
				bBDictionary.Add(5, GameID.Invalid);
				bBDictionary.Add(6, GameID.Invalid);
				bBDictionary.Add(4, GameID.Invalid);
				bBDictionary.Add(3, GameID.Invalid);
				bBDictionary.Add(7, GameID.Invalid);
				bBDictionary.Add(9, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_forlie_siege);
				bBDictionary.Add(8, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_italy_forlie_dusk);
				BBDictionary<int, GameID> value6 = bBDictionary;
				m_environmentMoods.Add(9, value6);
				bBDictionary = new BBDictionary<int, GameID>();
				bBDictionary.Add(0, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_tutorial_lp);
				bBDictionary.Add(1, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_tutorial_lp);
				bBDictionary.Add(2, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_tutorial_lp);
				bBDictionary.Add(5, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_tutorial_lp);
				bBDictionary.Add(6, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_tutorial_lp);
				bBDictionary.Add(4, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_tutorial_lp);
				bBDictionary.Add(3, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_tutorial_lp);
				bBDictionary.Add(7, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_tutorial_lp);
				bBDictionary.Add(9, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_tutorial_lp);
				bBDictionary.Add(8, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_animus_tutorial_lp);
				BBDictionary<int, GameID> value7 = bBDictionary;
				m_environmentMoods.Add(7, value7);
			}

			private void CreateAudioHandle(ERegion _region)
			{
				m_hEnvironmentMood = null;
				GameID environmentMoodGameId = GetEnvironmentMoodGameId(_region, Mood);
				if (GameID.IsValid(environmentMoodGameId))
				{
					m_hEnvironmentMood = AudioManager.Instance.CreateHandle(environmentMoodGameId, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				}
			}

			private GameID GetEnvironmentMoodGameId(ERegion _region, UIMapData.ESoundMood _mood)
			{
				if (m_environmentMoods.ContainsKey((int)_region) && m_environmentMoods[(int)_region].ContainsKey((int)_mood))
				{
					GameID gameID = m_environmentMoods[(int)_region][(int)_mood];
					if (GameID.IsValid(gameID))
					{
						return gameID;
					}
				}
				return GameID.Invalid;
			}

			private IEnumerator ThunderInterval(float _min, float _max)
			{
				yield return new WaitForSeconds(UnityEngine.Random.Range(_min, _max));
				while (m_isIngame)
				{
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_generic_storm_rumble);
					yield return new WaitForSeconds(UnityEngine.Random.Range(_min, _max));
				}
			}

			public void StartParkourTheme()
			{
				if (m_parkourSound == null)
				{
					m_parkourSound = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_parcour_run, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				}
				m_parkourSound.Load();
				m_parkourSound.Play(true);
			}

			public void EndParkourTheme()
			{
				if (m_parkourSound != null)
				{
					m_parkourSound.Stop();
				}
			}
		}

		private class HotspotHandler : IAudioHandler
		{
			private IAudioHandle m_denySound;

			public void OnEnterIngame()
			{
				LoadAudioHandles();
				LogicMain.PlayerEntity.PlayerCharacterStateMachines.PlayerMain.StateChanged += PlayerMainOnStateChanged;
			}

			private void PlayerMainOnStateChanged(IStateMachine _stateMachine, EStateType _from, EStateType _to)
			{
				if (_to == EStateType.PlayerMain_HidingInObject)
				{
					PlayerMain.HidingInObjectState.HidingInObjectPayload payload = _stateMachine.GetPayload<PlayerMain.HidingInObjectState.HidingInObjectPayload>(_to);
					if (payload.ShouldExit)
					{
						m_denySound.Play();
					}
				}
			}

			public void OnExitIngame()
			{
				LogicMain.PlayerEntity.PlayerCharacterStateMachines.PlayerMain.StateChanged -= PlayerMainOnStateChanged;
				UnloadAudioHandles();
			}

			private void LoadAudioHandles()
			{
				UnloadAudioHandles();
				m_denySound = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_menu_global_deny, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
				m_denySound.Load();
			}

			private void UnloadAudioHandles()
			{
				if (m_denySound != null)
				{
					m_denySound.Unload();
				}
			}
		}

		private class IngameHandler
		{
			private readonly MixStateHandler m_mixStateHandler;

			private readonly SlowMotionHandler m_slowMotionHandler;

			private readonly RegionIntroOutroHandler m_regionIntroOutroHandler;

			private readonly EnvironmentMoodHandler m_environmentMoodHandler;

			private readonly CombatHandler m_combatHandler;

			private readonly CrowdHandler m_crowdHandler;

			private readonly CodexHandler m_codexHandler;

			private readonly HotspotHandler m_hotspotHandler;

			private readonly AmbientSfxHandler m_ambientSfxHandler;

			private bool m_isIngame;

			private bool m_isResultScreenOpen;

			public UIMapData.ESoundMood Mood
			{
				get
				{
					return m_environmentMoodHandler.Mood;
				}
				set
				{
					m_environmentMoodHandler.Mood = value;
				}
			}

			public IngameHandler(MixStateHandler _mixStateHandler)
			{
				m_mixStateHandler = _mixStateHandler;
				m_slowMotionHandler = new SlowMotionHandler(m_mixStateHandler);
				m_regionIntroOutroHandler = new RegionIntroOutroHandler(m_mixStateHandler);
				m_environmentMoodHandler = new EnvironmentMoodHandler();
				m_combatHandler = new CombatHandler(m_mixStateHandler);
				m_crowdHandler = new CrowdHandler(m_mixStateHandler);
				m_codexHandler = new CodexHandler(m_mixStateHandler);
				m_hotspotHandler = new HotspotHandler();
				m_ambientSfxHandler = new AmbientSfxHandler();
			}

			public void Begin(ERegion _region)
			{
				m_isIngame = true;
				Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
				instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Combine(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnPause));
				Messenger.Instance.AddListener<PlayerStateChangeEvent>(OnPlayerStateChange);
				m_mixStateHandler.OnEnterIngame();
				m_slowMotionHandler.OnEnterIngame();
				m_environmentMoodHandler.OnEnterIngame(_region);
				m_crowdHandler.OnEnterIngame();
				m_codexHandler.OnEnterIngame();
				m_hotspotHandler.OnEnterIngame();
				m_ambientSfxHandler.OnEnterIngame();
				m_combatHandler.OnEnterIngame();
			}

			public void End()
			{
				m_isIngame = false;
				Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
				instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Remove(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnPause));
				Messenger.Instance.RemoveListener<PlayerStateChangeEvent>(OnPlayerStateChange);
				OnResultScreenExit();
				m_combatHandler.OnExitIngame();
				m_slowMotionHandler.OnExitIngame();
				m_environmentMoodHandler.OnExitIngame();
				m_mixStateHandler.OnExitIngame();
				m_crowdHandler.OnExitIngame();
				m_codexHandler.OnExitIngame();
				m_hotspotHandler.OnExitIngame();
				m_ambientSfxHandler.OnExitIngame();
			}

			public void OnPlayerDie()
			{
				if (m_isIngame)
				{
					m_combatHandler.End(true);
					m_environmentMoodHandler.InCombat = false;
					m_environmentMoodHandler.IsAlive = false;
					m_mixStateHandler.OnPlayerDie();
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_generic_player_die);
				}
			}

			public void OnPlayerRevive()
			{
				m_mixStateHandler.OnPlayerRevive();
				m_environmentMoodHandler.IsAlive = true;
			}

			public void OnResultScreenEnter()
			{
				m_isResultScreenOpen = true;
				Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
				instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Remove(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnPause));
				Messenger.Instance.RemoveListener<PlayerStateChangeEvent>(OnPlayerStateChange);
				m_combatHandler.End(true);
				m_environmentMoodHandler.InCombat = false;
				m_environmentMoodHandler.InIntro = true;
				m_mixStateHandler.OnResultScreenEnter();
				m_slowMotionHandler.OnResultScreenEnter();
				bool flag = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState == EMissionState.Success;
				AudioManager.Instance.Play((!flag) ? UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_generic_mission_failed : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_generic_mission_complete);
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_missionreward_open);
			}

			public void OnResultScreenExit()
			{
				if (m_isResultScreenOpen)
				{
					m_isResultScreenOpen = false;
					m_mixStateHandler.OnResultScreenExit();
				}
			}

			public void OnEnterCodex()
			{
				m_codexHandler.Begin();
				m_environmentMoodHandler.InCodex = true;
			}

			public void OnLeaveCodex()
			{
				m_codexHandler.End();
				m_environmentMoodHandler.InCodex = false;
			}

			public void OnParkourStart()
			{
				m_environmentMoodHandler.StartParkourTheme();
				m_combatHandler.IsMuted = true;
			}

			public void OnParkourEnd()
			{
				m_environmentMoodHandler.EndParkourTheme();
				m_combatHandler.IsMuted = false;
			}

			private void OnPause(Timer.EPauseState _pauseReason)
			{
				AudioManager.Instance.Play((_pauseReason == Timer.EPauseState.Running) ? UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_unpause : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_pause);
			}

			private void OnPlayerStateChange(PlayerStateChangeEvent _playerStateChangeEvent)
			{
				EPlayerState playerState = _playerStateChangeEvent.PlayerState;
				if (playerState == EPlayerState.Combat)
				{
					m_combatHandler.Begin();
					m_environmentMoodHandler.InCombat = true;
				}
				else
				{
					m_combatHandler.End();
					m_environmentMoodHandler.InCombat = false;
				}
			}

			public void BeginIntro(ERegion _currentRegion)
			{
				m_regionIntroOutroHandler.BeginOutro(_currentRegion);
				m_environmentMoodHandler.InIntro = true;
			}

			public void EndIntro()
			{
				m_regionIntroOutroHandler.EndIntro();
				m_environmentMoodHandler.InIntro = false;
			}

			public void EndOutro()
			{
				m_regionIntroOutroHandler.EndOutro();
				m_environmentMoodHandler.InIntro = false;
			}

			public void BeginOutro()
			{
				m_regionIntroOutroHandler.BeginOutro();
				m_environmentMoodHandler.InIntro = true;
			}

			public void OnIntroMissionCathedralClimbAutomation()
			{
				m_regionIntroOutroHandler.OnIntroMissionCathedralClimbAutomation();
			}

			public void OnIntroMissionCathedralClimbWindStart()
			{
				m_regionIntroOutroHandler.OnIntroMissionCathedralClimbWindStart();
			}

			public void OnEscorteeThreatened(bool _escorteeThreatened)
			{
				m_combatHandler.OnEscorteeThreatened(_escorteeThreatened);
			}
		}

		private class LoadingHandler
		{
			public enum EReason
			{
				Idle = 0,
				Loading = 1,
				Downloading = 2
			}

			private readonly MixStateHandler m_mixStateHandler;

			private EReason m_currentReason;

			private IAudioHandle m_downloadMusicHandler;

			private bool m_mixStateOn;

			public EReason CurrentReason
			{
				get
				{
					return m_currentReason;
				}
				set
				{
					if (m_currentReason != value)
					{
						UnloadAudioHandles();
						m_currentReason = value;
						LoadAudioHandles();
						UpdateAudio();
					}
				}
			}

			private bool MixStateOn
			{
				get
				{
					return m_mixStateOn;
				}
				set
				{
					if (MixStateOn != value)
					{
						m_mixStateOn = value;
						if (m_mixStateOn)
						{
							m_mixStateHandler.OnEnterLoading();
						}
						else
						{
							m_mixStateHandler.OnLeaveLoading();
						}
					}
				}
			}

			public LoadingHandler(MixStateHandler _mixStateHandler)
			{
				m_mixStateHandler = _mixStateHandler;
			}

			private void UpdateAudio()
			{
				switch (CurrentReason)
				{
				case EReason.Idle:
					MixStateOn = false;
					break;
				case EReason.Loading:
					MixStateOn = true;
					break;
				case EReason.Downloading:
					m_downloadMusicHandler.Play(true);
					MixStateOn = true;
					break;
				}
			}

			private void LoadAudioHandles()
			{
				if (CurrentReason == EReason.Downloading && m_downloadMusicHandler == null)
				{
					m_downloadMusicHandler = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_loading, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
					m_downloadMusicHandler.Load();
				}
			}

			private void UnloadAudioHandles()
			{
				if (CurrentReason == EReason.Downloading && m_downloadMusicHandler != null)
				{
					AudioManager.Instance.CoroutineHelper.StartCoroutine(FadeOutMusic());
				}
			}

			private IEnumerator FadeOutMusic()
			{
				m_downloadMusicHandler.VolumeFader.Start(0f, 3f);
				yield return new WaitForSeconds(3f);
				m_downloadMusicHandler.Unload();
			}

			public void OnTapToStart()
			{
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_taptostart_prompt);
			}
		}

		private class MixStateHandler
		{
			private readonly AudioManager.MixGroupCarrier m_debriefScreenMixState;

			private readonly AudioManager.MixGroupCarrier m_pauseMixState;

			private readonly AudioManager.MixGroupCarrier m_slowmoMixState;

			private readonly AudioManager.MixGroupCarrier m_playerDieMixState;

			private readonly AudioManager.MixGroupCarrier m_combatMixState;

			private readonly AudioManager.MixGroupCarrier m_levelIntroMixState;

			private readonly AudioManager.MixGroupCarrier m_highAltitudeMixState;

			private readonly AudioManager.MixGroupCarrier m_loadingMixState;

			private readonly AudioManager.MixGroupCarrier m_heightDifferenceState;

			private readonly AudioManager.MixGroupCarrier m_adColonyState;

			private readonly AudioManager.MixGroupCarrier m_pausePopUpMixState;

			public MixStateHandler()
			{
				m_debriefScreenMixState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.DebriefScreen);
				m_pauseMixState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.Pause);
				m_pausePopUpMixState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.PausePopUp);
				m_slowmoMixState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.SlowMotion);
				m_playerDieMixState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.PlayerDie);
				m_combatMixState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.Combat);
				m_levelIntroMixState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.LevelStartCameraPan);
				m_highAltitudeMixState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.HigherGround);
				m_loadingMixState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.Loading);
				m_heightDifferenceState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.WallaFade);
				m_adColonyState = new AudioManager.MixGroupCarrier(UbiCore.Utils.Singleton<MixStateGameIds>.Instance.Adcolony);
			}

			public void OnPlayerDie()
			{
				AudioManager.Instance.MixStateManager.Start(m_playerDieMixState);
			}

			public void OnPlayerRevive()
			{
				AudioManager.Instance.MixStateManager.Stop(m_playerDieMixState);
			}

			public void OnResultScreenEnter()
			{
				Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
				instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Remove(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnPause));
				AudioManager.Instance.MixStateManager.StopAll();
				AudioManager.Instance.MixStateManager.Start(m_debriefScreenMixState);
			}

			public void OnResultScreenExit()
			{
				OnSlowMotionExit();
				AudioManager.Instance.MixStateManager.Stop(m_debriefScreenMixState);
			}

			private void OnPause(Timer.EPauseState _pauseReason)
			{
				switch (_pauseReason)
				{
				case Timer.EPauseState.Running:
					AudioManager.Instance.MixStateManager.Stop(m_pauseMixState);
					AudioManager.Instance.MixStateManager.Stop(m_pausePopUpMixState);
					break;
				case Timer.EPauseState.PauseGeneral:
				case Timer.EPauseState.PauseOptions:
					AudioManager.Instance.MixStateManager.Start(m_pauseMixState);
					AudioManager.Instance.MixStateManager.Stop(m_pausePopUpMixState);
					break;
				case Timer.EPauseState.PauseDialog:
					AudioManager.Instance.MixStateManager.Start(m_pausePopUpMixState);
					AudioManager.Instance.MixStateManager.Stop(m_pauseMixState);
					break;
				}
			}

			public void OnHeightDifferenceStart()
			{
				AudioManager.Instance.MixStateManager.Start(m_heightDifferenceState);
			}

			public void OnHeightDifferenceEnd()
			{
				AudioManager.Instance.MixStateManager.Stop(m_heightDifferenceState);
			}

			public void OnSlowMotionEnter()
			{
				AudioManager.Instance.MixStateManager.Start(m_slowmoMixState);
			}

			public void OnSlowMotionExit()
			{
				AudioManager.Instance.MixStateManager.Stop(m_slowmoMixState);
			}

			public void OnRegionIntroStart()
			{
				AudioManager.Instance.MixStateManager.Start(m_levelIntroMixState);
			}

			public void OnRegionIntroEnd()
			{
				AudioManager.Instance.MixStateManager.Stop(m_levelIntroMixState);
			}

			public void OnAdColonyStart()
			{
				AudioManager.Instance.MixStateManager.Start(m_adColonyState);
			}

			public void OnAdColonyEnd()
			{
				AudioManager.Instance.MixStateManager.Stop(m_adColonyState);
			}

			public void OnHighAltidudeReached()
			{
				AudioManager.Instance.MixStateManager.Start(m_highAltitudeMixState);
			}

			public void OnLowAltitudeReached()
			{
				AudioManager.Instance.MixStateManager.Stop(m_highAltitudeMixState);
			}

			public void OnEnterLoading()
			{
				AudioManager.Instance.MixStateManager.StopAll();
				AudioManager.Instance.MixStateManager.Start(m_loadingMixState);
			}

			public void OnLeaveLoading()
			{
				AudioManager.Instance.MixStateManager.StopAll();
			}

			public void OnEnterIngame()
			{
				Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
				instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Combine(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnPause));
			}

			public void OnExitIngame()
			{
				Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
				instance.OnPauseChanged = (Timer.OnPauseChangeHandler)Delegate.Remove(instance.OnPauseChanged, new Timer.OnPauseChangeHandler(OnPause));
				AudioManager.Instance.MixStateManager.StopAll();
			}

			public void OnEnterCombat()
			{
				AudioManager.Instance.MixStateManager.Start(m_combatMixState);
			}

			public void OnExitCombat()
			{
				AudioManager.Instance.MixStateManager.Stop(m_combatMixState);
			}
		}

		private class RegionIntroOutroHandler
		{
			private readonly MixStateHandler m_mixStateHandler;

			private IAudioHandle m_hRegionIntro;

			private IAudioHandle m_introMissionWind;

			private readonly BBDictionary<int, GameID> m_regionIntroductionMusic = new BBDictionary<int, GameID>();

			public RegionIntroOutroHandler(MixStateHandler _mixStatehandler)
			{
				m_mixStateHandler = _mixStatehandler;
				Init();
			}

			public void BeginOutro(ERegion _region)
			{
				m_mixStateHandler.OnRegionIntroStart();
				GameID regionIntroGameId = GetRegionIntroGameId(_region);
				if (GameID.IsValid(regionIntroGameId))
				{
					m_hRegionIntro = AudioManager.Instance.CreateHandle(regionIntroGameId, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
					m_hRegionIntro.Load();
					m_hRegionIntro.Play();
				}
				else if (m_hRegionIntro != null)
				{
					m_hRegionIntro.Stop();
					m_hRegionIntro.Unload();
					m_hRegionIntro = null;
				}
			}

			public void EndIntro()
			{
				m_mixStateHandler.OnRegionIntroEnd();
				if (m_hRegionIntro != null && m_hRegionIntro.IsPlaying)
				{
					m_hRegionIntro.VolumeFader.Start(0f, 3f);
				}
			}

			public void BeginOutro()
			{
			}

			public void EndOutro()
			{
				if (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.GetCategory(ETutorialCategory.TutCatIntroMission).Finished)
				{
					m_mixStateHandler.OnLowAltitudeReached();
					if (m_introMissionWind != null)
					{
						m_introMissionWind.VolumeFader.Start(0f, 0.5f);
						m_introMissionWind = null;
					}
				}
			}

			public void OnIntroMissionCathedralClimbAutomation()
			{
				m_mixStateHandler.OnHighAltidudeReached();
			}

			public void OnIntroMissionCathedralClimbWindStart()
			{
				m_introMissionWind = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_env_wind_cameraheight_lp, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadAll);
				m_introMissionWind.Load();
				m_introMissionWind.Volume = 0f;
				m_introMissionWind.Play(true);
				m_introMissionWind.VolumeFader.Start(1f, 3f);
			}

			private void Init()
			{
				m_regionIntroductionMusic.Add(0, GameID.Invalid);
				m_regionIntroductionMusic.Add(8, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_generic_levelstart_camerapan);
				m_regionIntroductionMusic.Add(2, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_generic_levelstart_camerapan);
				m_regionIntroductionMusic.Add(3, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_generic_levelstart_camerapan);
				m_regionIntroductionMusic.Add(1, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_generic_levelstart_camerapan);
				m_regionIntroductionMusic.Add(7, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_generic_levelstart_camerapan);
				m_regionIntroductionMusic.Add(4, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_generic_levelstart_camerapan);
				m_regionIntroductionMusic.Add(5, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_generic_levelstart_camerapan);
				m_regionIntroductionMusic.Add(6, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_generic_levelstart_camerapan);
				m_regionIntroductionMusic.Add(9, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_mus_italy_generic_levelstart_camerapan);
			}

			private GameID GetRegionIntroGameId(ERegion _region)
			{
				if (m_regionIntroductionMusic.ContainsKey((int)_region))
				{
					GameID gameID = m_regionIntroductionMusic[(int)_region];
					if (GameID.IsValid(gameID))
					{
						return gameID;
					}
				}
				return GameID.Invalid;
			}
		}

		private class SlowMotionHandler
		{
			private readonly MixStateHandler m_mixStateHandler;

			private IAudioHandle m_hSlowmoStart;

			private bool m_isSlowmo;

			public SlowMotionHandler(MixStateHandler _mixStatehandler)
			{
				m_mixStateHandler = _mixStatehandler;
			}

			public void OnEnterIngame()
			{
				m_isSlowmo = false;
				Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
				instance.OnScaleChanged = (Timer.OnScaleChangeHandler)Delegate.Combine(instance.OnScaleChanged, new Timer.OnScaleChangeHandler(OnTimeScaleChanged));
				LoadAudioHandles();
			}

			public void OnExitIngame()
			{
				Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
				instance.OnScaleChanged = (Timer.OnScaleChangeHandler)Delegate.Remove(instance.OnScaleChanged, new Timer.OnScaleChangeHandler(OnTimeScaleChanged));
				UnloadAudioHandles();
			}

			public void OnResultScreenEnter()
			{
				Timer instance = UbiCore.Utils.Singleton<Timer>.Instance;
				instance.OnScaleChanged = (Timer.OnScaleChangeHandler)Delegate.Remove(instance.OnScaleChanged, new Timer.OnScaleChangeHandler(OnTimeScaleChanged));
			}

			private void OnTimeScaleChanged(float _scale)
			{
				Timer.EPauseState pauseState = UbiCore.Utils.Singleton<Timer>.Instance.PauseState;
				if (m_isSlowmo && _scale >= 1f)
				{
					if (pauseState == Timer.EPauseState.Running)
					{
						m_isSlowmo = false;
						OnSlowMotionExit();
					}
				}
				else if (!m_isSlowmo && _scale < 1f && _scale > 0.3f && pauseState == Timer.EPauseState.Running)
				{
					m_isSlowmo = true;
					OnSlowMotionEnter();
				}
			}

			private void OnSlowMotionEnter()
			{
				EMissionState missionState = UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState;
				if (missionState != EMissionState.Failed && missionState != EMissionState.Success)
				{
					m_mixStateHandler.OnSlowMotionEnter();
					m_hSlowmoStart.Play();
				}
			}

			private void OnSlowMotionExit()
			{
				m_mixStateHandler.OnSlowMotionExit();
			}

			private void LoadAudioHandles()
			{
				UnloadAudioHandles();
				m_hSlowmoStart = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_slowmotion_start, ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
				m_hSlowmoStart.Load();
			}

			private void UnloadAudioHandles()
			{
				if (m_hSlowmoStart != null)
				{
					m_hSlowmoStart.Unload();
				}
			}
		}

		public delegate void BoolDelegate(bool _active);

		private bool m_deactivateLoadingScreenDucking;

		private ERegion m_currentRegion;

		private readonly AnimusHandler m_animusHandler;

		private readonly IngameHandler m_ingameHandler;

		private readonly LoadingHandler m_loadingHandler;

		public bool ThemeSongOn
		{
			get
			{
				return m_animusHandler.IsThemeOn;
			}
			set
			{
				m_animusHandler.IsThemeOn = value;
			}
		}

		public event BoolDelegate Ingame;

		public AudioEventManager()
		{
			MixStateHandler mixStateHandler = new MixStateHandler();
			m_animusHandler = new AnimusHandler(mixStateHandler);
			m_loadingHandler = new LoadingHandler(mixStateHandler);
			m_ingameHandler = new IngameHandler(mixStateHandler);
		}

		private void InvokeIngame(bool _active)
		{
			BoolDelegate ingame = this.Ingame;
			if (ingame != null)
			{
				ingame(_active);
			}
		}

		public void OnRegionIntroStarted()
		{
			m_ingameHandler.BeginIntro(m_currentRegion);
		}

		public void OnRegionIntroEnded()
		{
			m_ingameHandler.EndIntro();
		}

		public void OnRegionOutroStarted()
		{
			m_ingameHandler.BeginOutro();
		}

		public void OnRegionOutroEnded()
		{
			m_ingameHandler.EndOutro();
		}

		public void OnIntroMissionCathedralClimbAutomation()
		{
			m_ingameHandler.OnIntroMissionCathedralClimbAutomation();
		}

		public void OnIntroMissionCathedralClimbWindStart()
		{
			m_ingameHandler.OnIntroMissionCathedralClimbWindStart();
		}

		public void OnIntroMissionReachCathedralTop()
		{
			m_deactivateLoadingScreenDucking = true;
			m_animusHandler.StartAnimusThemeEarly();
		}

		public void OnMissionLoadedUiMapData(UIMapData _mapData)
		{
			m_ingameHandler.Mood = ((_mapData != null) ? _mapData.m_mood : UIMapData.ESoundMood.Day);
		}

		public void OnPlayerDie()
		{
			m_ingameHandler.OnPlayerDie();
		}

		public void OnPlayerRevive()
		{
			m_ingameHandler.OnPlayerRevive();
		}

		public void OnResultScreenEnter()
		{
			m_ingameHandler.OnResultScreenEnter();
		}

		public void OnResultScreenExit()
		{
			m_ingameHandler.OnResultScreenExit();
		}

		public void OnTapStartMission()
		{
			m_ingameHandler.Begin(m_currentRegion);
			m_loadingHandler.CurrentReason = LoadingHandler.EReason.Idle;
			InvokeIngame(true);
		}

		public void OnTapToStartMission()
		{
			m_loadingHandler.OnTapToStart();
		}

		public void OnLeaveCredits()
		{
			m_animusHandler.OnLeaveCredits();
		}

		public void OnEnterCredits()
		{
			m_animusHandler.OnEnterCredits();
		}

		public void OnEnterDownloadScreen()
		{
			m_loadingHandler.CurrentReason = LoadingHandler.EReason.Downloading;
		}

		public void OnLeaveDownloadScreen()
		{
			m_loadingHandler.CurrentReason = LoadingHandler.EReason.Idle;
		}

		public void ConnectEvents()
		{
			Messenger.Instance.AddListener<StateChangedMessage<CharacterAwareness>>(OnCharacterAwarenessStateChanged);
			Messenger.Instance.AddListener<StateChangedMessage<PlayerAlertLevel>>(OnPlayerAlertStateChanged);
			Messenger.Instance.AddListener<StateChangedMessage<PlayerDisguise>>(OnPlayerDisguiseStateChanged);
			Messenger.Instance.AddListener<StateChangedMessage<PlayerMain>>(OnPlayerMainStateChanged);
			Main.Instance.SceneLoader.OnStep += OnSceneLoaderStep;
		}

		private void OnCharacterAwarenessStateChanged(StateChangedMessage<CharacterAwareness> _stateChangeMessage)
		{
			EStateType to = _stateChangeMessage.To;
			if (to == EStateType.CharacterAwareness_Low && _stateChangeMessage.From != EStateType.CharacterAwareness_Unaware)
			{
			}
		}

		private void OnPlayerAlertStateChanged(StateChangedMessage<PlayerAlertLevel> _stateChangeMessage)
		{
			if (_stateChangeMessage.From == EStateType.CharacterAlertLevel_Anonymous)
			{
				EStateType to = _stateChangeMessage.To;
				if (to == EStateType.CharacterAlertLevel_Alerted || to == EStateType.CharacterAlertLevel_Exposed)
				{
					AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ui_state_change_alert);
				}
			}
		}

		private void OnPlayerDisguiseStateChanged(StateChangedMessage<PlayerDisguise> _stateChangeMessage)
		{
			switch (_stateChangeMessage.To)
			{
			case EStateType.PlayerDisguise_Disguised:
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ability_blend);
				break;
			case EStateType.PlayerDisguise_Undisguised:
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ability_unblend);
				break;
			}
		}

		private void OnPlayerMainStateChanged(StateChangedMessage<PlayerMain> _stateChangeMessage)
		{
			switch (_stateChangeMessage.To)
			{
			case EStateType.PlayerMain_HidingInObject:
			case EStateType.PlayerMain_HidingInPatrol:
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ability_blend);
				break;
			}
			switch (_stateChangeMessage.From)
			{
			case EStateType.PlayerMain_HidingInObject:
			case EStateType.PlayerMain_HidingInPatrol:
				AudioManager.Instance.Play(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_ability_unblend);
				break;
			case EStateType.PlayerMain_HidingInObjectExitAndMoveTo:
				break;
			}
		}

		public void OnAnimusFullyLoadedAndInitialized()
		{
			m_animusHandler.Begin();
			if (!m_deactivateLoadingScreenDucking)
			{
				m_loadingHandler.CurrentReason = LoadingHandler.EReason.Idle;
			}
			m_deactivateLoadingScreenDucking = false;
		}

		private void OnSceneLoaderStep(SceneLoader.ELoadingStep _step)
		{
			switch (_step)
			{
			case SceneLoader.ELoadingStep.LoadingStart:
				OnSceneUnloadStarted();
				break;
			case SceneLoader.ELoadingStep.LoadingSceneDone:
				OnSceneLoaded(Main.Instance.SceneLoader.CurrentMissionDescription);
				break;
			}
		}

		private void OnSceneLoaded(MissionRootData _desc)
		{
			m_currentRegion = _desc.Region;
			if (Main.Instance.BuildDefinition.EditorMode && _desc.MissionType != 0)
			{
				OnTapStartMission();
			}
		}

		private void OnSceneUnloadStarted()
		{
			if (Main.HasInstance && Main.Instance.SceneLoader != null && Main.Instance.SceneLoader.CurrentMissionDescription != null)
			{
				if (Main.Instance.SceneLoader.InAnimus)
				{
					m_animusHandler.End();
				}
				else
				{
					m_ingameHandler.End();
					InvokeIngame(false);
				}
			}
			if (!m_deactivateLoadingScreenDucking)
			{
				m_loadingHandler.CurrentReason = LoadingHandler.EReason.Loading;
			}
		}

		public void OnEnterCodex()
		{
			m_ingameHandler.OnEnterCodex();
		}

		public void OnLeaveCodex()
		{
			m_ingameHandler.OnLeaveCodex();
		}

		public void OnEnterParkour()
		{
			m_ingameHandler.OnParkourStart();
		}

		public void OnLeaveParkour()
		{
			m_ingameHandler.OnParkourEnd();
		}

		public void OnEscorteeThreatened(bool _escorteeThreatened)
		{
			m_ingameHandler.OnEscorteeThreatened(_escorteeThreatened);
		}
	}
}
