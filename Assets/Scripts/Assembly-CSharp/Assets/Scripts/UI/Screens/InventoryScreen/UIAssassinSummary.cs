using AcierProtocol;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIAssassinSummary : MonoBehaviour
	{
		[SerializeField]
		private UIExtendedLabel m_healtLabel;

		[SerializeField]
		private UIExtendedLabel m_weaponLabel;

		[SerializeField]
		private UIExtendedLabel m_armourLabel;

		[SerializeField]
		private UIExtendedLabel m_powerLabel;

		private Assassin m_assassin;

		private bool m_updateSummeryOnUpdate;

		private Assassin Assassin
		{
			get
			{
				if (m_assassin != null)
				{
					return m_assassin;
				}
				return Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin;
			}
		}

		public void SetAssassin(Assassin _assassin)
		{
			m_assassin = _assassin;
			UpdateSummary();
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Add(OnAssassinChange);
			Singleton<StateManager>.Instance.ProfileManager.OnInventoryUpdate += OnInventoryUpdated;
			m_updateSummeryOnUpdate = true;
		}

		private void OnDisable()
		{
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Remove(OnAssassinChange);
			Singleton<StateManager>.Instance.ProfileManager.OnInventoryUpdate -= OnInventoryUpdated;
		}

		private void Update()
		{
			if (m_updateSummeryOnUpdate)
			{
				m_updateSummeryOnUpdate = false;
				UpdateSummary();
			}
		}

		private void OnAssassinChange(params object[] _params)
		{
			UpdateSummary();
		}

		private void OnInventoryUpdated(InventoryElement[] _new, InventoryElement[] _old, int _newSize, int _oldSize)
		{
			UpdateSummary();
		}

		private void UpdateSummary()
		{
			if (Assassin != null)
			{
				AssassinSummary summary = Assassin.GetSummary();
				m_healtLabel.text = LocalizationManager.Instance.GetDecimal(summary.Health);
				m_weaponLabel.text = LocalizationManager.Instance.GetDecimal(summary.Damage);
				m_armourLabel.text = LocalizationManager.Instance.GetDecimal(summary.Armour);
				m_powerLabel.text = LocalizationManager.Instance.GetInteger(summary.Power);
			}
		}
	}
}
