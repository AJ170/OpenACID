using System.Collections.Generic;
using Assets.Scripts.Mission.Logic.StateMachines;
using Database;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioFootstepPlayer : MonoBehaviour, IAnimatorEventListener
	{
		public enum EFootstepType
		{
			Crouch = 0,
			Walk = 1,
			Run = 2,
			Sprint = 3,
			Slide = 4,
			Stop = 5
		}

		private readonly List<IAudioHandle> m_playHandles = new List<IAudioHandle>();

		private List<Dictionary<int, IAudioHandle>> m_handles = new List<Dictionary<int, IAudioHandle>>();

		private PlayerMain.EFloorType m_currentFloorType;

		private AnimatorDataController m_animatorDataController;

		private void Awake()
		{
			m_animatorDataController = GetComponent<AnimatorDataController>();
			m_animatorDataController.RegisterEventListener(this);
			FillHandles(ref m_handles);
		}

		private void OnDestroy()
		{
			m_animatorDataController.UnregisterEventListener(this);
			Unload(m_currentFloorType);
		}

		private void Update()
		{
			if (LogicMain.PlayerEntity != null && LogicMain.PlayerEntity.PlayerCharacterStateMachines != null && LogicMain.PlayerEntity.PlayerCharacterStateMachines.PlayerMain != null)
			{
				PlayerMain.EFloorType floorType = LogicMain.PlayerEntity.PlayerCharacterStateMachines.PlayerMain.FloorType;
				if (floorType != m_currentFloorType && floorType != PlayerMain.EFloorType.Invalid)
				{
					Unload(m_currentFloorType);
					m_currentFloorType = floorType;
					Load(m_currentFloorType);
				}
			}
		}

		private void Load(PlayerMain.EFloorType _type)
		{
			if (_type == PlayerMain.EFloorType.Invalid)
			{
				return;
			}
			Profiling.Start("AudioFootstepPlayer::Load()", float.MaxValue);
			Dictionary<int, IAudioHandle> dictionary = m_handles[(int)_type];
			foreach (KeyValuePair<int, IAudioHandle> item in dictionary)
			{
				item.Value.Load();
			}
			Profiling.Stop();
		}

		private void Unload(PlayerMain.EFloorType _type)
		{
			if (_type == PlayerMain.EFloorType.Invalid)
			{
				return;
			}
			Dictionary<int, IAudioHandle> dictionary = m_handles[(int)_type];
			foreach (KeyValuePair<int, IAudioHandle> item in dictionary)
			{
				item.Value.UnloadAllNowOrOnDone();
			}
		}

		private IAudioHandle GetHandle(PlayerMain.EFloorType _type, EFootstepType _footstepType)
		{
			IAudioHandle audioHandle = null;
			if (_type != PlayerMain.EFloorType.Invalid)
			{
				GameID audioControllerGameId = GetAudioControllerGameId(_type, _footstepType);
				if (GameID.IsValid(audioControllerGameId))
				{
					audioHandle = CreateOrGetFreeHandle(audioControllerGameId);
				}
			}
			if (audioHandle == null)
			{
			}
			return audioHandle;
		}

		private GameID GetAudioControllerGameId(PlayerMain.EFloorType _type, EFootstepType _footstepType)
		{
			GameID result = GameID.Invalid;
			AudioGameIds instance = UbiCore.Utils.Singleton<AudioGameIds>.Instance;
			switch (_type)
			{
			case PlayerMain.EFloorType.Ground:
				switch (_footstepType)
				{
				case EFootstepType.Crouch:
					result = instance.ac_sfx_step_ground_crouch;
					break;
				case EFootstepType.Walk:
					result = instance.ac_sfx_step_ground_walk;
					break;
				case EFootstepType.Run:
					result = instance.ac_sfx_step_ground_run;
					break;
				case EFootstepType.Sprint:
					result = instance.ac_sfx_step_ground_sprint;
					break;
				case EFootstepType.Slide:
					result = instance.ac_sfx_step_ground_slide;
					break;
				case EFootstepType.Stop:
					result = instance.ac_sfx_step_ground_stop;
					break;
				}
				break;
			case PlayerMain.EFloorType.Roof:
				switch (_footstepType)
				{
				case EFootstepType.Crouch:
					result = instance.ac_sfx_step_rooftop_crouch;
					break;
				case EFootstepType.Walk:
					result = instance.ac_sfx_step_rooftop_walk;
					break;
				case EFootstepType.Run:
					result = instance.ac_sfx_step_rooftop_run;
					break;
				case EFootstepType.Sprint:
					result = instance.ac_sfx_step_rooftop_sprint;
					break;
				case EFootstepType.Slide:
					result = instance.ac_sfx_step_rooftop_slide;
					break;
				case EFootstepType.Stop:
					result = instance.ac_sfx_step_rooftop_stop;
					break;
				}
				break;
			}
			return result;
		}

		private IAudioHandle CreateOrGetFreeHandle(GameID _id)
		{
			if (!GameID.IsValid(_id))
			{
				return null;
			}
			for (int i = 0; i < m_playHandles.Count; i++)
			{
				IAudioHandle audioHandle = m_playHandles[i];
				if (audioHandle.AudioController.Id == _id && !audioHandle.IsPlaying)
				{
					return audioHandle;
				}
			}
			return CreateAddHandle(_id);
		}

		private IAudioHandle CreateAddHandle(GameID _id)
		{
			IAudioHandle audioHandle = AudioManager.Instance.CreateHandle(_id, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
			m_playHandles.Add(audioHandle);
			return audioHandle;
		}

		public bool OnAnimatorEvent(AnimatorEvent _e)
		{
			EFootstepType _outFootstepType = EFootstepType.Run;
			if (AnimatorEventTypeToFootstepType(_e.Type, ref _outFootstepType))
			{
				IAudioHandle handle = GetHandle(m_currentFloorType, _outFootstepType);
				if (handle != null)
				{
					handle.Load();
					handle.Play();
				}
			}
			return false;
		}

		public static bool AnimatorEventTypeToFootstepType(AnimatorEventType _animatorEventType, ref EFootstepType _outFootstepType)
		{
			switch (_animatorEventType)
			{
			case AnimatorEventType.SoundFootCrouch:
				_outFootstepType = EFootstepType.Crouch;
				return true;
			case AnimatorEventType.SoundFootWalk:
				_outFootstepType = EFootstepType.Walk;
				return true;
			case AnimatorEventType.SoundFootRun:
				_outFootstepType = EFootstepType.Run;
				return true;
			case AnimatorEventType.SoundFootSprint:
				_outFootstepType = EFootstepType.Sprint;
				return true;
			case AnimatorEventType.SoundFootSlide:
				_outFootstepType = EFootstepType.Slide;
				return true;
			case AnimatorEventType.SoundFootStop:
				_outFootstepType = EFootstepType.Stop;
				return true;
			default:
				return false;
			}
		}

		public static void FillHandles(ref List<Dictionary<int, IAudioHandle>> _handles)
		{
			AudioGameIds instance = UbiCore.Utils.Singleton<AudioGameIds>.Instance;
			ELoadStrategy loadStrategy = ELoadStrategy.LoadAll;
			EStoppedFinishedStrategy stoppedFinishedStrategy = EStoppedFinishedStrategy.UnloadNone;
			_handles.Add(new BBDictionary<int, IAudioHandle>());
			_handles.Add(new BBDictionary<int, IAudioHandle>());
			int index = 0;
			_handles[index].Add(0, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_ground_crouch, loadStrategy, stoppedFinishedStrategy));
			_handles[index].Add(1, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_ground_walk, loadStrategy, stoppedFinishedStrategy));
			_handles[index].Add(2, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_ground_run, loadStrategy, stoppedFinishedStrategy));
			_handles[index].Add(3, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_ground_sprint, loadStrategy, stoppedFinishedStrategy));
			_handles[index].Add(4, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_ground_slide, loadStrategy, stoppedFinishedStrategy));
			_handles[index].Add(5, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_ground_stop, loadStrategy, stoppedFinishedStrategy));
			int index2 = 1;
			_handles[index2].Add(0, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_rooftop_crouch, loadStrategy, stoppedFinishedStrategy));
			_handles[index2].Add(1, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_rooftop_walk, loadStrategy, stoppedFinishedStrategy));
			_handles[index2].Add(2, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_rooftop_run, loadStrategy, stoppedFinishedStrategy));
			_handles[index2].Add(3, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_rooftop_sprint, loadStrategy, stoppedFinishedStrategy));
			_handles[index2].Add(4, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_rooftop_slide, loadStrategy, stoppedFinishedStrategy));
			_handles[index2].Add(5, AudioManager.Instance.CreateHandle(instance.ac_sfx_step_rooftop_stop, loadStrategy, stoppedFinishedStrategy));
		}
	}
}
