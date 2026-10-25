using AcierProtocol;
using Assets.Scripts.UI.State;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIAssassinContainer : MonoBehaviour
	{
		[SerializeField]
		private UIAssassinRankDisplay m_display;

		public UIAssassinRankDisplay Display
		{
			get
			{
				return m_display;
			}
		}

		public bool IsVisible
		{
			get
			{
				return base.gameObject.activeSelf;
			}
		}

		private void Awake()
		{
			Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			if (currentAssassin == null)
			{
				base.gameObject.SetActive(false);
			}
		}

		public void SetVisibility(bool _visible)
		{
			if (base.gameObject != null)
			{
				base.gameObject.SetActive(_visible);
			}
		}

		public void Show()
		{
			if (base.gameObject != null)
			{
				Assassin currentAssassin = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
				base.gameObject.SetActive(currentAssassin != null);
			}
		}

		public void Hide()
		{
			if (base.gameObject != null)
			{
				base.gameObject.SetActive(false);
			}
		}
	}
}
