using System;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.HUD;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.Ingame;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using UbiCore;
using UbiCore.Messenger;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.UI.Combat
{
	public class CombatUI : IUISubSystemLayout, IUiSubSystem
	{
		private const float StackOffsetFactor = 1.2f;

		private readonly BBDictionary<IGameEntity, BBList<ICombatAnnotation>> m_ActiveAnnotations = new BBDictionary<IGameEntity, BBList<ICombatAnnotation>>(5);

		private readonly BBList<ParticipantHandler> m_ParticipantHandlers = new BBList<ParticipantHandler>();

		private Vector2 m_HitVignette = new Vector2(0f, 0f);

		private int m_ParticipantsInFight;

		private readonly BBList<GameObject> m_ComboCounter = new BBList<GameObject>();

		private bool m_wasHidden = true;

		private Vignette m_vignette;

		public static CombatUI Instance { get; private set; }

		private bool IsInCombat { get; set; }

		public bool IsActive { get; private set; }

		public bool InitiallyVisible
		{
			get
			{
				return true;
			}
		}

		public Vector2 BorderOffset { get; set; }

		public ScreenAnchor ScreenAnchor { get; set; }

		public EUISubSystem SubSystem
		{
			get
			{
				return EUISubSystem.Combat;
			}
		}

		public CombatUI()
		{
			IsInCombat = false;
			Reset();
			IsActive = false;
			Instance = this;
		}

		public void UpdateFrame(float _deltaTime)
		{
			if (IsInCombat)
			{
				int participantsInFight = m_ParticipantsInFight;
				for (int i = 0; i < participantsInFight; i++)
				{
					m_ParticipantHandlers[i].Update(_deltaTime);
				}
			}
			if (m_vignette != null)
			{
				CombatUiData combatUiData = CombatController.Instance.CombatUiData;
				float _value = 0f;
				GetInterpolatedValue(_deltaTime, ref m_HitVignette, combatUiData.m_Vignette_Hit_Duration, out _value);
				m_vignette.SetVignetteColor(Vignette.Type.Hit, combatUiData.m_Vignette_Hit, _value);
			}
			foreach (BBList<ICombatAnnotation> value in m_ActiveAnnotations.Values)
			{
				float num = 0f;
				ICombatAnnotation combatAnnotation = null;
				for (int num2 = value.Count - 1; num2 >= 0; num2--)
				{
					ICombatAnnotation combatAnnotation2 = value[num2];
					if (combatAnnotation != null)
					{
						float num3 = combatAnnotation.gameObject.transform.localPosition.y - combatAnnotation.StackOffset + combatAnnotation.Height / 2f;
						float num4 = combatAnnotation2.gameObject.transform.localPosition.y - combatAnnotation2.StackOffset - combatAnnotation2.Height / 2f;
						num += (num3 - num4) * 1.2f;
						num = (combatAnnotation2.StackOffset = Mathf.Clamp(num, 0f, Screen.height));
					}
					combatAnnotation = combatAnnotation2;
				}
			}
		}

		public bool Tap(TapEvent _event)
		{
			return false;
		}

		public void OnMissionLoad()
		{
		}

		public void OnMissionLoaded()
		{
			IsActive = UbiCore.Utils.Singleton<UiMain>.Instance.MissionType != EMissionType.Animus_EMissionType;
			if (IsActive)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<PlayerStateChangeEvent>(OnPlayerStateChange);
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<CombatUiEvent>(OnCombatUiEvent);
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<CombatKillTokenUiEvent>(OnCombatKillTokenUiEvent);
				IconDisplay.Preload();
				MissionTargetIndicatorRenderer.Preload();
				CombatAnnotation.Preload();
				HealthBar.Preload();
				m_vignette = UnityEngine.Camera.main.GetComponent<Vignette>();
			}
			else
			{
				m_vignette = null;
			}
		}

		public void OnMissionUnload()
		{
			SetComboTokensActive(false);
			foreach (BBList<ICombatAnnotation> value in m_ActiveAnnotations.Values)
			{
				while (value.Count > 0)
				{
					value[0].Release();
				}
			}
			m_ActiveAnnotations.Clear();
			for (int i = 0; i < m_ParticipantHandlers.Count; i++)
			{
				m_ParticipantHandlers[i].CombatParticipant = null;
			}
			AdjustComboCounterAmount(0);
			Reset();
			m_vignette = null;
			if (IsActive)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<CombatUiEvent>(OnCombatUiEvent);
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<PlayerStateChangeEvent>(OnPlayerStateChange);
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<CombatKillTokenUiEvent>(OnCombatKillTokenUiEvent);
				IconDisplay.Unload();
				MissionTargetIndicatorRenderer.Unload();
				CombatAnnotation.Unload();
				HealthBar.Unload();
				IsActive = false;
			}
		}

		public void OnMissionUnloaded()
		{
		}

		public void OnMissionOutroStarted()
		{
		}

		public bool IsOnUi(Vector2 _mousePosition)
		{
			return false;
		}

		public void OnSceneLoaded()
		{
		}

		public void OnSceneUnload()
		{
		}

		public void OnCameraCreated()
		{
		}

		public void OnSharedUILoaded()
		{
		}

		public void OnUIAlignmentChanged(bool _isRightHanded)
		{
		}

		public void OnUIReadyToUse()
		{
		}

		public void SetVisible(bool _value)
		{
		}

		public void OnMissionEnded()
		{
			SetComboTokensActive(false);
		}

		private bool GetInterpolatedValue(float _deltaTime, ref Vector2 _val, float _total, out float _value)
		{
			_value = 0f;
			bool result = false;
			if (_val.y > 0f)
			{
				float num = Interpolate.Ease01(CombatController.Instance.CombatUiData.m_Vignette_Ease, _val.y / _total);
				_value = _val.x * num;
				_val.y = Math.Max(_val.y - _deltaTime, 0f);
				result = MathHelper.Approximately(_val.y, 0f);
			}
			return result;
		}

		private void OnPlayerStateChange(PlayerStateChangeEvent _event)
		{
			EPlayerState playerState = _event.PlayerState;
			if (playerState == EPlayerState.Combat)
			{
				IsInCombat = true;
				return;
			}
			IsInCombat = false;
			for (int i = 0; i < m_ParticipantHandlers.Count; i++)
			{
				m_ParticipantHandlers[i].CombatParticipant = null;
			}
		}

		private void OnCombatUiEvent(CombatUiEvent _event)
		{
			if (_event.Target != null && (_event.Target.Id == EntityId.InvalidId || !_event.Target.IsAttached))
			{
				return;
			}
			switch (_event.Type)
			{
			case CombatUiEvent.EType.DodgeNormal:
				AddAnnotation(CombatAnnotation.GetLocalizationKey(_event, CombatAnnotation.Type.DodgedNormal));
				break;
			case CombatUiEvent.EType.DodgeMiss:
				AddAnnotation(CombatAnnotation.GetLocalizationKey(_event, CombatAnnotation.Type.DodgedMiss));
				break;
			case CombatUiEvent.EType.KillMoveDeflect:
				AddAnnotation(CombatAnnotation.GetLocalizationKey(_event, CombatAnnotation.Type.KillMoveDeflect));
				break;
			case CombatUiEvent.EType.Timer:
				AddAnnotation(CombatAnnotation.GetNumber(_event, _event.Delta, CombatAnnotation.Type.Timer));
				break;
			case CombatUiEvent.EType.SoftCurrency:
				AddAnnotation(CombatAnnotation.GetNumber(_event, _event.Delta, CombatAnnotation.Type.SoftCurrency));
				break;
			case CombatUiEvent.EType.Heal:
				AddAnnotation(CombatAnnotation.GetNumber(_event, _event.Delta, CombatAnnotation.Type.Heal));
				break;
			case CombatUiEvent.EType.MissionGoalLoot:
				break;
			case CombatUiEvent.EType.SkillAvailable:
				if (_event.SkillType.Name == "Disguise_01")
				{
					AddAnnotation(CombatAnnotation.GetLocalizationKey(_event, CombatAnnotation.Type.SkillAvailable));
				}
				break;
			case CombatUiEvent.EType.SkillUnavailable:
				if (_event.SkillType.Name == "Disguise_01")
				{
					AddAnnotation(CombatAnnotation.GetLocalizationKey(_event, CombatAnnotation.Type.SkillUnavailable));
				}
				break;
			case CombatUiEvent.EType.Hit:
			{
				CombatAnnotation.Type type = CombatAnnotation.Type.Damage;
				if ((_event.Flags & Assets.Scripts.Mission.Logic.Components.LogicInterface.ECombatEventModifierFlags.Critical) > Assets.Scripts.Mission.Logic.Components.LogicInterface.ECombatEventModifierFlags.None)
				{
					type = CombatAnnotation.Type.CriticalDamage;
				}
				AddAnnotation(CombatAnnotation.GetNumber(_event, _event.Delta, type));
				if (UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.PlayerId == _event.Target.Id && _event.Delta > 0f)
				{
					float num = _event.Target.EntityStats.Get_float(StatTypes.EStatType.Character_Conflict_ActualMaxHealth);
					float value = _event.Delta / num;
					CombatUiData combatUiData = CombatController.Instance.CombatUiData;
					m_HitVignette.x = Mathf.Clamp(value, combatUiData.m_Vignette_Hit_Min, 1f);
					m_HitVignette.y = combatUiData.m_Vignette_Hit_Duration;
				}
				break;
			}
			case CombatUiEvent.EType.Enter:
				m_ParticipantsInFight++;
				if (m_ParticipantsInFight > m_ParticipantHandlers.Count)
				{
					m_ParticipantHandlers.Add(new ParticipantHandler(this));
				}
				m_ParticipantHandlers[m_ParticipantsInFight - 1].CombatParticipant = _event.Target.Cast<IParticipantCarrier>().CombatParticipantProxy;
				break;
			case CombatUiEvent.EType.Exit:
			{
				for (int i = 0; i < m_ParticipantsInFight; i++)
				{
					if (m_ParticipantHandlers[i].CombatParticipant != null && m_ParticipantHandlers[i].CombatParticipant.Character == _event.Target)
					{
						m_ParticipantHandlers.RemoveAt(i);
						m_ParticipantsInFight--;
						break;
					}
				}
				break;
			}
			case CombatUiEvent.EType.Select:
			case CombatUiEvent.EType.PrepareAttack:
				break;
			}
		}

		private void OnCombatKillTokenUiEvent(CombatKillTokenUiEvent _event)
		{
		}

		private void AdjustComboCounterAmount(int _amount)
		{
			if (m_ComboCounter.Count == _amount)
			{
				return;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag("UiIconCamera");
			if (gameObject == null)
			{
				return;
			}
			GameObject gameObject2 = gameObject.FindDirectChildByName(Assets.Scripts.UI.Ingame.Constants.IconPoolTag);
			if (m_ComboCounter.Count > _amount)
			{
				for (int i = _amount; i < m_ComboCounter.Count; i++)
				{
					GameObject obj = m_ComboCounter[i];
					UnityEngine.Object.Destroy(obj);
				}
				while (m_ComboCounter.Count > _amount)
				{
					m_ComboCounter.RemoveAt(_amount);
				}
				return;
			}
			for (int j = m_ComboCounter.Count; j < _amount; j++)
			{
				GameObject gameObject3 = GameObject.CreatePrimitive(PrimitiveType.Quad);
				UnityEngine.Object.Destroy(gameObject3.GetComponent<Collider>());
				gameObject3.layer = Layers.NonInteractableUiLayer;
				gameObject3.GetComponent<Renderer>().material = UbiCore.Utils.Singleton<IconManager>.Instance.GetMaterial(IconMaterial.InteractAssassinate);
				float num = 0.05f * (float)Mathf.Min(Screen.width, Screen.height);
				gameObject3.transform.localScale = new Vector3(num, num, 1f);
				gameObject3.transform.parent = gameObject2.transform;
				m_ComboCounter.Add(gameObject3);
			}
		}

		private void SetComboTokensActive(bool _value)
		{
			for (int i = 0; i < m_ComboCounter.Count; i++)
			{
				GameObject gameObject = m_ComboCounter[i];
				if (gameObject != null)
				{
					gameObject.SetActive(_value);
				}
			}
		}

		private void UpdateComboTokens()
		{
			CombatController instance = CombatController.Instance;
			int num = LogicMain.PlayerEntity.PlayerStats.Get_int(StatTypes.EStatType.Player_KillTokenCount);
			bool flag = (num <= 0 && !IsInCombat) || !instance.EnableKillTokenKill;
			if (m_wasHidden && flag)
			{
				return;
			}
			AdjustComboCounterAmount(instance.KillTokenCountRequired);
			if (flag)
			{
				m_wasHidden = true;
				SetComboTokensActive(false);
				return;
			}
			if (m_wasHidden)
			{
				m_wasHidden = false;
				SetComboTokensActive(true);
			}
			Vector2 b = new Vector2(Screen.width, Screen.height);
			Vector3 position = LogicMain.PlayerEntity.Position;
			position = Vector3.Scale(UnityEngine.Camera.main.WorldToScreenPoint(position), new Vector3(1f / (float)UnityEngine.Camera.main.pixelWidth, 1f / (float)UnityEngine.Camera.main.pixelHeight, 1f));
			position = Vector2.Scale(position, b);
			for (int i = 0; i < m_ComboCounter.Count; i++)
			{
				GameObject gameObject = m_ComboCounter[i];
				if (gameObject.GetComponent<Renderer>().material.HasProperty("_Color"))
				{
					Color color = gameObject.GetComponent<Renderer>().material.color;
					color.a = ((i >= num) ? 0.2f : 1f);
					gameObject.GetComponent<Renderer>().material.color = color;
				}
				else
				{
					gameObject.SetActive(i < num);
				}
				gameObject.transform.localPosition = new Vector3(position.x - gameObject.transform.localScale.x * 0.5f - ((float)(instance.KillTokenCountRequired - 1) * 0.5f - (float)i) * gameObject.transform.localScale.x, position.y - gameObject.transform.localScale.y * 0.5f, 0f);
			}
		}

		private void Reset()
		{
			m_HitVignette.Set(0f, 0f);
			m_ParticipantsInFight = 0;
		}

		private void AddAnnotation(ICombatAnnotation _annotation)
		{
			_annotation.OnRelease += OnAnnotationRelease;
			IGameEntity target = _annotation.Target;
			BBList<ICombatAnnotation> value = null;
			if (!m_ActiveAnnotations.TryGetValue(target, out value))
			{
				value = new BBList<ICombatAnnotation>(5);
				m_ActiveAnnotations[target] = value;
			}
			value.Add(_annotation);
		}

		private void OnAnnotationRelease(ICombatAnnotation _o)
		{
			BBList<ICombatAnnotation> value = null;
			if (m_ActiveAnnotations.TryGetValue(_o.Target, out value))
			{
				value.Remove(_o);
			}
			_o.OnRelease -= OnAnnotationRelease;
		}
	}
}
