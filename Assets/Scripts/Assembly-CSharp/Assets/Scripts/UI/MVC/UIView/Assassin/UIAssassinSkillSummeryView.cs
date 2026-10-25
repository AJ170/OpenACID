using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Assassin
{
	public class UIAssassinSkillSummeryView : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_healthLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_weaponLabel;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_armourLabel;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_powerLabel;

		public string HealthLabel
		{
			get
			{
				return m_healthLabel.text;
			}
			set
			{
				m_healthLabel.text = value;
			}
		}

		public string WeaponLabel
		{
			get
			{
				return m_weaponLabel.text;
			}
			set
			{
				m_weaponLabel.text = value;
			}
		}

		public string ArmourLabel
		{
			get
			{
				return m_armourLabel.text;
			}
			set
			{
				m_armourLabel.text = value;
			}
		}

		public string PowerLabel
		{
			get
			{
				return m_powerLabel.text;
			}
			set
			{
				m_powerLabel.text = value;
			}
		}
	}
}
