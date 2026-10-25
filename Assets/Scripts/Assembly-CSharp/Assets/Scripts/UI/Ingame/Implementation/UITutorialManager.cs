using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Ingame.Behaviours;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Tutorial;
using Assets.Scripts.UI.Tutorial.Flow;
using Assets.Scripts.UI.Utils;
using Mission;
using SharedBaseLib.Core;
using SharedBaseLib.System;
using SharedBaseLib.System.Extensions;
using UbiCore.Messenger;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Implementation
{
	public class UITutorialManager : UISubSystem
	{
		private const ETutorialCategory LastBasicTutorialCategory = ETutorialCategory.TutCatGuideEraSelect;

		private readonly BBDictionary<string, GameObject> m_ActiveHighlightFX = new BBDictionary<string, GameObject>();

		private readonly BBDictionary<string, GameObject> m_TutorialWidgets = new BBDictionary<string, GameObject>();

		private readonly BBDictionary<GameObject, bool> m_TutorialWidgetsVisibility = new BBDictionary<GameObject, bool>();

		private readonly BBHashSet<GameObject> m_IgnoreInputWidgets = new BBHashSet<GameObject>();

		private bool m_CheckInput;

		private readonly BBList<TutorialUiEvent> m_WaitingEvents = new BBList<TutorialUiEvent>();

		private readonly TutorialCategory[] m_categories = new TutorialCategory[14]
		{
			new TutorialCategory(ETutorialCategory.TutCatIntroMission),
			new TutorialCategory(ETutorialCategory.TutCatAssassinCreation),
			new TutorialCategory(ETutorialCategory.TutCatGuideEraSelect),
			new TutorialCategory(ETutorialCategory.TutCatMs26Codex),
			new TutorialCategory(ETutorialCategory.TutCatMs25Equip),
			new TutorialCategory(ETutorialCategory.TutCatMs25SkillPoints),
			new TutorialCategory(ETutorialCategory.TutCatGuideShop),
			new TutorialCategory(ETutorialCategory.TutCatMs26Modules),
			new TutorialCategory(ETutorialCategory.TutCatMs26Contracts),
			new TutorialCategory(ETutorialCategory.TutCatMs25Forge),
			new TutorialCategory(ETutorialCategory.TutCatMs26Hirelings),
			new TutorialCategory(ETutorialCategory.TutCatMs26HeroicMode),
			new TutorialCategory(ETutorialCategory.TutCatMs26QuestIntro),
			new TutorialCategory(ETutorialCategory.TutCatMs26QuestRewards)
		};

		private readonly ETutorialCategory[] m_categoryWithDirectPredecessor = new ETutorialCategory[6]
		{
			ETutorialCategory.TutCatAssassinCreation,
			ETutorialCategory.TutCatGuideEraSelect,
			ETutorialCategory.TutCatMs26Codex,
			ETutorialCategory.TutCatMs25SkillPoints,
			ETutorialCategory.TutCatMs26Modules,
			ETutorialCategory.TutCatMs26QuestIntro
		};

		public TutorialCategory CurrentCategory { get; private set; }

		public bool IsTutorialActive
		{
			get
			{
				TutorialCategory lastCategory = LastCategory;
				return lastCategory != null && IsACategoryActive(lastCategory.Category);
			}
		}

		public bool IsTutorialFinished
		{
			get
			{
				TutorialCategory lastCategory = LastCategory;
				return lastCategory == null || lastCategory.Finished;
			}
		}

		public bool IsFirstContactGuideActive
		{
			get
			{
				return IsACategoryActive(ETutorialCategory.TutCatGuideEraSelect);
			}
		}

		public bool IsEraSelectOrBeforeActive
		{
			get
			{
				return IsACategoryActive(ETutorialCategory.TutCatGuideEraSelect);
			}
		}

		public bool IsShopGuideActive
		{
			get
			{
				return GetCategory(ETutorialCategory.TutCatGuideShop).GetIsStepActive();
			}
		}

		public bool HasCompletedAssassinCreation
		{
			get
			{
				TutorialCategory category = GetCategory(ETutorialCategory.TutCatAssassinCreation);
				return category != null && category.Finished;
			}
		}

		public bool IsForgeActive
		{
			get
			{
				return IsACategoryActive(ETutorialCategory.TutCatMs25Forge);
			}
		}

		public TutorialCategory[] Categories
		{
			get
			{
				return m_categories;
			}
		}

		public TutorialCategory LastCategory
		{
			get
			{
				int num = Categories.Length;
				return (num != 0) ? Categories[num - 1] : null;
			}
		}

		public bool IsBasicTutorialFinished
		{
			get
			{
				TutorialCategory category = GetCategory(ETutorialCategory.TutCatGuideEraSelect);
				return category == null || category.Finished;
			}
		}

		public event Action<ETutorialCategory> OnCategoryFinished;

		public UITutorialManager()
		{
			for (int i = 0; i < Categories.Length; i++)
			{
				Categories[i].Index = i;
			}
			CurrentCategory = Categories[0];
		}

		public bool GetIsATutorialStepActive()
		{
			for (int i = 0; i < Categories.Length; i++)
			{
				TutorialCategory tutorialCategory = Categories[i];
				if (tutorialCategory.GetIsStepActive())
				{
					return true;
				}
			}
			return false;
		}

		public void PurgeTutorial()
		{
			for (int i = 0; i < Categories.Length; i++)
			{
				Categories[i].Purge();
			}
		}

		public void RegisterId(UITutorialId _id)
		{
			if (m_TutorialWidgets.ContainsKey(_id.FullPath))
			{
				m_TutorialWidgets[_id.FullPath] = _id.gameObject;
			}
			else
			{
				m_TutorialWidgets.Add(_id.FullPath, _id.gameObject);
			}
		}

		public void UnRegisterId(UITutorialId _id)
		{
			m_TutorialWidgets.Remove(_id.FullPath);
		}

		public void RegisterToCategory(ITutorialFlowable _tutorial)
		{
			TutorialCategory category = GetCategory(_tutorial.Category);
			if (category != null)
			{
				category.Add(_tutorial);
			}
		}

		public void UnRegisterFromCategory(ITutorialFlowable _tutorial)
		{
			TutorialCategory category = GetCategory(_tutorial.Category);
			if (category != null)
			{
				category.Remove(_tutorial);
			}
		}

		public void InitFromProfile()
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			for (int i = 0; i < Categories.Length; i++)
			{
				TutorialCategory category = Categories[i];
				InitCategoryFromProfile(profile, category);
			}
		}

		public bool SetFirstAvailableStep()
		{
			UserProfile profile = Singleton<StateManager>.Instance.ProfileManager.Profile;
			bool flag = false;
			TutorialCategory tutorialCategory = null;
			for (int i = 0; i < Categories.Length; i++)
			{
				TutorialCategory tutorialCategory2 = Categories[i];
				InitCategoryFromProfile(profile, tutorialCategory2);
				if (!tutorialCategory2.Finished && TutorialHelper.OwningRequiredEntitlements(tutorialCategory2.Category))
				{
					flag = tutorialCategory2.SetFirstAvailableStep();
					if (flag)
					{
						tutorialCategory = tutorialCategory2;
						break;
					}
					if (tutorialCategory == null && tutorialCategory2 > CurrentCategory)
					{
						tutorialCategory = tutorialCategory2;
					}
				}
			}
			if (tutorialCategory != null)
			{
				CurrentCategory = tutorialCategory;
				if (tutorialCategory.CurrentFlowId == STutorialFlowId.Invalid)
				{
					tutorialCategory.Init();
				}
			}
			return flag;
		}

		public void FinishTutorial()
		{
			OnFinishTutorial(null, 0);
		}

		private void OnFinishTutorial(IAsyncResult _ac, int _i)
		{
			if (_i >= Categories.Length)
			{
				return;
			}
			TutorialCategory tutorialCategory = Categories[_i];
			if (TutorialHelper.OwningFinishedEntitlements(tutorialCategory.Category))
			{
				OnFinishTutorial(null, _i + 1);
				return;
			}
			tutorialCategory.SetFinished(delegate(IAsyncResult _asc)
			{
				OnFinishTutorial(_asc, _i + 1);
			});
		}

		public bool NextStep(ETutorialCategory _category)
		{
			bool result = false;
			TutorialCategory category = GetCategory(_category);
			if (category != null)
			{
				result = category.NextStep(category.CurrentFlowId);
				if (category.CurrentFlowId == STutorialFlowId.Invalid)
				{
					SetFirstAvailableStep();
				}
			}
			return result;
		}

		public void FinishCategory(ETutorialCategory _category, AsyncCallback _callback)
		{
			TutorialCategory category = GetCategory(_category);
			if (category != null)
			{
				category.SetFinished(delegate(IAsyncResult _ar)
				{
					OnSetFinished(_ar, _callback, _category);
				});
			}
		}

		private void OnSetFinished(IAsyncResult _ar, AsyncCallback _callback, ETutorialCategory _category)
		{
			SetFirstAvailableStep();
			if (_callback != null)
			{
				_callback(_ar);
			}
			if (this.OnCategoryFinished != null)
			{
				this.OnCategoryFinished(_category);
			}
			if (_category == LastCategory.Category)
			{
				MATManager.LogCompletedTutorial();
			}
		}

		public TutorialCategory GetCategory(ETutorialCategory _category)
		{
			TutorialCategory result = null;
			for (int i = 0; i < Categories.Length; i++)
			{
				TutorialCategory tutorialCategory = Categories[i];
				if (tutorialCategory.Category == _category)
				{
					result = tutorialCategory;
					break;
				}
			}
			return result;
		}

		public void GotInput(string _idFullPath)
		{
			if (m_CheckInput)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.WidgetGotPressed(_idFullPath);
			}
		}

		public bool GotInput(GameObject go, string funcName, object obj)
		{
			if (m_CheckInput)
			{
				bool flag = funcName == "OnPress" && !(bool)obj;
				UITutorialId component = go.GetComponent<UITutorialId>();
				if (flag && component != null && m_TutorialWidgets.ContainsKey(component.FullPath))
				{
					UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.WidgetGotPressed(component.FullPath);
				}
				if (m_IgnoreInputWidgets.Contains(go))
				{
					return false;
				}
			}
			return true;
		}

		public override bool Tap(TapEvent _gesture)
		{
			return false;
		}

		public bool AdjustVisibility(GameObject _go)
		{
			bool flag = false;
			UITutorialId component = _go.GetComponent<UITutorialId>();
			if (component == null)
			{
				return false;
			}
			int num = 0;
			while (num < m_WaitingEvents.Count)
			{
				TutorialUiEvent tutorialUiEvent = m_WaitingEvents[num];
				if (tutorialUiEvent.Path == component.FullPath)
				{
					TutorialUiEvent.EType type = tutorialUiEvent.Type;
					if (type == TutorialUiEvent.EType.ShowWidget || type == TutorialUiEvent.EType.HideWidget)
					{
						SynchronizeVisiblity(tutorialUiEvent, _go);
						m_WaitingEvents.RemoveAt(num);
						flag = true;
					}
					else
					{
						num++;
					}
				}
				else
				{
					num++;
				}
			}
			if (!flag && m_TutorialWidgetsVisibility.ContainsKey(_go))
			{
				return m_TutorialWidgetsVisibility[_go];
			}
			return flag;
		}

		public override void UpdateFrame(float _deltaTime)
		{
			if (UbiCore.Utils.Singleton<MissionManager>.Instance.MissionState != EMissionState.InProgress)
			{
				return;
			}
			int num = 0;
			while (num < m_WaitingEvents.Count)
			{
				TutorialUiEvent tutorialUiEvent = m_WaitingEvents[num];
				GameObject value = null;
				if (m_TutorialWidgets.TryGetValue(tutorialUiEvent.Path, out value))
				{
					switch (tutorialUiEvent.Type)
					{
					case TutorialUiEvent.EType.ShowWidget:
					case TutorialUiEvent.EType.HideWidget:
						SynchronizeVisiblity(tutorialUiEvent, value);
						break;
					case TutorialUiEvent.EType.BlockWidget:
						m_IgnoreInputWidgets.Add(value);
						break;
					case TutorialUiEvent.EType.UnBlockWidget:
						m_IgnoreInputWidgets.Remove(value);
						break;
					case TutorialUiEvent.EType.SetHighlight:
						if (!m_ActiveHighlightFX.ContainsKey(tutorialUiEvent.Path))
						{
							UnityEngine.Object original = UnityEngine.Resources.Load("UI/Misc/ButtonHighlight");
							GameObject gameObject = UnityEngine.Object.Instantiate(original) as GameObject;
							gameObject.SetLayerRecursively(Layers.UI3DLayer);
							UIWidget component = value.GetComponent<UIWidget>();
							gameObject.transform.parent = value.transform;
							gameObject.transform.localPosition = UIUtil.GetWidgetCenter(component);
							m_ActiveHighlightFX.Add(tutorialUiEvent.Path, gameObject);
						}
						break;
					case TutorialUiEvent.EType.RemoveHighlight:
					{
						GameObject value2 = null;
						if (m_ActiveHighlightFX.TryGetValue(tutorialUiEvent.Path, out value2))
						{
							UnityEngine.Object.Destroy(value2);
							m_ActiveHighlightFX.Remove(tutorialUiEvent.Path);
						}
						break;
					}
					}
					m_WaitingEvents.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
			foreach (KeyValuePair<string, GameObject> item in m_ActiveHighlightFX)
			{
				ParticleSystem[] componentsInChildren = item.Value.GetComponentsInChildren<ParticleSystem>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].Simulate(UbiCore.Utils.Singleton<Timer>.Instance.realDeltaTime, true, false);
				}
				item.Value.SetActive(CheckIfHighlightIsVisible(item.Value));
			}
		}

		public override void OnMissionLoad()
		{
			base.IsActive = UbiCore.Utils.Singleton<UiMain>.Instance.MissionType != EMissionType.Animus_EMissionType;
			if (IsActive)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.AddListener<TutorialUiEvent>(OnTutorialUiEvent);
			}
			string[] array = m_TutorialWidgets.Keys.ToArray();
			string[] array2 = array;
			foreach (string key in array2)
			{
				if (m_TutorialWidgets[key] == null)
				{
					m_TutorialWidgets.Remove(key);
				}
			}
		}

		public override void OnMissionUnload()
		{
			if (IsActive)
			{
				UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.RemoveListener<TutorialUiEvent>(OnTutorialUiEvent);
				foreach (KeyValuePair<string, GameObject> item in m_ActiveHighlightFX)
				{
					UnityEngine.Object.Destroy(item.Value);
				}
				m_ActiveHighlightFX.Clear();
				base.IsActive = false;
			}
			m_CheckInput = false;
			m_IgnoreInputWidgets.Clear();
			m_WaitingEvents.Clear();
			m_TutorialWidgetsVisibility.Clear();
		}

		public bool IsSettingsReturnToAnimusButtonVisible()
		{
			TutorialCategory category = GetCategory(ETutorialCategory.TutCatGuideEraSelect);
			return category == null || category.Finished;
		}

		public bool IsDebriefingConfirmationVisible(bool _missionCompleted)
		{
			TutorialCategory category = GetCategory(ETutorialCategory.TutCatGuideEraSelect);
			return (category != null && category.Finished) || _missionCompleted;
		}

		public bool IsTutorialCategoryActive(ETutorialCategory _category)
		{
			TutorialCategory category = GetCategory(_category);
			if (category == null)
			{
			}
			return category != null && category.GetIsStepActive();
		}

		public bool IsHirelingTutorialActive()
		{
			TutorialCategory category = GetCategory(ETutorialCategory.TutCatGuideEraSelect);
			return category != null && !category.Finished;
		}

		public bool IsShopTutorialBeyondGotoShop(ETutorialCategory _category, EFlowId _step)
		{
			TutorialCategory category = GetCategory(_category);
			return category != null && (int)category.CurrentFlowId >= (int)_step;
		}

		public bool IsCategoryAtFlowId(ETutorialCategory _category, int _flowId)
		{
			TutorialCategory category = GetCategory(_category);
			return category != null && category.IsAtFlowId(_flowId);
		}

		private void OnTutorialUiEvent(TutorialUiEvent _event)
		{
			switch (_event.Type)
			{
			case TutorialUiEvent.EType.ShowWidget:
			case TutorialUiEvent.EType.HideWidget:
			case TutorialUiEvent.EType.SetHighlight:
			case TutorialUiEvent.EType.RemoveHighlight:
			case TutorialUiEvent.EType.BlockWidget:
			case TutorialUiEvent.EType.UnBlockWidget:
				m_WaitingEvents.Add(_event);
				break;
			case TutorialUiEvent.EType.ActivateSystem:
				m_CheckInput = true;
				break;
			case TutorialUiEvent.EType.DeactivateSystem:
				m_CheckInput = false;
				break;
			}
		}

		private bool IsACategoryActive(ETutorialCategory _highestCategoryChecked)
		{
			bool result = false;
			bool flag = false;
			int index = GetCategory(_highestCategoryChecked).Index;
			for (int i = 0; i <= index; i++)
			{
				TutorialCategory tutorialCategory = Categories[i];
				if (tutorialCategory.Finished)
				{
					flag = true;
					continue;
				}
				if (tutorialCategory.CurrentFlowId != STutorialFlowId.Invalid || (flag && m_categoryWithDirectPredecessor.IndexOf(tutorialCategory.Category) >= 0))
				{
					result = true;
					break;
				}
				flag = false;
			}
			return result;
		}

		private bool CheckIfHighlightIsVisible(GameObject _effect)
		{
			bool flag = true;
			Transform parent = _effect.transform.parent;
			if (parent != null)
			{
				UITutorialId component = parent.GetComponent<UITutorialId>();
				if (component != null)
				{
					flag = component.ConditionFulfilled;
				}
			}
			return flag && Singleton<UIScreenManager>.Instance.CurrentScreen == null;
		}

		private void SynchronizeVisiblity(TutorialUiEvent evt, GameObject go)
		{
			bool flag = evt.Type == TutorialUiEvent.EType.ShowWidget;
			UIBaseBehaviour component = go.GetComponent<UIBaseBehaviour>();
			if (!m_TutorialWidgetsVisibility.ContainsKey(go))
			{
				m_TutorialWidgetsVisibility.Add(go, flag);
			}
			else
			{
				m_TutorialWidgetsVisibility[go] = flag;
			}
			if (component != null)
			{
				component.SetVisibility(flag);
			}
			else
			{
				go.SetActive(flag);
			}
		}

		private static void InitCategoryFromProfile(UserProfile _profile, TutorialCategory _category)
		{
			AcierProtocol.Tutorial.Category profileTutorialCategory = TutorialHelper.GetProfileTutorialCategory(_profile, _category.Category);
			_category.Init(profileTutorialCategory);
		}
	}
}
