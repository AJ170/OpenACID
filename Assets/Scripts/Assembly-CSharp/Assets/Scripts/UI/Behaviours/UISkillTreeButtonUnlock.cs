using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Behaviours
{
	internal class UISkillTreeButtonUnlock : MonoBehaviour
	{
		[SerializeField]
		private UIButtonProxy m_button;

		[SerializeField]
		private BoxCollider m_secondaryCollider;

		private bool ShouldEnable
		{
			get
			{
				return Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(StaticEntitlementIds.SkillTreeEntitlementGUID);
			}
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
			if (m_button != null)
			{
				m_button.AllowDisabledClick = ShouldEnable;
				m_button.isEnabled = ShouldEnable;
			}
			if (m_secondaryCollider != null)
			{
				m_secondaryCollider.enabled = ShouldEnable;
			}
		}
	}
}
