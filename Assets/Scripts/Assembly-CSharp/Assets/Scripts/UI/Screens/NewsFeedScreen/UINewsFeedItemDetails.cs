using System;
using System.Collections.Generic;
using Assets.Scripts.UI.NGUIExtensions;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Screens.NewsFeedScreen.Data;
using Assets.Scripts.UI.TweenHandling;
using Assets.Scripts.UI.Utils;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.NewsFeedScreen
{
	public class UINewsFeedItemDetails : MonoBehaviour
	{
		[SerializeField]
		private UIWidget m_widget;

		[SerializeField]
		private UIExtendedLabel m_headerLabel;

		[SerializeField]
		private UIExtendedLabel m_feedText;

		[SerializeField]
		private UISprite m_feedIcon;

		[SerializeField]
		private UIResizeBox m_resizeBox;

		[SerializeField]
		private BBTweenPosition m_positionTween;

		private string m_feedUrl = string.Empty;

		public List<EventDelegate> OnResized
		{
			get
			{
				return m_resizeBox.OnResized;
			}
		}

		public void UpdateData(NewsFeedDataBase _newsFeedEntry)
		{
			m_headerLabel.text = _newsFeedEntry.Header;
			m_feedText.text = _newsFeedEntry.Message;
			m_feedText.MarkAsChanged();
			m_feedUrl = _newsFeedEntry.GoToUrl;
			NewsFeedDescription value = null;
			if (UbiCore.Utils.Singleton<UIDataMapper>.Instance.NewsFeedDescription.TryGetValue((int)_newsFeedEntry.IconType, out value))
			{
				m_feedIcon.spriteName = value.SpriteName;
			}
			m_resizeBox.ResizeNow = true;
		}

		public void OnTapText()
		{
			string urlAtPosition = m_feedText.GetUrlAtPosition(UICamera.lastHit.point);
			if (!string.IsNullOrEmpty(urlAtPosition))
			{
				Application.OpenURL(urlAtPosition);
			}
		}

		public void OnTap()
		{
			if (!string.IsNullOrEmpty(m_feedUrl))
			{
				Application.OpenURL(m_feedUrl);
			}
		}

		public void UpdateAnchorTarget(UIRect _anchorTarget)
		{
			m_widget.leftAnchor.Set(_anchorTarget.transform, 0f, 0f);
			m_widget.rightAnchor.Set(_anchorTarget.transform, 1f, 0f);
			m_widget.ForceUpdate(true);
		}

		public void UpdateTween(BBTweenPosition _tweenItemRef, int _startOrder, TweenHandler _handler, out float _tweenDistance)
		{
			m_widget.ForceUpdate(true);
			float offscreenPositionX = UIUtil.GetOffscreenPositionX(m_widget, UIUtil.ESide.Right);
			m_positionTween.CopyFrom(_tweenItemRef);
			m_positionTween.SetEndToCurrentValue();
			m_positionTween.from = new Vector3(offscreenPositionX, 0f);
			m_positionTween.StartOrder = _startOrder;
			_tweenDistance = Math.Abs(m_positionTween.from.x - m_positionTween.to.x);
			_handler.AddTweens(new BBTweenPosition[1] { m_positionTween }, 1, false);
		}

		private void Awake()
		{
		}
	}
}
