using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class UIExtendedButton : UIButton
	{
		public delegate void HandleDelayUpdate(float _total, float _normalized);

		public List<EventDelegate> onStateChange = new List<EventDelegate>();

		public List<EventDelegate> onVisualsUpdated = new List<EventDelegate>();

		public List<EventDelegate> onTypeChange = new List<EventDelegate>();

		public List<EventDelegate> onBeforeClick = new List<EventDelegate>();

		[SerializeField]
		private bool m_enableRescaleCollision = true;

		[SerializeField]
		private bool m_allowDisabledClick;

		[SerializeField]
		private EButtonTypes m_type;

		[SerializeField]
		private float m_clickDelay;

		[SerializeField]
		private float m_disabledDelay;

		private UIRescaleCollision m_rescaleCollision;

		public bool AllowDisabledClick
		{
			get
			{
				return m_allowDisabledClick;
			}
			set
			{
				m_allowDisabledClick = value;
				Collider component = base.gameObject.GetComponent<Collider>();
				if (component != null)
				{
					component.enabled = m_allowDisabledClick || base.state != State.Disabled;
				}
			}
		}

		public float ClickDelay
		{
			get
			{
				return m_clickDelay;
			}
			set
			{
				m_clickDelay = Mathf.Max(value, 0f);
			}
		}

		public float DisabledDelay
		{
			get
			{
				return m_disabledDelay;
			}
			set
			{
				if (!(Math.Abs(m_disabledDelay - value) < 0.0001f))
				{
					m_disabledDelay = value;
					SetState((!(m_disabledDelay <= 0f)) ? State.Disabled : State.Normal, true);
					isEnabled = m_disabledDelay <= 0f;
				}
			}
		}

		public bool InstantTrigger
		{
			get
			{
				return ClickDelay.Equals(0f);
			}
		}

		public EButtonTypes Type
		{
			get
			{
				return m_type;
			}
			set
			{
				if (m_type != value)
				{
					m_type = value;
					EventDelegate.Execute(onTypeChange);
					UpdateVisuals(false, true);
				}
			}
		}

		public override bool isEnabled
		{
			get
			{
				return (!m_allowDisabledClick) ? base.isEnabled : (base.state != State.Disabled);
			}
			set
			{
				if (m_allowDisabledClick)
				{
					Collider component = base.gameObject.GetComponent<Collider>();
					if (component != null)
					{
						component.enabled = true;
					}
					SetState((!value) ? State.Disabled : State.Normal, true);
				}
				else if (base.isEnabled != value)
				{
					base.isEnabled = value;
				}
			}
		}

		public event HandleDelayUpdate OnDelayUpdate;

		public override void SetState(State _state, bool _immediate)
		{
			State state = base.state;
			base.SetState(_state, _immediate);
			if (_state != state)
			{
				EventDelegate.Execute(onStateChange);
			}
			if (_state == State.Pressed && base.enabled && !InstantTrigger)
			{
				StartCoroutine(co_triggerDelay());
			}
			else if (m_disabledDelay > 0f && isEnabled && base.gameObject.activeInHierarchy)
			{
				StartCoroutine(co_triggerDisabledDelay());
			}
			else
			{
				StopAllCoroutines();
			}
		}

		protected override void OnInit()
		{
			base.OnInit();
			HandleRescaleCollisionEnabled(m_enableRescaleCollision);
			UpdateSettings(true);
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			DisabledDelay = 0f;
		}

		private void Awake()
		{
			if (!mInitDone)
			{
				OnInit();
			}
		}

		private void HandleRescaleCollisionEnabled(bool _enable)
		{
			if (m_enableRescaleCollision)
			{
				m_rescaleCollision = base.gameObject.AddComponentIfNotExist<UIRescaleCollision>();
				return;
			}
			m_rescaleCollision = base.gameObject.GetComponent<UIRescaleCollision>();
			if (m_rescaleCollision != null)
			{
				m_rescaleCollision.enabled = false;
			}
		}

		protected override void OnClick()
		{
			EventDelegate.Execute(onBeforeClick);
			if (m_allowDisabledClick)
			{
				UIButton.current = this;
				EventDelegate.Execute(onClick);
				UIButton.current = null;
			}
			else if (InstantTrigger)
			{
				base.OnClick();
			}
		}

		public void UpdateVisuals(bool _trigger = true, bool _setColor = false)
		{
			UpdateSettings(_setColor);
			if (_trigger)
			{
				EventDelegate.Execute(onVisualsUpdated);
			}
		}

		private void UpdateSettings(bool _setColor = false)
		{
			ButtonConfiguration value;
			if (UIDataMapper.ButtonConfigurations.TryGetValue((int)Type, out value))
			{
				SetNormalColor(value.DefaultColor, _setColor);
				hover = value.HoverColor;
				pressed = value.PressedColor;
				disabledColor = value.DisabledColor;
				duration = value.TransitionDuration;
			}
			else if (!Application.isPlaying)
			{
			}
		}

		private void SetNormalColor(Color _color, bool _setColor = false)
		{
			UIWidget component = GetComponent<UIWidget>();
			if (component != null && _setColor)
			{
				component.color = _color;
			}
			mDefaultColor = _color;
			mStartingColor = _color;
			if (tweenTarget != null)
			{
				UIWidget component2 = tweenTarget.GetComponent<UIWidget>();
				if (component2 != null && _setColor)
				{
					component2.color = _color;
				}
			}
		}

		private IEnumerator co_triggerDelay()
		{
			float start = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
			float elapsed = 0f;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (elapsed < m_clickDelay)
			{
				yield return wait;
				elapsed = UbiCore.Utils.Singleton<Timer>.Instance.realTime - start;
				if (this.OnDelayUpdate != null)
				{
					this.OnDelayUpdate(elapsed, elapsed / m_clickDelay);
				}
			}
			base.OnClick();
			SetState(State.Normal, true);
		}

		private IEnumerator co_triggerDisabledDelay()
		{
			isEnabled = false;
			float start = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
			float elapsed = 0f;
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (elapsed < m_disabledDelay)
			{
				yield return wait;
				elapsed = UbiCore.Utils.Singleton<Timer>.Instance.realTime - start;
			}
			isEnabled = true;
		}
	}
}
