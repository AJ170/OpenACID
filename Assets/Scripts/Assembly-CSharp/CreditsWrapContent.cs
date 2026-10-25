using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Engine;
using Assets.Scripts.UI.Interaction;
using Assets.Scripts.UI.Screens.CreditsScreen;
using Assets.Scripts.UI.Utils;
using SharedBaseLib.Core;
using SharedBaseLib.Serialization;
using UnityEngine;

public class CreditsWrapContent : MonoBehaviour, IExtendedWrapContent
{
	private class ItemComparerPosition : IComparer<ICreditsItem>
	{
		public int Compare(ICreditsItem _x, ICreditsItem _y)
		{
			return (_x.Position + _x.Size).CompareTo(_y.Position + _y.Size);
		}
	}

	public const string CreditsBIN = "UI/Misc/credits";

	[SerializeField]
	private GameObject PrefabSection;

	[SerializeField]
	private GameObject PrefabDepartment;

	[SerializeField]
	private GameObject PrefabJob;

	[SerializeField]
	private GameObject PrefabPerson;

	[SerializeField]
	private GameObject PrefabText;

	private BBList<ICreditsItem> m_items;

	private GameObject[] m_prefabs = new GameObject[5];

	private BBList<CreditsTab>[] m_children = new BBList<CreditsTab>[5];

	private CreditsDefinition[] m_definitions = new CreditsDefinition[5];

	private float m_allItemHeight;

	private Vector2 m_totalDimension = Vector2.zero;

	private float m_panelHeight;

	private float m_panelWidth;

	private float m_fontScale;

	private Transform m_cachedTransform;

	private UIPanel m_panel;

	private UIWrapScrollView m_wrapScrollView;

	private UIFocusChildInScrollView m_focusChildInScrollView;

	public BBList<EventDelegate> OnFocusFinishedEvent = new BBList<EventDelegate>();

	private readonly CreditsItem m_creditsCompareItem = new CreditsItem(ECreditsItemType.Invalid);

	private readonly ItemComparerPosition m_itemComparerBottomPosition = new ItemComparerPosition();

	public UIWrapScrollView WrapScrollView
	{
		get
		{
			return m_wrapScrollView;
		}
	}

	public float FontScale
	{
		get
		{
			return m_fontScale;
		}
	}

	public Bounds Bounds
	{
		get
		{
			Vector3 center = m_totalDimension * 0.5f;
			center.y = 0f - center.y;
			return new Bounds(center, m_totalDimension);
		}
	}

	public float ExitScrollPosition
	{
		get
		{
			return m_totalDimension.y - m_panelHeight;
		}
	}

	public float PanelHeight
	{
		get
		{
			return m_panelHeight;
		}
	}

	public UIFocusChildInScrollView FocusChildInScrollView
	{
		get
		{
			if (m_focusChildInScrollView == null)
			{
				m_focusChildInScrollView = GetComponent<UIFocusChildInScrollView>();
				EventDelegate.Add(m_focusChildInScrollView.OnFinishedEvent, OnFocusFinished);
			}
			return m_focusChildInScrollView;
		}
	}

	public void Init()
	{
		float num = 1536f * (float)Screen.width / (float)Screen.height;
		m_fontScale = (num - 1224f) / 824f;
		m_fontScale = (m_fontScale - 1f) * 0.9f + 1f;
		m_prefabs[0] = PrefabSection;
		m_prefabs[1] = PrefabDepartment;
		m_prefabs[2] = PrefabJob;
		m_prefabs[3] = PrefabPerson;
		m_prefabs[4] = PrefabText;
		Load();
		ValidateChildren();
		WrapContent();
		if (m_wrapScrollView != null)
		{
			m_wrapScrollView.GetComponent<UIPanel>().onClipMove = OnMove;
			m_wrapScrollView.restrictWithinPanel = true;
			m_wrapScrollView.disableDragIfFits = true;
			m_wrapScrollView.dragEffect = UIScrollView.DragEffect.MomentumAndSpring;
			m_wrapScrollView.ExtendedWraperContent = this;
			m_wrapScrollView.UpdateScrollbars(true);
		}
	}

	private void Load()
	{
		TextAsset textAsset = Resources.Load<TextAsset>("UI/Misc/credits");
		using (Stream stream = new MemoryStream(textAsset.bytes))
		{
			using (DataBinarySerializer dataBinarySerializer = new DataBinarySerializer(stream, EMode.EMode_Read))
			{
				dataBinarySerializer.SerializeBeginNode("credits");
				dataBinarySerializer.SerializeBeginNode("definitions");
				int num = dataBinarySerializer.SerializeItemCount();
				for (int i = 0; i < num; i++)
				{
					CreditsDefinition creditsDefinition = new CreditsDefinition();
					creditsDefinition.Serialize("definition", dataBinarySerializer);
					m_definitions[(int)creditsDefinition.Type] = creditsDefinition;
				}
				dataBinarySerializer.SerializeEndNode();
				dataBinarySerializer.SerializeBeginNode("items");
				m_allItemHeight = 0f;
				int num2 = dataBinarySerializer.SerializeItemCount();
				m_items = new BBList<ICreditsItem>(num2);
				for (int j = 0; j < num2; j++)
				{
					CreditsItem creditsItem = CreditsItem.Serialize(dataBinarySerializer, null);
					creditsItem.Position *= m_fontScale;
					creditsItem.Size *= m_fontScale;
					m_items.Add(creditsItem);
					m_allItemHeight = Mathf.Max(m_allItemHeight, creditsItem.Position + creditsItem.Size);
				}
				if (m_items.Count > 0)
				{
					m_allItemHeight -= m_items[0].Position;
				}
				dataBinarySerializer.SerializeEndNode();
				dataBinarySerializer.SerializeEndNode();
			}
		}
	}

	public void Exit()
	{
		m_items = null;
		for (int i = 0; i < 5; i++)
		{
			BBList<CreditsTab> bBList = m_children[i];
			for (int j = 0; j < bBList.Count; j++)
			{
				NGUITools.Destroy(bBList[j]);
			}
			bBList.Clear();
		}
	}

	protected virtual void OnMove(UIPanel _panel)
	{
		if (m_items != null)
		{
			WrapContent();
		}
	}

	private void OnDestroy()
	{
		if (m_focusChildInScrollView != null)
		{
			EventDelegate.Remove(m_focusChildInScrollView.OnFinishedEvent, OnFocusFinished);
		}
		for (int i = 0; i < m_children.Length; i++)
		{
			m_children[i] = null;
		}
	}

	private void OnFocusFinished()
	{
		EventDelegate.Execute(OnFocusFinishedEvent);
	}

	private bool CacheScrollView()
	{
		if (m_panel != null)
		{
			return true;
		}
		m_cachedTransform = base.transform;
		m_panel = NGUITools.FindInParents<UIPanel>(base.gameObject);
		m_wrapScrollView = m_panel.GetComponent<UIWrapScrollView>();
		if (m_wrapScrollView == null)
		{
			return false;
		}
		return true;
	}

	private void WrapContent()
	{
		float y = m_panel.transform.localPosition.y;
		float num = y + m_panelHeight;
		for (int i = 0; i < 5; i++)
		{
			BBList<CreditsTab> bBList = m_children[i];
			for (int j = 0; j < bBList.Count; j++)
			{
				CreditsTab creditsTab = bBList[j];
				bool visible = creditsTab.Visible;
				ICreditsItem item = creditsTab.Item;
				if (item == null)
				{
					creditsTab.Distance = float.MaxValue;
				}
				else
				{
					creditsTab.Distance = Mathf.Max(y - item.Position - item.Size, item.Position - num);
				}
				if (visible != creditsTab.Visible)
				{
					bBList[j].gameObject.SetActive(creditsTab.Visible);
				}
			}
		}
		int num2 = m_items.Count;
		m_creditsCompareItem.Position = y;
		m_creditsCompareItem.Size = 0f;
		int k = m_items.BinarySearch(m_creditsCompareItem, m_itemComparerBottomPosition);
		if (k < 0)
		{
			k = ~k;
		}
		for (; k < m_items.Count; k++)
		{
			if (m_items[k].Position + m_items[k].Size >= y)
			{
				num2 = k;
				break;
			}
		}
		int num3 = m_items.Count;
		for (k = num2 + 1; k < m_items.Count; k++)
		{
			if (m_items[k].Position > num)
			{
				num3 = k;
				break;
			}
		}
		for (int l = num2; l < num3; l++)
		{
			ICreditsItem creditsItem = m_items[l];
			if (creditsItem.Tab != null)
			{
				continue;
			}
			float num4 = 0f;
			int num5 = -1;
			BBList<CreditsTab> bBList2 = m_children[(int)creditsItem.Type];
			for (int m = 0; m < bBList2.Count; m++)
			{
				if (!(bBList2[m].Distance <= num4))
				{
					num4 = bBList2[m].Distance;
					num5 = m;
				}
			}
			if (num5 >= 0)
			{
				CreditsTab creditsTab2 = bBList2[num5];
				creditsTab2.UpdateItem(creditsItem);
				creditsTab2.Distance = 0f;
				creditsTab2.gameObject.SetActive(true);
			}
		}
	}

	private void ValidateChildren()
	{
		if (!CacheScrollView())
		{
			return;
		}
		m_panel.Update();
		Vector4 finalClipRegion = m_panel.finalClipRegion;
		m_panelWidth = finalClipRegion.z;
		m_panelHeight = finalClipRegion.w;
		for (int i = 0; i < 5; i++)
		{
			ECreditsItemType type = (ECreditsItemType)i;
			CreditsDefinition creditsDefinition = m_definitions[i];
			int num = Mathf.CeilToInt(m_panelHeight / creditsDefinition.MinTotalAdvance);
			BBList<CreditsTab> bBList = m_children[i];
			if (bBList == null)
			{
				bBList = (m_children[i] = new BBList<CreditsTab>(num));
			}
			while (bBList.Count < num)
			{
				CreateItem(type);
			}
		}
		if (m_items.Count > 0)
		{
			float num2 = m_panelHeight - m_items[0].Position;
			for (int j = 0; j < m_items.Count; j++)
			{
				m_items[j].Cast<CreditsItem>().Position += num2;
			}
		}
		m_totalDimension.x = m_panelWidth;
		m_totalDimension.y = m_allItemHeight + 2f * m_panelHeight;
	}

	private void CreateItem(ECreditsItemType _type)
	{
		GameObject prefab = m_prefabs[(int)_type];
		GameObject gameObject = NGUITools.AddChild(m_cachedTransform.gameObject, prefab);
		CreditsTab component = gameObject.GetComponent<CreditsTab>();
		m_children[(int)_type].Add(component);
		component.Init();
		CreditsTextTab creditsTextTab = component as CreditsTextTab;
		if (creditsTextTab != null)
		{
			creditsTextTab.SetFontScale(m_fontScale);
		}
	}
}
