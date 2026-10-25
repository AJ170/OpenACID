using System.Collections.Generic;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI;
using SharedBaseLib.Core;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIKillTokenBarBehavior : UIBaseBehaviour
	{
		public static bool AttachToCharacter;

		public static bool HideFirstToken = true;

		public float DisplayDamping = 0.1f;

		public float DisplayInitialDelay = 0.25f;

		public float DisplayNextDelay = 0.125f;

		public float DisplayProgress;

		public float DisableDelay = 1f;

		public float PositionDamping = 0.1f;

		public float TokenWidth = 194f;

		public int MaxTokens = 3;

		public bool GroupTokenEvents;

		public float EndIconDamping = 0.1f;

		[SerializeField]
		private Camera m_camera;

		[SerializeField]
		private UIWidget m_tokenBarWidget;

		[SerializeField]
		private List<UISlider> m_tokenSlider;

		[SerializeField]
		private List<UISprite> m_tokenForeground;

		[SerializeField]
		private List<ParticleSystem> m_tokenSplatterStartEffect;

		[SerializeField]
		private List<Animator> m_tokenAnimator;

		[SerializeField]
		private Transform m_centerTransform;

		[SerializeField]
		private ParticleSystem m_barFullFX;

		[SerializeField]
		private ParticleSystem m_triggerFX;

		[SerializeField]
		private ParticleSystem m_breakFX;

		[SerializeField]
		private ParticleSystem m_hideFX;

		[SerializeField]
		private ParticleSystem m_showFX;

		[SerializeField]
		private UISprite m_barFullForeground;

		private int m_currentTokensDisplayed;

		private float m_currentTokenUpdateTime;

		private bool m_currentTokenDelayed;

		private Vector3 m_initialPosition;

		private Vector3 m_currentPosition;

		private bool m_cachedAttached;

		private BBSimpleList<CombatKillTokenUiEvent> m_tokenEventList = new BBSimpleList<CombatKillTokenUiEvent>();

		private int m_currentTokens;

		private bool m_isFull;

		private bool m_wasTriggered;

		[SerializeField]
		private bool m_displayed;

		private float m_disableTime;

		private BBSimpleList<float> m_tokenFillRatio = new BBSimpleList<float>();

		private BBSimpleList<float> m_tokenFillDelta = new BBSimpleList<float>();

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.KillTokenBar;
			}
		}

		public void Start()
		{
			for (int i = 0; i < m_tokenAnimator.Count; i++)
			{
				Animator animator = m_tokenAnimator[i];
				animator.StopPlayback();
			}
			m_initialPosition = base.transform.localPosition;
			if ((bool)m_triggerFX)
			{
				m_triggerFX.GetComponent<Renderer>().sortingLayerName = "Foreground";
				m_triggerFX.GetComponent<Renderer>().sortingOrder = 2;
			}
			if ((bool)m_breakFX)
			{
				m_breakFX.GetComponent<Renderer>().sortingLayerName = "Foreground";
				m_breakFX.GetComponent<Renderer>().sortingOrder = 2;
			}
			if ((bool)m_barFullFX)
			{
				m_barFullFX.GetComponent<Renderer>().sortingLayerName = "Foreground";
				m_barFullFX.GetComponent<Renderer>().sortingOrder = 2;
			}
			Reset();
		}

		public void Reset()
		{
			m_currentTokensDisplayed = 0;
			m_currentTokenUpdateTime = 0f;
			m_currentTokenDelayed = false;
			m_cachedAttached = false;
			m_tokenEventList.Clear();
			m_currentTokens = 0;
			m_isFull = false;
			m_wasTriggered = false;
			m_displayed = false;
			m_disableTime = 0f;
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Exclude()
		{
			base.gameObject.SetActive(false);
		}

		public void OnCombatKillTokenUiEvent(CombatKillTokenUiEvent _event)
		{
			int value = _event.Value;
			if (m_currentTokens != value)
			{
				int num = value - m_currentTokens;
				if (GroupTokenEvents)
				{
					m_tokenEventList.Add(_event);
				}
				else
				{
					int num2 = Mathf.RoundToInt(Mathf.Sign(num));
					for (int i = m_currentTokens; i != value; i += num2)
					{
						m_tokenEventList.Add(new CombatKillTokenUiEvent(_event.Type, i + num2));
					}
				}
				m_currentTokens = value;
			}
			else
			{
				m_tokenEventList.Add(_event);
			}
		}

		public bool UpdateProgress()
		{
			MaxTokens = ((LogicMain.PlayerEntity != null) ? LogicMain.PlayerEntity.PlayerStats.Get_int(StatTypes.EStatType.Player_KillTokenRequiredForKill) : 0);
			int num = ((LogicMain.PlayerEntity != null) ? LogicMain.PlayerEntity.PlayerStats.Get_int(StatTypes.EStatType.Player_KillTokenCount) : 0);
			if (m_currentTokens != num)
			{
				int num2 = num - m_currentTokens;
				if (GroupTokenEvents)
				{
					m_tokenEventList.Add(new CombatKillTokenUiEvent((num2 <= 0) ? CombatKillTokenUiEvent.EType.Lost : CombatKillTokenUiEvent.EType.Gain, num));
				}
				else
				{
					int num3 = Mathf.RoundToInt(Mathf.Sign(num2));
					for (int i = m_currentTokens; i != num; i += num3)
					{
						m_tokenEventList.Add(new CombatKillTokenUiEvent((num2 <= 0) ? CombatKillTokenUiEvent.EType.Lost : CombatKillTokenUiEvent.EType.Gain, i + num3));
					}
				}
				m_currentTokens = num;
			}
			while (m_tokenEventList.Count > 0)
			{
				if (!m_currentTokenDelayed)
				{
					m_currentTokenDelayed = true;
					m_currentTokenUpdateTime = LogicMain.CurrentTime + DisplayInitialDelay;
				}
				if (!m_currentTokenDelayed || !(LogicMain.CurrentTime >= m_currentTokenUpdateTime))
				{
					break;
				}
				CombatKillTokenUiEvent combatKillTokenUiEvent = m_tokenEventList[0];
				m_tokenEventList.RemoveAt(0);
				m_currentTokensDisplayed = combatKillTokenUiEvent.Value;
				m_wasTriggered = combatKillTokenUiEvent.Type == CombatKillTokenUiEvent.EType.Spend;
				if (combatKillTokenUiEvent.Type == CombatKillTokenUiEvent.EType.EnterKillTokenMode && !m_displayed)
				{
					if (!base.gameObject.activeSelf)
					{
						Show();
					}
					m_displayed = true;
					if ((bool)m_showFX)
					{
						m_showFX.Play();
					}
				}
				if (combatKillTokenUiEvent.Type == CombatKillTokenUiEvent.EType.ExitKillTokenMode && m_displayed)
				{
					m_displayed = false;
					if ((bool)m_hideFX)
					{
						m_hideFX.Play();
					}
				}
				if (m_tokenEventList.Count > 0)
				{
					m_currentTokenDelayed = true;
					m_currentTokenUpdateTime = LogicMain.CurrentTime + DisplayNextDelay;
					continue;
				}
				m_currentTokenDelayed = false;
				break;
			}
			DisplayProgress = Mathf.Lerp(DisplayProgress, m_currentTokensDisplayed, LogicMain.FrameTime / (LogicMain.FrameTime + DisplayDamping));
			return m_displayed;
		}

		public void LateUpdate()
		{
			if (!m_displayed && LogicMain.CurrentTime > m_disableTime)
			{
				Exclude();
				return;
			}
			if (m_displayed)
			{
				m_disableTime = LogicMain.CurrentTime + DisableDelay;
			}
			int count = m_tokenSlider.Count;
			int num = count - MaxTokens;
			for (int i = 0; i < count; i++)
			{
				UISlider uISlider = m_tokenSlider[i];
				bool flag = i >= num;
				uISlider.alpha = (flag ? 1 : 0);
				if (flag)
				{
					while (m_tokenFillRatio.Count <= i)
					{
						m_tokenFillRatio.Add(0f);
					}
					while (m_tokenFillDelta.Count <= i)
					{
						m_tokenFillDelta.Add(-1f);
					}
					float num2 = Mathf.Clamp01((float)(num - i) + DisplayProgress);
					float num3 = m_tokenFillRatio[i];
					float num4 = num2 - num3;
					float num5 = m_tokenFillDelta[i];
					m_tokenFillRatio[i] = num2;
					if (Mathf.Abs(num4) > float.Epsilon)
					{
						m_tokenFillDelta[i] = num4;
					}
					if (num4 > 0f && num5 < 0f)
					{
						OnTokenTriggerFill(i, num2);
					}
					if (num4 < 0f && num5 > 0f)
					{
						OnTokenTriggerEmpty(i, num2);
					}
					OnTokenUpdate(i, num2);
				}
			}
			bool flag2 = DisplayProgress >= (float)MaxTokens - 0.1f;
			if (m_barFullFX != null)
			{
				if (!m_isFull && flag2)
				{
					m_barFullFX.Play();
				}
				else if (m_isFull && !flag2)
				{
					m_barFullFX.Stop();
				}
			}
			if ((bool)m_triggerFX && m_isFull && !flag2 && m_wasTriggered)
			{
				m_triggerFX.Emit((int)m_triggerFX.emissionRate);
			}
			if ((bool)m_breakFX)
			{
			}
			if (m_barFullForeground != null)
			{
				m_barFullForeground.alpha = Damping.Damp(m_barFullForeground.alpha, (!flag2) ? 0f : 1f, EndIconDamping, 1f);
			}
			m_isFull = flag2;
			if (m_centerTransform != null)
			{
				float num6 = TokenWidth * (float)num * 0.5f;
				m_centerTransform.localPosition = new Vector3(0f - num6, 0f, 0f);
			}
			if (AttachToCharacter && LogicMain.PlayerEntity != null)
			{
				Vector2 vector = Camera.main.WorldToScreenPoint(LogicMain.PlayerEntity.Position);
				Vector3 to = m_camera.ScreenToWorldPoint(vector);
				if (!m_cachedAttached)
				{
					m_currentPosition = m_tokenBarWidget.transform.position;
				}
				m_currentPosition = Damping.Damp(m_currentPosition, to, PositionDamping, 1f);
				m_tokenBarWidget.transform.position = m_currentPosition;
				m_cachedAttached = true;
			}
			else if (m_cachedAttached)
			{
				base.transform.localPosition = m_initialPosition;
				m_cachedAttached = false;
			}
		}

		private void OnTokenTriggerEmpty(int i, float _targetValue)
		{
			if (!Singleton<EffectManager>.Instance.HasCondition(EffectManager.ECondition.ReducedGore))
			{
				m_tokenAnimator[i].Play("disappear");
			}
		}

		private void OnTokenTriggerFill(int i, float _targetValue)
		{
			if (!Singleton<EffectManager>.Instance.HasCondition(EffectManager.ECondition.ReducedGore))
			{
				m_tokenAnimator[i].Play("appear");
				m_tokenSplatterStartEffect[i].Emit(1);
			}
		}

		private void OnTokenUpdate(int i, float _targetValue)
		{
			UISlider uISlider = m_tokenSlider[i];
			UISprite uISprite = ((i > m_tokenForeground.Count) ? null : m_tokenForeground[i]);
			bool flag = _targetValue > 0.01f;
			uISlider.value = _targetValue;
			if (uISprite != null)
			{
				uISprite.enabled = flag;
				uISprite.alpha = ReampRange(_targetValue, 0f, 1f, 0.2f, 1f);
			}
		}

		private float ReampRange(float _value, float _oldMin, float _oldMax, float _newMin, float _newMax)
		{
			float num = _oldMax - _oldMin;
			float num2 = _newMax - _newMin;
			return (_value - _oldMin) * num2 / num + _newMin;
		}
	}
}
