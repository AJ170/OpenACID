using Assets.Scripts.Mission.Logic;
using UbiCore.Utils;

namespace Assets.Scripts.Mission.Selection
{
	public class SelectionManager : UbiCore.Utils.Singleton<SelectionManager>, ISelectionProvider
	{
		private ISelectable m_selection;

		public EntityId SelectedEntityId
		{
			get
			{
				return (Selection != null) ? Selection.Id : EntityId.InvalidId;
			}
		}

		public ISelectable Selection
		{
			get
			{
				return m_selection;
			}
			set
			{
				if (value != m_selection && (value == null || value.IsSelectable()))
				{
					if (m_selection != null)
					{
						m_selection.SetSelected(false);
					}
					m_selection = value;
					if (m_selection != null)
					{
						m_selection.SetSelected(true);
					}
					LogicMain.LogicInterface.Combat_Select();
				}
			}
		}

		public void UpdateSelection()
		{
			if (Selection != null && !Selection.IsSelectable())
			{
				Selection = null;
			}
		}
	}
}
