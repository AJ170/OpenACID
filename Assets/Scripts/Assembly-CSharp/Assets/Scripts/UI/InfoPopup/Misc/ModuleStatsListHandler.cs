using AcierProtocol;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Screens.AssassinInventoryScreen;
using Assets.Scripts.UI.Screens.ModuleScreen;

namespace Assets.Scripts.UI.InfoPopup.Misc
{
	public class ModuleStatsListHandler : InfoStatsListHandler
	{
		private ModuleData m_module;

		public void Init(ModuleData _module)
		{
			m_module = _module;
			if (m_module != null)
			{
				Setup();
			}
		}

		private void Setup()
		{
			if (m_gameObjectCache == null)
			{
				m_gameObjectCache = new GameObjectCache(m_statItemPrefab, m_scrollTable.transform);
			}
			m_boxCollider.enabled = false;
			ReleaseEntries();
			for (int i = 0; i < m_module.modifications.Length; i++)
			{
				Modification modification = m_module.modifications[i];
				ModuleModifierValueHandler cachedGameObject = GetCachedGameObject<ModuleModifierValueHandler>(i);
				string text = m_module.BuildUnitStringRaw(i);
				cachedGameObject.Init(text, modification.effectValue);
			}
			m_scrollTable.repositionNow = true;
		}

		protected override void OnEnabled()
		{
			base.OnEnabled();
			if (m_module != null)
			{
				Setup();
			}
		}
	}
}
