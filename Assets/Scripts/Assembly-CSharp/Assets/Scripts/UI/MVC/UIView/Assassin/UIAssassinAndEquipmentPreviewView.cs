using System;
using System.Collections;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Assassin
{
	public class UIAssassinAndEquipmentPreviewView : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		protected UnityEngine.Animation m_mainAnimation;

		[SerializeField]
		[CheckAssigned]
		protected UIExtendedLabel m_playerName;

		[SerializeField]
		[CheckAssigned]
		protected UIExtendedLabel m_assassinName;

		[CheckAssigned]
		[SerializeField]
		protected UIExtendedLabel m_heroicRank;

		[CheckAssigned]
		[SerializeField]
		protected UIExtendedLabel m_assassinClass;

		[SerializeField]
		[CheckAssigned]
		protected UIExtendedLabel m_leaderboardRank;

		[SerializeField]
		[CheckAssigned]
		protected UISprite m_background;

		public string PlayerName
		{
			get
			{
				return m_playerName.text;
			}
			set
			{
				m_playerName.text = value;
			}
		}

		public string AssassinName
		{
			get
			{
				return m_assassinName.text;
			}
			set
			{
				m_assassinName.text = value;
			}
		}

		public string AssassinClass
		{
			get
			{
				return m_assassinClass.text;
			}
			set
			{
				m_assassinClass.text = value;
			}
		}

		public string HeroicRank
		{
			get
			{
				return m_heroicRank.text;
			}
			set
			{
				m_heroicRank.text = value;
			}
		}

		public string LeaderboardRank
		{
			get
			{
				return m_leaderboardRank.text;
			}
			set
			{
				m_leaderboardRank.text = value;
			}
		}

		public Color BackgroundColor
		{
			get
			{
				return m_background.color;
			}
			set
			{
				m_background.color = value;
			}
		}

		public void StartAnimation(string _name, Action _callback = null)
		{
			StartCoroutine(OnStartAnimation(_name, _callback));
		}

		private IEnumerator OnStartAnimation(string _name, Action _callback = null)
		{
			m_mainAnimation.SampleAnimation(0f, _name);
			m_mainAnimation.Play(_name);
			WaitForEndOfFrame wait = new WaitForEndOfFrame();
			while (m_mainAnimation.isPlaying)
			{
				yield return wait;
			}
			if (_callback != null)
			{
				_callback();
			}
		}
	}
}
