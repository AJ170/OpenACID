using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.Interfaces;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils;
using Assets.Scripts.UI.Utils.Development;
using Assets.Scripts.UI.Utils.Extensions;
using Assets.Scripts.UI.Widget;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIAssassinStats : UIScreenChild, IInitialized<UIInventoryScreenData>
	{
		[CheckAssigned]
		[SerializeField]
		private UIExtendedButton m_closeButton;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_tab;

		[CheckAssigned]
		[SerializeField]
		private GameObject m_header;

		[CheckAssigned]
		[SerializeField]
		private UITable m_table;

		[SerializeField]
		[CheckAssigned]
		private UIRect m_boundsRect;

		[CheckAssigned]
		[SerializeField]
		private UIScrollView m_scrollView;

		private UIInventoryScreenData m_data;

		private GameObjectCache m_tabCache;

		private GameObjectCache m_headerCache;

		private IdGenerator<UIAssassinStats> m_id;

		private AssassinStatsTab m_currenTab;

		private readonly List<GameObjectCache.GameObjectCacheEntry> m_tabs = new List<GameObjectCache.GameObjectCacheEntry>();

		private readonly List<GameObjectCache.GameObjectCacheEntry> m_headings = new List<GameObjectCache.GameObjectCacheEntry>();

		private string NextName
		{
			get
			{
				return string.Format("{0:D3}", (long)m_id.Next());
			}
		}

		protected override void OnScreenInit()
		{
			m_headerCache = new GameObjectCache(m_header, m_table.transform);
			m_tabCache = new GameObjectCache(m_tab, m_table.transform);
			m_headerCache.Reserve(2);
			m_tabCache.Reserve(16);
			base.gameObject.SetActive(false);
		}

		protected override void OnScreenDestroy()
		{
			m_headerCache.Destroy();
			m_headerCache = null;
			m_tabCache.Destroy();
			m_tabCache = null;
		}

		public void Initialize(UIInventoryScreenData _data)
		{
			if (m_data == null)
			{
				base.gameObject.SetActive(true);
				m_data = _data;
				m_id = new IdGenerator<UIAssassinStats>();
				Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Add(OnAssassinChange);
				Singleton<StateManager>.Instance.ProfileManager.OnInventoryUpdate += OnInventoryUpdated;
				SetupStats();
				OnAssassinChange();
			}
		}

		public void Clear()
		{
			m_data = null;
			m_id = null;
			m_currenTab = null;
			m_headerCache.ReleaseAll();
			m_tabCache.ReleaseAll(OnTabReleased);
			m_tabs.Clear();
			m_headings.Clear();
			m_table.onReposition = null;
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Remove(OnAssassinChange);
			Singleton<StateManager>.Instance.ProfileManager.OnInventoryUpdate -= OnInventoryUpdated;
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_closeButton.onClick, OnCloseButtonClick);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_closeButton.onClick, OnCloseButtonClick);
		}

		private void OnCloseButtonClick()
		{
			m_data.State = UIInventoryScreenData.EScreenState.Inventory;
		}

		private void SetupStats()
		{
			CreateList();
			m_table.onReposition = OnReposition;
			m_table.repositionNow = true;
		}

		private void OnReposition()
		{
			m_table.onReposition = null;
			m_scrollView.ResetPosition();
		}

		private void CreateList()
		{
			List<AssassinStatsTab.AssassinStatData> list = from _x in UbiCore.Utils.Singleton<UIDataMapper>.Instance.StatDescriptions
				where _x.Value.Primary
				select new AssassinStatsTab.AssassinStatData
				{
					Stat = (EAssassinStat)_x.Key,
					Description = _x.Value.DescriptionId,
					IconName = _x.Value.StatTextureIdentifier,
					Name = _x.Value.NameId,
					Primary = _x.Value.Primary,
					Factor = AssassinStatsHelper.IsFactor((EAssassinStat)_x.Key),
					GetStatsValue = (AvatarData.IAvatarDefinition _definition, Assassin _assassin) => GetNormalStatValue(_definition, (EAssassinStat)_x.Key)
				};
			list.Sort((AssassinStatsTab.AssassinStatData l, AssassinStatsTab.AssassinStatData r) => l.Stat.CompareTo(r.Stat));
			List<AssassinStatsTab.AssassinStatData> list2 = from _x in UbiCore.Utils.Singleton<UIDataMapper>.Instance.StatDescriptions
				where !_x.Value.Primary
				select new AssassinStatsTab.AssassinStatData
				{
					Stat = (EAssassinStat)_x.Key,
					Description = _x.Value.DescriptionId,
					IconName = _x.Value.StatTextureIdentifier,
					Name = _x.Value.NameId,
					Primary = _x.Value.Primary,
					Factor = AssassinStatsHelper.IsFactor((EAssassinStat)_x.Key),
					GetStatsValue = (AvatarData.IAvatarDefinition _definition, Assassin _assassin) => GetNormalStatValue(_definition, (EAssassinStat)_x.Key)
				};
			list2.Sort((AssassinStatsTab.AssassinStatData l, AssassinStatsTab.AssassinStatData r) => l.Stat.CompareTo(r.Stat));
			AddPowerStat(list);
			m_headings.Add(CreateHeading(1773));
			for (int i = 0; i < list.Count; i++)
			{
				m_tabs.Add(CreateTab(list[i]));
			}
			m_headings.Add(CreateHeading(1774));
			for (int j = 0; j < list2.Count; j++)
			{
				m_tabs.Add(CreateTab(list2[j]));
			}
		}

		private GameObjectCache.GameObjectCacheEntry CreateTab(AssassinStatsTab.AssassinStatData _data)
		{
			GameObjectCache.GameObjectCacheEntry gameObjectCacheEntry = m_tabCache.Dequeue();
			gameObjectCacheEntry.Item.name = NextName;
			UIWidget component = gameObjectCacheEntry.Item.GetComponent<UIWidget>();
			AssassinStatsTab component2 = gameObjectCacheEntry.Item.GetComponent<AssassinStatsTab>();
			component.leftAnchor.target = m_boundsRect.transform;
			component.leftAnchor.absolute = 20;
			component.rightAnchor.target = m_boundsRect.transform;
			component.rightAnchor.absolute = -20;
			component2.OnTabClick += OnTabSelected;
			component2.Initialize(_data);
			component2.Select(false, true);
			return gameObjectCacheEntry;
		}

		private GameObjectCache.GameObjectCacheEntry CreateHeading(StringId _id)
		{
			GameObjectCache.GameObjectCacheEntry gameObjectCacheEntry = m_headerCache.Dequeue();
			gameObjectCacheEntry.Item.name = NextName;
			UIWidget component = gameObjectCacheEntry.Item.GetComponent<UIWidget>();
			component.leftAnchor.target = m_boundsRect.transform;
			component.leftAnchor.absolute = 0;
			component.rightAnchor.target = m_boundsRect.transform;
			component.rightAnchor.absolute = 0;
			StatsHeader component2 = gameObjectCacheEntry.Item.GetComponent<StatsHeader>();
			component2.Initialize(new StatsHeader.HeaderData
			{
				Header = _id
			});
			return gameObjectCacheEntry;
		}

		private void RefreshStats()
		{
			AvatarData.IAvatarDefinition playerDefinition = UbiCore.Utils.Singleton<AvatarData>.Instance.GetPlayerDefinition();
			for (int i = 0; i < m_tabs.Count; i++)
			{
				m_tabs[i].Item.GetComponent<AssassinStatsTab>().Refresh(playerDefinition);
			}
		}

		private void OnTabSelected(AssassinStatsTab _tab)
		{
			if (m_currenTab != null)
			{
				m_currenTab.Select(false);
			}
			if (m_currenTab == _tab)
			{
				m_currenTab = null;
				return;
			}
			m_currenTab = _tab;
			m_currenTab.Select(true);
		}

		private void OnTabReleased(GameObject _gameObject)
		{
			AssassinStatsTab component = _gameObject.GetComponent<AssassinStatsTab>();
			component.OnTabClick -= OnTabSelected;
			component.Select(false, true);
		}

		private void OnAssassinChange(params object[] _params)
		{
			if (Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin != null)
			{
				UbiCore.Utils.Singleton<AvatarData>.Instance.SetPlayer(Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin);
			}
			RefreshStats();
		}

		private void OnInventoryUpdated(InventoryElement[] _newElements, InventoryElement[] _oldElements, int _newSize, int _oldSize)
		{
			RefreshStats();
		}

		private static AssassinStatsTab.AssassinStatValue GetNormalStatValue(AvatarData.IAvatarDefinition _avatarDefinition, EAssassinStat _type)
		{
			bool _isBuffed;
			float stat = AssassinStatsHelper.GetStat(_avatarDefinition, _type, true, out _isBuffed);
			float stat2 = AssassinStatsHelper.GetStat(_avatarDefinition, _type, false, out _isBuffed);
			AssassinStatsTab.AssassinStatValue assassinStatValue = new AssassinStatsTab.AssassinStatValue();
			assassinStatValue.BuffedValue = stat2;
			assassinStatValue.NormalValue = stat;
			return assassinStatValue;
		}

		private static AssassinStatsTab.AssassinStatValue GetItemPowerValue(Assassin _assassin)
		{
			AssassinStatsTab.AssassinStatValue assassinStatValue = new AssassinStatsTab.AssassinStatValue();
			assassinStatValue.BuffedValue = _assassin.TotalItemPower();
			assassinStatValue.NormalValue = _assassin.TotalItemPower();
			return assassinStatValue;
		}

		private static void AddPowerStat(List<AssassinStatsTab.AssassinStatData> _list)
		{
			_list.Insert(0, new AssassinStatsTab.AssassinStatData
			{
				Description = new StringId(7306),
				IconName = "aUI_icon_statPower",
				Name = new StringId(7305),
				Primary = true,
				Factor = false,
				GetStatsValue = (AvatarData.IAvatarDefinition _definition, Assassin _assassin) => GetItemPowerValue(_assassin)
			});
		}
	}
}
