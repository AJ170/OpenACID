using System;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Screens;
using Assets.Scripts.UI.TweenHandling;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIMenubars : MonoBehaviour
	{
		[SerializeField]
		private UIWidget m_topBar;

		[SerializeField]
		private UIWidget m_bottomBar;

		[SerializeField]
		private GameObject m_topBarBlocker;

		[SerializeField]
		private GameObject m_bottomBarBlocker;

		[SerializeField]
		private UIWalletHandler m_walletHandler;

		[SerializeField]
		private UIButtonHeaderManager m_buttonHeaderManager;

		private TweenHandler m_tweenHandler;

		public UIButtonHeaderManager ButtonHeaderManager
		{
			get
			{
				return m_buttonHeaderManager;
			}
		}

		public GameObject TopBar
		{
			get
			{
				return m_topBar.gameObject;
			}
		}

		public UIWalletHandler WalletHandler
		{
			get
			{
				return m_walletHandler;
			}
		}

		public void EnableBarBlockers(bool _enabled)
		{
			EnableTopBarBlocker(_enabled);
			EnableBottomBarBlocker(_enabled);
		}

		public void EnableTopBarBlocker(bool _enabled)
		{
			if (m_topBarBlocker.gameObject != null)
			{
				m_topBarBlocker.gameObject.SetActive(_enabled);
			}
		}

		public void EnableBottomBarBlocker(bool _enabled)
		{
			if (m_bottomBarBlocker.gameObject != null)
			{
				m_bottomBarBlocker.gameObject.SetActive(_enabled);
			}
		}

		public void Show()
		{
			if (Math.Abs(m_topBar.alpha) < 0.001f || Math.Abs(m_bottomBar.alpha) < 0.001f)
			{
				EventDelegate.Remove(m_tweenHandler.OnTweeningFinished, Exclude);
				m_topBar.alpha = 1f;
				m_bottomBar.alpha = 1f;
				EnableBarBlockers(false);
				m_tweenHandler.StartTweeningForward();
			}
		}

		public void Hide()
		{
			EventDelegate.Add(m_tweenHandler.OnTweeningFinished, Exclude, false);
			m_tweenHandler.StartTweeningReverse();
		}

		public void Exclude()
		{
			EventDelegate.Remove(m_tweenHandler.OnTweeningFinished, Exclude);
			m_tweenHandler.ResetToBeginning();
			m_topBar.alpha = 0f;
			m_bottomBar.alpha = 0f;
			EnableBarBlockers(false);
		}

		public void BlockColliders(bool _shouldBlock)
		{
			Collider component = m_topBar.GetComponent<Collider>();
			if (component != null)
			{
				component.enabled = _shouldBlock;
			}
			component = m_bottomBar.GetComponent<Collider>();
			if (component != null)
			{
				component.enabled = _shouldBlock;
			}
		}

		private void Awake()
		{
			m_tweenHandler = base.gameObject.AddComponent<TweenHandler>();
			m_tweenHandler.StartTimeDelay = 0.3f;
			m_tweenHandler.AddAllSiblingsAttachedToTween(m_topBar.GetComponent<BBTargetedTween>(), 0, false);
			m_tweenHandler.AddAllSiblingsAttachedToTween(m_bottomBar.GetComponent<BBTargetedTween>(), 0, false);
			EnableBarBlockers(false);
		}
	}
}
