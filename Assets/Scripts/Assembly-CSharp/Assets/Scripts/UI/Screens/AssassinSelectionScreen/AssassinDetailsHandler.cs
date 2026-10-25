using UnityEngine;

namespace Assets.Scripts.UI.Screens.AssassinSelectionScreen
{
	public class AssassinDetailsHandler : MonoBehaviour
	{
		[SerializeField]
		private UIButton skillTreeButton;

		[SerializeField]
		private UIButton inventoryButton;

		[SerializeField]
		private UIButton statisticsButton;

		private void Awake()
		{
		}

		public void EnableHandler(bool _enable)
		{
			if (skillTreeButton != null)
			{
				skillTreeButton.isEnabled = _enable;
			}
			if (inventoryButton != null)
			{
				inventoryButton.isEnabled = _enable;
			}
			if (statisticsButton != null)
			{
				statisticsButton.isEnabled = _enable;
			}
		}

		public void HideHandler(bool _hide)
		{
			base.gameObject.SetActive(!_hide);
		}
	}
}
