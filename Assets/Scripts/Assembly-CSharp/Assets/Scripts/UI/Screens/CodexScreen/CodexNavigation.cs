using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Loading;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.ScreenManager.Layout;
using Database;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.CodexScreen
{
	public class CodexNavigation : MonoBehaviour
	{
		private const int m_highQualityAtlasSize = 1024;

		private List<GenericTab> m_tabs = new List<GenericTab>();

		private List<GenericTab> m_EraTabs = new List<GenericTab>();

		private List<GenericTab> m_CategoryTabs = new List<GenericTab>();

		private List<GenericTab> m_EntryTabs = new List<GenericTab>();

		private UITable m_rootContainer;

		private GenericTab currentTab;

		private GenericTab lastTab;

		private LoadAnimationManager m_loadAnimMgr;

		private int m_originalHeight;

		private CodexLayout layout;

		[SerializeField]
		private CodexLayout m_portrait;

		[SerializeField]
		private CodexLayout m_Landscape;

		[SerializeField]
		private CodexLayout m_landscapeCutout;

		[SerializeField]
		private CodexLayout m_plainText;

		[SerializeField]
		private UIExtendedLabel m_header;

		[SerializeField]
		private UIWidget m_headerContainer;

		[SerializeField]
		private UISprite m_MoveableHighlight;

		[SerializeField]
		private UIScrollBar m_scrollBar;

		[SerializeField]
		private UIScrollView m_scrollViewTable;

		[SerializeField]
		private UIScrollView m_scrollViewContent;

		private void Awake()
		{
			m_loadAnimMgr = LoadAnimationManager.CreateNewInstance(base.transform);
		}

		private void Start()
		{
			m_headerContainer.Update();
			m_originalHeight = m_headerContainer.height;
			UpdateHeaderSize();
		}

		private void OnDestroy()
		{
			for (int i = 0; i < m_tabs.Count; i++)
			{
				if (m_tabs[i] != null && m_tabs[i].gameObject != null)
				{
					UIButton component = m_tabs[i].gameObject.GetComponent<UIButton>();
					if (component != null)
					{
						EventDelegate.Remove(component.onClick, OnClickedTab);
						EventDelegate.Remove(m_tabs[i].gameObject.AddComponentIfNotExist<UIButton>().onClick, base.gameObject.GetComponent<CodexData>().UpdateReadStatus);
					}
				}
			}
			m_tabs.Clear();
			m_EntryTabs.Clear();
			m_CategoryTabs.Clear();
			m_EraTabs.Clear();
			lastTab = null;
		}

		private void UpdateHeaderSize()
		{
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
			{
				UILayoutManager layoutManager = UbiCore.Utils.Singleton<UiMain>.Instance.LayoutManager;
				if (layoutManager.SelectedFamilyType == EFamilyType.IphoneDPI326)
				{
					m_headerContainer.height = Convert.ToInt32((float)m_originalHeight * 1.6f);
				}
				else
				{
					m_headerContainer.height = m_originalHeight;
				}
			}
		}

		public void Initialize(List<GenericTab> getList, List<GenericTab> getListEra, List<GenericTab> getListCategory, List<GenericTab> getListEntry, UITable getContainer)
		{
			m_tabs = getList;
			m_EraTabs = getListEra;
			m_CategoryTabs = getListCategory;
			m_EntryTabs = getListEntry;
			m_rootContainer = getContainer;
			for (int i = 0; i < m_tabs.Count; i++)
			{
				EventDelegate.Add(m_tabs[i].gameObject.AddComponentIfNotExist<UIButton>().onClick, OnClickedTab);
				EventDelegate.Add(m_tabs[i].gameObject.AddComponentIfNotExist<UIButton>().onClick, base.gameObject.GetComponent<CodexData>().UpdateReadStatus);
			}
			for (int j = 0; j < m_tabs.Count; j++)
			{
				if (m_tabs[j].data.StructureType != 0)
				{
					m_tabs[j].SubTabContainer.gameObject.SetActive(false);
					m_tabs[j].SubTabContainer.Reposition();
					m_tabs[j].SubTabContainer.transform.localScale = new Vector3(1f, 0f, 1f);
				}
			}
			m_rootContainer.Reposition();
			m_MoveableHighlight.enabled = false;
			UpdateOnScreenEnter();
		}

		private IEnumerator LoadCodexImage(CodexDefinition _data, CodexLayout _layout, CodexDefinition.ELayout _layoutType)
		{
			UnityResource resource = _data.AtlasTexture;
			yield return StartCoroutine(m_loadAnimMgr.BeginLoadUnityResource(resource, new LoadAnimationManager.SLoadObjectInfo(_layout.Image)));
			_layout.ImageResource = resource;
			_layout.Image.mainTexture = resource.AccessObject<Texture>();
			_layout.Image.uvRect = _data.AtlasPosition;
			_layout.Image.MakePixelPerfect();
			if (layout.Image.mainTexture.height < 1024)
			{
				_layout.Image.width *= 2;
				_layout.Image.height *= 2;
			}
			if (UbiCore.Utils.Singleton<UiMain>.IsAvailable)
			{
				UILayoutManager layoutMgr = UbiCore.Utils.Singleton<UiMain>.Instance.LayoutManager;
				if (layoutMgr.SelectedFamilyType == EFamilyType.IphoneDPI326)
				{
					_layout.Image.width = Convert.ToInt32((float)_layout.Image.width * 1.6f);
					_layout.Image.height = Convert.ToInt32((float)_layout.Image.height * 1.6f);
				}
			}
		}

		private void ClearImage(CodexLayout _layout)
		{
			if (_layout.ImageResource != null)
			{
				_layout.ImageResource.ClearCache();
			}
			if (Main.IsInitialized)
			{
				Main.Instance.RequestResourceUnload();
			}
			_layout.ImageResource = null;
			_layout.Image.ClearTexture();
			_layout.gameObject.SetActive(false);
		}

		private IEnumerator SetHighLight(bool _delayed)
		{
			if (_delayed)
			{
				yield return new WaitForSeconds(0.4f);
			}
			m_MoveableHighlight.enabled = true;
			m_MoveableHighlight.transform.position = new Vector3(currentTab.transform.position.x - 0.9f, currentTab.transform.position.y - 0.01f, currentTab.transform.position.z);
		}

		private void UpdateTabs(GenericTab tab, bool delayHighLight = false)
		{
			if (!currentTab.activated)
			{
				currentTab.ActivateChildren();
			}
			else if (currentTab.activated && !m_EntryTabs.Contains(currentTab))
			{
				currentTab.DeActivateChildren();
			}
			if (currentTab.data.StructureType == CodexDefinition.EStructureType.Entry)
			{
				currentTab.readMarker.gameObject.SetActive(false);
				UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexSetEntryReadFlag(currentTab.GameId);
				StartCoroutine(SetHighLight(delayHighLight));
			}
			else
			{
				StartAlignment();
			}
			if (lastTab != currentTab)
			{
				currentTab.ActivationHighlight();
				if (lastTab != null)
				{
					lastTab.DeactivationHighlight();
				}
			}
			if (currentTab.data.StructureType == CodexDefinition.EStructureType.CategoryOverview)
			{
				m_MoveableHighlight.enabled = false;
			}
			if (currentTab.data.StructureType == CodexDefinition.EStructureType.EraOverview)
			{
				m_MoveableHighlight.enabled = false;
			}
			m_scrollBar.value = 0f;
			if (currentTab != lastTab)
			{
				ClearImage(m_Landscape);
				ClearImage(m_landscapeCutout);
				ClearImage(m_portrait);
				switch (tab.data.Layout)
				{
				case CodexDefinition.ELayout.Landscape:
					layout = m_Landscape;
					StartCoroutine(LoadCodexImage(tab.data, layout, tab.data.Layout));
					break;
				case CodexDefinition.ELayout.Landscape_cutout:
					layout = m_landscapeCutout;
					StartCoroutine(LoadCodexImage(tab.data, layout, tab.data.Layout));
					break;
				case CodexDefinition.ELayout.Portrait:
					layout = m_portrait;
					StartCoroutine(LoadCodexImage(tab.data, layout, tab.data.Layout));
					break;
				case CodexDefinition.ELayout.Plain_Text:
					layout = m_plainText;
					break;
				case CodexDefinition.ELayout.Banner:
					layout = m_Landscape;
					break;
				case CodexDefinition.ELayout.LeftPicture:
					layout = m_Landscape;
					break;
				}
				layout.gameObject.SetActive(true);
				layout.text.LocalizationKey = tab.data.BodyLocaKey.Key;
				if (tab.data.BodyLocaKey.Key == 0)
				{
					layout.text.text = "No LocaKey available";
				}
				m_header.LocalizationKey = tab.data.HeaderLocaKey.Key;
			}
			m_scrollViewContent.UpdateScrollbars(true);
			m_scrollViewTable.UpdateScrollbars(true);
			if (!currentTab.activated)
			{
				currentTab.ActivationAnimation();
			}
			else if (currentTab.activated)
			{
				currentTab.DeactivationAnimation();
			}
			if (m_EntryTabs.Contains(lastTab) && currentTab != lastTab && lastTab != null)
			{
				lastTab.DeactivationAnimation();
			}
		}

		private void OnClickedTab()
		{
			currentTab = UIButton.current.GetComponent<GenericTab>();
			UpdateTabs(currentTab);
			lastTab = currentTab;
		}

		private void StartAlignment()
		{
			StopAllCoroutines();
			StartCoroutine(AlignElements());
		}

		private IEnumerator AlignElements()
		{
			float timer = 0f;
			while (true)
			{
				yield return new WaitForEndOfFrame();
				timer += Time.deltaTime;
				for (int i = 0; i < m_tabs.Count; i++)
				{
					if (m_tabs[i].data.StructureType != 0)
					{
						m_tabs[i].SubTabContainer.Reposition();
					}
				}
				m_rootContainer.Reposition();
				for (int j = 0; j < m_tabs.Count; j++)
				{
					m_tabs[j].transform.localPosition = new Vector3(Mathf.Round(m_tabs[j].transform.localPosition.x), Mathf.Round(m_tabs[j].transform.localPosition.y), Mathf.Round(m_tabs[j].transform.localPosition.z));
				}
				RepositionScrollViewifOutofBounds();
				if (timer > 0.8f)
				{
					break;
				}
				yield return null;
			}
		}

		private void RepositionScrollViewifOutofBounds()
		{
			m_scrollViewTable.UpdateScrollbars(true);
			if (m_scrollViewTable.canMoveVertically && !m_scrollViewTable.shouldMoveVertically)
			{
				m_scrollViewTable.RestrictWithinBounds(false);
			}
		}

		public void UpdateOnScreenEnter()
		{
			lastTab = null;
			for (int i = 0; i < m_tabs.Count; i++)
			{
				if (m_tabs[i].GameId == UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_startCodexEntryEra)
				{
					currentTab = m_tabs[i];
					break;
				}
			}
			UpdateTabs(currentTab);
			lastTab = currentTab;
			for (int j = 0; j < m_tabs.Count; j++)
			{
				if (m_tabs[j].GameId == UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_startCodexEntryCategory)
				{
					currentTab = m_tabs[j];
					break;
				}
			}
			UpdateTabs(currentTab);
			lastTab = currentTab;
			for (int k = 0; k < m_tabs.Count; k++)
			{
				if (m_tabs[k].GameId == UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_startCodexEntryEntry)
				{
					currentTab = m_tabs[k];
					break;
				}
			}
			UpdateTabs(currentTab, true);
			lastTab = currentTab;
			m_scrollViewTable.verticalScrollBar.value = 0f;
			m_scrollViewTable.ResetPosition();
		}

		private void OnApplicationPause(bool pause)
		{
			if (!pause && m_scrollViewContent != null && m_scrollViewTable != null)
			{
				m_scrollBar.value = 0f;
			}
		}
	}
}
