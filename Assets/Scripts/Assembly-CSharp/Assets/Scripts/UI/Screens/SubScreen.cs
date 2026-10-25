using System.Collections.Generic;
using Assets.Scripts.UI.EnvironmentBridge;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens
{
	public class SubScreen : MonoBehaviour
	{
		[SerializeField]
		private UIWidget m_content;

		[SerializeField]
		private List<BBTargetedTween> m_showTweens;

		[SerializeField]
		private List<BBTargetedTween> m_showReverseTweens;

		[SerializeField]
		private List<BBTargetedTween> m_hideTweens;

		[SerializeField]
		private List<BBTargetedTween> m_hideReverseTweens;

		[SerializeField]
		private List<GameObject> m_ignoreOnIOS;

		[SerializeField]
		private CameraMenuManager.EType m_cameraType;

		private bool m_show;

		private bool m_reverse;

		private int m_finishedCount;

		private List<BBTargetedTween> TweensOfCurrentState
		{
			get
			{
				return m_show ? (m_reverse ? m_showReverseTweens : m_showTweens) : (m_reverse ? m_hideReverseTweens : m_hideTweens);
			}
		}

		private void Awake()
		{
			m_show = m_content.gameObject.activeSelf;
			List<BBTargetedTween>[] array = new List<BBTargetedTween>[4] { m_showTweens, m_showReverseTweens, m_hideTweens, m_hideReverseTweens };
			foreach (List<BBTargetedTween> list in array)
			{
				if (list != null)
				{
					int num = 0;
					while (num < list.Count)
					{
						list[num++].AddOnFinished(OnFinishedIntermediate);
					}
				}
			}
		}

		private void OnDestroy()
		{
			List<BBTargetedTween>[] array = new List<BBTargetedTween>[4] { m_showTweens, m_showReverseTweens, m_hideTweens, m_hideReverseTweens };
			for (int i = 0; i < array.Length; i++)
			{
				for (int j = 0; j < array[i].Count; j++)
				{
					array[i][j].onFinished.Clear();
				}
			}
		}

		public void TweenCamera()
		{
			if (m_cameraType != CameraMenuManager.EType.None)
			{
				BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager.Current = m_cameraType;
			}
		}

		public void InstantCamera()
		{
			if (m_cameraType != CameraMenuManager.EType.None)
			{
				BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager.SetCurrentInstant(m_cameraType);
			}
		}

		[ContextMenu("Show")]
		public void Show()
		{
			m_show = true;
			m_reverse = false;
			m_content.gameObject.SetActive(true);
			for (int i = 0; i < m_showTweens.Count; i++)
			{
				m_showTweens[i].ValidateTween();
				m_showTweens[i].StartTweenPlayForward();
			}
		}

		[ContextMenu("ReverseShow")]
		public void ReverseShow()
		{
			m_show = true;
			m_reverse = true;
			for (int i = 0; i < m_showReverseTweens.Count; i++)
			{
				m_showReverseTweens[i].ValidateTween();
				m_showReverseTweens[i].StartTweenPlayForward();
			}
		}

		[ContextMenu("Hide")]
		public void Hide()
		{
			m_show = false;
			m_reverse = false;
			for (int i = 0; i < m_hideTweens.Count; i++)
			{
				m_hideTweens[i].ValidateTween();
				m_hideTweens[i].StartTweenPlayForward();
			}
		}

		[ContextMenu("ReverseHide")]
		public void ReverseHide()
		{
			m_show = false;
			m_reverse = true;
			m_content.gameObject.SetActive(true);
			for (int i = 0; i < m_hideReverseTweens.Count; i++)
			{
				m_hideReverseTweens[i].ValidateTween();
				m_hideReverseTweens[i].StartTweenPlayForward();
			}
		}

		private void OnFinishedIntermediate()
		{
			m_finishedCount++;
			if (TweensOfCurrentState.Count >= m_finishedCount)
			{
				m_finishedCount = 0;
				OnFinishedAll();
			}
		}

		private void OnFinishedAll()
		{
			if ((!m_show && !m_reverse) || (m_show && m_reverse))
			{
				m_content.gameObject.SetActive(false);
			}
		}
	}
}
