using System;
using Assets.Scripts.Database;
using Assets.Scripts.Engine;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Screens.Generic;
using Assets.Scripts.UI.Screens.MissionScreen.EarthPins;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.MissionScreen
{
	public abstract class UIMissionItemBase : MonoBehaviour
	{
		private const int c_selectedItemDepthOffset = 100;

		[CheckAssigned]
		[SerializeField]
		protected UIWidget m_mainContainer;

		[SerializeField]
		[CheckAssigned]
		protected UIExtendedLabel m_title;

		[CheckAssigned]
		[SerializeField]
		protected UIAchievementTexture m_texture;

		[SerializeField]
		[CheckAssigned]
		protected BBTweenScale m_selectionTween;

		[CheckAssigned]
		[SerializeField]
		protected BBTweenColor m_headerColorTween;

		[CheckAssigned]
		[SerializeField]
		protected AnimatedLineRenderer m_lineAnimation;

		[CheckAssigned]
		[SerializeField]
		protected BoxCollider m_boxCollider;

		[SerializeField]
		[CheckAssigned]
		protected UISwapTweenValuesHelper m_tweenSwaper;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_highLightContainer;

		private bool m_hasDepthChanges;

		private UIWidget[] m_uiWidgets;

		public Action<UIMissionItemBase> OnItemClickedCallback;

		private bool m_isSelected;

		protected bool m_isUnlocked;

		public string ItemId { get; protected set; }

		public int Index { get; protected set; }

		public RegionDefinition RegionData { get; protected set; }

		public virtual bool IsUnlocked
		{
			get
			{
				return m_isUnlocked;
			}
			protected set
			{
				m_isUnlocked = value;
				m_texture.Desaturation = ((!m_isUnlocked) ? 1 : 0);
			}
		}

		public bool IsSelected
		{
			get
			{
				return m_isSelected;
			}
			set
			{
				m_isSelected = value;
				if (m_isSelected)
				{
					SetActivated(false);
				}
				else
				{
					SetDeActivated(false);
				}
			}
		}

		protected bool IsElite { get; set; }

		public virtual bool IsMissionValid()
		{
			return true;
		}

		public virtual void Clear()
		{
			m_title.text = string.Empty;
			m_texture.mainTexture = null;
		}

		public void ChangeSelectedStatusInstantly(bool _isSelected)
		{
			m_isSelected = _isSelected;
			if (m_isSelected)
			{
				SetActivated(true);
			}
			else
			{
				SetDeActivated(true);
			}
		}

		public void HideLine()
		{
			m_lineAnimation.StopLineAnimation();
		}

		private void Awake()
		{
			m_uiWidgets = GetComponentsInChildren<UIWidget>(true);
			OnAwake();
		}

		public void UpdateItemAnchor(UIRect _anchorTarget)
		{
			m_mainContainer.leftAnchor.Set(_anchorTarget.transform, 0f, 0f);
			m_mainContainer.rightAnchor.Set(_anchorTarget.transform, 1f, 0f);
			m_mainContainer.ForceUpdate(true);
		}

		public void SetEarthPin(UIPanel _parentPanel, EarthPinBase _getPin)
		{
			if (_getPin == null)
			{
				return;
			}
			_getPin.IsSelected = true;
			m_lineAnimation.StopLineAnimation();
			if (!m_selectionTween.enabled)
			{
				m_lineAnimation.StartLineAnimation(_parentPanel, _getPin);
				return;
			}
			EventDelegate.Add(m_selectionTween.onFinished, delegate
			{
				if (IsSelected)
				{
					m_lineAnimation.StartLineAnimation(_parentPanel, _getPin);
				}
			}, true);
		}

		public virtual bool CanSelectItem()
		{
			return true;
		}

		protected virtual void OnAwake()
		{
		}

		protected abstract void OnDestroyItem();

		protected virtual void OnClick()
		{
			if (IsMissionValid())
			{
				if (!IsSelected)
				{
					IsSelected = CanSelectItem() && !IsSelected;
				}
				OnItemClickedCallback(this);
			}
		}

		protected virtual void SetActivated(bool _completeInstantly)
		{
			if (m_tweenSwaper.IsValuesSwaped)
			{
				m_tweenSwaper.SwapValues();
			}
			UpdateTweenColors();
			m_tweenSwaper.DurationOffset = 0f;
			if (_completeInstantly)
			{
				m_selectionTween.SetTweenToEnd();
				m_headerColorTween.SetTweenToEnd();
			}
			else
			{
				m_selectionTween.StartTweenPlayForward();
				m_headerColorTween.StartTweenPlayForward();
			}
			m_highLightContainer.SetActive(true);
			if (!m_hasDepthChanges)
			{
				m_hasDepthChanges = true;
				for (int i = 0; i < m_uiWidgets.Length; i++)
				{
					m_uiWidgets[i].depth += 100;
				}
			}
		}

		protected virtual void SetDeActivated(bool _completeInstantly)
		{
			if (!m_tweenSwaper.IsValuesSwaped)
			{
				m_tweenSwaper.SwapValues();
			}
			UpdateTweenColors();
			m_tweenSwaper.DurationOffset = -0.1f;
			if (_completeInstantly)
			{
				m_selectionTween.SetTweenToEnd();
				m_headerColorTween.SetTweenToEnd();
			}
			else
			{
				m_selectionTween.StartTweenPlayForward();
				m_headerColorTween.StartTweenPlayForward();
			}
			m_highLightContainer.SetActive(false);
			m_lineAnimation.StopLineAnimation();
			if (m_hasDepthChanges)
			{
				m_hasDepthChanges = false;
				for (int i = 0; i < m_uiWidgets.Length; i++)
				{
					m_uiWidgets[i].depth -= 100;
				}
			}
		}

		protected virtual void UpdateTweenColors()
		{
			if (IsSelected)
			{
				m_headerColorTween.to = UIDataMapper.UITextColors.SelectedHeaderText;
				m_headerColorTween.from = ((!IsElite) ? UIDataMapper.UITextColors.InactiveHeaderText : UIDataMapper.UITextColors.EliteMissionTitleColor);
			}
			else
			{
				m_headerColorTween.to = ((!IsElite) ? UIDataMapper.UITextColors.InactiveHeaderText : UIDataMapper.UITextColors.EliteMissionTitleColor);
				m_headerColorTween.from = UIDataMapper.UITextColors.SelectedHeaderText;
			}
		}

		private void OnDisable()
		{
			m_lineAnimation.StopLineAnimation();
		}

		private void OnDestroy()
		{
			RegionData = null;
			m_uiWidgets.ClearIfNotEmpty();
			OnDestroyItem();
		}
	}
}
