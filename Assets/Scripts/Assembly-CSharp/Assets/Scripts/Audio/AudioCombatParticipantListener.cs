using System.Collections.Generic;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Combat;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioCombatParticipantListener : IAnimatorEventListener
	{
		private static Dictionary<int, GameID> m_weaponSwooshTable;

		private readonly CombatParticipant m_participant;

		private AnimatorDataController m_animatorDataController;

		private IAudioHandle m_handleWeaponSwoosh;

		private WeaponHandler m_weaponHandler;

		private AudioSource m_audioSourceTemplate;

		private HumanoidDef.EArmorType m_cachedCurrentTargetArmorType;

		public AudioCombatParticipantListener(CombatParticipant _participant)
		{
			if (m_weaponSwooshTable == null)
			{
				m_weaponSwooshTable = new BBDictionary<int, GameID>();
				InitWeaponSwooshAudio();
			}
			m_participant = _participant;
			m_participant.OnCombatBehaviourTypeChanged += OnCombatBehaviourTypeChanged;
			m_participant.CurrentTargetChanged += CurrentTargetChanged;
			m_participant.AttackWasKillMoveChanged += AttackWasKillMoveChanged;
		}

		public void CurrentTargetChanged(CombatParticipant _old, CombatParticipant _new)
		{
			if (_new != null && !m_participant.AttackWasKillMove)
			{
				m_cachedCurrentTargetArmorType = CombatController.Instance.AudioController.GetArmorType(_new);
			}
		}

		public void AttackWasKillMoveChanged(bool _old, bool _new)
		{
			if (!_new && m_participant.CurrentTarget != null)
			{
				m_cachedCurrentTargetArmorType = CombatController.Instance.AudioController.GetArmorType(m_participant.CurrentTarget);
			}
		}

		public void Attach(AnimatorDataController _animatorDataContoller, WeaponHandler _weaponHandler, AudioSource _template)
		{
			m_animatorDataController = _animatorDataContoller;
			m_animatorDataController.RegisterEventListener(this);
			m_weaponHandler = _weaponHandler;
			if (m_weaponHandler != null)
			{
				m_weaponHandler.OnCurrentWeaponChanged += OnWeaponChanged;
				OnWeaponChanged(m_weaponHandler.CurrentWeaponDesc);
			}
			m_audioSourceTemplate = _template;
		}

		public void Detach()
		{
			m_animatorDataController.UnregisterEventListener(this);
			m_animatorDataController = null;
			if (m_weaponHandler != null)
			{
				m_weaponHandler.OnCurrentWeaponChanged -= OnWeaponChanged;
				m_weaponHandler = null;
			}
			m_handleWeaponSwoosh.UnloadAllNowOrOnDone();
			m_handleWeaponSwoosh = null;
			m_audioSourceTemplate = null;
		}

		public bool OnAnimatorEvent(AnimatorEvent _event)
		{
			switch (_event.Type)
			{
			case AnimatorEventType.SoundCombatSwoosh:
				m_handleWeaponSwoosh.Play(false, m_audioSourceTemplate);
				break;
			case AnimatorEventType.SoundCombatImpact:
			{
				if (!(m_participant.GameObject != null))
				{
					break;
				}
				WeaponHandler component = m_participant.GameObject.GetComponent<WeaponHandler>();
				WeaponHandler.WeaponDesc weaponDesc = ((!(component != null)) ? null : component.CurrentWeaponDesc);
				if (weaponDesc != null)
				{
					IAudioHandle handle = CombatController.Instance.AudioController.GetHandle(weaponDesc.AudioType, m_cachedCurrentTargetArmorType);
					if (handle != null)
					{
						handle.Load();
						handle.Play(false, m_audioSourceTemplate);
					}
				}
				break;
			}
			}
			return false;
		}

		private void OnWeaponChanged(WeaponHandler.WeaponDesc _old)
		{
			IAudioHandle handleWeaponSwoosh = m_handleWeaponSwoosh;
			m_handleWeaponSwoosh = AudioManager.Instance.CreateHandle(m_weaponSwooshTable[(int)m_weaponHandler.CurrentWeaponDesc.AudioType], ELoadStrategy.LoadAll, EStoppedFinishedStrategy.UnloadNone);
			m_handleWeaponSwoosh.Load();
			handleWeaponSwoosh.UnloadAllNowOrOnDone();
		}

		private void OnCombatBehaviourTypeChanged(ECombatBehaviourType _type, params object[] _params)
		{
			switch (_type)
			{
			case ECombatBehaviourType.Die:
				OnDie();
				break;
			case ECombatBehaviourType.GetHit:
				if (_params != null && _params.Length > 0)
				{
					CombatParticipant source2 = (CombatParticipant)_params[0];
					OnGetHit(source2);
				}
				break;
			case ECombatBehaviourType.Parry:
				if (_params != null && _params.Length > 0)
				{
					CombatParticipant source = (CombatParticipant)_params[0];
					OnParry(source);
				}
				break;
			}
		}

		private void OnDie()
		{
			IAudioHandle audioHandle = AudioManager.Instance.CreateHandle((!m_participant.IsPlayer) ? UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_enemy_die : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_player_die, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
			audioHandle.Load();
			audioHandle.Play(false, m_audioSourceTemplate);
			if (m_participant.IsPlayer)
			{
				AudioManager.Instance.EventManager.OnPlayerDie();
			}
		}

		private void OnGetHit(CombatParticipant _source)
		{
			if (!m_participant.Character.GetHasZeroHealth() && !_source.AttackWasKillMove)
			{
				IAudioHandle audioHandle = AudioManager.Instance.CreateHandle((!m_participant.IsPlayer) ? UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_enemy_takehit : UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_player_takehit, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				audioHandle.Load();
				audioHandle.Play(false, m_audioSourceTemplate);
			}
			if (_source.AttackWasKillMove)
			{
				return;
			}
			WeaponHandler.WeaponDesc weaponDesc = null;
			if (_source != null && _source.GameObject != null)
			{
				WeaponHandler component = _source.GameObject.GetComponent<WeaponHandler>();
				weaponDesc = ((!(component != null)) ? null : component.CurrentWeaponDesc);
			}
			if (weaponDesc != null)
			{
				IAudioHandle handle = CombatController.Instance.AudioController.GetHandle(weaponDesc.AudioType, CombatController.Instance.AudioController.GetArmorType(m_participant));
				if (handle != null)
				{
					handle.Load();
					handle.Play(false, m_audioSourceTemplate);
				}
			}
		}

		private void OnParry(CombatParticipant _source)
		{
			WeaponHandler.WeaponDesc weaponDesc = null;
			if (_source != null && _source.GameObject != null)
			{
				WeaponHandler component = _source.GameObject.GetComponent<WeaponHandler>();
				if (component != null)
				{
					weaponDesc = component.CurrentWeaponDesc;
				}
			}
			if (weaponDesc != null)
			{
				IAudioHandle handle = CombatController.Instance.AudioController.GetHandle(weaponDesc.AudioType, m_weaponHandler.CurrentWeaponDesc.AudioType);
				if (handle != null)
				{
					handle.Load();
					handle.Play(false, m_audioSourceTemplate);
				}
			}
		}

		private static void InitWeaponSwooshAudio()
		{
			m_weaponSwooshTable.Add(0, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_sword_whoosh);
			m_weaponSwooshTable.Add(1, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_blunt_whoosh);
			m_weaponSwooshTable.Add(2, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_knife_whoosh);
		}
	}
}
