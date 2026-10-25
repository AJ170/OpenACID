using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialAssassinSelectionGotoBase : TutorialGotoBase
	{
		[SerializeField]
		private UIWidget m_skillWheel;

		protected void Awake()
		{
		}

		public override void Activate()
		{
			base.Activate();
			if (m_skillWheel != null)
			{
				m_skillWheel.gameObject.SetActive(false);
			}
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				base.Deactivate();
				if (m_skillWheel != null)
				{
					m_skillWheel.gameObject.SetActive(true);
				}
			}
		}
	}
}
