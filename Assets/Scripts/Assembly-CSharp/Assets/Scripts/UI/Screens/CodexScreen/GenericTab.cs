using Assets.Scripts.Database;
using Assets.Scripts.UI.NGUIExtensions.Tweens;
using Database;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.CodexScreen
{
	public class GenericTab : MonoBehaviour
	{
		public UITable SubTabContainer;

		public TweenScale ScaleTween;

		public TweenAlpha AlphaTween;

		public UIExtendedLabel numberOfEntries;

		public UISprite readMarker;

		public UISprite backGround;

		public TweenAlpha Highlight;

		[HideInInspector]
		public GameID GameId;

		[SerializeField]
		private TweenScale m_scaleTweenEntry;

		[SerializeField]
		private BBTweenRotation m_arrowTween;

		[SerializeField]
		private UISprite m_arrowSprite;

		public UIExtendedLabel Header;

		public string text;

		public bool activated;

		public CodexDefinition data;

		private bool m_tweening;

		private Color32 m_activeColor = new Color32(6, 88, 102, byte.MaxValue);

		private Color32 m_whiteColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		private void Awake()
		{
		}

		private void Start()
		{
			if (null != ScaleTween)
			{
				ScaleTween.updateTable = true;
			}
		}

		public void ActivateChildren()
		{
			if (null != SubTabContainer)
			{
				SubTabContainer.gameObject.SetActive(true);
			}
		}

		public void DeActivateChildren()
		{
		}

		public void DeactivateSubcontainer()
		{
			m_tweening = false;
			if (null != SubTabContainer)
			{
				SubTabContainer.gameObject.SetActive(false);
			}
			activated = false;
		}

		public void ActivateSubcontainer()
		{
			m_tweening = false;
			activated = true;
		}

		public void ActivationAnimation()
		{
			if (null != ScaleTween && !m_tweening)
			{
				activated = true;
				ScaleTween.from = new Vector3(1f, 0f, 1f);
				ScaleTween.to = new Vector3(1f, 1f, 1f);
				ScaleTween.ResetToBeginning();
				ScaleTween.PlayForward();
				m_tweening = true;
				EventDelegate.Add(ScaleTween.onFinished, ActivateSubcontainer, true);
			}
			if (null != m_arrowTween)
			{
				m_arrowTween.from = new Vector3(0f, 0f, 0f);
				m_arrowTween.to = new Vector3(0f, 0f, -180f);
				m_arrowTween.StartTweenPlayForward();
			}
			if (null != m_scaleTweenEntry)
			{
				m_scaleTweenEntry.from = new Vector3(1f, 1f, 1f);
				m_scaleTweenEntry.to = new Vector3(1.07f, 1f, 1f);
				m_scaleTweenEntry.ResetToBeginning();
				m_scaleTweenEntry.PlayForward();
			}
		}

		public void DeactivationAnimation()
		{
			if (null != ScaleTween && !m_tweening)
			{
				ScaleTween.from = new Vector3(1f, 1f, 1f);
				ScaleTween.to = new Vector3(1f, 0f, 1f);
				ScaleTween.ResetToBeginning();
				ScaleTween.PlayForward();
				m_tweening = true;
				EventDelegate.Add(ScaleTween.onFinished, DeactivateSubcontainer, true);
			}
			if (null != m_scaleTweenEntry)
			{
				m_scaleTweenEntry.from = new Vector3(1.07f, 1f, 1f);
				m_scaleTweenEntry.to = new Vector3(1f, 1f, 1f);
				m_scaleTweenEntry.ResetToBeginning();
				m_scaleTweenEntry.PlayForward();
			}
			if (null != m_arrowTween)
			{
				m_arrowTween.from = new Vector3(0f, 0f, -180f);
				m_arrowTween.to = new Vector3(0f, 0f, 0f);
				m_arrowTween.StartTweenPlayForward();
			}
		}

		public void ActivationHighlight()
		{
			if (Header.color != m_activeColor)
			{
				Highlight.from = 0f;
				Highlight.to = 1f;
				Highlight.ResetToBeginning();
				Highlight.PlayForward();
				Header.color = m_activeColor;
				if (m_arrowSprite != null)
				{
					m_arrowSprite.color = m_activeColor;
				}
				if (numberOfEntries != null)
				{
					numberOfEntries.color = m_activeColor;
				}
			}
		}

		public void DeactivationHighlight()
		{
			if (Header.color != m_whiteColor)
			{
				Highlight.from = 1f;
				Highlight.to = 0f;
				Highlight.ResetToBeginning();
				Highlight.PlayForward();
				Header.color = m_whiteColor;
				if (m_arrowSprite != null)
				{
					m_arrowSprite.color = m_whiteColor;
				}
				if (numberOfEntries != null)
				{
					numberOfEntries.color = m_whiteColor;
				}
			}
		}
	}
}
