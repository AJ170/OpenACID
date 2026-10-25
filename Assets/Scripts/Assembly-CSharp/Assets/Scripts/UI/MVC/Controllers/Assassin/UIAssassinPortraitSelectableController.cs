using System;
using Assets.Scripts.UI.MVC.Models.Assassin;
using Assets.Scripts.UI.MVC.UIView.Assassin;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Assassin
{
	public class UIAssassinPortraitSelectableController : UIControllerBase
	{
		private static readonly Vector4 s_portraitUV = new Vector4(0f, 0.4f, 0.6f, 1f);

		[SerializeField]
		[CheckAssigned]
		private UIAssassinPortraitSelectableView m_assassinPortraitView;

		public Action<UIAssassinPortraitSelectableController> OnPortraitClicked;

		private bool m_isAllowedToDeselectOnClick;

		private bool m_isSelected;

		public bool CanBeSelected
		{
			get
			{
				return !IsSelected && Model != null;
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
					m_assassinPortraitView.PlayForward();
				}
				else
				{
					m_assassinPortraitView.PlayReverse();
				}
			}
		}

		public UIAssassinBaseModel Model { get; set; }

		public void UpdatePortrait(UIAssassinBaseModel _assassinModel, bool _isSelected)
		{
			Model = _assassinModel;
			m_assassinPortraitView.SetTitle(_assassinModel.Title);
			m_assassinPortraitView.SetColorForColorTween(_assassinModel.SelectedTextColor, _assassinModel.DeselectedTextColor);
			if (_assassinModel.Assassin != null)
			{
				GameID _headId = GameID.Invalid;
				GameID _humanoidId = GameID.Invalid;
				GameID _colorId = GameID.Invalid;
				GameID _itemId = GameID.Invalid;
				_assassinModel.GetAssasssinPortraitData(ref _headId, ref _humanoidId, ref _colorId, ref _itemId);
				UIPortraitUtil.UpdatePortraitByColorDef(_headId, _humanoidId, _colorId, _itemId, m_assassinPortraitView.AssassinPortrait, true, s_portraitUV);
			}
			SetSelectedIntantly(_isSelected);
			base.IsInitialized = true;
			Show();
		}

		public void SetSelectedIntantly(bool _isSelected)
		{
			m_isSelected = _isSelected;
			if (m_isSelected)
			{
				m_assassinPortraitView.SetTweenToEnd();
			}
			else
			{
				m_assassinPortraitView.SetTweenToBeginning();
			}
		}

		public override void OnControllerClicked()
		{
			bool isSelected = IsSelected;
			if (m_isAllowedToDeselectOnClick)
			{
				IsSelected = !IsSelected;
			}
			else if (!IsSelected)
			{
				IsSelected = true;
			}
			if (isSelected != IsSelected)
			{
				m_assassinPortraitView.PlaySelectedAudioSound();
				Action<UIAssassinPortraitSelectableController> onPortraitClicked = OnPortraitClicked;
				if (onPortraitClicked != null)
				{
					onPortraitClicked(this);
				}
			}
		}
	}
}
