using Assets.Scripts.UI.MVC.Controllers;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.NGUIExtentions;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Assassin
{
	public class UIAssassinPortraitSelectableView : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIPortraitTexture m_assassinPortrait;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_titleText;

		[CheckAssigned]
		[SerializeField]
		private BBTweenScale m_selectionTween;

		[SerializeField]
		[CheckAssigned]
		private BBTweenAlpha m_selectionHighlightTween;

		[CheckAssigned]
		[SerializeField]
		private BBTweenColor m_selectionColorTween;

		[CheckAssigned]
		[SerializeField]
		private UIPlayAudio m_playSelectedAudio;

		[CheckAssigned]
		[SerializeField]
		private UIControllerBase m_controller;

		public UIPortraitTexture AssassinPortrait
		{
			get
			{
				return m_assassinPortrait;
			}
		}

		public void SetTitle(string _title)
		{
			m_titleText.text = _title;
		}

		public void SetColorForColorTween(Color _selectedColor, Color _deselectedColor)
		{
			m_selectionColorTween.to = _selectedColor;
			m_selectionColorTween.from = _deselectedColor;
		}

		public void PlaySelectedAudioSound()
		{
			m_playSelectedAudio.Play();
		}

		public void SetTweenToBeginning()
		{
			m_selectionTween.SetTweenToBeginning();
			m_selectionHighlightTween.SetTweenToBeginning();
			m_selectionColorTween.SetTweenToBeginning();
		}

		public void SetTweenToEnd()
		{
			m_selectionTween.SetTweenToEnd();
			m_selectionHighlightTween.SetTweenToEnd();
			m_selectionColorTween.SetTweenToEnd();
		}

		public void PlayForward()
		{
			m_selectionTween.PlayForward();
			m_selectionHighlightTween.PlayForward();
			m_selectionColorTween.PlayForward();
		}

		public void PlayReverse()
		{
			m_selectionTween.PlayReverse();
			m_selectionHighlightTween.PlayReverse();
			m_selectionColorTween.PlayReverse();
		}

		private void OnClick()
		{
			m_controller.OnControllerClicked();
		}
	}
}
