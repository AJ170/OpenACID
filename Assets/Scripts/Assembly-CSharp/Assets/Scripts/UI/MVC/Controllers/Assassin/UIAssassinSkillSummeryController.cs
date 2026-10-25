using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.MVC.UIView.Assassin;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Assassin
{
	public class UIAssassinSkillSummeryController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIAssassinSkillSummeryView m_skillSummeryView;

		public virtual void UpdateSummery(AssassinSummary _summery)
		{
			if (_summery != null)
			{
				m_skillSummeryView.PowerLabel = LocalizationManager.Instance.GetInteger(_summery.Power);
				m_skillSummeryView.HealthLabel = LocalizationManager.Instance.GetDecimal(_summery.Health);
				m_skillSummeryView.WeaponLabel = LocalizationManager.Instance.GetDecimal(_summery.Damage);
				m_skillSummeryView.ArmourLabel = LocalizationManager.Instance.GetDecimal(_summery.Armour);
			}
			Show();
		}

		public override void Show()
		{
			base.Show();
			base.gameObject.SetActive(true);
		}

		public override void Hide()
		{
			base.Hide();
			base.gameObject.SetActive(false);
		}
	}
}
