using System.Collections.Generic;
using Assets.Scripts.Mission.Logic.Combat;
using Database;
using SharedBaseLib.Core;
using UbiCore.Debug;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.Audio
{
	public class AudioCombatController
	{
		public enum EAudioWeaponHitType
		{
			ArmorLight = 0,
			ArmorHeavy = 1,
			ParrySlash = 2,
			ParryBlunt = 3,
			ParryStab = 4
		}

		private class PreloadHandler
		{
			private IAudioLoadHandle m_handle;

			public EAudioWeaponType WeaponType;

			public EAudioWeaponHitType WeaponHitType;

			public int RefCount { get; private set; }

			public PreloadHandler(EAudioWeaponType _weaponType, EAudioWeaponHitType _weaponHitType, IAudioLoadHandle _handle)
			{
				RefCount = 0;
				WeaponType = _weaponType;
				WeaponHitType = _weaponHitType;
				m_handle = _handle;
			}

			public void IncreaseRefCount()
			{
				if (RefCount == 0)
				{
					m_handle.Load();
				}
				RefCount++;
			}

			public void DecreaseRefCount()
			{
				RefCount = Mathf.Max(RefCount - 1, 0);
				if (RefCount == 0)
				{
					m_handle.Unload();
				}
			}

			public void Release()
			{
				RefCount = 0;
				m_handle.Unload();
			}
		}

		private class WeaponChangedCallbackHandler
		{
			private AudioCombatController m_audioCtrl;

			private WeaponHandler m_weaponHandler;

			public CombatParticipant Participant { get; private set; }

			public WeaponChangedCallbackHandler(AudioCombatController _audioCtrl, CombatParticipant _participant)
			{
				m_audioCtrl = _audioCtrl;
				Participant = _participant;
				m_weaponHandler = Participant.GameObject.GetComponent<WeaponHandler>();
				if (m_weaponHandler != null)
				{
					m_weaponHandler.OnCurrentWeaponChanged += OnWeaponChanged;
				}
			}

			public void Release()
			{
				if (m_weaponHandler != null)
				{
					m_weaponHandler.OnCurrentWeaponChanged -= OnWeaponChanged;
				}
			}

			private void OnWeaponChanged(WeaponHandler.WeaponDesc _old)
			{
				HumanoidDef.EArmorType armorType = m_audioCtrl.GetArmorType(Participant);
				m_audioCtrl.Load(m_audioCtrl.GetWeaponType(Participant), armorType, Participant);
				m_audioCtrl.Unload(_old.AudioType, armorType, Participant);
			}
		}

		private static Dictionary<int, Dictionary<int, GameID>> m_weaponHitTable;

		private readonly List<WeaponChangedCallbackHandler> m_weaponChangedHandlers = new List<WeaponChangedCallbackHandler>();

		private readonly List<PreloadHandler> m_preloadHandlers = new List<PreloadHandler>();

		private readonly List<CombatParticipant> m_participants = new List<CombatParticipant>();

		public AudioCombatController()
		{
			if (m_weaponHitTable == null)
			{
				m_weaponHitTable = new BBDictionary<int, Dictionary<int, GameID>>();
				InitWeaponHitAudio();
			}
		}

		public void OnParticipantEnter(CombatParticipant _participant)
		{
			m_weaponChangedHandlers.Add(new WeaponChangedCallbackHandler(this, _participant));
			Load(GetWeaponType(_participant), GetArmorType(_participant), _participant);
			m_participants.Add(_participant);
		}

		public void OnParticipantExit(CombatParticipant _participant)
		{
			for (int i = 0; i < m_weaponChangedHandlers.Count; i++)
			{
				WeaponChangedCallbackHandler weaponChangedCallbackHandler = m_weaponChangedHandlers[i];
				if (weaponChangedCallbackHandler.Participant == _participant)
				{
					weaponChangedCallbackHandler.Release();
					m_weaponChangedHandlers.RemoveAt(i);
					break;
				}
			}
			Unload(GetWeaponType(_participant), GetArmorType(_participant), _participant);
			m_participants.Remove(_participant);
		}

		public HumanoidDef.EArmorType GetArmorType(CombatParticipant _participant)
		{
			HumanoidDef item = Main.SafeGameDb.GetItem<HumanoidDef>(_participant.Character.CharacterDefinition.Definition.HumanoidGameID);
			return (item != null) ? item.ArmorType : HumanoidDef.EArmorType.LightLeather;
		}

		public IAudioHandle GetHandle(EAudioWeaponType _weaponType, EAudioWeaponType _weaponParryType)
		{
			return GetHandle(_weaponType, WeaponHitTypeFromWeapon(_weaponParryType));
		}

		public IAudioHandle GetHandle(EAudioWeaponType _weaponType, HumanoidDef.EArmorType _armorType)
		{
			return GetHandle(_weaponType, WeaponHitTypeFromArmor(_armorType));
		}

		public void Release()
		{
			for (int i = 0; i < m_preloadHandlers.Count; i++)
			{
				PreloadHandler preloadHandler = m_preloadHandlers[i];
				preloadHandler.Release();
			}
			m_preloadHandlers.Clear();
		}

		private void Load(EAudioWeaponType _participantWeaponType, HumanoidDef.EArmorType _participantArmorType, CombatParticipant _participant)
		{
			Profiling.Start("AudioCombatController::Load()", float.MaxValue);
			EAudioWeaponHitType weaponHitType = WeaponHitTypeFromWeapon(_participantWeaponType);
			EAudioWeaponHitType weaponHitType2 = WeaponHitTypeFromArmor(_participantArmorType);
			for (int i = 0; i < m_participants.Count; i++)
			{
				CombatParticipant combatParticipant = m_participants[i];
				if (_participant.IsPlayer != combatParticipant.IsPlayer)
				{
					EAudioWeaponType weaponType = GetWeaponType(combatParticipant);
					HumanoidDef.EArmorType armorType = GetArmorType(combatParticipant);
					EAudioWeaponHitType weaponHitType3 = WeaponHitTypeFromWeapon(weaponType);
					EAudioWeaponHitType weaponHitType4 = WeaponHitTypeFromArmor(armorType);
					IncreasePreloadRef(_participantWeaponType, weaponHitType3);
					IncreasePreloadRef(_participantWeaponType, weaponHitType4);
					IncreasePreloadRef(weaponType, weaponHitType);
					IncreasePreloadRef(weaponType, weaponHitType2);
				}
			}
			Profiling.Stop();
		}

		private void Unload(EAudioWeaponType _participantWeaponType, HumanoidDef.EArmorType _participantArmorType, CombatParticipant _participant)
		{
			Profiling.Start("AudioCombatController::Unload()", float.MaxValue);
			EAudioWeaponHitType weaponHitType = WeaponHitTypeFromWeapon(_participantWeaponType);
			EAudioWeaponHitType weaponHitType2 = WeaponHitTypeFromArmor(_participantArmorType);
			for (int i = 0; i < m_participants.Count; i++)
			{
				CombatParticipant combatParticipant = m_participants[i];
				if (_participant.IsPlayer != combatParticipant.IsPlayer)
				{
					EAudioWeaponType weaponType = GetWeaponType(combatParticipant);
					HumanoidDef.EArmorType armorType = GetArmorType(combatParticipant);
					EAudioWeaponHitType weaponHitType3 = WeaponHitTypeFromWeapon(weaponType);
					EAudioWeaponHitType weaponHitType4 = WeaponHitTypeFromArmor(armorType);
					DecreasePreloadRef(_participantWeaponType, weaponHitType3);
					DecreasePreloadRef(_participantWeaponType, weaponHitType4);
					DecreasePreloadRef(weaponType, weaponHitType);
					DecreasePreloadRef(weaponType, weaponHitType2);
				}
			}
			Profiling.Stop();
		}

		private void IncreasePreloadRef(EAudioWeaponType _weaponType, EAudioWeaponHitType _weaponHitType)
		{
			PreloadHandler preload = GetPreload(_weaponType, _weaponHitType);
			if (preload != null)
			{
				preload.IncreaseRefCount();
			}
		}

		private void DecreasePreloadRef(EAudioWeaponType _weaponType, EAudioWeaponHitType _weaponHitType)
		{
			PreloadHandler preload = GetPreload(_weaponType, _weaponHitType);
			if (preload != null)
			{
				preload.DecreaseRefCount();
			}
		}

		private PreloadHandler GetPreload(EAudioWeaponType _weaponType, EAudioWeaponHitType _weaponHitType)
		{
			for (int i = 0; i < m_preloadHandlers.Count; i++)
			{
				PreloadHandler preloadHandler = m_preloadHandlers[i];
				if (preloadHandler.WeaponType == _weaponType && preloadHandler.WeaponHitType == _weaponHitType)
				{
					return preloadHandler;
				}
			}
			GameID audioControllerId = GetAudioControllerId(_weaponType, _weaponHitType);
			if (GameID.IsValid(audioControllerId))
			{
				PreloadHandler preloadHandler2 = new PreloadHandler(_weaponType, _weaponHitType, AudioManager.Instance.CreateLoadHandle(audioControllerId));
				m_preloadHandlers.Add(preloadHandler2);
				return preloadHandler2;
			}
			return null;
		}

		private EAudioWeaponHitType WeaponHitTypeFromWeapon(EAudioWeaponType _weaponType)
		{
			EAudioWeaponHitType eAudioWeaponHitType = EAudioWeaponHitType.ParrySlash;
			switch (_weaponType)
			{
			case EAudioWeaponType.Slash:
				return EAudioWeaponHitType.ParrySlash;
			case EAudioWeaponType.Stab:
				return EAudioWeaponHitType.ParryStab;
			case EAudioWeaponType.Blunt:
				return EAudioWeaponHitType.ParryBlunt;
			default:
				return EAudioWeaponHitType.ParrySlash;
			}
		}

		private EAudioWeaponHitType WeaponHitTypeFromArmor(HumanoidDef.EArmorType _armorType)
		{
			switch (_armorType)
			{
			case HumanoidDef.EArmorType.LightLeather:
			case HumanoidDef.EArmorType.Leather:
			case HumanoidDef.EArmorType.BoiledLeather:
			case HumanoidDef.EArmorType.StuddedLeather:
			case HumanoidDef.EArmorType.Scalemail:
			case HumanoidDef.EArmorType.Brigandine:
			case HumanoidDef.EArmorType.Chainmail:
				return EAudioWeaponHitType.ArmorLight;
			case HumanoidDef.EArmorType.Plate:
				return EAudioWeaponHitType.ArmorHeavy;
			default:
				return EAudioWeaponHitType.ArmorLight;
			}
		}

		private EAudioWeaponType GetWeaponType(CombatParticipant _participant)
		{
			WeaponHandler.WeaponDesc weaponDesc = null;
			if (_participant != null && _participant.GameObject != null)
			{
				WeaponHandler component = _participant.GameObject.GetComponent<WeaponHandler>();
				if (component != null)
				{
					weaponDesc = component.CurrentWeaponDesc;
				}
			}
			if (weaponDesc != null)
			{
				return weaponDesc.AudioType;
			}
			return EAudioWeaponType.Slash;
		}

		private IAudioHandle GetHandle(EAudioWeaponType _weaponType, EAudioWeaponHitType _weaponHitType)
		{
			if (m_weaponHitTable.ContainsKey((int)_weaponType))
			{
				Dictionary<int, GameID> dictionary = m_weaponHitTable[(int)_weaponType];
				if (dictionary.ContainsKey((int)_weaponHitType))
				{
					GameID eController = dictionary[(int)_weaponHitType];
					return AudioManager.Instance.CreateHandle(eController, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				}
			}
			return null;
		}

		private GameID GetAudioControllerId(EAudioWeaponType _weaponType, EAudioWeaponHitType _weaponHitType)
		{
			if (m_weaponHitTable.ContainsKey((int)_weaponType))
			{
				Dictionary<int, GameID> dictionary = m_weaponHitTable[(int)_weaponType];
				if (dictionary.ContainsKey((int)_weaponHitType))
				{
					return dictionary[(int)_weaponHitType];
				}
			}
			return GameID.Invalid;
		}

		private static void InitWeaponHitAudio()
		{
			BBDictionary<int, GameID> bBDictionary = new BBDictionary<int, GameID>();
			bBDictionary.Add(0, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_sword_slash_impact_body);
			bBDictionary.Add(1, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_sword_slash_impact_armour);
			bBDictionary.Add(2, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_sword_slash_impact_sword);
			bBDictionary.Add(3, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_sword_slash_impact_blunt);
			bBDictionary.Add(4, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_sword_slash_impact_knife);
			m_weaponHitTable.Add(0, bBDictionary);
			BBDictionary<int, GameID> bBDictionary2 = new BBDictionary<int, GameID>();
			bBDictionary2.Add(0, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_knife_slash_impact_body);
			bBDictionary2.Add(1, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_knife_slash_impact_armour);
			bBDictionary2.Add(2, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_knife_slash_impact_sword);
			bBDictionary2.Add(3, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_knife_slash_impact_blunt);
			bBDictionary2.Add(4, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_knife_slash_impact_knife);
			m_weaponHitTable.Add(2, bBDictionary2);
			BBDictionary<int, GameID> bBDictionary3 = new BBDictionary<int, GameID>();
			bBDictionary3.Add(0, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_blunt_impact_body);
			bBDictionary3.Add(1, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_blunt_impact_armour);
			bBDictionary3.Add(2, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_blunt_impact_sword);
			bBDictionary3.Add(3, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_blunt_impact_blunt);
			bBDictionary3.Add(4, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_blunt_impact_knife);
			m_weaponHitTable.Add(1, bBDictionary3);
			BBDictionary<int, GameID> bBDictionary4 = new BBDictionary<int, GameID>();
			bBDictionary4.Add(0, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_arrow_impact_body);
			bBDictionary4.Add(1, UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_sfx_wpn_arrow_impact_armour);
			m_weaponHitTable.Add(3, bBDictionary4);
		}
	}
}
