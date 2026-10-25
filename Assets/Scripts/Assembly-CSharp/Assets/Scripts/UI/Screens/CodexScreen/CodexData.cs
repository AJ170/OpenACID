using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Database;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.State.ProfileManager;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Screens.CodexScreen
{
	public class CodexData : UIScreenChild
	{
		public class CodexEntryClass
		{
			public EEra era;

			public int sortId;

			public CodexDefinition.EEntryType category;

			public CodexDefinition entry;

			public CodexDefinition.EStructureType structure;

			public bool unlocked;

			public bool read;

			public GenericTab correspondingTab;

			public GameID entitlementGameID;
		}

		public static bool NewCodexEntryUnlocked;

		[SerializeField]
		private UITable RootContainer;

		[SerializeField]
		private GenericTab EraTabPrefab;

		[SerializeField]
		private GenericTab CategoryTabPrefab;

		[SerializeField]
		private GenericTab EntryTabPrefab;

		private readonly List<GenericTab> m_eraTabs = new List<GenericTab>();

		private readonly List<GenericTab> m_categoryTabs = new List<GenericTab>();

		private readonly List<GenericTab> m_entryTabs = new List<GenericTab>();

		private readonly List<GenericTab> m_allTabs = new List<GenericTab>();

		private bool m_elementsCreated;

		private readonly BBDictionary<int, CodexEntryClass> m_eraEntryDictionary = new BBDictionary<int, CodexEntryClass>();

		private readonly BBDictionary<int, BBList<CodexEntryClass>> m_categoryEntryDicionary = new BBDictionary<int, BBList<CodexEntryClass>>();

		private readonly BBDictionary<int, BBDictionary<int, BBList<CodexEntryClass>>> m_entryDicionary = new BBDictionary<int, BBDictionary<int, BBList<CodexEntryClass>>>();

		private List<CodexEntryClass> m_codexEntryClassList;

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, InitScreenEnter);
			UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexUnlockEntry("29765290-1d10-4f47-8ea8-6a8eed4d07bb");
			GetData();
			CreateElements();
		}

		private void InitScreenEnter()
		{
			CheckNewEntries();
			UpdateLockedAndReadStatus();
		}

		private void CheckNewEntries()
		{
			if (NewCodexEntryUnlocked)
			{
				m_elementsCreated = false;
				ClearDict();
				GetData();
				CreateElements();
				NewCodexEntryUnlocked = false;
			}
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, InitScreenEnter);
			}
		}

		public void UpdateLockedAndReadStatus()
		{
			if (m_elementsCreated)
			{
				return;
			}
			GetComponent<CodexNavigation>().Initialize(m_allTabs, m_eraTabs, m_categoryTabs, m_entryTabs, RootContainer);
			EntitlementManager entitlementManager = Singleton<StateManager>.Instance.ProfileManager.EntitlementManager;
			foreach (int key in m_eraEntryDictionary.Keys)
			{
				int num = 0;
				for (int i = 0; i < m_categoryEntryDicionary[key].Count; i++)
				{
					CodexEntryClass codexEntryClass = m_categoryEntryDicionary[key][i];
					int num2 = 0;
					int num3 = 0;
					foreach (CodexEntryClass item in m_entryDicionary[key][(int)codexEntryClass.category])
					{
						bool flag = item.entitlementGameID == GameID.Invalid || entitlementManager.HasEntitlement(item.entitlementGameID.Guid);
						item.unlocked = UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexEntryIsUnlocked(m_entryDicionary[key][(int)codexEntryClass.category][num2].entry.GameID, out m_entryDicionary[key][(int)codexEntryClass.category][num2].read) && flag;
						if (item.unlocked)
						{
							item.correspondingTab.backGround.color = new Color32(103, 193, byte.MaxValue, byte.MaxValue);
							item.correspondingTab.GetComponent<BoxCollider>().enabled = true;
							item.correspondingTab.gameObject.SetActive(true);
							num3++;
							if (item.read)
							{
								item.correspondingTab.readMarker.gameObject.SetActive(false);
								codexEntryClass.correspondingTab.readMarker.gameObject.SetActive(false);
							}
							if (!item.read)
							{
								item.correspondingTab.readMarker.gameObject.SetActive(true);
								codexEntryClass.correspondingTab.readMarker.gameObject.SetActive(true);
								m_eraEntryDictionary[key].correspondingTab.readMarker.gameObject.SetActive(true);
							}
						}
						else
						{
							item.correspondingTab.gameObject.SetActive(false);
							for (int j = 0; j < m_allTabs.Count; j++)
							{
								if (m_allTabs[j].SubTabContainer != null)
								{
									m_allTabs[j].SubTabContainer.Reposition();
								}
							}
						}
						if (item.read)
						{
							item.correspondingTab.readMarker.enabled = false;
						}
						if (!item.read)
						{
							item.correspondingTab.readMarker.enabled = true;
						}
						num2++;
					}
					if (num3 == 0)
					{
						codexEntryClass.correspondingTab.gameObject.SetActive(false);
						continue;
					}
					codexEntryClass.correspondingTab.numberOfEntries.text = string.Format("{0} / {1}", num3, num2);
					num++;
				}
				if (num == 0)
				{
					m_eraEntryDictionary[key].correspondingTab.gameObject.SetActive(false);
				}
				else
				{
					m_eraEntryDictionary[key].correspondingTab.gameObject.SetActive(true);
				}
			}
			m_elementsCreated = true;
		}

		public void UpdateReadStatus()
		{
			foreach (int key in m_eraEntryDictionary.Keys)
			{
				int num = 0;
				bool active = false;
				foreach (CodexEntryClass item in m_categoryEntryDicionary[key])
				{
					num++;
					int num2 = 0;
					bool active2 = false;
					foreach (CodexEntryClass item2 in m_entryDicionary[key][(int)item.category])
					{
						item2.unlocked = UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexEntryIsUnlocked(m_entryDicionary[key][(int)item.category][num2].entry.GameID, out m_entryDicionary[key][(int)item.category][num2].read);
						if (item2.unlocked)
						{
							if (!item2.read)
							{
								active2 = true;
								active = true;
								item2.correspondingTab.readMarker.gameObject.SetActive(true);
								break;
							}
							item2.correspondingTab.readMarker.gameObject.SetActive(false);
						}
						item2.correspondingTab.readMarker.enabled = !item2.read;
						num2++;
					}
					item.correspondingTab.readMarker.gameObject.SetActive(active2);
				}
				m_eraEntryDictionary[key].correspondingTab.readMarker.gameObject.SetActive(active);
			}
		}

		private void GetData()
		{
			List<KeyValuePair<GameID, DBEntry>> items = Main.Instance.GameDB.GetItems<CodexDefinition>();
			m_codexEntryClassList = new List<CodexEntryClass>();
			int num = 0;
			foreach (KeyValuePair<GameID, DBEntry> item in items)
			{
				if (item.Value.Status == DBEntry.EStatus.Obsolete)
				{
					continue;
				}
				bool flag = false;
				m_codexEntryClassList.Add(new CodexEntryClass());
				m_codexEntryClassList[num].entry = (CodexDefinition)item.Value;
				m_codexEntryClassList[num].era = m_codexEntryClassList[num].entry.Age;
				m_codexEntryClassList[num].category = m_codexEntryClassList[num].entry.EntryType;
				m_codexEntryClassList[num].structure = m_codexEntryClassList[num].entry.StructureType;
				m_codexEntryClassList[num].entitlementGameID = m_codexEntryClassList[num].entry.ReqEntitlementID;
				flag = Singleton<StateManager>.Instance.ProfileManager.EntitlementManager.HasEntitlement(m_codexEntryClassList[num].entitlementGameID.Guid) || m_codexEntryClassList[num].entitlementGameID != GameID.Invalid;
				m_codexEntryClassList[num].unlocked = UbiCore.Utils.Singleton<LocalStorage>.Instance.CodexEntryIsUnlocked(m_codexEntryClassList[num].entry.GameID, out m_codexEntryClassList[num].read) && flag;
				m_codexEntryClassList[num].sortId = m_codexEntryClassList[num].entry.SortId;
				EEra era = m_codexEntryClassList[num].era;
				CodexDefinition.EEntryType category = m_codexEntryClassList[num].category;
				CodexDefinition.EStructureType structure = m_codexEntryClassList[num].structure;
				if (!m_entryDicionary.ContainsKey((int)era))
				{
					m_entryDicionary.Add((int)era, new BBDictionary<int, BBList<CodexEntryClass>>());
				}
				if (!m_entryDicionary[(int)era].ContainsKey((int)category))
				{
					m_entryDicionary[(int)era].Add((int)category, new BBList<CodexEntryClass>());
				}
				if (structure == CodexDefinition.EStructureType.StartingPage)
				{
				}
				if (structure == CodexDefinition.EStructureType.EraOverview)
				{
					m_eraEntryDictionary.Add((int)era, m_codexEntryClassList[num]);
				}
				if (structure == CodexDefinition.EStructureType.CategoryOverview)
				{
					if (!m_categoryEntryDicionary.ContainsKey((int)era))
					{
						m_categoryEntryDicionary.Add((int)era, new BBList<CodexEntryClass>());
					}
					m_categoryEntryDicionary[(int)era].Add(m_codexEntryClassList[num]);
				}
				if (structure == CodexDefinition.EStructureType.Entry)
				{
					m_entryDicionary[(int)era][(int)category].Add(m_codexEntryClassList[num]);
				}
				num++;
			}
		}

		private void CreateElements()
		{
			foreach (int key in m_eraEntryDictionary.Keys)
			{
				GenericTab genericTab = (GenericTab)Object.Instantiate(EraTabPrefab, Vector3.zero, Quaternion.identity);
				genericTab.transform.parent = RootContainer.transform;
				genericTab.transform.localScale = Vector3.one;
				genericTab.transform.localPosition = Vector3.zero;
				m_allTabs.Add(genericTab);
				m_eraTabs.Add(genericTab);
				genericTab.data = m_eraEntryDictionary[key].entry;
				genericTab.GameId = m_eraEntryDictionary[key].entry.GameID;
				m_eraEntryDictionary[key].correspondingTab = genericTab;
				int num = 0;
				foreach (CodexEntryClass item in m_categoryEntryDicionary[key])
				{
					GenericTab genericTab2 = (GenericTab)Object.Instantiate(CategoryTabPrefab, Vector3.zero, Quaternion.identity);
					genericTab2.transform.parent = genericTab.SubTabContainer.transform;
					genericTab2.transform.localScale = Vector3.one;
					genericTab2.transform.localPosition = Vector3.zero;
					m_allTabs.Add(genericTab2);
					m_categoryTabs.Add(genericTab2);
					genericTab2.data = m_categoryEntryDicionary[key][num].entry;
					m_categoryEntryDicionary[key][num].correspondingTab = genericTab2;
					genericTab2.GameId = m_categoryEntryDicionary[key][num].entry.GameID;
					num++;
					int num2 = 0;
					foreach (CodexEntryClass item2 in m_entryDicionary[key][(int)item.category])
					{
						GenericTab genericTab3 = (GenericTab)Object.Instantiate(EntryTabPrefab, Vector3.zero, Quaternion.identity);
						genericTab3.transform.parent = genericTab2.SubTabContainer.transform;
						genericTab3.transform.localScale = Vector3.one;
						genericTab3.transform.localPosition = Vector3.zero;
						m_allTabs.Add(genericTab3);
						m_entryTabs.Add(genericTab3);
						genericTab3.data = item2.entry;
						item2.correspondingTab = genericTab3;
						genericTab3.GameId = item2.entry.GameID;
						if (item2.unlocked)
						{
							genericTab3.backGround.color = new Color32(103, 193, byte.MaxValue, byte.MaxValue);
							genericTab3.GetComponent<BoxCollider>().enabled = true;
							if (item2.read)
							{
								genericTab3.readMarker.gameObject.SetActive(false);
								genericTab2.readMarker.gameObject.SetActive(false);
								genericTab.readMarker.gameObject.SetActive(false);
							}
							if (!item2.read)
							{
								genericTab3.readMarker.gameObject.SetActive(true);
								genericTab2.readMarker.gameObject.SetActive(true);
								genericTab.readMarker.gameObject.SetActive(true);
							}
						}
						else
						{
							genericTab3.backGround.color = new Color32(60, 83, 124, byte.MaxValue);
							genericTab3.GetComponent<BoxCollider>().enabled = false;
							genericTab3.readMarker.gameObject.SetActive(false);
						}
						genericTab3.readMarker.enabled = !item2.read;
						num2++;
					}
					genericTab2.SubTabContainer.Reposition();
				}
				genericTab.SubTabContainer.Reposition();
			}
			RootContainer.sorting = UITable.Sorting.Alphabetic;
			UnwrapData();
			RootContainer.Reposition();
		}

		private void UnwrapData()
		{
			for (int i = 0; i < m_allTabs.Count; i++)
			{
				CodexDefinition data = m_allTabs[i].data;
				GenericTab genericTab = m_allTabs[i];
				genericTab.name = genericTab.data.SortId.ToString("D3");
				if (LocalizationManager.Instance.HasKey(data.HeaderLocaKey.Key))
				{
					genericTab.Header.text = LocalizationManager.Instance.Get(data.HeaderLocaKey.Key);
				}
				else
				{
					genericTab.Header.text = data.Name + "      (Key Missing)";
				}
			}
		}

		private void ClearDict()
		{
			for (int i = 0; i < m_allTabs.Count; i++)
			{
				if ((bool)m_allTabs[i])
				{
					Object.Destroy(m_allTabs[i].gameObject);
				}
			}
			m_allTabs.Clear();
			m_eraEntryDictionary.Clear();
			m_categoryEntryDicionary.Clear();
			m_entryDicionary.Clear();
		}
	}
}
