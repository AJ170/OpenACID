using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Screens.Popup.CallToActionPopup;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.EventManager;
using Assets.Scripts.UI.State.EventManager.Data;
using Assets.Scripts.UI.State.EventManager.HppyCourCategoryManager;
using Assets.Scripts.UI.State.ThemeManager;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using Database;
using SharedBaseLib.Core;
using UbiCore.Debug.UI;
using UnityEngine;

public class DebugMenuGimpliEvents : DebugMenuItem
{
	private const int c_endTimeOffsetHours = 1;

	private readonly DateTime m_unixTimeReference = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

	private static string s_fakeEventName = "Fake Event";

	private static string s_discountFactorFormat = "#0.00";

	private GUIStyle m_lableStyle;

	private readonly string[] m_dateNames = new string[6] { "Second", "Minute", "Hour", "Day", "Month", "Year" };

	private int m_selectedIncreasIndex = 1;

	private DateTime m_startTime;

	private DateTime m_endTime;

	private float m_discountFactor = 0.5f;

	private bool m_isVisible;

	private int m_positionOverride;

	private string[] m_shopProductsName;

	private Dictionary<string, string> m_shopProductsIDToName;

	private string[] m_shopbannerTemplateNames;

	private int[] m_shopbannerEventTemplateIndex;

	private Vector2 m_eventTypeScrollPosition;

	private int m_selectedEventTypeIndex;

	private bool m_showEventTypeSelectionBox;

	private Vector2 m_flavorThemeScrollPosition;

	private int m_selectedFlavorThemeIndex;

	private bool m_showFlavorThemeSelectionBox;

	private Vector2 m_shopCategoryScrollPosition;

	private int m_selectedShopCategoryIndex;

	private bool m_showShopCategorySelectionBox;

	private Vector2 m_shopBundleScrollPosition;

	private int m_selectedShopBundleIndex;

	private bool m_showShopBundleSelectionBox;

	private Vector2 m_advertisementTemplateScrollPosition;

	private int m_selectedAdvertisementTemplateIndex;

	private bool m_showAdvertisementTemplateSelectionBox;

	private string[] m_eventThemesNames;

	private int[] m_sortedEventThemesIndex;

	private Vector2 m_eventThemeScrollPosition;

	private int m_selectedEventThemeIndex;

	private bool m_showEventThemeSelectionBox;

	private string[] m_eventHeaderNames;

	private int[] m_sortedEventHeaderIndex;

	private Vector2 m_eventHeaderScrollPosition;

	private int m_selectedEventHeaderIndex;

	private bool m_showEventHeaderSelectionBox;

	private string[] m_eventSubHeaderNames;

	private int[] m_sortedEventSubHeaderIndex;

	private Vector2 m_eventSubHeaderScrollPosition;

	private int m_selectedEventSubHeaderIndex;

	private bool m_showEventSubHeaderSelectionBox;

	private Vector2 m_shopBundleVisibilityPosition;

	private bool m_showCallToActionDebug;

	private List<KeyValuePair<GameID, DBEntry>> m_callToActionDefinitions;

	private string[] ShopProductsName
	{
		get
		{
			InitProductList();
			return m_shopProductsName;
		}
	}

	private Dictionary<string, string> ShopProductsIDToName
	{
		get
		{
			InitProductList();
			return m_shopProductsIDToName;
		}
	}

	private string[] ShopbannerTemplateNames
	{
		get
		{
			if (m_shopbannerTemplateNames == null)
			{
				EventTemplates eventTemplates = Singleton<StateManager>.Instance.TemplateManager.EventTemplates;
				List<string> list = new List<string>();
				List<int> list2 = new List<int>();
				for (int i = 0; i < eventTemplates.templates.Length; i++)
				{
					if (eventTemplates.templates[i].type == EEventType.Event_Shop_Ad_Banner)
					{
						list.Add(eventTemplates.templates[i].name);
						list2.Add(i);
					}
				}
				m_shopbannerTemplateNames = list.ToArray();
				m_shopbannerEventTemplateIndex = list2.ToArray();
			}
			return m_shopbannerTemplateNames;
		}
	}

	private string[] EventThemesNames
	{
		get
		{
			if (m_eventThemesNames == null)
			{
				EventsUserFriendlyMapping.ClientThemeAssetMappingEntry[] clientThemeAssetMapping = Singleton<StateManager>.Instance.TemplateManager.EventsUserFriendlyMapping.clientThemeAssetMapping;
				m_eventThemesNames = new string[clientThemeAssetMapping.Length];
				m_sortedEventThemesIndex = new int[clientThemeAssetMapping.Length];
				for (int i = 0; i < clientThemeAssetMapping.Length; i++)
				{
					m_eventThemesNames[i] = clientThemeAssetMapping[i].themeLabel;
					m_sortedEventThemesIndex[i] = i;
				}
				Array.Sort(m_sortedEventThemesIndex, (int _indexA, int _indexB) => string.Compare(m_eventThemesNames[_indexA], m_eventThemesNames[_indexB], StringComparison.InvariantCultureIgnoreCase));
				Array.Sort(m_eventThemesNames, (string _nameA, string _nameB) => string.Compare(_nameA, _nameB, StringComparison.InvariantCultureIgnoreCase));
			}
			return m_eventThemesNames;
		}
	}

	private string[] EventHeaderNames
	{
		get
		{
			if (m_eventHeaderNames == null)
			{
				EventsUserFriendlyMapping.DisplayMessageMappingEntry[] displayMessageMapping = Singleton<StateManager>.Instance.TemplateManager.EventsUserFriendlyMapping.displayMessageMapping;
				m_eventHeaderNames = new string[displayMessageMapping.Length];
				m_sortedEventHeaderIndex = new int[displayMessageMapping.Length];
				for (int i = 0; i < displayMessageMapping.Length; i++)
				{
					m_eventHeaderNames[i] = displayMessageMapping[i].messageLabel;
					m_sortedEventHeaderIndex[i] = i;
				}
				Array.Sort(m_sortedEventHeaderIndex, (int _indexA, int _indexB) => string.Compare(m_eventHeaderNames[_indexA], m_eventHeaderNames[_indexB], StringComparison.InvariantCultureIgnoreCase));
				Array.Sort(m_eventHeaderNames, (string _nameA, string _nameB) => string.Compare(_nameA, _nameB, StringComparison.InvariantCultureIgnoreCase));
			}
			return m_eventHeaderNames;
		}
	}

	private string[] EventSubHeaderNames
	{
		get
		{
			if (m_eventSubHeaderNames == null)
			{
				EventsUserFriendlyMapping.DisplayMessageMappingEntry[] displaySubMessageMapping = Singleton<StateManager>.Instance.TemplateManager.EventsUserFriendlyMapping.displaySubMessageMapping;
				m_eventSubHeaderNames = new string[displaySubMessageMapping.Length];
				m_sortedEventSubHeaderIndex = new int[displaySubMessageMapping.Length];
				for (int i = 0; i < displaySubMessageMapping.Length; i++)
				{
					m_eventSubHeaderNames[i] = displaySubMessageMapping[i].messageLabel;
					m_sortedEventSubHeaderIndex[i] = i;
				}
				Array.Sort(m_sortedEventSubHeaderIndex, (int _indexA, int _indexB) => string.Compare(m_eventSubHeaderNames[_indexA], m_eventSubHeaderNames[_indexB], StringComparison.InvariantCultureIgnoreCase));
				Array.Sort(m_eventSubHeaderNames, (string _nameA, string _nameB) => string.Compare(_nameA, _nameB, StringComparison.InvariantCultureIgnoreCase));
			}
			return m_eventSubHeaderNames;
		}
	}

	public override string GetName()
	{
		return "Fake Events";
	}

	private void InitProductList()
	{
		if (m_shopProductsName == null)
		{
			ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
			m_shopProductsName = new string[shopTable.bundles.Length + 1];
			m_shopProductsIDToName = new Dictionary<string, string>(shopTable.bundles.Length);
			m_shopProductsName[0] = "<None>";
			m_shopProductsIDToName[string.Empty] = m_shopProductsName[0];
			for (int i = 0; i < shopTable.bundles.Length; i++)
			{
				m_shopProductsName[i + 1] = shopTable.bundles[i].name;
				m_shopProductsIDToName[shopTable.bundles[i].id] = shopTable.bundles[i].name;
			}
		}
	}

	private void IncreaseCount(ref DateTime _currentTime, bool _add)
	{
		int num = (_add ? 1 : (-1));
		switch (m_selectedIncreasIndex)
		{
		case 0:
			_currentTime = _currentTime.AddSeconds(num);
			break;
		case 1:
			_currentTime = _currentTime.AddMinutes(num);
			break;
		case 2:
			_currentTime = _currentTime.AddHours(num);
			break;
		case 3:
			_currentTime = _currentTime.AddDays(num);
			break;
		case 4:
			_currentTime = _currentTime.AddMonths(num);
			break;
		case 5:
			_currentTime = _currentTime.AddYears(num);
			break;
		}
	}

	public override void OnOpen()
	{
		DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
		m_startTime = new DateTime(utcNow.Ticks);
		m_endTime = new DateTime(utcNow.Ticks).AddHours(1.0);
	}

	public override void OnApply()
	{
	}

	public override void OnMenu()
	{
		m_lableStyle = GUI.skin.GetStyle("Label");
		GUILayout.BeginVertical();
		DrawHeaderIncreaseButtons();
		DrawEventTimers();
		GUILayout.BeginVertical("Box");
		DrawDropDown("Event", ref m_eventTypeScrollPosition, ref m_selectedEventTypeIndex, ref m_showEventTypeSelectionBox, Enum.GetNames(typeof(EEventType)));
		GUILayout.EndVertical();
		DrawEventData();
		GUILayout.Space(10f);
		DrawOptions();
		GUILayout.Space(10f);
		DrawEventInfoTable();
		GUILayout.EndVertical();
		GUILayout.Space(10f);
		GUILayout.BeginVertical("Box");
		m_showCallToActionDebug = GUILayout.Toggle(m_showCallToActionDebug, "Debug CallToAction");
		if (m_showCallToActionDebug)
		{
			bool flag = GUI.enabled;
			if (m_callToActionDefinitions == null)
			{
				m_callToActionDefinitions = CallToActionPopup.GetSortedDefinitions();
			}
			for (int i = 0; i < m_callToActionDefinitions.Count; i++)
			{
				CTADefinitionBase cTADefinitionBase = (CTADefinitionBase)m_callToActionDefinitions[i].Value;
				string text = cTADefinitionBase.Name;
				text = text + " " + cTADefinitionBase.Priority;
				GUI.enabled = flag && !(cTADefinitionBase is CTADefinitionCommonCondition);
				if (GUILayout.Button(text, GUILayout.Width(400f)))
				{
					m_debugMenu.CloseDebugMenu();
					CallToActionPopup.Show(cTADefinitionBase);
				}
			}
			GUI.enabled = flag;
		}
		else
		{
			m_callToActionDefinitions = null;
		}
		GUILayout.EndVertical();
		GUILayout.FlexibleSpace();
	}

	private void DrawHeaderIncreaseButtons()
	{
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal("Box");
		GUILayout.Label("Settings");
		m_selectedIncreasIndex = GUI.SelectionGrid(new Rect(80f, 5f, 400f, 25f), m_selectedIncreasIndex, m_dateNames, 6);
		GUILayout.Space(60f);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private void DrawEventTimers()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.BeginHorizontal();
		DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
		GUILayout.Label("Server Time", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Space(100f);
		GUILayout.Label(utcNow.ToString(), m_lableStyle, GUILayout.Width(150f));
		GUILayout.Space(10f);
		GUILayout.EndHorizontal();
		DrawIncDecTimeField("Start Date", ref m_startTime);
		DrawIncDecTimeField("End Date", ref m_endTime);
		GUILayout.EndVertical();
	}

	private void DrawOptions()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.Space(30f);
		GUILayout.BeginHorizontal();
		Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
		EventManager eventManager = Singleton<StateManager>.Instance.EventManager;
		if (GUILayout.Button("Remove all Events", GUILayout.Width(150f)))
		{
			events.events = new AcierProtocol.Event[0];
			eventManager.OnUpdateEvents();
		}
		GUILayout.Space(30f);
		if (GUILayout.Button("Remove Fake Events", GUILayout.Width(150f)))
		{
			int num = 0;
			while (num < events.events.Length)
			{
				if (events.events[num].name == s_fakeEventName)
				{
					events.events = events.events.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
			eventManager.OnUpdateEvents();
		}
		GUILayout.Space(30f);
		if (GUILayout.Button("Create Fake Events", GUILayout.Width(150f)))
		{
			CreateFakeEvent();
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Create All banners", GUILayout.Width(150f)))
		{
			CreateAllBanners();
		}
		GUILayout.Space(30f);
		if (GUILayout.Button("Discount All Categories", GUILayout.Width(150f)))
		{
			DiscountAllCategories();
		}
		GUILayout.Space(30f);
		if (GUILayout.Button("Show All Items", GUILayout.Width(150f)))
		{
			ShowAllItems();
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}

	private void DrawEventData()
	{
		GUILayout.BeginVertical();
		switch ((EEventType)m_selectedEventTypeIndex)
		{
		case EEventType.Event_Shop_Item_Visibility:
			DrawShopBundleVisibility();
			break;
		case EEventType.Event_Shop_Category_Visibility:
			DrawShopCategoryVisibility();
			break;
		case EEventType.Event_Shop_Item_Percentage:
			DrawShopBundlePercentage();
			break;
		case EEventType.Event_Shop_Category_Percentage:
			DrawShopCategoryPercentage();
			break;
		case EEventType.Event_Shop_Ad_Banner:
			DrawShopAdvertisements();
			break;
		case EEventType.Event_Shop_Hard_Currency_Payout_Increase:
			DrawHappyHourPercentage();
			break;
		case EEventType.Event_Shop_Category_Payout_Increase:
			DrawHappyHourCategoryPercentage();
			break;
		case EEventType.Event_Progression_Challenges:
			DrawEventProgressionChallenges();
			break;
		case EEventType.Event_Flavor:
			DrawFlavorTheme();
			break;
		}
		GUILayout.EndVertical();
	}

	private void DrawShopBundleVisibility()
	{
		GUILayout.BeginVertical("Box");
		DrawVisibilityProperty();
		DrawPositionProperty();
		DrawShopBundleProperty();
		GUILayout.EndVertical();
	}

	private void DrawShopBundlePercentage()
	{
		GUILayout.BeginVertical("Box");
		DrawPercentageProperty();
		DrawShopBundleProperty();
		GUILayout.EndVertical();
	}

	private void DrawHappyHourCategoryPercentage()
	{
		GUILayout.BeginVertical("Box");
		DrawPercentageProperty();
		DrawShopCategoriesProperty();
		GUILayout.EndVertical();
	}

	private void DrawHappyHourPercentage()
	{
		GUILayout.BeginVertical("Box");
		DrawPercentageProperty();
		DrawShopBundleProperty();
		GUILayout.EndVertical();
	}

	private void DrawShopCategoryVisibility()
	{
		GUILayout.BeginVertical("Box");
		DrawVisibilityProperty();
		DrawShopCategoriesProperty();
		GUILayout.EndVertical();
	}

	private void DrawShopCategoryPercentage()
	{
		GUILayout.BeginVertical("Box");
		DrawPercentageProperty();
		DrawShopCategoriesProperty();
		GUILayout.EndVertical();
	}

	private void DrawShopAdvertisements()
	{
		GUILayout.BeginVertical("Box");
		DrawAdvertisementTemplateProperty();
		GUILayout.EndVertical();
	}

	private void DrawEventProgressionChallenges()
	{
		GUILayout.BeginVertical("Box");
		GUILayout.Space(10f);
		DrawEventProgressionThemes();
		GUILayout.Space(10f);
		DrawEventProgressionHeader();
		GUILayout.Space(10f);
		DrawEventProgressionSubHeader();
		GUILayout.EndVertical();
	}

	private void DrawFlavorTheme()
	{
		GUILayout.BeginVertical("Box");
		DrawFlavorThemeProperty();
		GUILayout.EndVertical();
	}

	private void DrawVisibilityProperty()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Is Visible:");
		bool value = m_isVisible;
		Toggle(ref value, "Show In Shop");
		m_isVisible = value;
		GUILayout.EndHorizontal();
	}

	private void DrawPositionProperty()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Position", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Space(20f);
		if (GUILayout.Button("-1", GUILayout.Width(60f)))
		{
			m_positionOverride--;
		}
		GUILayout.Label(m_positionOverride.ToString(), m_lableStyle, GUILayout.Width(150f));
		if (GUILayout.Button("+1", GUILayout.Width(60f)))
		{
			m_positionOverride++;
		}
		GUILayout.Space(20f);
		if (GUILayout.Button("0", GUILayout.Width(80f)))
		{
			m_positionOverride = 0;
		}
		GUILayout.EndHorizontal();
	}

	private void DrawPercentageProperty()
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label("Multiplier", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Space(20f);
		if (GUILayout.Button("-0.1", GUILayout.Width(60f)))
		{
			m_discountFactor -= 0.1f;
		}
		GUILayout.Label(m_discountFactor.ToString(s_discountFactorFormat), m_lableStyle, GUILayout.Width(150f));
		if (GUILayout.Button("+0.1", GUILayout.Width(60f)))
		{
			m_discountFactor += 0.1f;
		}
		GUILayout.Space(20f);
		if (GUILayout.Button("1", GUILayout.Width(80f)))
		{
			m_discountFactor = 1f;
		}
		GUILayout.EndHorizontal();
	}

	private void DrawShopCategoriesProperty()
	{
		DrawDropDown("Shop Category", ref m_shopCategoryScrollPosition, ref m_selectedShopCategoryIndex, ref m_showShopCategorySelectionBox, Enum.GetNames(typeof(EShopCategory)));
	}

	private void DrawShopBundleProperty()
	{
		DrawDropDown("Shop Bundle", ref m_shopBundleScrollPosition, ref m_selectedShopBundleIndex, ref m_showShopBundleSelectionBox, ShopProductsName);
	}

	private void DrawAdvertisementTemplateProperty()
	{
		DrawDropDown("Banner Template", ref m_advertisementTemplateScrollPosition, ref m_selectedAdvertisementTemplateIndex, ref m_showAdvertisementTemplateSelectionBox, ShopbannerTemplateNames);
	}

	private void DrawEventProgressionThemes()
	{
		DrawDropDown("Themes", ref m_eventThemeScrollPosition, ref m_selectedEventThemeIndex, ref m_showEventThemeSelectionBox, EventThemesNames);
	}

	private void DrawFlavorThemeProperty()
	{
		DrawDropDown("Flavor Theme", ref m_flavorThemeScrollPosition, ref m_selectedFlavorThemeIndex, ref m_showFlavorThemeSelectionBox, Enum.GetNames(typeof(ThemesManager.EFlavorTheme)));
	}

	private void DrawEventProgressionHeader()
	{
		DrawDropDown("Header", ref m_eventHeaderScrollPosition, ref m_selectedEventHeaderIndex, ref m_showEventHeaderSelectionBox, EventHeaderNames);
	}

	private void DrawEventProgressionSubHeader()
	{
		DrawDropDown("Sub Header", ref m_eventSubHeaderScrollPosition, ref m_selectedEventSubHeaderIndex, ref m_showEventSubHeaderSelectionBox, EventSubHeaderNames);
	}

	private void DrawIncDecTimeField(string _label, ref DateTime _timer)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(_label, m_lableStyle, GUILayout.Width(150f));
		GUILayout.Space(20f);
		if (GUILayout.Button("- " + m_dateNames[m_selectedIncreasIndex], GUILayout.Width(80f)))
		{
			IncreaseCount(ref _timer, false);
		}
		GUILayout.Label(_timer.ToString(), m_lableStyle, GUILayout.Width(150f));
		if (GUILayout.Button("+ " + m_dateNames[m_selectedIncreasIndex], GUILayout.Width(80f)))
		{
			IncreaseCount(ref _timer, true);
		}
		GUILayout.Space(20f);
		if (GUILayout.Button("Now", GUILayout.Width(80f)))
		{
			_timer = new DateTime(Singleton<StateManager>.Instance.ServerTimeManager.UtcNow.Ticks);
		}
		GUILayout.EndHorizontal();
	}

	private void DrawDropDown(string _label, ref Vector2 _position, ref int _selectedIndex, ref bool _isSelected, string[] _nameList)
	{
		if (_nameList.Length != 0)
		{
			GUILayout.BeginHorizontal();
			_isSelected = GUILayout.Toggle(_isSelected, ((!_isSelected) ? "> " : "^ ") + _label + "(" + _nameList[_selectedIndex] + ")", "button", GUILayout.Width(600f));
			GUILayout.EndHorizontal();
			if (_isSelected)
			{
				DrawDropDownSelector(ref _position, ref _selectedIndex, ref _isSelected, _nameList);
			}
		}
	}

	private void DrawDropDownSelector(ref Vector2 _position, ref int _selectedIndex, ref bool _isSelected, string[] _nameList)
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(600f));
		_position = GUILayout.BeginScrollView(_position, GUILayout.MinHeight(150f), GUILayout.MaxHeight(150f));
		int num = _selectedIndex;
		_selectedIndex = GUILayout.SelectionGrid(_selectedIndex, _nameList, 1, "toggle");
		if (num != _selectedIndex)
		{
			_isSelected = false;
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private ThemesManager.EFlavorTheme GetSelectedFlavorTheme()
	{
		string[] names = Enum.GetNames(typeof(ThemesManager.EFlavorTheme));
		return (ThemesManager.EFlavorTheme)(int)Enum.Parse(typeof(ThemesManager.EFlavorTheme), names[m_selectedFlavorThemeIndex], true);
	}

	private EShopCategory GetSelectedShopCategory()
	{
		string[] names = Enum.GetNames(typeof(EShopCategory));
		return (EShopCategory)(int)Enum.Parse(typeof(EShopCategory), names[m_selectedShopCategoryIndex], true);
	}

	private string GetSelectedShopBundle()
	{
		ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
		return (m_selectedShopBundleIndex != 0) ? shopTable.bundles[m_selectedShopBundleIndex - 1].id : string.Empty;
	}

	private void CreateFakeEvent()
	{
		EEventType selectedEventTypeIndex = (EEventType)m_selectedEventTypeIndex;
		BBSimpleList<AcierProtocol.Event.Property> bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>();
		BBSimpleList<AcierProtocol.Event.LootProperty> bBSimpleList2 = new BBSimpleList<AcierProtocol.Event.LootProperty>();
		switch (selectedEventTypeIndex)
		{
		case EEventType.Event_Shop_Item_Visibility:
			bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[3]
			{
				new AcierProtocol.Event.Property
				{
					name = "shop.item.id",
					value = GetSelectedShopBundle()
				},
				new AcierProtocol.Event.Property
				{
					name = "visibility",
					value = m_isVisible.ToString()
				},
				new AcierProtocol.Event.Property
				{
					name = "position",
					value = m_positionOverride.ToString()
				}
			});
			break;
		case EEventType.Event_Shop_Category_Visibility:
			bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[2]
			{
				new AcierProtocol.Event.Property
				{
					name = "shop.category",
					value = GetSelectedShopCategory().ToString()
				},
				new AcierProtocol.Event.Property
				{
					name = "visibility",
					value = m_isVisible.ToString()
				}
			});
			break;
		case EEventType.Event_Shop_Item_Percentage:
			bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[2]
			{
				new AcierProtocol.Event.Property
				{
					name = "shop.item.id",
					value = GetSelectedShopBundle()
				},
				new AcierProtocol.Event.Property
				{
					name = "multiplier",
					value = m_discountFactor.ToString(s_discountFactorFormat)
				}
			});
			break;
		case EEventType.Event_Shop_Category_Percentage:
			bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[2]
			{
				new AcierProtocol.Event.Property
				{
					name = "shop.category",
					value = GetSelectedShopCategory().ToString()
				},
				new AcierProtocol.Event.Property
				{
					name = "multiplier",
					value = m_discountFactor.ToString(s_discountFactorFormat)
				}
			});
			break;
		case EEventType.Event_Shop_Hard_Currency_Payout_Increase:
			bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[2]
			{
				new AcierProtocol.Event.Property
				{
					name = "shop.item.id",
					value = GetSelectedShopBundle()
				},
				new AcierProtocol.Event.Property
				{
					name = "multiplier",
					value = m_discountFactor.ToString(s_discountFactorFormat)
				}
			});
			break;
		case EEventType.Event_Shop_Category_Payout_Increase:
			bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[2]
			{
				new AcierProtocol.Event.Property
				{
					name = "shop.category",
					value = GetSelectedShopCategory().ToString()
				},
				new AcierProtocol.Event.Property
				{
					name = "multiplier",
					value = m_discountFactor.ToString(s_discountFactorFormat)
				}
			});
			break;
		case EEventType.Event_Flavor:
			bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[1]
			{
				new AcierProtocol.Event.Property
				{
					name = "flavor",
					value = GetSelectedFlavorTheme().ToString()
				}
			});
			break;
		case EEventType.Event_Shop_Ad_Banner:
		{
			int num = m_shopbannerEventTemplateIndex[m_selectedAdvertisementTemplateIndex];
			EventTemplate eventTemplate = Singleton<StateManager>.Instance.TemplateManager.EventTemplates.templates[num];
			for (int i = 0; i < eventTemplate.propertyDefinitions.Length; i++)
			{
				EventTemplate.PropertyDefinition propertyDefinition = eventTemplate.propertyDefinitions[i];
				bBSimpleList.Add(new AcierProtocol.Event.Property
				{
					name = propertyDefinition.name,
					value = propertyDefinition.defaultValue
				});
			}
			break;
		}
		case EEventType.Event_Progression_Challenges:
			bBSimpleList = CreateFakeEventProgressionChallengeProperties();
			bBSimpleList2 = CreateFakeEventProgressionChallengelootProperties();
			break;
		}
		bBSimpleList.TrimExcess();
		Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
		AcierProtocol.Event @event = new AcierProtocol.Event();
		@event.startTimeUTC = (long)(m_startTime - m_unixTimeReference).TotalMilliseconds;
		@event.endTimeUTC = (long)(m_endTime - m_unixTimeReference).TotalMilliseconds;
		@event.type = selectedEventTypeIndex;
		@event.name = s_fakeEventName;
		@event.properties = bBSimpleList.Data;
		@event.lootProperties = bBSimpleList2.Data;
		AcierProtocol.Event item = @event;
		events.events = events.events.Add(item);
		Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
	}

	private void DiscountAllCategories()
	{
		Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
		BBSimpleList<AcierProtocol.Event.Property> bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[2]
		{
			new AcierProtocol.Event.Property
			{
				name = "shop.category",
				value = EShopCategory.Shopcat_HcPackage.ToString()
			},
			new AcierProtocol.Event.Property
			{
				name = "multiplier",
				value = 0.5f.ToString(s_discountFactorFormat)
			}
		});
		AcierProtocol.Event @event = new AcierProtocol.Event();
		@event.startTimeUTC = (long)(m_startTime - m_unixTimeReference).TotalMilliseconds;
		@event.endTimeUTC = (long)(m_endTime - m_unixTimeReference).TotalMilliseconds;
		@event.type = EEventType.Event_Shop_Category_Payout_Increase;
		@event.name = s_fakeEventName;
		@event.properties = bBSimpleList.Data;
		AcierProtocol.Event item = @event;
		events.events = events.events.Add(item);
		BBSimpleList<AcierProtocol.Event.Property> bBSimpleList2 = new BBSimpleList<AcierProtocol.Event.Property>();
		Array values = Enum.GetValues(typeof(EShopCategory));
		ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
		for (int i = 0; i < values.Length; i++)
		{
			bBSimpleList2.Clear();
			EShopCategory eShopCategory = (EShopCategory)(int)values.GetValue(i);
			bBSimpleList2 = new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[2]
			{
				new AcierProtocol.Event.Property
				{
					name = "shop.category",
					value = eShopCategory.ToString()
				},
				new AcierProtocol.Event.Property
				{
					name = "multiplier",
					value = 0.5f.ToString(s_discountFactorFormat)
				}
			});
			bBSimpleList2.TrimExcess();
			@event = new AcierProtocol.Event();
			@event.startTimeUTC = (long)(m_startTime - m_unixTimeReference).TotalMilliseconds;
			@event.endTimeUTC = (long)(m_endTime - m_unixTimeReference).TotalMilliseconds;
			@event.type = EEventType.Event_Shop_Category_Percentage;
			@event.name = s_fakeEventName;
			@event.properties = bBSimpleList2.Data;
			AcierProtocol.Event item2 = @event;
			events.events = events.events.Add(item2);
		}
		Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
	}

	private void ShowAllItems()
	{
		BBSimpleList<AcierProtocol.Event.Property> bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>();
		ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
		for (int i = 0; i < shopTable.bundles.Length; i++)
		{
			bBSimpleList.Clear();
			ShopBundle shopBundle = shopTable.bundles[i];
			bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[2]
			{
				new AcierProtocol.Event.Property
				{
					name = "shop.item.id",
					value = shopBundle.name
				},
				new AcierProtocol.Event.Property
				{
					name = "visibility",
					value = true.ToString()
				}
			});
			bBSimpleList.TrimExcess();
			Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
			AcierProtocol.Event @event = new AcierProtocol.Event();
			@event.startTimeUTC = (long)(m_startTime - m_unixTimeReference).TotalMilliseconds;
			@event.endTimeUTC = (long)(m_endTime - m_unixTimeReference).TotalMilliseconds;
			@event.type = EEventType.Event_Shop_Item_Visibility;
			@event.name = s_fakeEventName;
			AcierProtocol.Event event2 = @event;
			event2.properties = bBSimpleList.Data;
			events.events = events.events.Add(event2);
		}
		Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
	}

	private void CreateAllBanners()
	{
		BBSimpleList<AcierProtocol.Event.Property> bBSimpleList = new BBSimpleList<AcierProtocol.Event.Property>();
		EventTemplate[] templates = Singleton<StateManager>.Instance.TemplateManager.EventTemplates.templates;
		foreach (EventTemplate eventTemplate in templates)
		{
			if (eventTemplate.type == EEventType.Event_Shop_Ad_Banner && (eventTemplate.name.StartsWith("1x1") || eventTemplate.name.StartsWith("1x2")))
			{
				bBSimpleList.Clear();
				for (int j = 0; j < eventTemplate.propertyDefinitions.Length; j++)
				{
					EventTemplate.PropertyDefinition propertyDefinition = eventTemplate.propertyDefinitions[j];
					bBSimpleList.Add(new AcierProtocol.Event.Property
					{
						name = propertyDefinition.name,
						value = propertyDefinition.defaultValue
					});
				}
				bBSimpleList.TrimExcess();
				Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
				AcierProtocol.Event @event = new AcierProtocol.Event();
				@event.startTimeUTC = (long)(m_startTime - m_unixTimeReference).TotalMilliseconds;
				@event.endTimeUTC = (long)(m_endTime - m_unixTimeReference).TotalMilliseconds;
				@event.type = EEventType.Event_Shop_Ad_Banner;
				@event.name = s_fakeEventName;
				AcierProtocol.Event event2 = @event;
				event2.properties = bBSimpleList.Data;
				events.events = events.events.Add(event2);
			}
		}
		Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
	}

	private BBSimpleList<AcierProtocol.Event.Property> CreateFakeEventProgressionChallengeProperties()
	{
		EventsUserFriendlyMapping eventsUserFriendlyMapping = Singleton<StateManager>.Instance.TemplateManager.EventsUserFriendlyMapping;
		int locaKey = eventsUserFriendlyMapping.displayMessageMapping[m_sortedEventHeaderIndex[m_selectedEventHeaderIndex]].locaKey;
		int locaKey2 = eventsUserFriendlyMapping.displaySubMessageMapping[m_sortedEventSubHeaderIndex[m_selectedEventSubHeaderIndex]].locaKey;
		EventsUserFriendlyMapping.ClientThemeAssetMappingEntry clientThemeAssetMappingEntry = eventsUserFriendlyMapping.clientThemeAssetMapping[m_sortedEventThemesIndex[m_selectedEventThemeIndex]];
		return new BBSimpleList<AcierProtocol.Event.Property>(new AcierProtocol.Event.Property[18]
		{
			new AcierProtocol.Event.Property
			{
				name = "screenImageGuiId",
				value = clientThemeAssetMappingEntry.screenImageGuiId
			},
			new AcierProtocol.Event.Property
			{
				name = "notificationImageGuId",
				value = clientThemeAssetMappingEntry.notificationImageGuId
			},
			new AcierProtocol.Event.Property
			{
				name = "bannerImageGuiId",
				value = clientThemeAssetMappingEntry.bannerImageGuiId
			},
			new AcierProtocol.Event.Property
			{
				name = "bannerBackgroundLargeGuId",
				value = clientThemeAssetMappingEntry.bannerBackgroundLargeGuId
			},
			new AcierProtocol.Event.Property
			{
				name = "bannerBackgroundSmallGuId",
				value = clientThemeAssetMappingEntry.bannerBackgroundSmallGuId
			},
			new AcierProtocol.Event.Property
			{
				name = "bannerFontColor1GuId",
				value = clientThemeAssetMappingEntry.bannerFontColor1GuId
			},
			new AcierProtocol.Event.Property
			{
				name = "bannerFontColor2GuId",
				value = clientThemeAssetMappingEntry.bannerFontColor2GuId
			},
			new AcierProtocol.Event.Property
			{
				name = "bannerFontColor3GuId",
				value = clientThemeAssetMappingEntry.bannerFontColor3GuId
			},
			new AcierProtocol.Event.Property
			{
				name = "bannerFontColor4GuId",
				value = clientThemeAssetMappingEntry.bannerFontColor4GuId
			},
			new AcierProtocol.Event.Property
			{
				name = "bannerTintingColorGuId",
				value = clientThemeAssetMappingEntry.bannerTintingColorGuId
			},
			new AcierProtocol.Event.Property
			{
				name = "h",
				value = locaKey.ToString()
			},
			new AcierProtocol.Event.Property
			{
				name = "sh",
				value = locaKey2.ToString()
			},
			new AcierProtocol.Event.Property
			{
				name = "reward.m.1",
				value = "100"
			},
			new AcierProtocol.Event.Property
			{
				name = "reward.mu.1",
				value = "1"
			},
			new AcierProtocol.Event.Property
			{
				name = "reward.m.2",
				value = "200"
			},
			new AcierProtocol.Event.Property
			{
				name = "reward.mu.2",
				value = "2"
			},
			new AcierProtocol.Event.Property
			{
				name = "reward.m.3",
				value = "500"
			},
			new AcierProtocol.Event.Property
			{
				name = "reward.mu.3",
				value = "1"
			}
		});
	}

	private BBSimpleList<AcierProtocol.Event.LootProperty> CreateFakeEventProgressionChallengelootProperties()
	{
		BBSimpleList<AcierProtocol.Event.LootProperty> bBSimpleList = new BBSimpleList<AcierProtocol.Event.LootProperty>();
		ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
		LootTemplate lootTemplate = null;
		LootTemplate lootTemplate2 = null;
		LootTemplate value = null;
		for (int i = 0; i < shopTable.bundles.Length; i++)
		{
			ShopBundle shopBundle = shopTable.bundles[i];
			if (shopBundle.payout.lootTemplates.Length == 0)
			{
				continue;
			}
			if (shopBundle.category == EShopCategory.Shopcat_Items)
			{
				if (lootTemplate == null)
				{
					lootTemplate = shopBundle.payout.lootTemplates[0];
				}
				else if (lootTemplate2 == null)
				{
					lootTemplate2 = shopBundle.payout.lootTemplates[0];
				}
			}
			else if (shopBundle.payout.lootTemplates[0].resourceTemplate != null && shopBundle.payout.lootTemplates[0].resourceTemplate.maxResource.amount < 1000000)
			{
				value = shopBundle.payout.lootTemplates[0];
			}
		}
		bBSimpleList.Add(new AcierProtocol.Event.LootProperty
		{
			value = lootTemplate,
			name = "reward.1"
		});
		bBSimpleList.Add(new AcierProtocol.Event.LootProperty
		{
			value = value,
			name = "reward.2"
		});
		bBSimpleList.Add(new AcierProtocol.Event.LootProperty
		{
			value = lootTemplate2,
			name = "reward.3"
		});
		return bBSimpleList;
	}

	private void DrawEventInfoTable()
	{
		GUILayout.BeginVertical();
		GUILayout.Label("Created Events", m_lableStyle, GUILayout.Width(150f));
		EventManagerBase eventManger = Singleton<StateManager>.Instance.EventManager.GetEventManger((EEventType)m_selectedEventTypeIndex);
		switch ((EEventType)m_selectedEventTypeIndex)
		{
		case EEventType.Event_Shop_Item_Visibility:
			DrawShopBundleVisibilityEventsInfo((EventShopBundleVisibilityManager)eventManger);
			break;
		case EEventType.Event_Shop_Category_Visibility:
			DrawShopCategoryVisibilityEventsInfo((EventCategoryVisibilityManager)eventManger);
			break;
		case EEventType.Event_Shop_Item_Percentage:
			DrawShopBundlePercentageEventsInfo((EventShopBundlePercentageManager)eventManger);
			break;
		case EEventType.Event_Shop_Category_Percentage:
			DrawShopCategoryPercentageEventsInfo((EventCategoryPercentageManager)eventManger);
			break;
		case EEventType.Event_Shop_Ad_Banner:
			DrawAdvertisementsEventsInfo((EventShopAdvertisementsManager)eventManger);
			break;
		case EEventType.Event_Shop_Hard_Currency_Payout_Increase:
			DrawShopBundleHappyHourEventsInfo((EventShopBundleHappyHourManager)eventManger);
			break;
		case EEventType.Event_Shop_Category_Payout_Increase:
			DrawShopCategoryHappyHourInfo((EventShopCategoryHappyHourManager)eventManger);
			break;
		case EEventType.Event_Progression_Challenges:
			DrawEventProgressionChallengessInfo((EventProgressionChallengesManager)eventManger);
			break;
		case EEventType.Event_Flavor:
			DrawFlavorThemeEventsInfo(eventManger);
			break;
		}
		GUILayout.EndVertical();
	}

	private void DrawShopBundleVisibilityEventsInfo(EventShopBundleVisibilityManager _eventMgr)
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(450f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("Valid", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("StartTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("EndTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("Bundle Name", m_lableStyle, GUILayout.Width(200f));
		GUILayout.Label("IsVisible", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Space(10f);
		GUILayout.Label("Del", m_lableStyle, GUILayout.Width(100f));
		GUILayout.EndHorizontal();
		m_shopBundleVisibilityPosition = GUILayout.BeginScrollView(m_shopBundleVisibilityPosition, GUILayout.MinHeight(150f), GUILayout.MaxHeight(150f));
		if (_eventMgr.Events.Count == 0)
		{
			GUILayout.Label("No Events for this Type was found", m_lableStyle, GUILayout.Width(300f));
		}
		else
		{
			AcierProtocol.Event @event = null;
			foreach (EventShopBundleVisibility event2 in _eventMgr.Events)
			{
				GUILayout.BeginHorizontal();
				DateTime dateTime = ServerTimeManager.ToUtcTime(event2.Event.startTimeUTC);
				DateTime dateTime2 = ServerTimeManager.ToUtcTime(event2.Event.endTimeUTC);
				GUILayout.Label(event2.Event.name, m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(event2.LastValidState.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime2.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				ShopTable shopTable = Singleton<StateManager>.Instance.TemplateManager.ShopTable;
				GUILayout.Label(shopTable.GetShopBundleById(event2.ShopBundleId).name, m_lableStyle, GUILayout.Width(200f), GUILayout.ExpandWidth(false));
				GUILayout.Label(event2.IsVisible.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(event2.Position.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Space(10f);
				if (GUILayout.Button("Delete Event", GUILayout.Width(100f), GUILayout.ExpandWidth(false)))
				{
					@event = event2.Event;
				}
				GUILayout.EndHorizontal();
			}
			if (@event != null)
			{
				Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
				events.events = events.events.Remove(@event);
				Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawShopCategoryVisibilityEventsInfo(EventCategoryVisibilityManager _eventMgr)
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(450f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("Valid", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("StartTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("EndTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("Category", m_lableStyle, GUILayout.Width(200f));
		GUILayout.Label("IsVisible", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Space(10f);
		GUILayout.Label("Del", m_lableStyle, GUILayout.Width(100f));
		GUILayout.EndHorizontal();
		m_shopBundleVisibilityPosition = GUILayout.BeginScrollView(m_shopBundleVisibilityPosition, GUILayout.MinHeight(150f), GUILayout.MaxHeight(150f));
		if (_eventMgr.Events.Count == 0)
		{
			GUILayout.Label("No Events for this Type was found", m_lableStyle, GUILayout.Width(300f));
		}
		else
		{
			AcierProtocol.Event @event = null;
			foreach (EventCategoryVisibility event2 in _eventMgr.Events)
			{
				GUILayout.BeginHorizontal();
				DateTime dateTime = ServerTimeManager.ToUtcTime(event2.Event.startTimeUTC);
				DateTime dateTime2 = ServerTimeManager.ToUtcTime(event2.Event.endTimeUTC);
				GUILayout.Label(event2.Event.name, m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(event2.LastValidState.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime2.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(event2.ShopCategory.ToString(), m_lableStyle, GUILayout.Width(200f), GUILayout.ExpandWidth(false));
				GUILayout.Label(event2.IsVisible.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Space(10f);
				if (GUILayout.Button("Delete Event", GUILayout.Width(100f), GUILayout.ExpandWidth(false)))
				{
					@event = event2.Event;
				}
				GUILayout.EndHorizontal();
			}
			if (@event != null)
			{
				Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
				events.events = events.events.Remove(@event);
				Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawShopBundlePercentageEventsInfo(EventShopBundlePercentageManager _eventMgr)
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(450f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("Valid", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("StartTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("EndTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("Bundle Name", m_lableStyle, GUILayout.Width(200f));
		GUILayout.Label("Percentage", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Space(10f);
		GUILayout.Label("Del", m_lableStyle, GUILayout.Width(100f));
		GUILayout.EndHorizontal();
		m_shopBundleVisibilityPosition = GUILayout.BeginScrollView(m_shopBundleVisibilityPosition, GUILayout.MinHeight(150f), GUILayout.MaxHeight(150f));
		if (_eventMgr.Events.Count == 0)
		{
			GUILayout.Label("No Events for this Type was found", m_lableStyle, GUILayout.Width(300f));
		}
		else
		{
			AcierProtocol.Event @event = null;
			for (int i = 0; i < _eventMgr.Events.Count; i++)
			{
				GUILayout.BeginHorizontal();
				EventShopBundlePercentage eventShopBundlePercentage = (EventShopBundlePercentage)_eventMgr.Events[i];
				DateTime dateTime = ServerTimeManager.ToUtcTime(eventShopBundlePercentage.Event.startTimeUTC);
				DateTime dateTime2 = ServerTimeManager.ToUtcTime(eventShopBundlePercentage.Event.endTimeUTC);
				GUILayout.Label(eventShopBundlePercentage.Event.name, m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventShopBundlePercentage.LastValidState.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime2.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventShopBundlePercentage.ShopBundleId, m_lableStyle, GUILayout.Width(200f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventShopBundlePercentage.Multiplier.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Space(10f);
				if (GUILayout.Button("Delete Event", GUILayout.Width(100f), GUILayout.ExpandWidth(false)))
				{
					@event = eventShopBundlePercentage.Event;
				}
				GUILayout.EndHorizontal();
			}
			if (@event != null)
			{
				Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
				events.events = events.events.Remove(@event);
				Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawShopBundleHappyHourEventsInfo(EventShopBundleHappyHourManager _eventMgr)
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(450f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("Valid", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("StartTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("EndTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("Bundle Name", m_lableStyle, GUILayout.Width(200f));
		GUILayout.Label("Percentage", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Space(10f);
		GUILayout.Label("Del", m_lableStyle, GUILayout.Width(100f));
		GUILayout.EndHorizontal();
		m_shopBundleVisibilityPosition = GUILayout.BeginScrollView(m_shopBundleVisibilityPosition, GUILayout.MinHeight(150f), GUILayout.MaxHeight(150f));
		if (_eventMgr.Events.Count == 0)
		{
			GUILayout.Label("No Events for this Type was found", m_lableStyle, GUILayout.Width(300f));
		}
		else
		{
			AcierProtocol.Event @event = null;
			for (int i = 0; i < _eventMgr.Events.Count; i++)
			{
				GUILayout.BeginHorizontal();
				GUILayout.EndHorizontal();
			}
			if (@event != null)
			{
				Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
				events.events = events.events.Remove(@event);
				Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawShopCategoryHappyHourInfo(EventShopCategoryHappyHourManager _eventMgr)
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(450f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("Valid", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("StartTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("EndTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("Category", m_lableStyle, GUILayout.Width(200f));
		GUILayout.Label("IsVisible", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Space(10f);
		GUILayout.Label("Del", m_lableStyle, GUILayout.Width(100f));
		GUILayout.EndHorizontal();
		m_shopBundleVisibilityPosition = GUILayout.BeginScrollView(m_shopBundleVisibilityPosition, GUILayout.MinHeight(150f), GUILayout.MaxHeight(150f));
		if (_eventMgr.Events.Count == 0)
		{
			GUILayout.Label("No Events for this Type was found", m_lableStyle, GUILayout.Width(300f));
		}
		else
		{
			AcierProtocol.Event @event = null;
			for (int i = 0; i < _eventMgr.Events.Count; i++)
			{
				GUILayout.BeginHorizontal();
				EventShopCategoryHappyHour eventShopCategoryHappyHour = (EventShopCategoryHappyHour)_eventMgr.Events[i];
				DateTime dateTime = ServerTimeManager.ToUtcTime(eventShopCategoryHappyHour.Event.startTimeUTC);
				DateTime dateTime2 = ServerTimeManager.ToUtcTime(eventShopCategoryHappyHour.Event.endTimeUTC);
				GUILayout.Label(eventShopCategoryHappyHour.Event.name, m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventShopCategoryHappyHour.LastValidState.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime2.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventShopCategoryHappyHour.ShopCategory.ToString(), m_lableStyle, GUILayout.Width(200f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventShopCategoryHappyHour.Multiplier.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Space(10f);
				if (GUILayout.Button("Delete Event", GUILayout.Width(100f), GUILayout.ExpandWidth(false)))
				{
					@event = eventShopCategoryHappyHour.Event;
				}
				GUILayout.EndHorizontal();
			}
			if (@event != null)
			{
				Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
				events.events = events.events.Remove(@event);
				Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawShopCategoryPercentageEventsInfo(EventCategoryPercentageManager _eventMgr)
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(450f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("Valid", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("StartTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("EndTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("Category", m_lableStyle, GUILayout.Width(200f));
		GUILayout.Label("IsVisible", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Space(10f);
		GUILayout.Label("Del", m_lableStyle, GUILayout.Width(100f));
		GUILayout.EndHorizontal();
		m_shopBundleVisibilityPosition = GUILayout.BeginScrollView(m_shopBundleVisibilityPosition, GUILayout.MinHeight(150f), GUILayout.MaxHeight(150f));
		if (_eventMgr.Events.Count == 0)
		{
			GUILayout.Label("No Events for this Type was found", m_lableStyle, GUILayout.Width(300f));
		}
		else
		{
			AcierProtocol.Event @event = null;
			for (int i = 0; i < _eventMgr.Events.Count; i++)
			{
				GUILayout.BeginHorizontal();
				EventCategoryPercentage eventCategoryPercentage = (EventCategoryPercentage)_eventMgr.Events[i];
				DateTime dateTime = ServerTimeManager.ToUtcTime(eventCategoryPercentage.Event.startTimeUTC);
				DateTime dateTime2 = ServerTimeManager.ToUtcTime(eventCategoryPercentage.Event.endTimeUTC);
				GUILayout.Label(eventCategoryPercentage.Event.name, m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventCategoryPercentage.LastValidState.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime2.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventCategoryPercentage.ShopCategory.ToString(), m_lableStyle, GUILayout.Width(200f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventCategoryPercentage.Multiplier.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Space(10f);
				if (GUILayout.Button("Delete Event", GUILayout.Width(100f), GUILayout.ExpandWidth(false)))
				{
					@event = eventCategoryPercentage.Event;
				}
				GUILayout.EndHorizontal();
			}
			if (@event != null)
			{
				Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
				events.events = events.events.Remove(@event);
				Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawAdvertisementsEventsInfo(EventShopAdvertisementsManager _eventMgr)
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(450f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("Valid", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("StartTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("EndTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("ShopId", m_lableStyle, GUILayout.Width(140f));
		GUILayout.Label("Category", m_lableStyle, GUILayout.Width(140f));
		GUILayout.Space(10f);
		GUILayout.Label("Del", m_lableStyle, GUILayout.Width(100f));
		GUILayout.EndHorizontal();
		m_shopBundleVisibilityPosition = GUILayout.BeginScrollView(m_shopBundleVisibilityPosition, GUILayout.MinHeight(150f), GUILayout.MaxHeight(150f));
		if (_eventMgr.Events.Count == 0)
		{
			GUILayout.Label("No Events for this Type was found", m_lableStyle, GUILayout.Width(300f));
		}
		else
		{
			AcierProtocol.Event @event = null;
			for (int i = 0; i < _eventMgr.Events.Count; i++)
			{
				GUILayout.BeginHorizontal();
				EventShopAdvertisement eventShopAdvertisement = (EventShopAdvertisement)_eventMgr.Events[i];
				DateTime dateTime = ServerTimeManager.ToUtcTime(eventShopAdvertisement.Event.startTimeUTC);
				DateTime dateTime2 = ServerTimeManager.ToUtcTime(eventShopAdvertisement.Event.endTimeUTC);
				GUILayout.Label(eventShopAdvertisement.Event.name, m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventShopAdvertisement.LastValidState.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime2.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				string value = eventShopAdvertisement.Template.ShopProductId;
				if (!ShopProductsIDToName.TryGetValue(eventShopAdvertisement.Template.ShopProductId, out value))
				{
					value = "No Valid";
				}
				GUILayout.Label(value, m_lableStyle, GUILayout.Width(140f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventShopAdvertisement.Template.ShopCategory.ToString(), m_lableStyle, GUILayout.Width(140f), GUILayout.ExpandWidth(false));
				GUILayout.Space(10f);
				if (GUILayout.Button("Delete Event", GUILayout.Width(100f), GUILayout.ExpandWidth(false)))
				{
					@event = eventShopAdvertisement.Event;
				}
				GUILayout.EndHorizontal();
			}
			if (@event != null)
			{
				Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
				events.events = events.events.Remove(@event);
				Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawEventProgressionChallengessInfo(EventProgressionChallengesManager _eventMgr)
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(450f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("Valid", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("StartTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("EndTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Space(10f);
		GUILayout.Label("Del", m_lableStyle, GUILayout.Width(100f));
		GUILayout.EndHorizontal();
		m_shopBundleVisibilityPosition = GUILayout.BeginScrollView(m_shopBundleVisibilityPosition, GUILayout.MinHeight(150f), GUILayout.MaxHeight(150f));
		if (_eventMgr.Events.Count == 0)
		{
			GUILayout.Label("No Events for this Type was found", m_lableStyle, GUILayout.Width(300f));
		}
		else
		{
			AcierProtocol.Event @event = null;
			for (int i = 0; i < _eventMgr.Events.Count; i++)
			{
				GUILayout.BeginHorizontal();
				EventProgressionChallenges eventProgressionChallenges = (EventProgressionChallenges)_eventMgr.Events[i];
				DateTime dateTime = ServerTimeManager.ToUtcTime(eventProgressionChallenges.Event.startTimeUTC);
				DateTime dateTime2 = ServerTimeManager.ToUtcTime(eventProgressionChallenges.Event.endTimeUTC);
				GUILayout.Label(eventProgressionChallenges.Event.name, m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventProgressionChallenges.LastValidState.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime2.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Space(10f);
				if (GUILayout.Button("Delete Event", GUILayout.Width(100f), GUILayout.ExpandWidth(false)))
				{
					@event = eventProgressionChallenges.Event;
				}
				GUILayout.EndHorizontal();
			}
			if (@event != null)
			{
				Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
				events.events = events.events.Remove(@event);
				Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}

	private void DrawFlavorThemeEventsInfo(EventManagerBase _eventMgr)
	{
		GUILayout.BeginVertical("Box", GUILayout.MinWidth(450f));
		GUILayout.BeginHorizontal();
		GUILayout.Label("Name", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("Valid", m_lableStyle, GUILayout.Width(80f));
		GUILayout.Label("StartTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("EndTime", m_lableStyle, GUILayout.Width(150f));
		GUILayout.Label("Flavor Theme", m_lableStyle, GUILayout.Width(200f));
		GUILayout.Space(10f);
		GUILayout.Label("Del", m_lableStyle, GUILayout.Width(100f));
		GUILayout.EndHorizontal();
		m_shopBundleVisibilityPosition = GUILayout.BeginScrollView(m_shopBundleVisibilityPosition, GUILayout.MinHeight(150f), GUILayout.MaxHeight(150f));
		if (_eventMgr.Events.Count == 0)
		{
			GUILayout.Label("No Events for this Type was found", m_lableStyle, GUILayout.Width(300f));
		}
		else
		{
			AcierProtocol.Event @event = null;
			foreach (EventBase event2 in _eventMgr.Events)
			{
				EventFlavorTheme eventFlavorTheme = (EventFlavorTheme)event2;
				GUILayout.BeginHorizontal();
				DateTime dateTime = ServerTimeManager.ToUtcTime(eventFlavorTheme.Event.startTimeUTC);
				DateTime dateTime2 = ServerTimeManager.ToUtcTime(eventFlavorTheme.Event.endTimeUTC);
				GUILayout.Label(eventFlavorTheme.Event.name, m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventFlavorTheme.LastValidState.ToString(), m_lableStyle, GUILayout.Width(80f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(dateTime2.ToString(), m_lableStyle, GUILayout.Width(150f), GUILayout.ExpandWidth(false));
				GUILayout.Label(eventFlavorTheme.FlavorThemeType.ToString(), m_lableStyle, GUILayout.Width(200f), GUILayout.ExpandWidth(false));
				GUILayout.Space(10f);
				if (GUILayout.Button("Delete Event", GUILayout.Width(100f), GUILayout.ExpandWidth(false)))
				{
					@event = eventFlavorTheme.Event;
				}
				GUILayout.EndHorizontal();
			}
			if (@event != null)
			{
				Events events = Singleton<StateManager>.Instance.TemplateManager.Events;
				events.events = events.events.Remove(@event);
				Singleton<StateManager>.Instance.EventManager.OnUpdateEvents();
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndVertical();
	}
}
