using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Screens.Popup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Utils.Extensions;
using SharedBaseLib.System.Extensions;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.InventoryScreen
{
	public class UIInventoryScreenData
	{
		public sealed class QualityRank
		{
			public Quality.EQuality Quality;

			public int Rank;
		}

		public enum EScreenState
		{
			Inventory = 0,
			Details = 1,
			Forge = 2
		}

		public class StateChangeData
		{
			public EScreenState OldState;

			public EScreenState NewState;
		}

		private const int k_maxForgeSize = 6;

		public static readonly List<QualityRank> s_QualityRanks = new List<QualityRank>
		{
			new QualityRank
			{
				Quality = Quality.EQuality.Mundane,
				Rank = 1
			},
			new QualityRank
			{
				Quality = Quality.EQuality.Common,
				Rank = 2
			},
			new QualityRank
			{
				Quality = Quality.EQuality.Uncommon,
				Rank = 3
			},
			new QualityRank
			{
				Quality = Quality.EQuality.Rare,
				Rank = 4
			},
			new QualityRank
			{
				Quality = Quality.EQuality.Perfect,
				Rank = 5
			}
		};

		private static readonly int[] s_index3 = new int[3] { 0, 1, 2 };

		private static readonly int[] s_index4 = new int[4] { 0, 1, 2, 4 };

		private static readonly int[] s_index5 = new int[5] { 0, 1, 2, 3, 5 };

		private static readonly int[] s_index6 = new int[6] { 0, 1, 2, 3, 4, 5 };

		private readonly ItemCompareBase m_defaultSorting = new ItemCompareDefault();

		private Predicate<InventoryItemBase> m_filter = ItemFilters.FilterAll;

		private ItemCompareBase m_sorting;

		private EScreenState m_state;

		private InventoryElement[] m_forge = new InventoryElement[6];

		private InventoryElement m_selectedItemToForge;

		private List<InventoryItemBase> m_inventory = new List<InventoryItemBase>();

		public EScreenState State
		{
			get
			{
				return m_state;
			}
			set
			{
				if (m_state != value)
				{
					EScreenState state = m_state;
					m_state = value;
					Action<StateChangeData> onStateChanged = this.OnStateChanged;
					if (onStateChanged != null)
					{
						onStateChanged(new StateChangeData
						{
							NewState = m_state,
							OldState = state
						});
					}
				}
			}
		}

		public List<InventoryItemBase> VisibleInventory
		{
			get
			{
				return m_inventory;
			}
			set
			{
				m_inventory = value;
				Action<List<InventoryItemBase>> onVisibleInventoryChange = this.OnVisibleInventoryChange;
				if (onVisibleInventoryChange != null)
				{
					onVisibleInventoryChange(m_inventory);
				}
			}
		}

		public Predicate<InventoryItemBase> CurrentFilter
		{
			get
			{
				return m_filter;
			}
			set
			{
				if (!object.ReferenceEquals(value, m_filter))
				{
					m_filter = value;
					Trigger(this.OnFilterChange);
				}
			}
		}

		public ItemCompareBase CurrentSorting
		{
			get
			{
				return m_sorting ?? m_defaultSorting;
			}
			set
			{
				if (m_sorting != value)
				{
					m_sorting = value;
					Trigger(this.OnSortingChange);
				}
			}
		}

		public bool CanUpdateInventory
		{
			get
			{
				return !IsForging;
			}
		}

		public int ForgeSize
		{
			get
			{
				int num = 0;
				for (int i = 0; i < m_forge.Length; i++)
				{
					if (m_forge[i] != null)
					{
						num++;
					}
				}
				return num;
			}
		}

		public int RequiredForgeSize
		{
			get
			{
				ForgeMapping.NeededSlotCount[] neededSlotCounts = Singleton<StateManager>.Instance.TemplateManager.ForgeMapping.neededSlotCounts;
				Quality.EQuality eQuality = ForgedQuality;
				if (!IsForgeValid && m_selectedItemToForge != null)
				{
					eQuality = m_selectedItemToForge.quality;
				}
				for (int i = 0; i < neededSlotCounts.Length; i++)
				{
					if (neededSlotCounts[i].quality == eQuality)
					{
						return neededSlotCounts[i].slotCount;
					}
				}
				return 6;
			}
		}

		public bool IsForgeFull
		{
			get
			{
				return ForgeSize >= RequiredForgeSize;
			}
		}

		public bool IsForgeEmpty
		{
			get
			{
				return ForgeSize == 0;
			}
		}

		public List<string> ForgedIds
		{
			get
			{
				return from _x in m_forge
					where _x != null
					select _x.id;
			}
		}

		public List<UIPopupResourcesContent.ResourceConfiguration> ForgeCostConfiguration
		{
			get
			{
				Resource resource = NormalForgeCost();
				if (!IsForgeFull)
				{
					Resource resource2 = OverspendForgeCost();
					resource.amount += resource2.amount;
				}
				List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>();
				list.Add(new UIPopupResourcesContent.ResourceConfiguration
				{
					Resource = resource,
					HasEnoughResources = Singleton<StateManager>.Instance.ProfileManager.Profile.HasEnoughResources(resource)
				});
				return list;
			}
		}

		public Resource ReRollCost
		{
			get
			{
				Resource resource;
				if (!IsForgeValid)
				{
					resource = new Resource();
					resource.amount = 0L;
					resource.type = EResourceType.SoftCurrency;
					return resource;
				}
				ForgeMapping.ReRollCost[] reRollCosts = Singleton<StateManager>.Instance.TemplateManager.ForgeMapping.reRollCosts;
				for (int i = 0; i < reRollCosts.Length; i++)
				{
					if (reRollCosts[i].quality == ForgedQuality)
					{
						return reRollCosts[i].cost;
					}
				}
				resource = new Resource();
				resource.amount = 0L;
				resource.type = EResourceType.SoftCurrency;
				return resource;
			}
		}

		public List<UIPopupResourcesContent.ResourceConfiguration> ReRollCostConfigurations
		{
			get
			{
				List<UIPopupResourcesContent.ResourceConfiguration> list = new List<UIPopupResourcesContent.ResourceConfiguration>();
				list.Add(new UIPopupResourcesContent.ResourceConfiguration
				{
					Resource = ReRollCost,
					HasEnoughResources = Singleton<StateManager>.Instance.ProfileManager.Profile.HasEnoughResources(ReRollCost)
				});
				return list;
			}
		}

		public int ForgeOutcomeLevel
		{
			get
			{
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < m_forge.Length; i++)
				{
					if (m_forge[i] != null && !m_forge[i].IsBlueprint())
					{
						num2 += m_forge[i].itemData.level;
						num++;
					}
				}
				if (num == 0)
				{
					return 0;
				}
				double num3 = Math.Round((float)num2 * Singleton<StateManager>.Instance.TemplateManager.ForgeMapping.levelReferenceFactor / (float)num, MidpointRounding.AwayFromZero);
				return Mathf.Clamp((int)num3, 1, Singleton<StateManager>.Instance.TemplateManager.Globals.maxAssassinLevel);
			}
		}

		public int MaxForgeLevel
		{
			get
			{
				int num = 0;
				for (int i = 0; i < m_forge.Length; i++)
				{
					if (m_forge[i] != null && m_forge[i].itemData != null && m_forge[i].itemData.level > num)
					{
						num = m_forge[i].itemData.level;
					}
				}
				return num;
			}
		}

		public bool IsOutcomeKnown
		{
			get
			{
				return m_forge.Find((InventoryElement _x) => _x != null && _x.IsBlueprint()) != null;
			}
		}

		public EBlueprintType ForgeOutcomeType
		{
			get
			{
				InventoryElement inventoryElement = m_forge.Find((InventoryElement _x) => _x != null && _x.IsBlueprint());
				return (inventoryElement == null) ? EBlueprintType.BlueprintWeapon : inventoryElement.blueprintData.type;
			}
		}

		public Quality.EQuality ForgeOutcomeQuality
		{
			get
			{
				return (Quality.EQuality)Math.Min(5, (int)(ForgedQuality + 1));
			}
		}

		public Quality.EQuality ForgedQuality
		{
			get
			{
				InventoryElement inventoryElement = m_forge.Find((InventoryElement _x) => _x != null && _x.IsItem());
				return (inventoryElement == null) ? Quality.EQuality.Common : inventoryElement.quality;
			}
		}

		public bool IsForging { get; set; }

		public bool IsForgeValid
		{
			get
			{
				return m_forge.Count((InventoryElement _x) => _x != null && _x.IsItem()) > 0;
			}
		}

		public bool ForgePossible
		{
			get
			{
				if (UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsForgeActive)
				{
					return true;
				}
				if (Singleton<StateManager>.Instance.ProfileManager.InventoryUsage <= Singleton<StateManager>.Instance.ProfileManager.InventorySize)
				{
					return true;
				}
				int num = Singleton<StateManager>.Instance.ProfileManager.InventorySize + ForgeSize - Singleton<StateManager>.Instance.ProfileManager.InventoryUsage;
				if (num <= 0)
				{
					return false;
				}
				if (num > 1)
				{
					return true;
				}
				InventoryElement inventoryElement = m_forge.Find((InventoryElement _x) => _x != null && _x.IsBlueprint());
				if (inventoryElement != null && inventoryElement.stackSize > 1)
				{
					return false;
				}
				return true;
			}
		}

		public event Action<InventoryElement, InventoryElement> OnEquipItem;

		public event Action<InventoryElement, InventoryElement> OnUnEquipItem;

		public event Action<List<InventoryItemBase>> OnVisibleInventoryChange;

		public event Action OnFilterChange;

		public event Action OnSortingChange;

		public event Action<StateChangeData> OnStateChanged;

		public event Action<InventoryElement, int> OnAddedToForge;

		public event Action<InventoryElement, int> OnRemovedFromForge;

		public event Action OnForgeChange;

		public event Action OnForgeCleared;

		public event Action OnForgeShowGridPreview;

		public event Action OnForgeHideGridPreview;

		public void ResetSorting()
		{
			CurrentSorting = null;
		}

		public void ItemEquipped(InventoryElement _newItem, InventoryElement _oldItem, bool _equipped)
		{
			if (this.OnEquipItem != null && _equipped)
			{
				this.OnEquipItem(_newItem, _oldItem);
			}
			else if (this.OnUnEquipItem != null && !_equipped)
			{
				this.OnUnEquipItem(_newItem, _oldItem);
			}
		}

		public void ClearForge(bool _trigger = true)
		{
			m_forge = new InventoryElement[6];
			m_selectedItemToForge = null;
			Action onForgeCleared = this.OnForgeCleared;
			if (onForgeCleared != null && _trigger)
			{
				onForgeCleared();
			}
		}

		public void AddToForge(InventoryElement _element)
		{
			if (!_element.CanBeForged())
			{
				return;
			}
			int num = ((!_element.IsBlueprint()) ? (-1) : ForgeIndexBluePrint());
			if (num == -1)
			{
				num = NextForgeIndex();
			}
			else
			{
				Action<InventoryElement, int> onRemovedFromForge = this.OnRemovedFromForge;
				if (onRemovedFromForge != null)
				{
					onRemovedFromForge(m_forge[num], num);
				}
			}
			m_forge[num] = _element;
			Action<InventoryElement, int> onAddedToForge = this.OnAddedToForge;
			if (onAddedToForge != null)
			{
				onAddedToForge(_element, num);
			}
			Trigger(this.OnForgeChange);
		}

		public int ForgeIndexBluePrint()
		{
			for (int i = 0; i < m_forge.Length; i++)
			{
				if (m_forge[i] != null && m_forge[i].IsBlueprint())
				{
					return i;
				}
			}
			return -1;
		}

		public void RemoveFromForge(InventoryElement _element)
		{
			int num = ElementIndex(_element);
			if (num != -1)
			{
				m_forge[num] = null;
				if (this.OnRemovedFromForge != null)
				{
					this.OnRemovedFromForge(_element, num);
				}
				Trigger(this.OnForgeChange);
			}
		}

		public int ElementIndex(InventoryElement _element)
		{
			return m_forge.FindIndex((InventoryElement _x) => _x != null && _element.id == _x.id);
		}

		public InventoryElement ElementAt(int _index)
		{
			return m_forge[_index];
		}

		public void ShowForgeGridPreview(InventoryElement _item)
		{
			InventoryElement selectedItemToForge = m_selectedItemToForge;
			m_selectedItemToForge = _item;
			if (selectedItemToForge == null || selectedItemToForge.quality != _item.quality)
			{
				Action onForgeShowGridPreview = this.OnForgeShowGridPreview;
				if (onForgeShowGridPreview != null)
				{
					onForgeShowGridPreview();
				}
			}
		}

		public void HideForgeGridPreview()
		{
			if (m_selectedItemToForge != null)
			{
				m_selectedItemToForge = null;
				Action onForgeHideGridPreview = this.OnForgeHideGridPreview;
				if (onForgeHideGridPreview != null)
				{
					onForgeHideGridPreview();
				}
			}
		}

		private int NextForgeIndex()
		{
			if (IsForgeFull)
			{
				return -1;
			}
			int forgeSize = ForgeSize;
			int requiredForgeSize = RequiredForgeSize;
			int num = 5;
			if (forgeSize >= 0 && forgeSize <= 2)
			{
				num = forgeSize;
			}
			if (forgeSize == 3 && requiredForgeSize == 4)
			{
				num = 4;
			}
			if (forgeSize == 3 && requiredForgeSize > 4)
			{
				num = 3;
			}
			if (forgeSize == 4 && requiredForgeSize == 5)
			{
				num = 5;
			}
			if (forgeSize == 4 && requiredForgeSize == 6)
			{
				num = 4;
			}
			if (m_forge[num] != null)
			{
				for (int i = 0; i < requiredForgeSize; i++)
				{
					if (m_forge[i] == null)
					{
						num = i;
						break;
					}
				}
			}
			return num;
		}

		public bool IndexVisible(int _index)
		{
			if (_index == -1)
			{
				return true;
			}
			switch (RequiredForgeSize)
			{
			case 3:
				return s_index3.Contains(_index);
			case 4:
				return s_index4.Contains(_index);
			case 5:
				return s_index5.Contains(_index);
			case 6:
				return s_index6.Contains(_index);
			default:
				return false;
			}
		}

		public bool IsInForge(InventoryElement _element)
		{
			return m_forge.Exists((InventoryElement _x) => _x != null && _x.id.Equals(_element.id));
		}

		public bool CanBeAddedToForge(InventoryElement _element)
		{
			if (!_element.CanBeForged())
			{
				return false;
			}
			if (!IsForgeEmpty && IsForgeFull && ForgeIndexBluePrint() == -1)
			{
				return false;
			}
			if (ForgeSize == 0)
			{
				return _element.CanStartForge();
			}
			return true;
		}

		private Resource NormalForgeCost()
		{
			ForgeMapping.CostData.Entry entry = Singleton<StateManager>.Instance.TemplateManager.ForgeMapping.costData.entries.Find((ForgeMapping.CostData.Entry _x) => _x.quality == ForgedQuality && _x.level == ForgeOutcomeLevel);
			Resource resource = new Resource();
			resource.amount = 0L;
			resource.type = EResourceType.SoftCurrency;
			Resource resource2 = resource;
			if (entry != null)
			{
				resource2.type = entry.cost.type;
				resource2.amount = entry.cost.amount;
			}
			return resource2;
		}

		private Resource OverspendForgeCost()
		{
			ForgeMapping.AutoCompletionCost[] autoCompletionCosts = Singleton<StateManager>.Instance.TemplateManager.ForgeMapping.autoCompletionCosts;
			for (int i = 0; i < autoCompletionCosts.Length; i++)
			{
				if (autoCompletionCosts[i].quality == ForgedQuality && autoCompletionCosts[i].occupiedSlots == ForgeSize)
				{
					return autoCompletionCosts[i].cost;
				}
			}
			Resource resource = new Resource();
			resource.amount = 0L;
			resource.type = EResourceType.HardCurrency;
			return resource;
		}

		private void Trigger(Action _action)
		{
			if (_action != null)
			{
				_action();
			}
		}
	}
}
